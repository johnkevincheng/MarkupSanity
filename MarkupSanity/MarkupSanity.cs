using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


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

            foreach (var tag in Configure.InternalDefaultLists.InternalRequiredTags)
            { //-- Some "tags" are always required by HtmlAgilityPack, so make sure they are included.
                if (!Configure.InternalDefaultLists.WhitelistedTags.Exists(p => String.Equals(p, tag, StringComparison.OrdinalIgnoreCase)))
                    whitelistedTags.Add(tag);
            }


            //-- Remove black-listed tags from the whitelist.
            whitelistedTags = whitelistedTags.Except(Configure.CustomBlacklistedTags).ToList();


            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(dirtyInput);

            //-- First remove all tags not included in the whitelist.
            IEnumerable<HtmlNode> removalTargets = htmlDoc.DocumentNode.DescendantsAndSelf().Where(p => !whitelistedTags.Exists(q => String.Equals(q, p.Name, StringComparison.OrdinalIgnoreCase)));
            if (removalTargets != null && removalTargets.Count() > 0)
            {
                foreach (var node in removalTargets.Reverse())
                {
                    if (Configure.RemoveTagsOnly)
                        node.ParentNode.InsertBefore(htmlDoc.CreateTextNode(node.InnerHtml), node);

                    node.Remove();
                }
            }


            //-- Remove nodes containing dangerous Type values.
            var scriptTypes = new String[] { "text/javascript", "text/vbscript" };
            foreach (var node in htmlDoc.DocumentNode.DescendantsAndSelf().Where(n => n.HasAttributes && n.Attributes.ToList().Exists(a => String.Equals(a.Name, "type", StringComparison.OrdinalIgnoreCase) && scriptTypes.Contains(a.Value.ToLower()))).Reverse())
                node.Remove();  //-- Always remove entire node where this attribute signature is found (e.g. script blocks).


            //-- Next find all nodes that has an attribute.
            foreach (HtmlNode node in htmlDoc.DocumentNode.DescendantsAndSelf().Where(p => p.HasAttributes))
            {
                //-- Next remove any attributes not included in the whitelist of any tags still retained from previous step.
                IEnumerable<HtmlAttribute> attributes = node.Attributes.Where(p => !whitelistedAttributes.Exists(q => String.Equals(q, p.Name, StringComparison.OrdinalIgnoreCase)));
                if (attributes != null && attributes.Count() > 0)
                    foreach (var attr in attributes.Reverse())
                    {
                        attr.Remove();
                        continue;
                    }

                //-- Additionally, remove any attributes that contain scripts which the browser can execute.
                IEnumerable<HtmlAttribute> scriptAttributes = node.Attributes.Where(p => scriptableAttributes.Exists(q => String.Equals(q, p.Name, StringComparison.OrdinalIgnoreCase)));
                if (scriptAttributes != null && scriptAttributes.Count() > 0)
                {
                    foreach (var attr in scriptAttributes.Reverse())
                    {
                        //-- Check each scriptable attribute for known signatures.
                        foreach (var signature in Configure.InternalDefaultLists.ScriptableAttributesScriptSignatures)
                        {
                            if (attr.Value.ProcessString(HttpUtility.UrlDecode, HttpUtility.HtmlDecode).StartsWith(signature))
                            {
                                attr.Remove();
                                continue;
                            }
                        }
                    }
                }
            }

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
