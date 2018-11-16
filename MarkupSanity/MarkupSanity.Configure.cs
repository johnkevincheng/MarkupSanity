using System;
using System.Collections.Generic;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        public static SanitizeConfigurations Configure { get; set; } = new SanitizeConfigurations();

        /// <summary>
        /// Manage runtime configurations for how MarkupSanity behaves.
        /// </summary>
        public partial class SanitizeConfigurations
        {
            /// <summary>
            /// Gets or sets the custom whitelist of allowed tags to validate against instead of the built-in ones.
            /// </summary>
            public List<String> CustomWhitelistedTags { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom whitelist of allowed attributes to validate against instead of the built-in ones.
            /// </summary>
            public List<String> CustomWhitelistedAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of known attributes that supports and executes javascript code. These attributes shall be inspected to ensure they don't contain any code that may potentially become dangerous.
            /// </summary>
            public List<String> CustomScriptableAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of known attribute signatures that supports and executes javascript code. If these signatures are found in attribute values, the whole attribute shall be removed.
            /// </summary>
            public List<String> CustomScriptableAttributeSignatures = new List<String>();


            /// <summary>
            /// Gets or sets the custom list of blacklisted tags. These tags are always removed from internal list and custom whitelists.
            /// </summary>
            /// <remarks>
            /// This removes tags from internal and custom whitelists, for cases when internal list is acceptable except for a few tags configured in it.
            /// </remarks>
            public List<String> CustomBlacklistedTags { get; set; } = new List<String>();


            /// <summary>
            /// Gets or sets the custom list of special supplemental tags to extend the default WhitelistedTags list. These are not added to the CustomWhitelistedTags property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public List<String> SupplementalTags { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of special supplemental attributes to extend the default WhitelistedAttributes list. These are not added to the CustomWhitelistedAttributes property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public List<String> SupplementalAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets the custom list of special supplemental scriptable attributes to extend the default ScriptableAttributes list. These are not added to the CustomScriptableAttributes property.
            /// </summary>
            /// <remarks>
            /// Add values here if the defaults are acceptable, but would like to add more to it.
            /// </remarks>
            public List<String> SupplementalScriptableAttributes { get; set; } = new List<String>();

            /// <summary>
            /// Gets or sets whether to remove the entire node (tag and contents) or only the invalid tag (retaining content).
            /// </summary>
            public Boolean RemoveMarkupTagsOnly { get; set; } = false;

            /// <summary>
            /// Gets or sets whether comments should be removed during cleanup.
            /// </summary>
            public Boolean RemoveComments { get; set; } = true;
        }
    }
}
