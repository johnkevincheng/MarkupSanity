using MarkupSanity;
using System;
using System.Web;

namespace MarkupSanityTester
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void sanitizeButton_Click(object sender, EventArgs e)
        {
            cleanHtmlDisplay.Text = HttpUtility.HtmlEncode(rawHtmlInput.Text.Sanitize());
        }
    }
}