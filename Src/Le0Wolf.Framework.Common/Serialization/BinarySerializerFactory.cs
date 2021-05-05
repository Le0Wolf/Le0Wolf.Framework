// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Serialization
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    ///     Делегат сериализации объекта в массив байтов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate byte[] BinarySerializer<in T>(T obj);

    /// <summary>
    ///     Делегат десериализации Span в объект
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedObj"></param>
    /// <returns></returns>
    public delegate T BinaryDeserializer<out T>(Span<byte> serializedObj);

    /// <summary>
    ///     Фабрика, формирующая сериализаторы и десериализаторы в/из массив байтов
    /// </summary>
    public class BinarySerializerFactory
    {
        private static readonly Type ByteListType = typeof(List<byte>);
        private static readonly Type ByteType = typeof(byte);

        private readonly Dictionary<Type, Delegate> typeDeserializers = new();

        private readonly Dictionary<Type, Delegate> typeSerializers = new();

        /// <summary>
        ///     Добавляет десериализатор массива байтов в поле указанного типа
        /// </summary>
        /// <typeparam name="T">Тип поля</typeparam>
        /// <param name="deserializer">делегат десериализации</param>
        public void AddPropertyValueDeserializer<T>(
            BinaryDeserializer<T> deserializer)
        {
            this.typeDeserializers.Add(typeof(T), deserializer);
        }

        /// <summary>
        ///     Добавляет сериализатор поля указанного типа в массив байтов
        /// </summary>
        /// <typeparam name="T">Тип поля</typeparam>
        /// <param name="serializer">Делегат сериализации</param>
        public void AddPropertyValueSerializer<T>(
            BinarySerializer<T> serializer)
        {
            this.typeSerializers.Add(typeof(T), serializer);
        }

        /// <summary>
        ///     Формирует делегат десериализации массива байтов в объект
        /// </summary>
        /// <typeparam name="T">Тип, в который будет десериализован объект</typeparam>
        /// <returns></returns>
        public BinaryDeserializer<T> MakeDeserializer<T>()
        {
            var type = typeof(T);

            var inputSpanExpr
                = Expression.Parameter(typeof(Span<byte>), "inputSpan");
            var resultObjExpr = Expression.Variable(type, "result");
            var lengthVarExpr = Expression.Variable(typeof(int),
                "currentArrayPropertyLength");

            var body = new List<Expression>
            {
                Expression.Assign(resultObjExpr, Expression.New(type))
            };

            Expression BuildDelegateCallExpr(Type valueType,
                params Expression[] valueExpr)
            {
                return this.typeDeserializers.TryGetValue(valueType,
                    out var typeDeserializer)
                    ? ExpressionBuilders.BuildDelegateCallExpression(
                        typeDeserializer, valueExpr)
                    : throw new NotSupportedException();
            }

            foreach (var propertyInfo in type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance))
            {
                if (!propertyInfo.CanWrite ||
                    propertyInfo.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                if (propertyInfo.PropertyType.IsArray)
                {
                    var elementType
                        = propertyInfo.PropertyType.GetElementType();
                    if (elementType != ByteType)
                    {
                        throw new NotSupportedException();
                    }

                    // Get array length and save to variable
                    var getLengthCallExpr
                        = BuildDelegateCallExpr(typeof(int), inputSpanExpr);
                    body.Add(
                        Expression.Assign(lengthVarExpr, getLengthCallExpr));

                    // Get property value as span
                    var propertyValueSpanExpr
                        = ExpressionBuilders.BuildSpanSliceCallExpression(
                            inputSpanExpr,
                            Expression.Constant(
                                sizeof(int)), // Skip array length counter bytes
                            lengthVarExpr);

                    var spanToArrayExpr
                        = ExpressionBuilders.BuildSpanToArrayCallExpression(
                            propertyValueSpanExpr);

                    // Assign array value to property
                    body.Add(Expression.Assign(
                        Expression.MakeMemberAccess(resultObjExpr,
                            propertyInfo),
                        spanToArrayExpr));

                    // Move to next property value position
                    var offsetToNextPropertyValueExp
                        = Expression.Add(Expression.Constant(sizeof(int)),
                            lengthVarExpr);
                    body.Add(Expression.Assign(inputSpanExpr,
                        ExpressionBuilders.BuildSpanSliceCallExpression(
                            inputSpanExpr, offsetToNextPropertyValueExp)));
                }
                else
                {
                    // Assign value to property
                    var getValueCallExpr
                        = BuildDelegateCallExpr(propertyInfo.PropertyType,
                            inputSpanExpr);
                    body.Add(Expression.Assign(
                        Expression.MakeMemberAccess(resultObjExpr,
                            propertyInfo),
                        getValueCallExpr));

                    // Move to next property value position
                    var offsetExpr
                        = Expression.Constant(
                            Marshal.SizeOf(propertyInfo.PropertyType));
                    body.Add(Expression.Assign(inputSpanExpr,
                        ExpressionBuilders.BuildSpanSliceCallExpression(
                            inputSpanExpr, offsetExpr)));
                }
            }

            body.Add(resultObjExpr);

            var lambda = Expression.Lambda<BinaryDeserializer<T>>(
                Expression.Block(new[]
                {
                    resultObjExpr,
                    lengthVarExpr
                }, body),
                inputSpanExpr);

            return lambda.Compile();
        }

        /// <summary>
        ///     Формирует делегат сериализации объекта в массив байтов
        /// </summary>
        /// <typeparam name="T">Тип сериализуемого объекта</typeparam>
        /// <returns>Делегат сериализации</returns>
        public BinarySerializer<T> MakeSerializer<T>()
        {
            var type = typeof(T);

            var objParamExpr = Expression.Parameter(type, "obj");
            var containerObjExpr
                = Expression.Variable(ByteListType, "container");

            var body = new List<Expression>
            {
                Expression.Assign(containerObjExpr,
                    Expression.New(ByteListType))
            };

            void AddExpression(Expression valueExpr)
            {
                if (this.typeSerializers.TryGetValue(valueExpr.Type,
                    out var typeSerializer))
                {
                    var subSerializerCallExpr
                        = ExpressionBuilders.BuildDelegateCallExpression(
                            typeSerializer, valueExpr);
                    var addValueCallExpr
                        = ExpressionBuilders.BuildAddRangeCallExpression(
                            containerObjExpr, subSerializerCallExpr);
                    body.Add(addValueCallExpr);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            foreach (var propertyInfo in type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance))
            {
                if (!propertyInfo.CanRead ||
                    propertyInfo.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                var valueExpr
                    = Expression.Property(objParamExpr, propertyInfo.Name);
                if (propertyInfo.PropertyType.IsArray)
                {
                    var elementType
                        = propertyInfo.PropertyType.GetElementType();
                    if (elementType != ByteType)
                    {
                        throw new NotSupportedException();
                    }

                    // Write array length
                    AddExpression(Expression.ArrayLength(valueExpr));

                    // Write array
                    body.Add(
                        ExpressionBuilders.BuildAddRangeCallExpression(
                            containerObjExpr, valueExpr));
                }
                else
                {
                    AddExpression(valueExpr);
                }
            }

            // Convert List<byte> to byte[]
            body.Add(
                ExpressionBuilders.BuildToArrayCallExpression(containerObjExpr,
                    ByteType));

            var lambda = Expression.Lambda<BinarySerializer<T>>(
                Expression.Block(new[]
                {
                    containerObjExpr
                }, body),
                objParamExpr);

            return lambda.Compile();
        }
    }
}