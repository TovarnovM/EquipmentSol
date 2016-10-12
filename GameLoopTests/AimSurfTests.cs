using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoop.Tests {
    [TestClass()]
    public class AimSurfTests {
        [TestMethod()]
        public void AimSurfTest() {
            var rect = new Rect(1,1,4,5,1);
            Assert.AreEqual(1d,rect.isHit(2,2),0.001);
        }

        [TestMethod()]
        public void getDamageTest() {
            var aim = new AimSurf();
            aim.AddBox(1,1,3,3,0.5);
            aim.AddBox(2,2,4,4,1);
            Assert.AreEqual(1d,aim.getDamage(2.5,2.5),0.0001);
            Assert.AreEqual(0.5,aim.getDamage(1.5,2.5),0.0001);
        }

    }
}