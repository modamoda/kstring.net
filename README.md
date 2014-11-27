KString.net
===========

Korean String Extension for .NET

한글 자모 시스템에 특화된 문자열 비교 및 다양한 기능을 제공합니다.
Extension library for String class, specially optimized for _"Hangul Jamo System"_

Download from Nuget
--------------

    PM> Install-Package KString
Link: [KString on Nuget](http://www.nuget.org/packages/KString/)

Quick Start / 간단한 사용법
----------------------
프로젝트에 어셈블리를 추가하신 후 " _using Moda.KString_ " 를 소스파일 상단에 넣어주시면 사용이 가능합니다.

    // KEquals : 초성이나 초성+중성이 정확히 일치해야 함
    "동해물과 백두산이".KEqulas("ㄷㅎㅁㄱ ㅂㄷㅅㅇ"); // true
    "동해물과 백두산이".KEqulas("도해무과 ㅂㄷㅅㅇ"); // true
    "동해물과 백두산이".KEqulas("ㄷㅎㅁㄱ ㅂㄷㅅ") // false 
    
    // KContains : 해당 초성이나 초중성이 들어가면 OK
    "동해물과 백두산이".KContains("ㄷㅎㅁㄱ"); // true
    
    // KIndexOf : 해당 패턴이 시작하는 부분을 리턴
    "동해물과 백두산이".KIndexOf("ㄷㅎㅁㄱ"); // 0
    "동해물과 백두산이".KIndexOf("ㅂㄷㅅ"); // 4
    
    // KMatch : 해당 패턴에 해당하는 스트링을 추출
    "동해물과 백두산이".KMatch("ㄷㅎㅁㄱ"); // "동해물과"
    "동해물과 백두산이".KMatch("ㅂㄷㅅ"); // "백두산"
    
    // KSeparate : 자소분리
    "동해물과".KSeparate(); // "ㄷㅗㅇㅎㅐㅁㅜㄹㄱㅘ"
    
    // lambda 또는 interface를 활용해 기능 확장이 가능
    "동해물과".KContains("ㅗㅐㅜㅘ", (i, p) => { /* ... */ });
    "동해물과".KContains("ㅗㅐㅜㅘ", ChosungOnlyComparator);
    
    // 초성 추출
    "동해물과 백두산이".KExtract(); // "ㄷㅎㅁㄱ ㅂㄷㅅㅇ"
    
    // 조사 자동추가
    "동해물".KAppendJosa(JosaType.WG); // "동해물과"
    "백두산".KAppendJosa("이", "가"); // "백두산이"

(소스코드의 Test코드들을 참고하셔도 됩니다)

Required
--------

* .NET 4.0 or higher
* Windows Phone 7.5 or higher
* Windows Store Apps
* Silverlight 5 or higher

See Also
--------
* [Hangul(wikipedia)|url:http://en.wikipedia.org/wiki/Hangul]
* [한글(위키백과)|http://ko.wikipedia.org/wiki/%ED%95%9C%EA%B8%80]

### Special Thanks to
* HeeJae Chang
* Junil Um
* Gilbok Lee

Feedbacks will be welcomed!
