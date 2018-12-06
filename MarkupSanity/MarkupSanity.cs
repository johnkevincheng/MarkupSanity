using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockFluid
{
    public static partial class MarkupSanity
    {
        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="configurations">Custom MarkupSanity configurations to use for this method call.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput, SanitizeConfigurations configurations)
        {
            var whitelistedTags = configurations.CustomWhitelistedTags;
            if (whitelistedTags == null || whitelistedTags.Count == 0)
                whitelistedTags = configurations.InternalDefaultLists.WhitelistedTags;

            var whitelistedAttributes = configurations.CustomWhitelistedAttributes;
            if (whitelistedAttributes == null || whitelistedAttributes.Count == 0)
                whitelistedAttributes = configurations.InternalDefaultLists.WhitelistedAttributes;

            var scriptableAttributes = configurations.CustomScriptableAttributes;
            if (scriptableAttributes == null || scriptableAttributes.Count == 0)
                scriptableAttributes = configurations.InternalDefaultLists.ScriptableAttributes;


            if (String.IsNullOrEmpty(dirtyInput) || whitelistedTags == null || whitelistedTags.Count == 0)
                return dirtyInput;

            //-- Some "tags" are always required by HtmlAgilityPack, so make sure they are included.
            configurations.InternalDefaultLists.InternalRequiredTags
                .Where(internalTag => !configurations.InternalDefaultLists.WhitelistedTags.Exists(whitelistedTag => whitelistedTag.Equals(internalTag, StringComparison.OrdinalIgnoreCase)))
                .ToList()
                .ForEach(internalTag => whitelistedTags.Add(internalTag));

            //-- Remove black-listed tags from the whitelist.
            whitelistedTags = whitelistedTags.Except(configurations.CustomBlacklistedTags).ToList();


            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(dirtyInput);
            IEnumerable<HtmlNode> docNodes = htmlDoc.DocumentNode.DescendantsAndSelf();

            //-- Perform cleanup steps.
            docNodes.FilterWhiteListedTags(configurations.RemoveMarkupTagsOnly, whitelistedTags, htmlDoc)
                .CleanComments(configurations.RemoveComments)
                .RemoveDangerousTypeNodes(configurations.InternalDefaultLists.ScriptTypeSignatures)
                .FilterWhitelistedAttributes(whitelistedAttributes)
                .CleanScriptableAttributes(scriptableAttributes);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="whitelistedTags">The list of allowed tags. If this list is empty, the input string shall be returned as-is.</param>
        /// <param name="whitelistedAttributes">The list of allowed tag attributes.</param>
        /// <param name="scriptableAttributes">The list of attributes to check for scripts.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput, List<String> whitelistedTags, List<String> whitelistedAttributes, List<String> scriptableAttributes)
        {
            var configurations = new SanitizeConfigurations
            {
                CustomWhitelistedTags = whitelistedTags,
                CustomWhitelistedAttributes = whitelistedAttributes,
                CustomScriptableAttributes = scriptableAttributes
            };

            return dirtyInput.SanitizeHtml(configurations);
        }

        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="whitelistedTags">The list of allowed tags. If this list is empty, the input string shall be returned as-is.</param>
        /// <param name="whitelistedAttributes">The list of allowed tag attributes.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput, List<String> whitelistedTags, List<String> whitelistedAttributes)
        {
            return dirtyInput.SanitizeHtml(whitelistedTags, whitelistedAttributes, Configure.CustomScriptableAttributes);
        }

        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput)
        {
            return dirtyInput.SanitizeHtml(Configure.CustomWhitelistedTags, Configure.CustomWhitelistedAttributes, Configure.CustomScriptableAttributes);
        }

        /// <summary>
        /// Sanitizes the text based on global MarkupSanity configurations. A return value indicates whether the input text is already clean/sanitized.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="sanitizedText">The sanitized results after processing the dirty input. Returns the same text if no dirty elements were found.</param>
        /// <returns>Returns True if output matches the input (no cleaning necessary), else False if the output was cleaned.</returns>
        public static Boolean TrySanitizeHtml(this String dirtyInput, out String sanitizedText)
        {
            String outputValue = SanitizeHtml(dirtyInput);

            sanitizedText = outputValue;

            return dirtyInput.Equals(outputValue, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Sanitizes the text based on provided MarkupSanity configurations. A return value indicates whether the input text is already clean/sanitized.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="configurations">Custom MarkupSanity configurations to use for this method call.</param>
        /// <param name="sanitizedText">The sanitized results after processing the dirty input. Returns the same text if no dirty elements were found.</param>
        /// <returns>Returns True if output matches the input (no cleaning necessary), else False if the output was cleaned.</returns>
        public static Boolean TrySanitizeHtml(this String dirtyInput, SanitizeConfigurations configurations, out String sanitizedText)
        {
            String outputValue = SanitizeHtml(dirtyInput, configurations);

            sanitizedText = outputValue;

            return dirtyInput.Equals(outputValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
