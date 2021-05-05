// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common
{
    #region usings

    using System;
    using System.Linq;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    ///     Базовый класс для методов построения дерева выражений
    /// </summary>
    public static class ExpressionBuilders
    {
        /// <summary>
        ///     Строит выражение для вызова метода AddRange(values)
        /// </summary>
        /// <param name="containerObjExpr">Выражение, содержащее контейнер, у которого есть метод AddRange</param>
        /// <param name="valuesExpr">Выражение, содержащее коллекцию элементов, которая будет передана в метод AddRange</param>
        /// <returns></returns>
        public static Expression BuildAddRangeCallExpression(
            Expression containerObjExpr, Expression valuesExpr)
        {
            return Expression.Call(
                containerObjExpr,
                containerObjExpr.Type.GetMethod("AddRange") ??
                throw new InvalidOperationException(),
                valuesExpr);
        }

        /// <summary>
        ///     Строит выражение вызова метода AsSpan()
        /// </summary>
        /// <param name="srcExpr">Выражение, содержащее коллекцию, для которой нужно вызвать метод</param>
        /// <param name="elementType">Тип элемента результатирующего Span</param>
        /// <returns></returns>
        public static Expression BuildAsSpanCallExpression(Expression srcExpr,
            Type elementType)
        {
            return Expression.Call(
                typeof(MemoryExtensions),
                nameof(MemoryExtensions.AsSpan),
                new[]
                {
                    elementType
                },
                srcExpr);
        }

        /// <summary>
        ///     Строит выражение вызова метода AsSpan(startPosition)
        /// </summary>
        /// <param name="srcExpr">Выражение, содержащее коллекцию, для которой нужно вызвать метод</param>
        /// <param name="elementType">Тип элемента результатирующего Span</param>
        /// <param name="startValueExpression">Выражение, содержащее начальную позицию</param>
        /// <returns></returns>
        public static Expression BuildAsSpanCallExpression(Expression srcExpr,
            Type elementType,
            Expression startValueExpression)
        {
            return Expression.Call(
                typeof(MemoryExtensions),
                nameof(MemoryExtensions.AsSpan),
                new[]
                {
                    elementType
                },
                srcExpr,
                startValueExpression);
        }

        /// <summary>
        ///     Строит выражение вызова делегата
        /// </summary>
        /// <param name="delegate">Делегат, который будет вызван</param>
        /// <param name="parameters">Параметры, передаваемые в делегат при вызове</param>
        /// <returns></returns>
        public static Expression BuildDelegateCallExpression(Delegate @delegate,
            params Expression[] parameters)
        {
            return Expression.Call(
                Expression.Constant(@delegate.Target),
                @delegate.Method,
                parameters);
        }

        /// <summary>
        ///     Строит выражение вызова Span.Slice(startPosition)
        /// </summary>
        /// <param name="spanExpr">Выражение, содержащее Span</param>
        /// <param name="startValueExpression">Выражение, содержащее начальную позицию</param>
        /// <returns></returns>
        public static Expression BuildSpanSliceCallExpression(
            Expression spanExpr, Expression startValueExpression)
        {
            var mi = spanExpr.Type.GetMethod(nameof(Span<object>.Slice), new[]
            {
                typeof(int)
            }) ?? throw new InvalidOperationException();

            return Expression.Call(
                spanExpr,
                mi,
                startValueExpression);
        }

        /// <summary>
        ///     Строит выражение вызова Span.Slice(startPosition, length)
        /// </summary>
        /// <param name="spanExpr">Выражение, содержащее Span</param>
        /// <param name="startValueExpression">Выражение, содержащее начальную позицию</param>
        /// <param name="lengthExpression">Выражение, содержащее длину</param>
        /// <returns></returns>
        public static Expression BuildSpanSliceCallExpression(
            Expression spanExpr, Expression startValueExpression,
            Expression lengthExpression)
        {
            var mi = spanExpr.Type.GetMethod(nameof(Span<object>.Slice), new[]
            {
                typeof(int),
                typeof(int)
            }) ?? throw new InvalidOperationException();

            return Expression.Call(
                spanExpr,
                mi,
                startValueExpression,
                lengthExpression);
        }

        /// <summary>
        ///     Строит выражение вызова Span.ToArray()
        /// </summary>
        /// <param name="spanExpr">Выражение, содержащее Span</param>
        /// <returns></returns>
        public static Expression BuildSpanToArrayCallExpression(
            Expression spanExpr)
        {
            var mi = spanExpr.Type.GetMethod(nameof(Span<object>.ToArray)) ??
                     throw new InvalidOperationException();

            return Expression.Call(spanExpr, mi);
        }

        /// <summary>
        ///     Строит выражение вызова метода ToArray()
        /// </summary>
        /// <param name="srcExpr">Выражение, содержащее коллекцию, для которой нужно вызвать метод</param>
        /// <param name="elementType">Тип элемента результатирующего массива</param>
        /// <returns></returns>
        public static Expression BuildToArrayCallExpression(Expression srcExpr,
            Type elementType)
        {
            return Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.ToArray),
                new[]
                {
                    elementType
                },
                srcExpr);
        }
    }
}