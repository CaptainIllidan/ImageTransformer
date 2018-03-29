using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageTransformer;
using System.Drawing;
using ImageTransformer.Data;

namespace UnitTestProject1
{
    [TestClass]
    public class ParseParams
    {
        [TestMethod]
        public void PositiveCoords()
        {
            var parameters = UrlParser.ParseParams("grayscale/1,1,1,1");

            Assert.AreEqual((Rectangle)parameters, new Rectangle(1, 1, 1, 1));
        }

        [TestMethod]
        public void NegativeCoords()
        {
            var parameters = UrlParser.ParseParams("grayscale/-1,-1,-1,-1");

            Assert.AreEqual((Rectangle)parameters, new Rectangle(-2, -2, 1, 1));
        }

        [TestMethod]
        public void HugeCoords()
        {
            var parameters = UrlParser.ParseParams("grayscale/-1000000,-1000000,1000000,1000000");

            Assert.AreEqual((Rectangle)parameters, new Rectangle(-1000000, -1000000, 1000000, 1000000));
        }
    }
}
