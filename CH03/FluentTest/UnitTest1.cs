using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace FluentTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NonFluentTest()
        {

            string actual = "ABCDEFGHI";

            Assert.IsTrue(actual.StartsWith("AB"));
            Assert.IsTrue(actual.EndsWith("HI"));
            Assert.IsTrue(actual.Contains("EF"));
            Assert.AreEqual(9, actual.Length);
        }

        [TestMethod]
        public void FluentTest()
        {
            string actual = "ABCDEFGHI";
            actual.Should().StartWith("AB")
                        .And.EndWith("HI")
                        .And.Contain("EF")
                        .And.HaveLength(9);
        }
    }
}
