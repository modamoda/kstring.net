using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moda.KString;

namespace KStringExtensionTest
{
    [TestClass]
    public class KStringUnitTest
    {
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
