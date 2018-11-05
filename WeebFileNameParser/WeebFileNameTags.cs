using System;
using System.Collections.Generic;
using System.Text;

namespace WeebFileNameParserLibrary
{
    static class WeebFileNameTags
    {
        public static readonly Dictionary<string, string[]> SeasonalPrefixes = new Dictionary<string, string[]>(){
            { "YES", new string[]{ "SEASON", "SAISON" } }
        };


        public static readonly Dictionary<string, string[]> SeasonPrefixes = new Dictionary<string, string[]>() {
            { "1", new string[]{ "FIRST",  "1ST", "1" } },
            { "2", new string[]{ "SECOND", "2ND", "2" } },
            { "3", new string[]{ "THIRD",  "3TH", "3" } },
            { "4", new string[]{ "FOURTH", "4TH", "4" } },
            { "5", new string[]{ "FIFTH",  "5TH", "5" } }
        };

        public static readonly Dictionary<string, string[]> AnimeSourcePrefixes = new Dictionary<string, string[]>(){
            {"MOVIE", new string[]{"GEKIJOUBAN", "MOVIE"} },
            {"OAD", new string[]{"OAD"} },
            {"OAV", new string[]{"OAV"} },
            {"ONA", new string[]{"ONA"} },
            {"OVA", new string[]{"OVA"} },
            {"SPECIAL", new string[]{"SPECIALS", "SPECIAL"} },
            {"TELEVISION", new string[]{"TV", "EPISODE"} },
            {"OPENING", new string[]{"NCOP", " OP ", "OPENING"} },
            {"ENDING", new string[]{"NCED", " ED ", "ENDING"} }
        };

        public static readonly Dictionary<string, string[]> AudioChannelsPrefixes = new Dictionary<string, string[]>(){
            { "STEREO", new string[]{"2.0CH", "2CH"} },
            { "SURROUND", new string[]{"5.1CH", "5.1", "DTS", "DOLBY", "DTS-ES", "DTS-HD", "DTS5.1", "DOLBY ATMOS", "TRUEHD"} }
        };

        public static readonly Dictionary<string, string[]> AudioCodecPrefixes = new Dictionary<string, string[]>(){
            { "AAC", new string[]{ "AAC", "AACX2","AACX3", "AACX4" } },
            { "AC-3", new string[]{  "AC3", "EAC3", "E-AC-3" } },
            { "FLAC", new string[]{  "FAC", "FACX2", "FACX3", "FACX4", "FLAC", "FLAC2", "FLAC5.1" } },
            { "OSSESS", new string[]{ "OSSESS" } },
            { "MP3", new string[]{ "MP3" } },
            { "OGG", new string[]{ "OGG" } },
            { "VORBIS", new string[]{ "VORBIS" } }
        };

        public static readonly Dictionary<string, string[]> AudioLanguagesPrefixes = new Dictionary<string, string[]>(){
            { "DUAL AUDIO", new string[]{ "DUAL AUDIO", "DUALAUDIO", "DUAL" } },
            { "MUTLI AUDIO", new string[]{ "MULTI", "AUDIO" } },
            { "ENGLISH", new string[]{ "ENGLISH" } },
        };

        public static readonly Dictionary<string, string[]> ExtensionPrefixes = new Dictionary<string, string[]>(){
            { "3GP", new string[]{ "3GP" } },
            { "AVI", new string[]{ "AVI" } },
            { "FLV", new string[]{ "FLV"} },
            { "M2TS", new string[]{ "M2TS" } },
            { "MKV", new string[]{ "MKV" } },
            { "MOV", new string[]{ "MOV" } },
            { "MP4", new string[]{ "MP4" } },
            { "MPG", new string[]{ "MPG" } },
            { "OGM", new string[]{ "OGM" } },
            { "WEBM", new string[]{ "WEBM" } },
            { "WMV", new string[]{ "WMV" } },
        };

        public static readonly Dictionary<string, string[]> ReleaseTypePrefix = new Dictionary<string, string[]>(){
            { "BLU-RAY", new string[]{ "BD", "BDRIP", "BLURAY", "BLU-RAY", "BLU", "RAY" } },
            { "DVD", new string[]{  "DVD","DVD5","DVD9", "DVD-R2J","DVDRIP","DVD-RIP","R2DVD","R2J","R2JDVD","R2JDVDRIP" } },
            { "TV", new string[]{   "HDTV", "HDTVRIP", "TVRIP", "TV-RIP", } },
            { "WEB", new string[]{  "WEBCAST", "WEBRIP" } }
        };

        public static readonly Dictionary<string, string[]> DubOrSubPrefix = new Dictionary<string, string[]>(){
            { "DUBBED", new string[]{  "DUB", "DUBBED" } },
            { "SUBTITLED", new string[]{   "BIG5","HARDSUB","HARDSUBS","SOFTSUB","SOFTSUBS","SUB","SUBBED","SUBTITLED","MULTI","SUBS", "SUBTITLES" } }
        };

        public static readonly Dictionary<string, string[]> VideoCodecPrefix = new Dictionary<string, string[]>(){
            { "8-BIT", new string[]{ "8BIT","8-BIT" } },
            { "10-BIT", new string[]{ "10BIT","10BITS","10-BIT","10-BITS","HI10","HI10P" } },
            { "HI-444", new string[]{ "HI444","HI444P","HI444PP" } },
            { "H264", new string[]{ "AVC", "H264", "H.264" } },
            { "H265", new string[]{ "H265", "H.265", "HEVC", "HEVC2" } },
            { "X264", new string[]{ "X264", "X.264" } },
            { "X265", new string[]{ "X265", "X.265" } },
            { "DIVX", new string[]{ "DIVX","DIVX5","DIVX6"} },
            { "XVID", new string[]{ "XVID"} },
            { "RMVB", new string[]{ "RMVB"} },
            { "WNV", new string[]{  "WMV","WMV3","WMV9"} }
        };

        public static readonly Dictionary<string, string[]> VideoResolutionTypePrefix = new Dictionary<string, string[]>(){
            { "HQ", new string[]{ "HQ" } },
            { "LQ", new string[]{ "LQ" } }
        };

        public static readonly Dictionary<string, string[]> VideoResolutionPrefix = new Dictionary<string, string[]>(){
            { "360P", new string[]{ "360","640x360","360P" } },
            { "480P", new string[]{ "480","848X480","640X480","480P", "SD" } },
            { "576P", new string[]{ "576","576P" } },
            { "720P", new string[]{ "720","1280X720","920X720","720P", "HD" } },
            { "1080P", new string[]{ "1080","1920X1080","1440X1080","1080P", "FHD" } },
            { "2160P", new string[]{ "2160","3840x2160","2160P", "4K", "UHD" } }
        };

        public static readonly Dictionary<string, string[]> YearPrefix = new Dictionary<string, string[]>(){

            { "1950", new string[]{ "1950" } },
            { "1951", new string[]{ "1951" } },
            { "1952", new string[]{ "1952" } },
            { "1953", new string[]{ "1953" } },
            { "1954", new string[]{ "1954" } },
            { "1955", new string[]{ "1955" } },
            { "1956", new string[]{ "1956" } },
            { "1957", new string[]{ "1957" } },
            { "1958", new string[]{ "1958" } },
            { "1959", new string[]{ "1959" } },
            { "1960", new string[]{ "1960" } },
            { "1961", new string[]{ "1961" } },
            { "1962", new string[]{ "1962" } },
            { "1963", new string[]{ "1963" } },
            { "1964", new string[]{ "1964" } },
            { "1965", new string[]{ "1965" } },
            { "1966", new string[]{ "1966" } },
            { "1967", new string[]{ "1967" } },
            { "1968", new string[]{ "1968" } },
            { "1969", new string[]{ "1969" } },
            { "1970", new string[]{ "1970" } },
            { "1971", new string[]{ "1971" } },
            { "1972", new string[]{ "1972" } },
            { "1973", new string[]{ "1973" } },
            { "1974", new string[]{ "1974" } },
            { "1975", new string[]{ "1975" } },
            { "1976", new string[]{ "1976" } },
            { "1977", new string[]{ "1977" } },
            { "1978", new string[]{ "1978" } },
            { "1979", new string[]{ "1979" } },
            { "1980", new string[]{ "1980" } },
            { "1981", new string[]{ "1981" } },
            { "1982", new string[]{ "1982" } },
            { "1983", new string[]{ "1983" } },
            { "1984", new string[]{ "1984" } },
            { "1985", new string[]{ "1985" } },
            { "1986", new string[]{ "1986" } },
            { "1987", new string[]{ "1987" } },
            { "1988", new string[]{ "1988" } },
            { "1989", new string[]{ "1989" } },
            { "1990", new string[]{ "1990" } },
            { "1991", new string[]{ "1991" } },
            { "1992", new string[]{ "1992" } },
            { "1993", new string[]{ "1993" } },
            { "1994", new string[]{ "1994" } },
            { "1995", new string[]{ "1995" } },
            { "1996", new string[]{ "1996" } },
            { "1997", new string[]{ "1997" } },
            { "1998", new string[]{ "1998" } },
            { "2000", new string[]{ "2000" } },
            { "2001", new string[]{ "2001" } },
            { "2002", new string[]{ "2002" } },
            { "2003", new string[]{ "2003" } },
            { "2004", new string[]{ "2004" } },
            { "2005", new string[]{ "2005" } },
            { "2006", new string[]{ "2006" } },
            { "2007", new string[]{ "2007" } },
            { "2008", new string[]{ "2008" } },
            { "2009", new string[]{ "2009" } },
            { "2010", new string[]{ "2010" } },
            { "2011", new string[]{ "2011" } },
            { "2012", new string[]{ "2012" } },
            { "2013", new string[]{ "2013" } },
            { "2014", new string[]{ "2014" } },
            { "2015", new string[]{ "2015" } },
            { "2016", new string[]{ "2016" } },
            { "2017", new string[]{ "2017" } },
            { "2018", new string[]{ "2018" } },
            { "2019", new string[]{ "2019" } },
            { "2020", new string[]{ "2020" } },
            { "2021", new string[]{ "2021" } },
            { "2022", new string[]{ "2022" } },
            { "2023", new string[]{ "2023" } },
            { "2024", new string[]{ "2024" } },
            { "2025", new string[]{ "2025" } },
            { "2026", new string[]{ "2026" } },
            { "2027", new string[]{ "2027" } },
            { "2028", new string[]{ "2028" } },
            { "2029", new string[]{ "2029" } },
            { "2030", new string[]{ "2030" } },
            { "2031", new string[]{ "2031" } },
            { "2032", new string[]{ "2032" } },
            { "2033", new string[]{ "2033" } },
            { "2034", new string[]{ "2034" } },
            { "2035", new string[]{ "2035" } },
            { "2036", new string[]{ "2036" } },
            { "2037", new string[]{ "2037" } },
            { "2038", new string[]{ "2038" } },
            { "2039", new string[]{ "2039" } },
            { "2040", new string[]{ "2040" } },
            { "2041", new string[]{ "2041" } },
            { "2042", new string[]{ "2042" } },
            { "2043", new string[]{ "2043" } },
            { "2044", new string[]{ "2044" } },
            { "2045", new string[]{ "2045" } },
            { "2046", new string[]{ "2046" } },
            { "2047", new string[]{ "2047" } },
            { "2048", new string[]{ "2048" } },
            { "2049", new string[]{ "2049" } },
            { "2050", new string[]{ "2050" } },
            { "2051", new string[]{ "2051" } },
            { "2052", new string[]{ "2052" } },
            { "2053", new string[]{ "2053" } },
            { "2054", new string[]{ "2054" } },
            { "2055", new string[]{ "2055" } },
            { "2056", new string[]{ "2056" } },
            { "2057", new string[]{ "2057" } },
            { "2058", new string[]{ "2058" } },
            { "2059", new string[]{ "2059" } },
            { "2060", new string[]{ "2060" } },
            { "2061", new string[]{ "2061" } },
            { "2062", new string[]{ "2062" } },
            { "2063", new string[]{ "2063" } },
            { "2064", new string[]{ "2064" } },
            { "2065", new string[]{ "2065" } },
            { "2066", new string[]{ "2066" } },
            { "2067", new string[]{ "2067" } },
            { "2068", new string[]{ "2068" } },
            { "2069", new string[]{ "2069" } },
            { "2070", new string[]{ "2070" } },
            { "2071", new string[]{ "2071" } },
            { "2072", new string[]{ "2072" } },
            { "2073", new string[]{ "2073" } },
            { "2074", new string[]{ "2074" } },
            { "2075", new string[]{ "2075" } },
            { "2076", new string[]{ "2076" } },
            { "2077", new string[]{ "2077" } },
            { "2078", new string[]{ "2078" } },
            { "2079", new string[]{ "2079" } },
            { "2080", new string[]{ "2080" } },
            { "2081", new string[]{ "2081" } },
            { "2082", new string[]{ "2082" } },
            { "2083", new string[]{ "2083" } },
            { "2084", new string[]{ "2084" } },
            { "2085", new string[]{ "2085" } },
            { "2086", new string[]{ "2086" } },
            { "2087", new string[]{ "2087" } },
            { "2088", new string[]{ "2088" } },
            { "2089", new string[]{ "2089" } },
            { "2090", new string[]{ "2090" } },
            { "2091", new string[]{ "2091" } },
            { "2092", new string[]{ "2092" } },
            { "2093", new string[]{ "2093" } },
            { "2094", new string[]{ "2094" } },
            { "2095", new string[]{ "2095" } },
            { "2096", new string[]{ "2096" } },
            { "2097", new string[]{ "2097" } },
            { "2098", new string[]{ "2098" } },
            { "2099", new string[]{ "2099" } },

        };

        public static readonly Dictionary<string, Dictionary<string, string[]>> ToCheckDefault = new Dictionary<string, Dictionary<string, string[]>>() {
            {"Year", YearPrefix },
            {"Seasonal", SeasonalPrefixes},
            {"Anime_Source", AnimeSourcePrefixes},
            {"Audio_Channels", AudioChannelsPrefixes },
            {"Audio_Language", AudioLanguagesPrefixes },
            {"Audio_Codec",  AudioCodecPrefixes },
            {"Extension", ExtensionPrefixes },
            {"Release_Type", ReleaseTypePrefix },
            {"Dubbed_Or_Subbed", DubOrSubPrefix },
            {"Video_Codec", VideoCodecPrefix },
            {"Video_Resolution_Type", VideoResolutionTypePrefix },
            {"Video_Resolution", VideoResolutionPrefix }
        };

    }
}
