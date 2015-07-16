using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringCalculatorProject;

namespace StringCalculatorTestProject
{
    [TestClass]
    public class StringCalculatorTests
    {
        [TestMethod]
        public void TestEvaluate()
        {
            Assert.AreEqual(StringCalculator.Evaluate("1+2"), 3);
            Assert.AreEqual(StringCalculator.Evaluate("(1+2)/3"), 1);
            Assert.AreEqual(StringCalculator.Evaluate("(2*-2)-(6/3)"), -6);
            Assert.AreEqual(StringCalculator.Evaluate("8/(2+4/2)"), 2);
            Assert.AreEqual(StringCalculator.Evaluate("1+2+3+5-2*7+4/2"), -1);
            Assert.AreEqual(StringCalculator.Evaluate("-(1+5)*(2+2)"), -24);
            Assert.AreEqual(StringCalculator.Evaluate("2++2"), 4);
            Assert.AreEqual(StringCalculator.Evaluate("2--2"), 4);
        }

        [TestMethod]
        public void TestHandleUnaryOperators()
        {
            List<string> input = new List<String> { "-", "5" };
            List<string> output = StringCalculator.HandleUnaryOperators(input);
            Assert.AreEqual(output.Count, 1);
            Assert.AreEqual(output[0], "-5");

            input = new List<String> { "+", "5", "-", "-", "2" };
            output = StringCalculator.HandleUnaryOperators(input);
            Assert.AreEqual(output.Count, 3);
            Assert.AreEqual(output[0], "+5");
            Assert.AreEqual(output[1], "-");
            Assert.AreEqual(output[2], "-2");
        }

        [TestMethod]
        public void TestIsOperator()
        {
            Assert.IsTrue(StringCalculator.IsOperator("*"));
            Assert.IsTrue(StringCalculator.IsOperator("/"));
            Assert.IsTrue(StringCalculator.IsOperator("+"));
            Assert.IsTrue(StringCalculator.IsOperator("-"));
            Assert.IsFalse(StringCalculator.IsOperator("1"));
            Assert.IsFalse(StringCalculator.IsOperator("A"));
            Assert.IsFalse(StringCalculator.IsOperator("("));
            Assert.IsFalse(StringCalculator.IsOperator(")"));
            Assert.IsFalse(StringCalculator.IsOperator("**"));
            Assert.IsFalse(StringCalculator.IsOperator("."));
        }

        [TestMethod]
        public void TestApplyOperator()
        {
            Assert.AreEqual(StringCalculator.ApplyOperator("+", "3", "-1"), "2");
            Assert.AreEqual(StringCalculator.ApplyOperator("-", "3", "-1"), "4");
            Assert.AreEqual(StringCalculator.ApplyOperator("*", "3", "-1"), "-3");
            Assert.AreEqual(StringCalculator.ApplyOperator("/", "6", "-2"), "-3");
        }

        [TestMethod]
        public void TestCalculate()
        {
            Assert.AreEqual(StringCalculator.Calculate("5"), "5");
            Assert.AreEqual(StringCalculator.Calculate("3--1"), "4");
            Assert.AreEqual(StringCalculator.Calculate("3*-1"), "-3");
            Assert.AreEqual(StringCalculator.Calculate("-6/-2"), "3");
            Assert.AreEqual(StringCalculator.Calculate("2*2*2+6/2/3-4"), "5");
            Assert.AreEqual(StringCalculator.Calculate("(2*2*2+6/2/3-4)"), "5");
            Assert.AreEqual(StringCalculator.Calculate("1-2+3"), "2");
            Assert.AreEqual(StringCalculator.Calculate("8/2*2"), "8");
        }

        [TestMethod]
        public void TestValidateInput()
        {
            Assert.IsFalse(StringCalculator.ValidateInput("1+A"));
            Assert.IsFalse(StringCalculator.ValidateInput("a+5"));
            Assert.IsFalse(StringCalculator.ValidateInput("!7"));
            Assert.IsFalse(StringCalculator.ValidateInput("[1+2]"));
            Assert.IsFalse(StringCalculator.ValidateInput("3=4"));
            Assert.IsFalse(StringCalculator.ValidateInput("4&1"));
            Assert.IsFalse(StringCalculator.ValidateInput("2^4"));
            Assert.IsFalse(StringCalculator.ValidateInput("100%"));
            Assert.IsFalse(StringCalculator.ValidateInput("$50"));
            Assert.IsFalse(StringCalculator.ValidateInput("1.5"));
            Assert.IsFalse(StringCalculator.ValidateInput("#1"));
            Assert.IsFalse(StringCalculator.ValidateInput("(1+2))"));
            Assert.IsTrue(StringCalculator.ValidateInput("13"));
            Assert.IsTrue(StringCalculator.ValidateInput("1+3"));
            Assert.IsTrue(StringCalculator.ValidateInput("3-6"));
            Assert.IsTrue(StringCalculator.ValidateInput("4*2"));
            Assert.IsTrue(StringCalculator.ValidateInput("4/2"));
            Assert.IsTrue(StringCalculator.ValidateInput("(3*2/(1+2))"));
        }
    }
}
