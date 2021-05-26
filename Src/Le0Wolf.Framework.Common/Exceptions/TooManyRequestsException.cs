// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Exceptions
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Исключение, выбрасываемое в случае, когда сделано слишком много запросов за единицу времени
    /// </summary>
    public class TooManyRequestsException : AppFrameworkException
    {
        public TooManyRequestsException(int retryAfterSec = 0)
        {
            this.RetryAfterSec = retryAfterSec;
        }

        public TooManyRequestsException(string message, int retryAfterSec = 0) : base(message)
        {
            this.RetryAfterSec = retryAfterSec;
        }

        public TooManyRequestsException(string message, Exception inner, int retryAfterSec = 0) : base(
            message, inner)
        {
            this.RetryAfterSec = retryAfterSec;
        }

        /// <summary>
        /// Количество секунд, через которые можно повторить запрос
        /// </summary>
        public int RetryAfterSec { get; set; }
    }
}