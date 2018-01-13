using System;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        /// <summary>
        /// Pre-process the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ProcessString(this String input)
        {
            return input.ProcessString(null);
        }

        /// <summary>
        /// Pre-process the string with additional custom function on it.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static String ProcessString(this String input, Func<String, String> func)
        {
            var val = input;

            if (func != null)
                val = func(input);

            val = val.Replace(" ", "").Replace(Environment.NewLine, " ").ToLower();

            return val;
        }
    }
}
