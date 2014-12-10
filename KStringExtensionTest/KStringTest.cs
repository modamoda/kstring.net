using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moda.KString;

namespace KStringExtensionTest
{
    [TestClass]
    public class KStringUnitTest
    {
        #region KExtractChosung()
        [TestMethod]
        public void TestExtractChosungBasic()
        {
            string expected = "ㄱㄴㄷㄹ";
            string actual = "가나다라".KExtractChosung();

            Assert.AreEqual(expected, actual, "Did not properly extracted");
        }

        [TestMethod]
        public void TestExtractChosungAdvanced()
        {
            string[] expecteds = { "ㄷㅎㅁㄱ ㅂㄷㅅㅇ", "ㅎㄴ2ㅅ4", "oneㅌthreeㅍ", "hello world" };
            string[] actualSources = { "동해물과 백두산이", "하나2셋4", "one투three포", "hello world" };
            string[] actuals = new string[actualSources.Length];

            for (int i = 0; i < actuals.Length; i++)
            {
                actuals[i] = actualSources[i].KExtractChosung();
                Assert.AreEqual(expecteds[i], actuals[i]);
            }
        }
        #endregion

        #region KMatches()
        [TestMethod]
        public void TestChosungMatchBasic()
        {
            string expected = "동해";
            string actual = "동해물과".KMatches("ㄷㅎ");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestChosungMatchAdvanced()
        {
            string[] expecteds = { null, null, "Kor", "강남 St" };
            string[] actuals = new string[expecteds.Length];
            actuals[0] = "동해물과".KMatches("ㅁㄴㅇㄹ");
            actuals[1] = "Korean".KMatches("kor"); // = null : Case sensitive
            actuals[2] = "Korean".KMatches("Kor");
            actuals[3] = "강남 Style".KMatches("ㄱㄴ St");

            for (int i = 0; i < actuals.Length; i++)
            {
                Assert.AreEqual(expecteds[i], actuals[i]);
            }
        } 
        #endregion

        #region KEquals()
        [TestMethod]
        public void TestEqualsBasic()
        {
            string word = "마르고닳도록";
            string[] candidates = { "ㅁㄹㄱㄷㄷㄹ", "ㅁㄹㄱ ㄷㄷㄹ", "ㅁㄹㄱㄷㄷ", "ㅁㄹㄱㄷㄷㄹㄹ" };

            Assert.IsTrue(word.KEquals(candidates[0]), string.Format("{0}.KEquals({1}) should be true", word, candidates[0]));
            for (int i = 1; i < candidates.Length; i++)
            {
                Assert.IsFalse(word.KEquals(candidates[i]), string.Format("{0}.KEquals({1}) should be false", word, candidates[i]));
            }
        }

        [TestMethod]
        public void TestEqualsAdvanced()
        {
            string word1 = "남산1위에A";
            string word1Candidate1 = "ㄴㅅ1ㅇㅇA";
            string word1Candidate2 = "ㄴㅅㅇㅇ";

            string word2 = "Namsan";
            string word2Candidate1 = "Namsan";
            string word2Candidate2 = "ㄴㅅ";

            Assert.IsTrue(word1.KEquals(word1Candidate1));
            Assert.IsFalse(word1.KEquals(word1Candidate2));
            Assert.IsTrue(word2.KEquals(word2Candidate1));
            Assert.IsFalse(word2.KEquals(word2Candidate2));
        }        
        #endregion

        #region KContains()
        [TestMethod]
        public void TestContainsBasic()
        {
            string word = "소나무";
            string[] validCandidates = { "ㅅ", "ㅅㄴㅁ", "ㄴㅁ", "ㅅㄴ"};
            string[] invalidCandidates = { "ㄴㅁㅅ", "ㅅ ㄴ ㅁ", "ㅁㄴㄹㅇ" };

            foreach(var validCandidate in validCandidates)
            {
                Assert.IsTrue(word.KContains(validCandidate), string.Format("{0}.KContains({1}) should be true", word, validCandidate));
            }

            foreach (var invalidCandidate in invalidCandidates)
            {
                Assert.IsFalse(word.KContains(invalidCandidate), string.Format("{0}.KContains({1}) should be false", word, invalidCandidate));
            }
        }

        [TestMethod]
        public void TestContainsAdvanced()
        {
            string word = "소1 나무";
            string[] validCandidates = { "ㅅ", "ㅅ1 ㄴㅁ", "ㄴㅁ", "ㅅ1 ㄴ" };
            string[] invalidCandidates = { "ㅅㄴㅁ", "ㅅ ㄴ ㅁ", "ㅁㄴㄹㅇ" };

            foreach (var validCandidate in validCandidates)
            {
                Assert.IsTrue(word.KContains(validCandidate), string.Format("{0}.KContains({1}) should be true", word, validCandidate));
            }

            foreach (var invalidCandidate in invalidCandidates)
            {
                Assert.IsFalse(word.KContains(invalidCandidate), string.Format("{0}.KContains({1}) should be false", word, invalidCandidate));
            }
        }
        #endregion

        // KSeparate()
        [TestMethod]
        public void TestSeparateMethod()
        {
            string[] expecteds = { "ㄷㅗㅇㅎㅐㅁㅜㄹ", "Korean", "ㅎㅏㄴRiver", "ㄳㄷㄷ" };
            string[] actualSources = { "동해물", "Korean", "한River", "ㄳㄷㄷ" };
            string[] actuals = new string[actualSources.Length];

            for (int i = 0; i < actuals.Length; i++)
            {
                actuals[i] = actualSources[i].KSeparate();
                Assert.AreEqual(expecteds[i], actuals[i]);
            }
        }
    }
}
