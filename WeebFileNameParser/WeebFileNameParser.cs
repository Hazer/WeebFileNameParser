using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WeebFileNameParserLibrary;

namespace WeebFileNameParserLibrary
{
    public class WeebFileNameParser
    {
        private readonly Dictionary<string, Dictionary<string, string>> Checkers;

        private readonly string[] Splitters = new string[] {
            " ",
            ".",
            "_",
            "-",
            ",",
            "/",
            "\\"
        };

        private readonly string[] SpecialChars = new string[]{
             "[","]","{","}","(",")"," ", ".", "_", " - ", ",", "/", "\\",":",";","`","'", "\"", "!", "@", "#", "$", "%","^","&","*","(",")","+","="
        };

        private readonly string[][] Encapsulations = new string[][] {

           new string[]{
                "[","]"
           },
           new string[]{
                "{","}"
           },
           new string[]{
                "(",")"
           }
        };

        public WeebFileNameParser()
        {
            Checkers = WeebFileNameTags.ToCheckDefault;
        }

        public WeebFileNameParser(Dictionary<string, Dictionary<string, string>> SetCheckers)
        {
            Checkers = SetCheckers;
        }

      


        public Dictionary<string, string> ParseFullString(string fullString)
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            List<string> words = new List<string>();
            List<string> wordsParsed = new List<string>();
            List<string> not_encapsulated = new List<string>();
            List<string> encapsulated = new List<string>();
            int lastBracketPosition = 0;
            int firstBracketPosition = fullString.Length - 1;
            string lowercase = fullString.ToLower();
            string forbracketdetection = lowercase;
            int bracketIndex = 0;

            string encapsulatedString = "";
            string not_encapsulatedString = "";

            //start splitting the string into seperate words
            string splittedString = fullString;
            foreach (string splitter in Splitters)
            {
                splittedString = splittedString.Replace(splitter, "|");
            }

            foreach (string removeThisChar in SpecialChars)
            {
                splittedString = splittedString.Replace(removeThisChar, "|");
            }

            string[] splitted = splittedString.Split('|');
            foreach (string word in splitted)
            {
                if (word.Length > 0)
                {
                    if (!words.Contains(word.ToUpper()))
                    {
                        words.Add(word.ToUpper());
                    }
                }
            }

            string nameToCheck = fullString.ToUpper();
            //compare words with values from static tags/checkers
            foreach (KeyValuePair<string, Dictionary<string, string>> toCheckPair in WeebFileNameTags.ToCheckDefault)
            {
                string key = toCheckPair.Key;



                foreach (KeyValuePair<string, string> keyToCheck in toCheckPair.Value)
                {
                    
                    int indexOfWord = words.IndexOf(keyToCheck.Key);
                    if (indexOfWord >= 0)
                    {
                        string wordFound = words[indexOfWord];
                        if (!wordsParsed.Contains(wordFound))
                        {
                            wordsParsed.Add(wordFound);
                            result.Add(key, keyToCheck.Value);
                            break;
                        }
                    }
                } 
            }

            //start disecting where the remaining words are (within or outside []/{}/())
            //try to determine start(key) and end (value) position of encapsulation brackets. 
            Dictionary<int, int> bracket_positions = new Dictionary<int, int>();
            
            foreach (string[] encapsulation in Encapsulations)
            {

                while (forbracketdetection.Contains(encapsulation[0]) && forbracketdetection.Contains(encapsulation[1]))
                {
                    int start = forbracketdetection.IndexOf(encapsulation[0]);
                    int end = forbracketdetection.IndexOf(encapsulation[1]);

                    if (end > lastBracketPosition)
                    {
                        lastBracketPosition = end;
                    }

                    if (start < firstBracketPosition )
                    {
                        firstBracketPosition = start;
                    }

                    if (start > -1 && end > -1)
                    {
                        bracket_positions.Add(start, end);

                        try
                        {


                            char[] ch = forbracketdetection.ToCharArray();
                            ch[start] = '|';
                            ch[end] = '|';
                            forbracketdetection = new string(ch);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }

            //Extract everything outside and inside encapsulation brackets and put values in a single string

            if (bracket_positions.Count > 0)
            {
                int previousEnd = 0;
                foreach (KeyValuePair<int, int> bracketPosition in bracket_positions)
                {
                    string insideBrackets = fullString.Substring(bracketPosition.Key + 1, bracketPosition.Value - bracketPosition.Key - 1);


                    if (bracketIndex == 1 && firstBracketPosition == 0)
                    {
                        not_encapsulatedString = "";
                    }

                    if (lastBracketPosition < fullString.Length - 5 && bracketIndex == 0)// (minus extension)
                    {
                        string behindEncapsulation = fullString.Substring(lastBracketPosition + 1);

                        encapsulatedString += behindEncapsulation + " ";
                    }

                    encapsulatedString += insideBrackets + " ";


                    if (previousEnd != 0 && previousEnd < lastBracketPosition)
                    {
                        not_encapsulatedString += fullString.Substring(previousEnd + 1, bracketPosition.Key - previousEnd - 1) + " ";
                    }
                    else if (previousEnd == 0 && firstBracketPosition != 0)
                    {
                        not_encapsulatedString += fullString.Substring(previousEnd, firstBracketPosition) + " ";
                    }
                    else if(previousEnd == 0 && bracketIndex == 0)
                    {
                        not_encapsulatedString = fullString.Substring(bracketPosition.Value) + " ";
                    }

                    if (firstBracketPosition > 0 && bracketIndex == 0)
                    {
                        not_encapsulatedString = fullString.Substring(0, firstBracketPosition - 1);
                    }

                    previousEnd = bracketPosition.Value;

                    bracketIndex++;
                    
                }


                //remove already parsed words from the encapsulated and not encapsulated strings
                foreach (string parsedWord in wordsParsed)
                {
                    encapsulatedString = encapsulatedString.ToLower().Replace(parsedWord.ToLower(), "");
                    not_encapsulatedString = not_encapsulatedString.ToLower().Replace(parsedWord.ToLower(), "");
                }

                //split the encapsulated string.
                foreach (string splitter in Splitters)
                {
                    if (splitter != "-")//some subgroups contain -
                    {
                        encapsulatedString = encapsulatedString.Replace(splitter, "|");
                    }
                }

                //remove all special chars except for - & :, as they can be used to seperate the generic anime title (such as "Sword Art Online") from the specific title ( "Alicization") [Sword Art Online - Alicization]
                foreach (string removeThisChar in SpecialChars)
                {
                    if (removeThisChar != "-" && removeThisChar != ":") 
                    {
                        encapsulatedString = encapsulatedString.Replace(removeThisChar, "|");
                    }
                }

                foreach (string encapsulatedWord in encapsulatedString.Split('|'))
                {
                    if (encapsulatedWord.Length > 1)//sometimes a seperator still persists, if the word is one char long, its not a word
                    {
                        encapsulated.Add(encapsulatedWord);
                    }
                }
            }
            else
            {
                //incase no incapusulation is happening, skip all this and use the input string for further parsing
                not_encapsulatedString = fullString;
            }



            foreach (string word in wordsParsed)
            {
                words.Remove(word);
            }

            //the name of the subgroup most often is within brackets at the beginning of a file name: [Hatsuyuki]_Sword_Art_Online_II_21_[1280x720][3E99D75F].mp4 -> Hatsuyuki == subgroup, 
            //but sometimes the sub group is actually not within brackets. But in that case, its at the end, after (possible) bracketes:Bakemonogatari_Ep13_[1080p,BluRay,x264]_-_qIIq-THORA.mkv -> qIIq-THORA == Subgroup



            if (not_encapsulatedString.Contains("("))
            {
                not_encapsulatedString = not_encapsulatedString.Split('(')[0];
            }

            if (not_encapsulatedString.Contains("{"))
            {
                not_encapsulatedString = not_encapsulatedString.Split('{')[0];
            }

            if (not_encapsulatedString.Contains("["))
            {
                not_encapsulatedString = not_encapsulatedString.Split('[')[0];
            }
           
            string animeWithoutEpisodeNum =  Regex.Match(not_encapsulatedString, @"^[^0-9]*").Value.Trim();

            if (animeWithoutEpisodeNum.Length <= 1)
            {
                animeWithoutEpisodeNum = not_encapsulatedString;
            }

            string mainAnime = animeWithoutEpisodeNum;
            string subAnime = animeWithoutEpisodeNum;

            if (animeWithoutEpisodeNum.Contains(':'))
            {
                mainAnime = animeWithoutEpisodeNum.Split(':')[0];
                subAnime = animeWithoutEpisodeNum.Split(':')[1];
            }
            else if (animeWithoutEpisodeNum.Contains("   "))
            {
                mainAnime = animeWithoutEpisodeNum.Split(new string[] { "   " }, StringSplitOptions.None)[0];
                subAnime = animeWithoutEpisodeNum.Split(new string[] { "   " }, StringSplitOptions.None)[1];
            }
            else if (animeWithoutEpisodeNum.Contains('-'))
            {
                mainAnime = animeWithoutEpisodeNum.Split('-')[0];
                subAnime = animeWithoutEpisodeNum.Split('-')[1];
            }         


            foreach (string removeThisChar in SpecialChars)
            {
                mainAnime = mainAnime.Replace(removeThisChar, " ").Trim();
                subAnime = subAnime.Replace(removeThisChar, " ").Trim(); 
            }

            foreach (string toRemove in WeebFileNameTags.WordsToRemove)
            {
                mainAnime = mainAnime.Replace(toRemove, "");
                subAnime = subAnime.Replace(toRemove, "");
            }

            if (mainAnime == subAnime)
            {
                subAnime = "";
            }

            if (not_encapsulatedString.Contains("v"))
            {
                not_encapsulatedString = not_encapsulatedString.Split('v')[0];
            }

            string episodeNumber = new String(not_encapsulatedString.Where(Char.IsDigit).ToArray());

           

            result.Add("Episode", episodeNumber);
            
            foreach (string word in words)
            {
                Regex regex = new Regex(@"[A-Za-z]+[\d@]+[\w@]*|[\d@]+[A-Za-z]+[\w@]*");
                Match match = regex.Match(word);

                if (match.Value.Contains("S") && match.Value.Contains("E"))
                {
                    if (result.ContainsKey("Season"))
                    {
                        result["Season"] = match.Value.Split('S')[1].Split('E')[0];
                    }
                    else
                    {
                        result.Add("Season", match.Value.Split('S')[1].Split('E')[0]);
                    }

                    if (result.ContainsKey("Episode"))
                    {
                        result["Episode"] = match.Value.Split('E')[1];
                    }
                    else
                    {
                        result.Add("Episode", match.Value.Split('E')[1]);
                    }
                }
            }


            result.Add("MainAnimeTitle", mainAnime);
            result.Add("SubAnimeTitle", subAnime);

            if (encapsulated.Count > 0)
            {
                result.Add("SubGroup", encapsulated[0]);
            }

            if (encapsulated.Count > 1)
            {
                result.Add("CRC32", encapsulated[encapsulated.Count - 1]);
            }

            result.Add("InsideEncapsulation", string.Join(" ", encapsulated));
            result.Add("OutsideEncapsulation", string.Join(" ", not_encapsulated));
            result.Add("NotParsedWords", string.Join(" ", words)); 


            return result;

        }

     
    }
}
