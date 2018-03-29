using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageTransformer;
using ImageTransformer.Data;

namespace UnitTestProject1
{
    [TestClass]
    public class CorrectParamsTests
    {

        [TestMethod]
        public void PositiveValues()
        {
            var parameters = UrlParser.CorrectParameters(new int[] { 1, 1, 1, 1 });

            CollectionAssert.AreEqual((int[])parameters, new int[] { 1, 1, 1, 1 });
        }

        [TestMethod]
        public void NegativeCoords()
        {
            var parameters = UrlParser.CorrectParameters(new int[] { -1, -1, 2, 2 });

            CollectionAssert.AreEqual((int[])parameters, new int[] { -1, -1, 2, 2 });
        }

        [TestMethod]
        public void NegativeSizeWithEmptyRect()
        {
            var parameters = UrlParser.CorrectParameters(new int[] { 0, 0, -10, -10 });

            CollectionAssert.AreEqual((int[])parameters, new int[] { -10, -10, 10, 10 });
        }

        [TestMethod]
        public void NegativeSizeWithNotNegativeRect()
        {
            var parameters = UrlParser.CorrectParameters(new int[] { 5, 5, -2, -2 });

            CollectionAssert.AreEqual((int[])parameters, new int[] { 3, 3, 2, 2 });
        }

    }
}
