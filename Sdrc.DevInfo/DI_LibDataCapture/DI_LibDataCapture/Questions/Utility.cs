using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    internal static class Utility
    {
        /// <summary>
        /// To remove quotes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string RemoveQuotes(string value)
        {
            return value.Replace("'", "''");
        }
    }
}
