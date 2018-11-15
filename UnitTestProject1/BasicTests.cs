using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockFluid;

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
    }
}