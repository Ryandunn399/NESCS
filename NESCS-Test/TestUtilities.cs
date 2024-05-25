using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESCS;

namespace NESCS_Test
{
    [TestClass]
    public class TestUtilities
    {
        [TestMethod]
        public void TestPagesCrossed()
        {
            uint v1 = 0xFF;
            byte v2 = 0x1;

            Assert.IsTrue(Utilities.PagesCrossed(v1, v2));

            v1 = 0x0;
            v2 = 0x0;

            Assert.IsFalse(Utilities.PagesCrossed(v1, v2));

            v1 = 0x280;
            v2 = 0xF0;

            Assert.IsTrue(Utilities.PagesCrossed(v1, v2));

            v1 = 0x210;
            v2 = 0x20;

            Assert.IsFalse(Utilities.PagesCrossed(v1, v2));
        }


        [TestMethod]
        public void TestZeroPageWrapAround()
        {
            byte v1 = 0xF0;
            byte v2 = 0x01;

            Assert.AreEqual(0xF1, Utilities.ZeroPageWrapAround(v1, v2));

            v1 = 0xFF;
            v2 = 0x80;

            Assert.AreEqual(0x7F, Utilities.ZeroPageWrapAround(v1, v2));
        }
    }
}
