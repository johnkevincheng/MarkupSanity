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
        private static String ProcessString(this String input)
        {
            return input.ProcessString(null);
        }

        /// <summary>
        /// Pre-process the string with additional custom function on it.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static String ProcessString(this String input, params Func<String, String>[] functions)
        {
            var val = input;

            foreach (var func in functions)
                if (func != null)
                    val = func(input);

            val = val.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "").Replace("\t","").ToLower();

            while (val.Contains(" "))
                val = val.Replace(" ", "");

            return val;
        }
    }
}
