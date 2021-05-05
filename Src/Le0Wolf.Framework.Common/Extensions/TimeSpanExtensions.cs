// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Extensions
{
    #region usings

    using System;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     Методы расширения для задания значений DateTime строкой
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly Regex ParseRegex
            = new(@"/(?<count>[0-9]+)\s*(?<name>s|m|h|d|mo|y)/gm", RegexOptions
                .Compiled);

        /// <summary>
        ///     Добавляет к дате значение из переданной строки
        /// </summary>
        /// <param name="current"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static DateTime AddFromString(this DateTime current,
            string timespan)
        {
            if (string.IsNullOrWhiteSpace(timespan))
            {
                throw new FormatException();
            }

            foreach (Match match in ParseRegex.Matches(timespan))
            {
                var value = int.Parse(match.Groups["count"].Value);

                switch (match.Groups["name"].Value)
                {
                    case "s":
                        current = current.AddSeconds(value);

                        break;
                    case "m":
                        current = current.AddMinutes(value);

                        break;
                    case "h":
                        current = current.AddHours(value);

                        break;
                    case "d":
                        current = current.AddDays(value);

                        break;
                    case "mo":
                        current = current.AddMonths(value);

                        break;
                    case "y":
                        current = current.AddYears(value);

                        break;
                    case "w":
                        current = current.AddDays(value * 7);

                        break;
                    default:
                        throw new FormatException();
                }
            }

            return current;
        }

        /// <summary>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static long GetOffsetInSeconds(this DateTime current,
            string timespan)
        {
            var newDate = current.AddFromString(timespan);
            var diff = newDate - current;

            return (long)diff.TotalSeconds;
        }
    }
}