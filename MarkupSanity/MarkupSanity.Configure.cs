using System;
using System.Collections.Generic;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        /// <summary>
        /// Manage runtime configurations for how MarkupSanity behaves.
        /// </summary>
        public static class Configure
        {
            /// <summary>
            /// Gets or sets the custom whitelist of allowed tags to validate against instead of the built-in ones.
            /// </summary>
            public static List<String> CustomWhitelistedTags { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom whitelist of allowed attributes to validate against instead of the built-in ones.
            /// </summary>
            public static List<String> CustomWhitelistedAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of known attributes that supports and executes javascript code. These attributes shall be inspected to ensure they don't contain any code that may potentially become dangerous.
            /// </summary>
            public static List<String> CustomScriptableAttributes { get; set; } = new List<String>();

            /// <summary>
            /// A list of internal required tags for HtmlAgilityPack. These are tags used by HtmlAgilityPack for elements not wrapped in HTML tags.
            /// </summary>
            internal static List<String> InternalRequiredTags = new List<String>() { "#document", "#text" };

            /// <summary>
            /// A list of default whitelisted tags to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedTags property.
            /// </summary>
            internal static List<String> WhitelistedTags = new List<String>() { "html", "head", "style", "body", "b", "strong", "i", "em", "u", "table", "th", "tr", "td", "ul", "ol", "li", "a", "p", "img", "iframe", "frameset", "frame", "title" };

            /// <summary>
            /// A list of default whitelisted attributes to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedAttributes property.
            /// </summary>
            internal static List<String> WhitelistedAttributes = new List<String>() { "id", "name", "src", "href", "style", "title", "type", "background" };

            /// <summary>
            /// A list of default attributes that are known to support and execute javascript code.
            /// </summary>
            internal static List<String> ScriptableAttributes = new List<String>() { "href", "src", "background" };
        }
    }
}
