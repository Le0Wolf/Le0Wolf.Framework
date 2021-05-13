// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Data
{
    #region usings

    using System;

    #endregion

    public class TestException : Exception
    {
        public int Code { get; set; }

        public object Value { get; set; }

        public TestException(int code)
        {
            this.Code = code;
        }
    }
}