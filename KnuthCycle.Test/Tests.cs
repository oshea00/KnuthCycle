using NUnit.Framework;
using System;

namespace KnuthCycle.Test
{
    class Tests : CycleExpression
    {
        [TestCase("(acf)",'a', ExpectedResult = 'c', Description = "Simple lookup")]
        [TestCase("(acf)",'f', ExpectedResult = 'a', Description = "Loopback lookup")]
        public char CanPermute(string expr, char c)
        {
            return CycleExpression.Permute(expr, c);
        }

        [Test]
        public void CanApplyExpression()
        {
            var result = CycleExpression.Permute("(acf)(bd)", "abcdef");
            Assert.AreEqual("cdfbea", result);
        }

        [TestCase("abcdef", ExpectedResult = true, Description = "Is a set")]
        [TestCase("aabcdef", ExpectedResult = false, Description = "Is not a set")]
        [TestCase("", ExpectedResult = true, Description = "Empty input")]
        [TestCase(null, ExpectedResult = false, Description = "Null")]
        public bool CanValidateInput(string input)
        {
            return CycleExpression.IsValidInput(input);
        }

        [TestCase("()", ExpectedResult = true,   Description = "Is unity")]
        [TestCase("()()", ExpectedResult = true, Description = "balanced parens")]
        [TestCase("", ExpectedResult = false, Description = "empty")]
        [TestCase(null, ExpectedResult = false, Description = "null")]
        [TestCase("(())", ExpectedResult = false, Description = "No nesting")]
        public bool CanValidateExpression(string expr)
        {
            return CycleExpression.IsValidExpression(expr);
        }

        [TestCase("(adg)(ceb)", ExpectedResult = "(adg)(ceb)", Description = "Disjoint cycles")]
        [TestCase("(acfg)(bcd)(aed)(fade)(bgfae)", ExpectedResult = "(adg)(ceb)", Description = "Non-Disjoint cycles")]
        public string CanMultiply(string expr)
        {
            return CycleExpression.Multiply(expr);
        }

        [Test]
        public void MultiplyInvalidThrowsException()
        {
            Assert.Throws<Exception>(() => {
                CycleExpression.Multiply("");
            });
        }

        [Test]
        public void CanInitExpression()
        {
            var marked = InitExpression("(acfg)(bcd)(aed)(fade)(bgfae)");
            Assert.AreEqual("(acfgA(bcdB(aedA(fadeF(bgfaeB",string.Join("",marked));
        }

        [TestCase(new char[] { 'A','a'},  ExpectedResult = 1)]
        [TestCase(new char[] { 'A' }, ExpectedResult = 0)]
        public int CanFindNextUnmarked(char[] marked)
        {
            return IndexOfFirstUntagged(marked);
        }

        [TestCase('A', 'a',ExpectedResult = true)]
        [TestCase('a', 'A',ExpectedResult = true)]
        [TestCase('x', 'a',ExpectedResult = false)]
        [TestCase('x', 'A', ExpectedResult = false)]
        public bool AreCaseInsensitiveEqual(char a, char b)
        {
            return AreEqual(a, b);
        }

        [TestCase(new char[] { 'A', 'a', 'b' }, 0, 'a', ExpectedResult = 1)]
        [TestCase(new char[] { 'A', '(', 'a', 'b' }, 1, 'b', ExpectedResult = 3)]
        [TestCase(new char[] { 'A', 'a', 'b' }, 2, 'b', ExpectedResult = 0)]
        public int CanFindNextMatch(char[] marked, int start, char match)
        {
            return IndexOfMatch(marked, start, match);
        }

        [Test]
        public void CanTag()
        {
            Assert.AreEqual('A', Tag('a'));
        }

        [Test]
        public void CanCannonicalize()
        {
            Assert.AreEqual("(ebc)(gad)", Cannonicalize("(adg)(ceb)"));
        }

        [Test]
        public void CannonicalAreEquivalent()
        {
            var expr = "(adg)(ceb)";
            var input = "abcdef";
            Assert.AreEqual(Permute(expr, input), Permute(Cannonicalize(expr), input));
        }

    }
}

