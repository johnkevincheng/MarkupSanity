using System;
using System.Collections.Generic;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        public static partial class Configure
        {
            internal static class InternalDefaultLists
            {
                /// <summary>
                /// A list of internal required tags for HtmlAgilityPack. These are tags used by HtmlAgilityPack for elements not wrapped in HTML tags.
                /// </summary>
                internal static List<String> InternalRequiredTags = new List<String>() { "#document", "#text" };

                /// <summary>
                /// A list of default whitelisted tags (plus the custom supplemental ones) to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedTags property.
                /// </summary>
                internal static List<String> WhitelistedTags
                {
                    get
                    {
                        var internalDefaultTags = new List<String>() {
                            "html", "head", "style", "body", "base", "main", "link",  // Document Tags
                            "a", "img", "title", "q", "s", "strike",  // Basic Tags
                            "b", "strong", "i", "em", "u", "bdi", "bdo", "del", "ins", "small", "h1", "h2", "h3", "h4", "h5", "h6", "sub", "sup", "tt",  // Formatting Tag
                            "table", "thead", "tbody", "tfoot", "col", "colgroup", "th", "tr", "td", "caption", "ul", "ol", "li", "p", "div", "span", "iframe", "frameset", "frame", "noframe", "map", "area", "br", "dl", "dt", "dd", "fieldset", "hr", "nav", "pre", "wbr",  // Layout Tags
                            "cite", "abbr", "address", "blockquote", "code", "details", "summary", "dfn", "kbd", "label", "legend", "menu", "menuitem", "meter", "output", "picture", "progress", "sample", "var",  // Element Type Tags
                            "acronym", "basefont", "big", "center", "dir", "font", // Deprecated Tags
                            "article", "header", "footer", "aside", "audio", "canvas", "datalist", "details", "dialog", "figure", "figcaption", "mark", "ruby", "rp", "rt", "section", "source", "summary", "time", "video"  // HTML5 Tags
                        };
                        internalDefaultTags.AddRange(SupplementalTags);

                        return internalDefaultTags;
                    }
                }

                /// <summary>
                /// A list of default whitelisted attributes (plus the custom supplemental ones) to validate against. This may be overriden by assigning custom attributes to the CustomWhitelistedAttributes property.
                /// </summary>
                internal static List<String> WhitelistedAttributes
                {
                    get
                    {
                        var internalDefaultAttributes = new List<String>() {
                            "xmlns", "id", "name", "type", "charset", "profile", "scope",  // Document Attributes
                            "src", "srcdoc", "href", "nohref", "target",  // Browsing Attributes
                            "style", "height","width", "color", "face", "size", "sizes", "dir", "alink", "background", "bgcolor", "link", "text", "vlink", "noshade", "border", "start",  // Formatting Attributes
                            "align", "valign", "char", "charoff", "usemap", "shape", "coords", "span", "frameborder", "longdesc", "marginheight", "marginwidth", "noresize", "scrolling", "sandbox", "cols", "rows", "hspace", "vspace", "compact", "reversed", "cellpadding", "cellspacing", "rules", "summary", "nowrap", "rowspan", "colspan",  // Layout Attributes
                            "title", "alt", "rev", "cite", "datetime", "for", "label", "abbr",  // Assistive Attributes
                            "autoplay", "controls", "loop", "muted", "preload", "ismap", "download", "media",  // Media Attributes
                            "form", "disabled", "value", "open", "checked", "default", "icon",  // Interactive Attributes
                            "rel", "hreflang", "autofocus", "formaction", "formenctype", "formmethod", "formnovalidate", "formtarget", "radiogroup", "high", "low", "min", "max", "optimum", "axis", "headers", "sorted"
                        };
                        internalDefaultAttributes.AddRange(SupplementalAttributes);

                        return internalDefaultAttributes;
                    }
                }

                /// <summary>
                /// A list of default attributes (plus the custom supplemental ones) that are known to support and execute javascript code.
                /// </summary>
                internal static List<String> ScriptableAttributes
                {
                    get
                    {
                        var internalDefaultScriptableAttributes = new List<String>() { "href", "src", "background" };
                        internalDefaultScriptableAttributes.AddRange(SupplementalScriptableAttributes);

                        return internalDefaultScriptableAttributes;
                    }
                }

                /// <summary>
                /// A list of default scripting signatures to watch out for on the scriptable attributes.
                /// </summary>
                internal static List<String> ScriptableAttributesScriptSignatures
                {
                    get
                    {
                        var internalDefaultScriptableAttributesScriptSignatures = new List<String>() { "javascript:", "vbscript:", "onmouseover=" };

                        return internalDefaultScriptableAttributesScriptSignatures;
                    }
                }
            }
        }
    }
}
