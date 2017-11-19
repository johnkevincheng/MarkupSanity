using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkupSanity
{
    public static class MarkupSanity
    {
        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <param name="whitelistedTags">The list of allowed tags. If this list is empty, the input string shall be returned as-is.</param>
        /// <param name="whitelistedAttributes">The list of allowed tag attributes.</param>
        /// <returns></returns>
        public static String Sanitize(this String dirtyInput, List<String> whitelistedTags, List<String> whitelistedAttributes)
        {
            if (whitelistedTags == null || whitelistedTags.Count == 0)
                return dirtyInput;

            foreach (var tag in InternalRequiredTags)
            { //-- Some "tags" are always required by HtmlAgilityPack, so make sure they are included.
                if (!WhitelistedTags.Exists(p => p.ToLower() == tag.ToLower()))
                    whitelistedTags.Add(tag);
            }


            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(dirtyInput);

            //-- First remove all tags not included in the whitelist.
            IEnumerable<HtmlNode> removalTargets = htmlDoc.DocumentNode.DescendantsAndSelf().Where(p => !whitelistedTags.Exists(q => q.ToLower() == p.Name.ToLower()));
            if (removalTargets != null && removalTargets.Count() > 0)
            {
                foreach (var node in removalTargets.Reverse())
                    node.Remove();
            }


            //-- Next find all nodes that has an attribute.
            foreach (HtmlNode node in htmlDoc.DocumentNode.DescendantsAndSelf().Where(p => p.HasAttributes))
            {
                //-- Next remove any attributes not included in the whitelist of any tags still retained from previous step.
                IEnumerable<HtmlAttribute> attributes = node.Attributes.Where(p => !whitelistedAttributes.Exists(q => q.ToLower() == p.Name.ToLower()));
                if (attributes != null && attributes.Count() > 0)
                {
                    foreach (var attr in attributes.Reverse())
                        attr.Remove();
                }
            }

            return htmlDoc.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Protect against cross-site scripting vulnerabilities by sanitizing the html input against unrecognized tags and attributes.
        /// </summary>
        /// <param name="dirtyInput">The raw html input.</param>
        /// <returns></returns>
        public static String Sanitize(this String dirtyInput)
        {
            return dirtyInput.Sanitize(WhitelistedTags, WhitelistedAttributes);
        }

        private static List<String> InternalRequiredTags = new List<String>() { "#document", "#text" };
        private static List<String> WhitelistedTags = new List<String>() { "b", "strong", "i", "em", "u", "table", "th", "tr", "td", "ul", "ol", "li" };
        private static List<String> WhitelistedAttributes = new List<String>() { "id", "name", "src", "href", "style" };
    }
}
