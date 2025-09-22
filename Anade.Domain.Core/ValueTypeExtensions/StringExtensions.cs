using System;
using System.Collections.Generic;
using System.Text;

namespace Anade.Domain.Core.ValueTypeExtensions
{
    public static class StringExtensions
    {
        public static string StringWritter(this string baseString, string addedString)
        {
            StringBuilder stringBuilder = new StringBuilder(baseString);
            baseString = stringBuilder.Append(addedString).ToString();
            return baseString;
        }
        public static string StringWritter(this string baseString, params string[] addedStrings)
        {
            StringBuilder stringBuilder = new StringBuilder(baseString);
            foreach (var s in addedStrings) stringBuilder.Append(s);                       
            return stringBuilder.ToString();
        }   
    }
}
