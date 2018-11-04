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
        /// <param name="whitelistedTags">The list of allowed tags. If this list is empty, the input string shall be returned as-is.</param>
        /// <param name="whitelistedAttributes">The list of allowed tag attributes.</param>
        /// <param name="scriptableAttributes">The list of attributes to check for scripts.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput, List<String> whitelistedTags, List<String> whitelistedAttributes, List<String> scriptableAttributes)
        {
            if (whitelistedTags == null || whitelistedTags.Count == 0)
                return dirtyInput;

            //-- Some "tags" are always required by HtmlAgilityPack, so make sure they are included.
            Configure.InternalDefaultLists.InternalRequiredTags
                .Where(internalTag => !Configure.InternalDefaultLists.WhitelistedTags.Exists(whitelistedTag => whitelistedTag.Equals(internalTag, StringComparison.OrdinalIgnoreCase)))
                .ToList()
                .ForEach(internalTag => whitelistedTags.Add(internalTag));

            //-- Remove black-listed tags from the whitelist.
            whitelistedTags = whitelistedTags.Except(Configure.CustomBlacklistedTags).ToList();


            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(dirtyInput);
            IEnumerable<HtmlNode> docNodes = htmlDoc.DocumentNode.DescendantsAndSelf();

            //-- Perform cleanup steps.
            docNodes.FilterWhiteListedTags(Configure.RemoveMarkupTagsOnly, whitelistedTags, htmlDoc)
                .CleanComments(Configure.RemoveComments)
                .RemoveDangerousTypeNodes()
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
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput, List<String> whitelistedTags, List<String> whitelistedAttributes)
        {
            return dirtyInput.SanitizeHtml(whitelistedTags, whitelistedAttributes, WorkingScriptableAttributes);
        }

        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <returns></returns>
        public static String SanitizeHtml(this String dirtyInput)
        {
            return dirtyInput.SanitizeHtml(WorkingWhitelistedTags, WorkingWhitelistedAttributes, WorkingScriptableAttributes);
        }

        internal static List<String> WorkingWhitelistedTags { get; set; } = Configure.InternalDefaultLists.WhitelistedTags;
        internal static List<String> WorkingWhitelistedAttributes { get; set; } = Configure.InternalDefaultLists.WhitelistedAttributes;
        internal static List<String> WorkingScriptableAttributes { get; set; } = Configure.InternalDefaultLists.ScriptableAttributes;
        internal static List<String> WorkingScriptableAttributesSignatures { get; set; } = Configure.InternalDefaultLists.ScriptableAttributesScriptSignatures;
    }
}
