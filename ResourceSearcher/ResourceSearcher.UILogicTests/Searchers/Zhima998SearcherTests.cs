using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceSearcher.UILogic.Searchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic.Searchers.Tests
{
    [TestClass()]
    public class Zhima998SearcherTests
    {
        [TestMethod()]
        public void GetDataAsyncTest()
        {
            var searcher = new Zhima998Searcher();
            var data = searcher.GetData("诛仙");
            Assert.IsTrue(data != null);
        }
    }
}