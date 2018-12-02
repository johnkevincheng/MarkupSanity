using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockFluid;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void CleanComments()
        {
            var actual = "<!-- comment here -->".SanitizeHtml();
            var expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ScriptTagsShouldBeRemoved()
        {
            var actual = "<script>var x = 'this should be removed.'; alert(x);</script>".SanitizeHtml();
            var expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ScriptAttributeShouldBeRemoved()
        {
            var actual = "<a href='javascript:alert('submitting...');'>Submit</a>".SanitizeHtml();
            var expected = "<a>Submit</a>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NonScriptAttributeShouldBeKept()
        {
            var actual = "<a href='www.google.com' onclick='alert('boo!');'>Visit Google</a>".SanitizeHtml();
            var expected = "<a href='www.google.com'>Visit Google</a>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnknownTagsShouldBeRemoved()
        {
            var actual = "<codeblock>String s = 'sample text';</codeblock>".SanitizeHtml();
            var expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCustomConfigurationsParamater()
        {
            var actual = "<codeblock>String s = 'use global here';</codeblock>".SanitizeHtml();
            var expected = "";
            Assert.AreEqual(expected, actual);

            var configs = new MarkupSanity.SanitizeConfigurations();
            configs.CustomWhitelistedTags = new List<String>() { "codeblock" };

            actual = "<codeblock>String s = 'use custom configs here';</codeblock>".SanitizeHtml(configs);
            expected = "<codeblock>String s = 'use custom configs here';</codeblock>";
            Assert.AreEqual(expected, actual);

            actual = "<codeblock>String s = 'use global again here';</codeblock>".SanitizeHtml();
            expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBlacklistedTags()
        {
            var config = new MarkupSanity.SanitizeConfigurations();
            var actual = "<b>sample text</b>".SanitizeHtml(config);
            var expected = "<b>sample text</b>";
            Assert.AreEqual(expected, actual);

            config.CustomBlacklistedTags = new List<String>() { "b" };

            actual = "<b>sample text</b>".SanitizeHtml(config);
            expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrySanitizeFunction()
        {
            var config = new MarkupSanity.SanitizeConfigurations();
            var actual = "<b>sample text</b>";
            String outVal;

            Boolean wasCleaned = actual.TrySanitizeHtml(config, out outVal);

            Assert.AreEqual("<b>sample text</b>", outVal);
            Assert.AreEqual(false, wasCleaned);


            config.CustomBlacklistedTags = new List<String>() { "b" };
            wasCleaned = actual.TrySanitizeHtml(config, out outVal);

            Assert.AreEqual("", outVal);
            Assert.AreEqual(true, wasCleaned);
        }
    }
}