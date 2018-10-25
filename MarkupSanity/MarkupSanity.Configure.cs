using System;
using System.Collections.Generic;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        /// <summary>
        /// Manage runtime configurations for how MarkupSanity behaves.
        /// </summary>
        public static partial class Configure
        {
            /// <summary>
            /// Gets or sets the custom whitelist of allowed tags to validate against instead of the built-in ones.
            /// </summary>
            public static List<String> CustomWhitelistedTags
            {
                get
                {
                    return _customWhitelistedTags;
                }
                set
                {
                    _customWhitelistedTags = value;

                    //-- Whenever the custom list is set, update the working list right away. If cleared, re-assign back the default list.
                    if (value == null || value.Count == 0)
                        WorkingWhitelistedTags = InternalDefaultLists.WhitelistedTags;
                    else
                        WorkingWhitelistedTags = value;
                }
            }
            private static List<String> _customWhitelistedTags = new List<String>();


            /// <summary>
            /// Gets or sets the custom whitelist of allowed attributes to validate against instead of the built-in ones.
            /// </summary>
            public static List<String> CustomWhitelistedAttributes
            {
                get
                {
                    return _customWhitelistedAttributes;
                }
                set
                {
                    _customWhitelistedAttributes = value;

                    //-- Whenever the custom list is set, update the working list right away. If cleared, re-assign back the default list.
                    if (value == null || value.Count == 0)
                        WorkingWhitelistedAttributes = InternalDefaultLists.WhitelistedAttributes;
                    else
                        WorkingWhitelistedAttributes = value;
                }
            }
            private static List<String> _customWhitelistedAttributes = new List<String>();


            /// <summary>
            /// Gets or sets the custom list of known attributes that supports and executes javascript code. These attributes shall be inspected to ensure they don't contain any code that may potentially become dangerous.
            /// </summary>
            public static List<String> CustomScriptableAttributes
            {
                get
                {
                    return _customScriptableAttributes;
                }
                set
                {
                    _customScriptableAttributes = value;

                    //-- Whenever the custom list is set, update the working list right away. If cleared, re-assign back the default list.
                    if (value == null || value.Count == 0)
                        WorkingScriptableAttributes = InternalDefaultLists.ScriptableAttributes;
                    else
                        WorkingScriptableAttributes = value;
                }
            }
            private static List<String> _customScriptableAttributes = new List<String>();


            /// <summary>
            /// Gets or sets the custom list of known attribute signatures that supports and executes javascript code. If these signatures are found in attribute values, the whole attribute shall be removed.
            /// </summary>
            public static List<String> CustomScriptableAttributeSignatures
            {
                get
                {
                    return _customScriptableAttributeSignatures;
                }
                set
                {
                    _customScriptableAttributeSignatures = value;

                    //-- Whenever the custom list is set, update the working list right away. If cleared, re-assign back the default list.
                    if (value == null || value.Count == 0)
                        WorkingScriptableAttributesSignatures = InternalDefaultLists.ScriptableAttributesScriptSignatures;
                    else
                        WorkingScriptableAttributesSignatures = value;
                }
            }
            private static List<String> _customScriptableAttributeSignatures { get; set; } = new List<String>();


            /// <summary>
            /// Gets or sets the custom list of blacklisted tags. These tags are always removed from internal list and custom whitelists.
            /// </summary>
            /// <remarks>
            /// This removes tags from internal and custom whitelists, for cases when internal list is acceptable except for a few tags configured in it.
            /// </remarks>
            public static List<String> CustomBlacklistedTags { get; set; } = new List<String>();


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
            /// Gets or sets whether to remove the entire node (tag and contents) or only the invalid tag (retaining content).
            /// </summary>
            public static Boolean RemoveMarkupTagsOnly { get; set; } = false;

            /// <summary>
            /// Gets or sets whether comments should be removed during cleanup.
            /// </summary>
            public static Boolean RemoveComments { get; set; } = true;
        }
    }
}
