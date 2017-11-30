using System;
using System.Collections.Generic;

namespace MarkupSanity
{
    /// <summary>
    /// Manage runtime configurations for how MarkupSanity behaves.
    /// </summary>
    public static class Configure
    {
        /// <summary>
        /// Gets or sets the custom whitelist of allowed tags to validate against instead of the built-in ones.
        /// </summary>
        public static List<String> CustomWhitelistedTags { get; set; }

        /// <summary>
        /// Gets or sets the custom whitelist of allowed attributes to validate against instead of the built-in ones.
        /// </summary>
        public static List<String> CustomWhitelistedAttributes { get; set; }

        /// <summary>
        /// A list of internal required tags for HtmlAgilityPack. These are tags used by HtmlAgilityPack for elements not wrapped in HTML tags.
        /// </summary>
        internal static List<String> InternalRequiredTags = new List<String>() { "#document", "#text" };

        /// <summary>
        /// A list of default whitelisted tags to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedTags property.
        /// </summary>
        internal static List<String> WhitelistedTags = new List<String>() { "b", "strong", "i", "em", "u", "table", "th", "tr", "td", "ul", "ol", "li" };

        /// <summary>
        /// A list of default whitelisted attributes to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedAttributes property.
        /// </summary>
        internal static List<String> WhitelistedAttributes = new List<String>() { "id", "name", "src", "href", "style" };
    }
}
