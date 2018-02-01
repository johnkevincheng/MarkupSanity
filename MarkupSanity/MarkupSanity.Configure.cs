using System;
using System.Collections.Generic;
using System.Linq;

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
            /// Gets or sets the custom list of known attribute signatures that supports and executes javascript code. If these signatures are found in attribute values, the whole attribute shall be removed.
            /// </summary>
            public static List<String> CustomScriptableAttributeSignatures { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of special supplemental tags to extend the default WhitelistedTags list. These are not added to the CustomWhitelistedTags property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public static List<String> SupplementalTags { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of special supplemental attributes to extend the default WhitelistedAttributes list. These are not added to the CustomWhitelistedAttributes property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public static List<String> SupplementalAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of special supplemental scriptable attributes to extend the default ScriptableAttributes list. These are not added to the CustomScriptableAttributes property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public static List<String> SupplementalScriptableAttributes { get; set; } = new List<String>();

            /// <summary>
            /// A list of internal required tags for HtmlAgilityPack. These are tags used by HtmlAgilityPack for elements not wrapped in HTML tags.
            /// </summary>
            internal static List<String> InternalRequiredTags = new List<String>() { "#document", "#text" };


            /// <summary>
            /// A list of default whitelisted tags to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedTags property.
            /// </summary>
            internal static List<String> WhitelistedTags = new List<String>() {
                "html", "head", "style", "body", "base", "main", "link",  // Document Tags
                "a", "img", "title", "q", "s", "strike",  // Basic Tags
                "b", "strong", "i", "em", "u", "bdi", "bdo", "del", "ins", "small", "h1", "h2", "h3", "h4", "h5", "h6", "sub", "sup", "tt",  // Formatting Tag
                "table", "thead", "tbody", "tfoot", "col", "colgroup", "th", "tr", "td", "caption", "ul", "ol", "li", "p", "div", "span", "iframe", "frameset", "frame", "noframe", "map", "area", "br", "dl", "dt", "dd", "fieldset", "hr", "nav", "pre", "wbr",  // Layout Tags
                "cite", "abbr", "address", "blockquote", "code", "details", "summary", "dfn", "kbd", "label", "legend", "menu", "menuitem", "meter", "output", "picture", "progress", "sample", "var",  // Element Type Tags
                "acronym", "basefont", "big", "center", "dir", "font", // Deprecated Tags
                "article", "header", "footer", "aside", "audio", "canvas", "datalist", "details", "dialog", "figure", "figcaption", "mark", "ruby", "rp", "rt", "section", "source", "summary", "time", "video"  // HTML5 Tags
            };

            /// <summary>
            /// A list of default whitelisted attributes to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedAttributes property.
            /// </summary>
            internal static List<String> WhitelistedAttributes = new List<String>() {
                "xmlns", "id", "name", "type", "charset", "profile", "scope",  // Document Attributes
                "src", "srcdoc", "href", "nohref", "target",  // Browsing Attributes
                "style", "height","width", "color", "face", "size", "sizes", "dir", "alink", "background", "bgcolor", "link", "text", "vlink", "noshade", "border", "start",  // Formatting Attributes
                "align", "valign", "char", "charoff", "usemap", "shape", "coords", "span", "frameborder", "longdesc", "marginheight", "marginwidth", "noresize", "scrolling", "sandbox", "cols", "rows", "hspace", "vspace", "compact", "reversed", "cellpadding", "cellspacing", "rules", "summary", "nowrap", "rowspan", "colspan",  // Layout Attributes
                "title", "alt", "rev", "cite", "datetime", "for", "label", "abbr",  // Assistive Attributes
                "autoplay", "controls", "loop", "muted", "preload", "ismap", "download", "media",  // Media Attributes
                "form", "disabled", "value", "open", "checked", "default", "icon",  // Interactive Attributes
                "rel", "hreflang", "autofocus", "formaction", "formenctype", "formmethod", "formnovalidate", "formtarget", "radiogroup", "high", "low", "min", "max", "optimum", "axis", "headers", "sorted"
            };

            /// <summary>
            /// A list of default attributes that are known to support and execute javascript code.
            /// </summary>
            internal static List<String> ScriptableAttributes = new List<String>() { "href", "src", "background" };

            /// <summary>
            /// A list of default scripting signatures to watch out for on the scriptable attributes.
            /// </summary>
            internal static List<String> ScriptableAttributesScriptSignatures = new List<String>() { "javascript:", "vbscript:", "onmouseover=" };
        }
    }
}
