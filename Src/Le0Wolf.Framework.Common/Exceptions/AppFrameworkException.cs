// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Exceptions
{
    #region usings

    using System;

    #endregion

    /// <summary>
    ///     Базовый класс исключений для всех исключений приложения
    /// </summary>
    public class AppFrameworkException : ApplicationException
    {
        public AppFrameworkException() { }

        public AppFrameworkException(string message) : base(message) { }

        public AppFrameworkException(string message, Exception inner)
            : base(message, inner) { }
    }
}