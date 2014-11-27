/*
 * KStringExtension.cs 
 * - String class extension that enables "Hangul Jamo Search" / 한글 자모 검색을 가능케 하는 라이브러리
 * - Used Poltable Class Library(PCL) technology so that this library works more than one .NET framework platform
 * -- Windows Store Apps
 * -- .NET 4+
 * -- Silverlight 4+
 * -- Windows Phone 7.5+
 * -- for more info about PCL, see: http://msdn.microsoft.com/en-us/library/gg597391(v=vs.100).aspx
 * 
 * Joon Hong / seoulmate.moda AT gmail .DOT. com / Feedbacks will be welcomed!
 * Published under MIT License
 * 
 * The MIT License (MIT)
 * Copyright (c) 2012-2014 Joon Hong
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * ----------------------------------------------------------------------------------
 * 
 * Special Thanks to:
 * - HeeJae Chang
 * - Junil Um
 * - Gilbok Lee
 */

using System.Collections.Generic;
using System.Text;

namespace Moda.KString
{
    /*
     * Reference : 
     * - Overview : https://www.facebook.com/photo.php?fbid=512619832084985
     * - Hangul unicode tables
     * -- Hangul jamo : http://www.unicode.org/charts/PDF/U1100.pdf
     * -- Hangul compatibility jamo : http://www.unicode.org/charts/PDF/U3130.pdf
     * -- Hangul syllables : http://www.unicode.org/charts/PDF/UAC00.pdf
     * 
     * Simple usage: 
     * Just put "using Moda.Libraries.StringExtension.Korean" in your source code
     * "동해물과 백두산이".Matches("ㄷㅎㅁㄱ") == true
     * "마르고 닳도록".Matches("마르고 다도로") == true
     * "하느님이 보우하사".Matches("ㅎ느님ㅇ ㅂㅇ하ㅅ") == true
     * "우리나라 만세".Matches("으리느르") == false
     * "Hangul".Matches("Han") == true
     * 
     */

    public static class KoreanStringExtension
    {
        #region Constants
        private const char HANGUL_SYLLABLES_START = '\xAC00';
        private const char HANGUL_SYLLABLES_END = '\xD79F';
        private const char HANGUL_JAMO_START = '\x1100';
        private const char HANGUL_JAMO_END = '\x11FF';
        private const char HANGUL_JAMO_MOUM_START = '\x1161';
        private const char HANGUL_JAMO_JONGSUNG_START = '\x11A8';
        private const char HANGUL_LETTER_START = '\x3130';
        private const char HANGUL_LETTER_END = '\x318F';
        private const char CHOSUNG_INTERVAL = (char)(28 * 21);
        private const char JOONGSUNG_INTERVAL = (char)28;
        #endregion

        #region (Jamo)-(Compatibility Jamo) Mappers

        /// <summary>
        /// 한글 호환자모영역에 있는 글자들을 한글 자모영역의 초성으로 맵핑
        /// </summary>
        private static Dictionary<char, char> _chosungMapper = new Dictionary<char, char>()
        {
            {'\x3131', '\x1100'}, // ㄱ
            {'\x3132', '\x1101'}, // ㄲ
            {'\x3134', '\x1102'}, // ㄴ
            {'\x3137', '\x1103'}, // ㄷ
            {'\x3138', '\x1104'}, // ㄸ
            {'\x3139', '\x1105'}, // ㄹ
            {'\x3141', '\x1106'}, // ㅁ
            {'\x3142', '\x1107'}, // ㅂ
            {'\x3143', '\x1108'}, // ㅃ
            {'\x3145', '\x1109'}, // ㅅ
            {'\x3146', '\x110A'}, // ㅆ
            {'\x3147', '\x110B'}, // ㅇ
            {'\x3148', '\x110C'}, // ㅈ
            {'\x3149', '\x110D'}, // ㅉ
            {'\x314A', '\x110E'}, // ㅊ
            {'\x314B', '\x110F'}, // ㅋ
            {'\x314C', '\x1110'}, // ㅌ
            {'\x314D', '\x1111'}, // ㅍ
            {'\x314E', '\x1112'}  // ㅎ
        };

        /// <summary>
        /// 한글 자모영역의 초성을 한글 호환자모영역에 있는 글자들로 맵핑
        /// (_chosungMapper의 반대기능)
        /// </summary>
        private static Dictionary<char, char> _reverseChosungMapper = new Dictionary<char, char>()
        {
            { '\x1100' , '\x3131' }, // ㄱ
            { '\x1101' , '\x3132' }, // ㄲ
            { '\x1102' , '\x3134' }, // ㄴ
            { '\x1103' , '\x3137' }, // ㄷ
            { '\x1104' , '\x3138' }, // ㄸ
            { '\x1105' , '\x3139' }, // ㄹ
            { '\x1106' , '\x3141' }, // ㅁ
            { '\x1107' , '\x3142' }, // ㅂ
            { '\x1108' , '\x3143' }, // ㅃ
            { '\x1109' , '\x3145' }, // ㅅ
            { '\x110A' , '\x3146' }, // ㅆ
            { '\x110B' , '\x3147' }, // ㅇ
            { '\x110C' , '\x3148' }, // ㅈ
            { '\x110D' , '\x3149' }, // ㅉ
            { '\x110E' , '\x314A' }, // ㅊ
            { '\x110F' , '\x314B' }, // ㅋ
            { '\x1110' , '\x314C' }, // ㅌ
            { '\x1111' , '\x314D' }, // ㅍ
            { '\x1112' , '\x314E' } // ㅎ
        };

        /// <summary>
        /// 한글 자모영역의 중성을 한글 호환자모영역에 있는 글자들로 맵핑
        /// </summary>
        private static Dictionary<char, char> _reverseJoongsungMapper = new Dictionary<char, char>()
        {
            {'\x1161','\x314F'}, // ㅏ
            {'\x1162','\x3150'}, // ㅐ
            {'\x1163','\x3151'}, // ㅑ
            {'\x1164','\x3152'}, // ㅒ
            {'\x1165','\x3153'}, // ㅓ
            {'\x1166','\x3154'}, // ㅔ
            {'\x1167','\x3155'}, // ㅕ
            {'\x1168','\x3156'}, // ㅖ
            {'\x1169','\x3157'}, // ㅗ
            {'\x116A','\x3158'}, // ㅘ
            {'\x116B','\x3159'}, // ㅙ
            {'\x116C','\x315A'}, // ㅚ
            {'\x116D','\x315B'}, // ㅛ
            {'\x116E','\x315C'}, // ㅜ
            {'\x116F','\x315D'}, // ㅝ
            {'\x1170','\x315E'}, // ㅞ
            {'\x1171','\x315F'}, // ㅟ
            {'\x1172','\x3160'}, // ㅠ
            {'\x1173','\x3161'}, // ㅡ
            {'\x1174','\x3162'}, // ㅢ
            {'\x1175','\x3163'}, // ㅣ
        };

        /// <summary>
        /// 한글 자모영역의 종성을 한글 호환자모영역에 있는 글자들로 맵핑
        /// </summary>
        private static Dictionary<char, char> _reverseJongsungMapper = new Dictionary<char, char>()
        {
            {'\x11A8','\x3131'}, // ㄱ
            {'\x11A9','\x3132'}, // ㄲ
            {'\x11AA','\x3133'}, // ㄱㅅ
            {'\x11AB','\x3134'}, // ㄴ
            {'\x11AC','\x3135'}, // ㄴㅈ
            {'\x11AD','\x3136'}, // ㄴㅎ
            {'\x11AE','\x3137'}, // ㄷ
            {'\x11AF','\x3139'}, // ㄹ
 
            {'\x11B0','\x313A'}, // ㄹㄱ 
            {'\x11B1','\x313B'}, // ㄹㅁ
            {'\x11B2','\x313C'}, // ㄹㅂ
            {'\x11B3','\x313D'}, // ㄹㅅ
            {'\x11B4','\x313E'}, // ㄹㅌ
            {'\x11B5','\x313F'}, // ㄹㅍ
            {'\x11B6','\x3140'}, // ㄹㅎ
            {'\x11B7','\x3141'}, // ㅁ
            {'\x11B8','\x3142'}, // ㅂ
            {'\x11B9','\x3144'}, // ㅂㅅ
            {'\x11BA','\x3145'}, // ㅅ
            {'\x11BB','\x3146'}, // ㅆ
            {'\x11BC','\x3147'}, // ㅇ
            {'\x11BD','\x3148'}, // ㅈ 
            {'\x11BE','\x314A'}, // ㅊ
            {'\x11BF','\x314B'}, // ㅋ
 
            {'\x11C0','\x31C'}, // ㅌ
            {'\x11C1','\x31D'}, // ㅍ
            {'\x11C2','\x31E'}, // ㅎ
        };

        #endregion

        #region Interfaces and Delegates

        public interface IStringSpecialComparator
        {
            string Matches(string s, string keyword);
            int IndexOf(string s, string keyword);
            bool Equals(char s, char keyword);
        }

        public delegate string SpecialStringMatcher(string s, string keyword);
        public delegate int SpecialIndexOfFinder(string s, string keyword);
        public delegate bool SpecialCharComparator(char c, char keyword);

        #endregion

        #region Josa Functions

        /// <summary>
        /// 단어 뒤에 적절한 조사를 자동으로 추가
        /// </summary>
        /// <param name="s">조사를 붙일단어</param>
        /// <param name="josaAfterJongsung">받침과 함께 끝나는 단어 뒤에 붙을 조사</param>
        /// <param name="josaAfterNonJongsung">받침이 없이 끝나는 단어 뒤에 붙을 조사</param>
        /// <returns>조사가 붙은 단어</returns>
        public static string KAppendJosa(this string s, string josaAfterJongsung, string josaAfterNonJongsung)
        {
            if (s.Trim().Length == 0)
                return s;

            char lastChar = s[s.Length - 1];
            if (lastChar.IsInHangulSyllablesRange())
            {
                return (lastChar.IsLevel3Char()) ? s + josaAfterJongsung : s + josaAfterNonJongsung;
            }

            // TODO 숫자 + 조사도 처리?

            return s;
        }

        /// <summary>
        /// 단어 뒤에 적절한 조사를 자동으로 추가
        /// </summary>
        /// <param name="s">조사를 붙일 단어</param>
        /// <param name="josaType">EN: 은+는, ER: 을+를, WG: 와+과, YDD:이다+다, YG:이+가</param>
        /// <returns>조사가 붙은 단어</returns>
        public static string KAppendJosa(this string s, JosaType josaType)
        {
            switch (josaType)
            {
                case JosaType.EN:
                    return s.KAppendJosa("은", "는");
                case JosaType.ER:
                    return s.KAppendJosa("을", "를");
                case JosaType.WG:
                    return s.KAppendJosa("와", "과");
                case JosaType.YDD:
                    return s.KAppendJosa("이다", "다");
                case JosaType.YG:
                    return s.KAppendJosa("이", "가");
                case JosaType.ERR:
                    return s.KAppendJosa("으로", "로");

                default:
                    return s;
            }
        }

        #endregion

        /// <summary>
        /// 해당 문장의 초성들을 추출해서 리턴
        /// "한글초성".KExtractChosung() == "ㅎㄱㅊㅅ"
        /// "Korean초성".KExtractChosung() == "Koreanㅊㅅ"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string KExtractChosung(this string s)
        {
            if (s.Trim().Length == 0)
                return "";

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].IsInHangulSyllablesRange())
                {
                    char chosung = (char)((s[i] - HANGUL_SYLLABLES_START) / CHOSUNG_INTERVAL + HANGUL_JAMO_START);
                    result.Append(ChosungToLetter(chosung));
                }
                else
                {
                    result.Append(s[i]);
                }
            }

            return result.ToString();
        }

        /*
         * KMatches()
         */

        /// <summary>
        /// "한글초성".KMatches("ㅊㅅ") == "초성" /
        /// "한글초성".KMatches("초서") == "초성" /
        /// 맞는 패턴이 없을 경우 null을 리턴
        /// </summary>
        /// <param name="s"></param>
        /// <param name="keyword">초성패턴</param>
        /// <returns></returns>
        public static string KMatches(this string s, string keyword)
        {
            return s.KMatches(keyword, PredefinedStringMatcher.Default);
        }

        public static string KMatches(this string s, string keyword, IStringSpecialComparator comparator)
        {
            return comparator.Matches(s, keyword);
        }

        public static string KMatches(this string s, string keyword, SpecialStringMatcher matcher)
        {
            return matcher(s, keyword);
        }

        public static class PredefinedStringMatcher
        {
            public static SpecialStringMatcher Default = KoreanJamoMatcher;
            public static SpecialStringMatcher Jamo = KoreanJamoMatcher;
            // public static SpecialStringMatcher ChosungOnly = ? // 구현예정
            // public static SpecialStringMatcher JoongsungOnly = ?// 구현예정

            private static string KoreanJamoMatcher(string s, string keyword)
            {
                if (s.KIndexOf(keyword) == -1)
                {
                    return null;
                }
                else
                {
                    return s.Substring(s.KIndexOf(keyword), keyword.Length);
                }
            }
        }

        /*
         * KEquals(string)
         */

        /// <summary>
        /// "한글초성".KEquals("ㅊㅅ") == false
        /// "한글초성".KEquals("ㅎㄱㅊㅅ") == true
        /// "한글초성".KEquals("하글ㅊㅅ") == true
        /// </summary>
        /// <param name="s"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool KEquals(this string s, string keyword)
        {
            return s.KEquals(keyword, PredefinedIndexOfFinder.Default);
        }

        public static bool KEquals(this string s, string keyword, SpecialIndexOfFinder indexOf)
        {
            return (s.Length == keyword.Length) && (indexOf(s, keyword) == 0);
        }

        public static bool KEquals(this string s, string keyword, IStringSpecialComparator comparator)
        {
            return (s.Length == keyword.Length) && (comparator.IndexOf(s, keyword) == 0);
        }


        /*
         * KContains() 
         */

        /// <summary>
        /// "한글초성".KContains("ㅊㅅ") == true
        /// </summary>
        /// <param name="s"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool KContains(this string s, string keyword)
        {
            return s.KContains(keyword, PredefinedIndexOfFinder.Default);
        }

        public static bool KContains(this string s, string keyword, IStringSpecialComparator comparator)
        {
            return (comparator.IndexOf(s, keyword) != -1);
        }

        public static bool KContains(this string s, string keyword, SpecialIndexOfFinder indexOf)
        {
            return (indexOf(s, keyword) != -1);
        }

        /*
         * KIndexOf() 
         */


        /// <summary>
        /// String.IndexOf(...)와 유사한 메서드
        /// "한글초성".KIndexOf("ㅊㅅ") == 2
        /// </summary>
        /// <param name="s">Source string</param>
        /// <param name="keyword">String to search</param>
        /// <returns>position of first occurence of keyword in s. -1 if not found</returns>
        public static int KIndexOf(this string s, string keyword)
        {
            return s.KIndexOf(keyword, PredefinedIndexOfFinder.Default);
        }

        public static int KIndexOf(this string s, string keyword, IStringSpecialComparator comparator)
        {
            return comparator.IndexOf(s, keyword);
        }

        public static int KIndexOf(this string s, string keyword, SpecialIndexOfFinder indexOf)
        {
            return indexOf(s, keyword);
        }


        public static class PredefinedIndexOfFinder
        {
            public static SpecialIndexOfFinder Default = KoreanJamoIndexOfFinder;
            public static SpecialIndexOfFinder Jamo = KoreanJamoIndexOfFinder;
            // public static SpecialIndexOfFinder ChosungOnly 
            // public static SpecialIndexOfFinder JoongsungOnly

            // TODO Enhance scalability by using delegate
            private static int KoreanJamoIndexOfFinder(string s, string keyword)
            {
                // algorithms are benchmarked from Java.lang.String.contains(...) / indexOf(...)

                if (keyword.Length > s.Length)
                {
                    return -1;
                }

                if (keyword.Length == 0)
                {
                    return 0;
                }

                int max = s.Length - keyword.Length;
                char firstKeyword = keyword[0];

                for (int i = 0; i <= max; i++)
                {
                    // looking for the first mattching char...
                    if (s[i].KEquals(firstKeyword) == false)
                    {
                        while (++i <= max && s[i].KEquals(firstKeyword) == false) ; // do nothing but traversing
                    }

                    // found first char, now look at the rest of string
                    if (i <= max)
                    {
                        int sIndex = i + 1;
                        int end = sIndex + keyword.Length - 1;
                        for (int keywordIndex = 1; sIndex < end; keywordIndex++, sIndex++)
                        {
                            if (s[sIndex].KEquals(keyword[keywordIndex]))
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (sIndex == end)
                        {
                            // found it!
                            return i;
                        }
                    }
                }

                return -1;
            }
        }



        /*
         * KEquals(char)
         */

        /// <summary>
        /// '한'.KEquals("ㅎ") == true
        /// '한'.KEquals("하") == true
        /// </summary>
        /// <param name="c"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool KEquals(this char c, char keyword)
        {
            return c.KEquals(keyword, PredefinedCharComparator.Default);
        }

        public static bool KEquals(this char c, char keyword, IStringSpecialComparator comparator)
        {
            return comparator.Equals(c, keyword);
        }

        public static bool KEquals(this char c, char keyword, SpecialCharComparator comparator)
        {
            return comparator(c, keyword);
        }


        public class PredefinedCharComparator
        {
            public static SpecialCharComparator Default = KoreanJamoComparator;
            public static SpecialCharComparator Jamo = KoreanJamoComparator;
            // public static SpecialCharComparator ChosungOnly
            // public static SpecialCharComparator JoongsungOnly

            private static bool KoreanJamoComparator(char c, char keyword)
            {
                bool result = false;
                int jamoLevel = keyword.FindJamoLevel();

                switch (jamoLevel)
                {
                    case 1:
                        result = c.Level1JamoMatch(keyword);
                        break;
                    case 2:
                        result = c.Level2JamoMatch(keyword);
                        break;
                    default:
                        result = (c == keyword);
                        break;
                }

                return result;
            }
        }

        /// <summary>
        /// "한".KSeperate() == "ㅎㅏㄴ";
        /// </summary>
        /// <param name="s">string to separate</param>
        /// <returns></returns>
        public static string KSeparate(this string s)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in s)
            {
                result.Append(c.KSeparate());
            }

            return result.ToString();
        }

        /// <summary>
        /// '한'.KSeparate() == "ㅎㅏㄴ";
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string KSeparate(this char c)
        {
            if (c.IsInHangulSyllablesRange() == false) return c.ToString();

            StringBuilder result = new StringBuilder();

            char chosung = (char)(((int)c - HANGUL_SYLLABLES_START) / CHOSUNG_INTERVAL + HANGUL_JAMO_START);
            char joongsung = (char)((((int)c - HANGUL_SYLLABLES_START) % CHOSUNG_INTERVAL) / JOONGSUNG_INTERVAL + HANGUL_JAMO_MOUM_START);
            char chosungLetter = ChosungToLetter(chosung);
            char joongsungLetter = JoongsungToLetter(joongsung);
            result.Append(chosungLetter).Append(joongsungLetter);

            if (c.IsLevel3Char() == true)
            {
                char jongsung = (char)((((int)c - HANGUL_SYLLABLES_START) % CHOSUNG_INTERVAL) % JOONGSUNG_INTERVAL + HANGUL_JAMO_JONGSUNG_START - 1);
                char jongsungLetter = JongsungToLetter(jongsung);
                result.Append(jongsungLetter);
            }

            return result.ToString();
        }

        

        // ------------------ private methods

        private static int FindJamoLevel(this char c)
        {
            if (c.IsLevel1Char())
            {
                return 1;
            }
            else if (c.IsLevel2Char())
            {
                return 2;
            }
            else if (c.IsLevel3Char())
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">c must be valid level 1 char </param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private static bool Level1JamoMatch(this char c, char keyword)
        {
            return c.Level1Normalizer() == keyword.Level1Normalizer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">c must be valied level 2 char</param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private static bool Level2JamoMatch(this char c, char keyword)
        {
            return c.Level2Normalizer() == keyword;
        }

        #region Normalizers

        /// <summary>
        /// normalize target's unicode: "가, 각, 간, 감, 공, 괔... => [ㄱ]"
        /// </summary>
        /// <param name="c">c must be valid hangul char</param>
        /// <returns>level 1 normalized chosung char</returns>
        private static char Level1Normalizer(this char c)
        {
            if (c.IsInJamoRange())
            {
                return c.LetterToChosung();
            }
            else if (c.IsInHangulSyllablesRange())
            {
                return (char)((c - HANGUL_SYLLABLES_START) / CHOSUNG_INTERVAL + HANGUL_JAMO_START);
            }

            return c;
        }

        /// <summary>
        /// normalize target's unicode: "가, 각, 간, 감, 갛 ... => [가]"
        /// </summary>
        /// <param name="c">parameter c must be valid Hangul char which is not chosung</param>
        /// <returns>level 2 normalized chosung + jungsung char</returns>
        private static char Level2Normalizer(this char c)
        {
            return (char)(c - ((c - HANGUL_SYLLABLES_START) % JOONGSUNG_INTERVAL));
        }

        #endregion


        /*
         * Category Finders / 한글 범주 검색과 관련된 메서드
         * Level1 : 초성만으로 이루어진 글자 / ㄳ같은 받침용 글자는 유효하지 않음 (ㄱ,ㄴ,ㄷ,ㄲ,ㄸ, ...)
         * Level2 : 초성+중성으로 이루어진 글자 (가,나,다,로,고,무,휘 ...)
         * Level3 : 초성+중성+종성으로 이루어진 글자
         */

        #region Hangul Category Finders


        /// <summary>
        /// identical to Char.IsInJamoRange()
        /// 초성만으로 이루어진 글자인지 판별, ㅀ처럼 받침 전용 글자는 false로 판정
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsLevel1Char(this char c)
        {
            if (c.IsInJamoRange())
            {
                c = c.LetterToChosung();
                return IsInRange('\x1100', c, '\x1112');
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 초성+중성으로 이루어진 글자인지 판별
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsLevel2Char(this char c)
        {
            if (c.IsInHangulSyllablesRange())
            {
                return (c - HANGUL_SYLLABLES_START) % JOONGSUNG_INTERVAL == 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 초성+중성+종성으로 이루어진 글자인지 판별. 종성이 반드시 있어야 함
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsLevel3Char(this char c)
        {
            return c.IsInHangulSyllablesRange() && (!c.IsLevel2Char());
        }

        private static bool IsInJamoRange(this char c)
        {
            return IsInRange(HANGUL_JAMO_START, c, HANGUL_JAMO_END) || IsInRange(HANGUL_LETTER_START, c, HANGUL_LETTER_END);
        }

        private static bool IsInHangulSyllablesRange(this char c)
        {
            return IsInRange(HANGUL_SYLLABLES_START, c, HANGUL_SYLLABLES_END);
        }

        private static bool IsInRange(char start, char c, char end)
        {
            return (c >= start && c <= end);
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// converts letter to chosung in unicode range
        /// ex) [ㄱ] in letter(0x3131) => [ㄱ] in chosung(0x1100)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char LetterToChosung(this char c)
        {
            try
            {
                return _chosungMapper[c];
            }
            catch (KeyNotFoundException)
            {
                return c;
            }
        }

        private static char ChosungToLetter(this char c)
        {
            try
            {
                return _reverseChosungMapper[c];
            }
            catch (KeyNotFoundException)
            {
                return c;
            }
        }

        /// <summary>
        /// Converts joongsung in jamo to letter
        /// ex) [ㅏ] in joonsung (0x1161) to [ㅏ] in letter (0x314F)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char JoongsungToLetter(this char c)
        {
            try
            {
                return _reverseJoongsungMapper[c];
            }
            catch (KeyNotFoundException)
            {
                return c;
            }
        }

        /// <summary>
        /// Converts jongsung in jamo unicode range to letter
        /// ex) [ㄱ] in jongsung (0x11A8) to [ㄱ] in letter (0x3131)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char JongsungToLetter(this char c)
        {
            try
            {
                return _reverseJongsungMapper[c];
            }
            catch (KeyNotFoundException)
            {
                return c;
            }
        }

        #endregion
    }


    public enum JosaType
    {
        EN, // 은는
        YG, // 이가
        ER, // 을를
        WG, // 와과
        YDD, // 이다,다
        ERR // 으로,로
    };
}
