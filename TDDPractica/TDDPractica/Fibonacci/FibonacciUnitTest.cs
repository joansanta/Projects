using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDDPractica;

namespace Test
{
    [TestClass]
    public class FibonacciUnitTest
    {
        [TestMethod]
        public void Fib_Given0_Return0()            
        {
            int n = 0;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(0, result);

        }

        [TestMethod]
        public void Fib_Given1_Return1()
        {
            int n = 1;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void Fib_Given2_Return1()
        {
            int n = 2;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Fib_Given3_Return2()
        {
            int n = 3;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Fib_Given4_Return3()
        {
            int n = 4;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Fib_Given5_Return5()
        {
            int n = 5;

            int result = Fibonacci.Fib(n);

            Assert.AreEqual(5, result);
        }
    }
}
