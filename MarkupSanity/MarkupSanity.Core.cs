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
        /// Remove all html markup not found in the whitelist. Optionally retain the contents of tags.
        /// </summary>
        /// <param name="rootNode">The root node of the html document parsed by HTML Agility Pack.</param>
        /// <param name="removeMarkupTagsOnly">Remove the entire node including the contents (and other child nodes), or only the tags for a node, retaining all contents and child nodes.</param>
        /// <param name="whitelistedTags">The list of whitelisted tag to retain after processing/cleanup</param>
        /// <param name="htmlDoc"></param>
        /// <returns></returns>
        internal static IEnumerable<HtmlNode> FilterWhiteListedTags(this IEnumerable<HtmlNode> rootNode, Boolean removeMarkupTagsOnly, List<String> whitelistedTags, HtmlDocument htmlDoc)
        {
            void CleanNode(HtmlNode node)
            {
                //-- Check if only markup tags are removed or also the contents.
                if (removeMarkupTagsOnly) //-- Insert contents as a sibling node before removing the node with invalid tag.
                    node.ParentNode.InsertBefore(htmlDoc.CreateTextNode(node.InnerHtml), node);

                node.Remove();
            }

            //-- Remove all tags not included in the whitelist.
            rootNode.Where(node => node.NodeType == HtmlNodeType.Element && !whitelistedTags.Exists(tag => tag.Equals(node.Name, StringComparison.OrdinalIgnoreCase)))
                    .ToList()
                    .ForEach(node => CleanNode(node));

            return rootNode;
        }

        /// <summary>
        /// Clean comments from the html document.
        /// </summary>
        /// <param name="rootNode">The root node of the html document parsed by HTML Agility Pack.</param>
        /// <param name="removeComments">Remove or retain comment elements from the html document. (Default = True)</param>
        /// <returns></returns>
        internal static IEnumerable<HtmlNode> CleanComments(this IEnumerable<HtmlNode> rootNode, Boolean removeComments)
        {
            if (removeComments)
                rootNode.Where(node => node.NodeType == HtmlNodeType.Comment)
                        .ToList()
                        .ForEach(node => node.Remove());

            return rootNode;
        }

        /// <summary>
        /// Remove entire nodes containing known script types.
        /// </summary>
        /// <param name="rootNode">The root node of the html document parsed by HTML Agility Pack.</param>
        /// <returns></returns>
        internal static IEnumerable<HtmlNode> RemoveDangerousTypeNodes(this IEnumerable<HtmlNode> rootNode)
        {
            var scriptTypes = new String[] { "text/javascript", "text/vbscript" };
            rootNode.Where(node => node.HasAttributes && node.Attributes.ToList().Exists(attr => attr.Name.Equals("type", StringComparison.OrdinalIgnoreCase) && scriptTypes.Contains(attr.Value.Replace(" ", string.Empty).ToLower())))
                    .ToList()
                    .ForEach(node => node.Remove());  //-- Always remove entire node where this attribute signature is found (e.g. script blocks).

            return rootNode;
        }

        /// <summary>
        /// Remove all attributes not in the whitelist from all html tags.
        /// </summary>
        /// <param name="rootNode">The root node of the html document parsed by HTML Agility Pack.</param>
        /// <param name="whitelistedAttributes">The list of whitelisted tag attributes to retain after processing/cleanup.</param>
        /// <returns></returns>
        internal static IEnumerable<HtmlNode> FilterWhitelistedAttributes(this IEnumerable<HtmlNode> rootNode, List<String> whitelistedAttributes)
        {
            rootNode.Where(node => node.HasAttributes)
                    .ToList()
                    .ForEach(node => node.Attributes
                                    .Where(attr => !whitelistedAttributes.Exists(allowedAttr => allowedAttr.Equals(attr.Name, StringComparison.OrdinalIgnoreCase)))
                                    .ToList()
                                    .ForEach(attr => attr.Remove()));

            return rootNode;
        }

        /// <summary>
        /// Process known scriptable attributes (e.g. src, href) that allows scripts in addition to their core purpose and remove the attribute if script patterns are found.
        /// </summary>
        /// <param name="rootNode">The root node of the html document parsed by HTML Agility Pack.</param>
        /// <param name="scriptableAttributes">The list of known attributes known to be scriptable which requires additional processing on their values.</param>
        /// <returns></returns>
        internal static IEnumerable<HtmlNode> CleanScriptableAttributes(this IEnumerable<HtmlNode> rootNode, List<String> scriptableAttributes)
        {
            Boolean AttributeContainsScriptableSignature(HtmlAttribute attribute)
            {
                var input = attribute.Value.ProcessString(HttpUtility.UrlDecode, HttpUtility.HtmlDecode);
                return Configure.InternalDefaultLists.ScriptableAttributesScriptSignatures.Exists(signature => input.Replace(" ", String.Empty).StartsWith(signature));
            }

            //-- Additionally, remove any attributes that contain scripts which the browser can execute.
            rootNode.Where(node => node.HasAttributes)
                    .ToList()
                    .ForEach(node => node.Attributes
                                    .Where(attr => scriptableAttributes.Exists(targetAttr => targetAttr.Equals(attr.Name, StringComparison.OrdinalIgnoreCase)) && AttributeContainsScriptableSignature(attr))
                                    .ToList()
                                    .ForEach(attr => attr.Remove()));

            return rootNode;
        }
    }
}
