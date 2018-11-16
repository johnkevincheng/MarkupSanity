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

            val = val.Collapse();

            return val;
        }

        /// <summary>
        /// Remove all white-space (space, tabs, new lines) from the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Collapse(this String input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            return input.Replace(Environment.NewLine, String.Empty).Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty).Replace(" ", String.Empty);
        }
    }
}
