using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDDPractica;

namespace TDDPractica
{

    //https://mva.microsoft.com/en-us/training-courses/testdriven-development-16458?l=dOa2MCwXC_3106218965
    [TestClass]
    public class BowlingKataUnitTests
    {
        private Game g;

        public BowlingKataUnitTests() {
            g = new Game();
        }

        [TestMethod]
        public void DoesGameExist() 
        {            
            Assert.IsNotNull(g);
        }

        [TestMethod]
        public void GutterGameReturns0()
        {                        

            rollMany(20, 0);

            Assert.AreEqual(0, g.scoreGame());
        }

        [TestMethod]
        public void SinglePinGameReturns20()
        {
            rollMany(20, 1);

            Assert.AreEqual(20, g.scoreGame());
        }

        [TestMethod]
        public void OneSpareReturnsAppropiateValue()
        {
            g.roll(5);
            g.roll(5); //spare
            g.roll(3);

            rollMany(17, 0);

            Assert.AreEqual(16, g.scoreGame());
        }

        [TestMethod]
        public void OneStrikeReturnsAppropiateValue()
        {
            g.roll(10); //strike
            g.roll(3);
            g.roll(4);

            rollMany(16, 0);

            Assert.AreEqual(24, g.scoreGame());
        }

        [TestMethod]
        public void PerfectGameReturn300()
        {            
            rollMany(12, 10);

            Assert.AreEqual(300, g.scoreGame());
        }

        public void rollMany(int rolls, int pins) {
            for (int i = 0; i < rolls; i++)
            {
                g.roll(pins);
            }
        }
    }
}
