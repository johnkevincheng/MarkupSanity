using RockFluid;
using System;
using System.Diagnostics;
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
            cleanHtmlDisplay.Text = HttpUtility.HtmlEncode(rawHtmlInput.Text.SanitizeHtml());
        }

        protected void measureButton_Click(object sender, EventArgs e)
        {
            String holder = String.Empty;

            var iterationsCount = 10000;
            var timer = new Stopwatch();

            timer.Start();
            for (var i = 0; i < iterationsCount; i++)
                holder = rawHtmlInput.Text;
            timer.Stop();
            var elapsedPlain = timer.Elapsed;

            timer.Reset();
            timer.Start();
            for (var i = 0; i < iterationsCount; i++)
                holder = rawHtmlInput.Text.SanitizeHtml();
            timer.Stop();
            var elapsedProcessed = timer.Elapsed;


            cleanHtmlDisplay.Text = $"Total Time for Unprocessed Reads: {elapsedPlain.TotalSeconds} second(s)<br>Average Time per Iteration: {elapsedPlain.TotalMilliseconds / iterationsCount} ms/item<br><br>Total Time for Processed Reads: {elapsedProcessed.TotalSeconds} second(s)<br>Average Time per Iteration: {elapsedProcessed.TotalMilliseconds / iterationsCount} ms/item<br><br>Percentage Added by Processing: {(elapsedProcessed.TotalMilliseconds - elapsedPlain.TotalMilliseconds) / elapsedPlain.TotalMilliseconds}%";
        }
    }
}