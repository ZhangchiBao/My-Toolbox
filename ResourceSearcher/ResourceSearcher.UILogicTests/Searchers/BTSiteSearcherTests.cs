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
    public class BTSiteSearcherTests
    {
        [TestMethod()]
        public void GetDataTest()
        {
            BTSiteSearcher searcher = new BTSiteSearcher();
            var data = searcher.GetData("叶问");
            Assert.IsNotNull(data);
        }
    }
}