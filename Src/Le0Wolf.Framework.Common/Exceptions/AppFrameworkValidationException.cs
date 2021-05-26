// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Exceptions
{
    #region usings

    using System;

    #endregion

    /// <summary>
    ///     Базовый класс исключений для всех исключений приложения, которые должны отдаваться на клиент
    /// </summary>
    public class AppFrameworkValidationException : AppFrameworkException
    {
        /// <summary>
        /// Создает исключение с кодом ошибки
        /// </summary>
        /// <param name="errorCode">Код ошибки, отправляемый на клиент</param>
        public AppFrameworkValidationException(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Создает исключение с кодом ошибки для клиента и сообщением для лога
        /// </summary>
        /// <param name="errorCode">Код ошибки, отправляемый на клиент</param>
        /// <param name="message">Сообщение для записи в лог</param>
        public AppFrameworkValidationException(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Создает исключение с кодом ошибки для клиента, сообщением для лога и вложенным исключением
        /// </summary>
        /// <param name="errorCode">Код ошибки, отправляемый на клиент</param>
        /// <param name="message">Сообщение для записи в лог</param>
        /// <param name="inner">Вложенное исключение для записи в лог</param>
        public AppFrameworkValidationException(string errorCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Код сообщения, отправляемый на клиент
        /// </summary>
        public string ErrorCode { get; set; }
    }
}