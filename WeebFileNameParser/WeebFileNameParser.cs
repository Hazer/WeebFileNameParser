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
        private readonly Dictionary<string, Dictionary<string, string[]>> Checkers;

        private readonly string[] Splitters = new string[] {
            " ",
            ".",
            "_",
            "-",
            ",",
            "/",
            "\\"
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
           },
        };

        private readonly Dictionary<string, int> RomanNumbers = new Dictionary<string, int>{
            { "I", 1 },
            { "II", 2 },
            { "III", 3 },
            { "IV", 4 },
            { "V", 5 },
            { "VI", 6 },
            { "VII", 7 },
            { "VIII", 8 },
            { "IX", 9 },
            { "X", 10 }
        };

        public WeebFileNameParser()
        {
            Checkers = WeebFileNameTags.ToCheckDefault;
        }

        public WeebFileNameParser(Dictionary<string, Dictionary<string, string[]>> SetCheckers)
        {
            Checkers = SetCheckers;
        }

        public Dictionary<string, string[]> ProcessFileName(string filename)
        {

            List<string> encapsulated = new List<string>();
            List<string> notencapsulated = new List<string>();

            for (int i = 0; i < Encapsulations.Length; i++)
            {
                try
                {
                    string patternInsideEncapsulations = @"(?<=\" + Encapsulations[i][0] + @")(.*?)(?=\" + Encapsulations[i][1] + ")";

                    MatchCollection matchesInsideEncapsulations = Regex.Matches(filename, patternInsideEncapsulations);

                    List<string[]> wordsInsideEncapsulations = new List<string[]>();

                    foreach (Match match in matchesInsideEncapsulations)
                    {
                        encapsulated.Add(match.Value);
                    }

                    string partBeforeEncapsulation = filename.Split(new string[] { Encapsulations[i][0] }, StringSplitOptions.None)[0];
                    string partAfterEncapsulation = filename.Split(new string[] { Encapsulations[i][1] }, StringSplitOptions.None)[filename.Split(new string[] { Encapsulations[i][1] }, StringSplitOptions.None).Length - 1];
                    List<string[]> wordsOutsideEncapsulations = new List<string[]>();


                    for (int x = 0; x < Encapsulations.Length; x++)
                    {
                        string patternOutsideEncapsulations = @"(?<=\" + Encapsulations[i][1] + @")(.*?)(?=\" + Encapsulations[x][0] + ")";

                        MatchCollection matchesOutsideEncapsulations = Regex.Matches(filename, patternOutsideEncapsulations);

                        foreach (Match match in matchesOutsideEncapsulations)
                        {
                            if (!notencapsulated.Contains(match.Value) && (match.Value != " " || match.Value != "_" || match.Value != "." || match.Value != "-"))
                            {
                                notencapsulated.Add(match.Value);
                            }
                        }

                        if (matchesOutsideEncapsulations.Count == 0)
                        {
                            notencapsulated.Add(filename);
                        }

                        if (!partBeforeEncapsulation.Contains(Encapsulations[i][0]) || !partBeforeEncapsulation.Contains(Encapsulations[i][1]))
                        {
                            if (!notencapsulated.Contains(partBeforeEncapsulation) && (partBeforeEncapsulation != " " || partBeforeEncapsulation != "_" || partBeforeEncapsulation != "." || partBeforeEncapsulation != "-"))
                            {
                                notencapsulated.Add(partBeforeEncapsulation);
                            }
                        }

                        if (!partAfterEncapsulation.Contains(Encapsulations[i][0]) || !partAfterEncapsulation.Contains(Encapsulations[i][1]))
                        {
                            if (!notencapsulated.Contains(partAfterEncapsulation) && (partAfterEncapsulation != " " || partAfterEncapsulation != "_" || partAfterEncapsulation != "." || partAfterEncapsulation != "-"))
                            {
                                notencapsulated.Add(partAfterEncapsulation);
                            }
                        }
                    }
                   

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }



            List<string> finalEncapsulated = new List<string>();
            foreach (string wordencapsulated in encapsulated)
            {
                string replacedSplitters = wordencapsulated;
                for (int i = 0; i < Splitters.Length; i++)
                {
                    if (replacedSplitters.Contains(Splitters[i]))
                    {
                        replacedSplitters = replacedSplitters.Replace(Splitters[i], "|");
                    }
                }

                string[] words = replacedSplitters.Split('|');
                if (words.Length > 1)
                {
                    foreach (string word in words)
                    {
                        if (!finalEncapsulated.Contains(word))
                        {

                            finalEncapsulated.Add(word);
                        }
                    }
                }
                else
                {
                    finalEncapsulated.Add(wordencapsulated);
                }
            }

            List<string> finalNotEncapsulated = new List<string>();

            foreach (string wordnotencapsulated in notencapsulated)
            {
                bool isencapsulated = false;
                foreach (string wordencapsulated in encapsulated)
                {
                    if (wordnotencapsulated.Contains(wordencapsulated))
                    {
                        isencapsulated = true;
                        break;
                    }
                }
                if (!isencapsulated)
                {
                    string replacedSplitters = wordnotencapsulated;
                    for (int i = 0; i < Splitters.Length; i++)
                    {
                        if (replacedSplitters.Contains(Splitters[i]))
                        {
                            replacedSplitters = replacedSplitters.Replace(Splitters[i], "|");
                        }
                    }

                    string[] words = replacedSplitters.Split('|');
                    if (words.Length > 1)
                    {
                        foreach (string word in words)
                        {
                            if (!finalNotEncapsulated.Contains(word))
                            {

                                finalNotEncapsulated.Add(word);
                            }
                        }
                    }
                }
            }

            encapsulated.Add(Path.GetExtension(filename).Substring(1));

            Dictionary<string, string[]> toReturn = new Dictionary<string, string[]>();
            toReturn.Add("encapsulated", finalEncapsulated.ToArray());
            toReturn.Add("notencapsulated", finalNotEncapsulated.ToArray());

            return toReturn;

        }


        public Dictionary<string, Dictionary<string, int>> ParseStaticWords(Dictionary<string, string[]> words)
        {

            Dictionary<string, Dictionary<string, int>> dictionaryResult = new Dictionary<string, Dictionary<string, int>>();
            foreach (KeyValuePair<string, string[]> resultparser in words)
            {


                string type = resultparser.Key;
                string[] listWithWords = resultparser.Value;

                List<string> wordsList = new List<string>(listWithWords);
                List<string> wordsToRemove = new List<string>();

                int wordIndex = 0;
                

                foreach (string word in listWithWords)
                {
                    foreach (KeyValuePair<string, Dictionary<string, string[]>> check in Checkers)
                    {
                        string check_type = check.Key;
                        int counter = 0;
                        
                        foreach (KeyValuePair<string, string[]> categoryToCheck in check.Value)
                        {

                            
                            foreach (string string_to_check in categoryToCheck.Value)
                            {
                                try
                                {
                                    if (word.ToUpper().Contains(string_to_check))
                                    {
                                     
                                        if (type == "encapsulated" && word.Length <= string_to_check.Length)
                                        {
                                            Dictionary<string, int> indexAndResult = new Dictionary<string, int>();
                                            indexAndResult.Add(categoryToCheck.Key, counter);

                                          
                                            if (!dictionaryResult.ContainsKey(check_type))
                                            {
                                                dictionaryResult.Add(check_type, indexAndResult);
                                            }
                                            wordsToRemove.Add(word);
                                            
                                               
                                            counter++;
                                        }
                                        else if (type != "encapsulated" && word.Length <= string_to_check.Length)
                                        {
                                            Dictionary<string, int> indexAndResult = new Dictionary<string, int>();
                                            indexAndResult.Add(categoryToCheck.Key, counter);
                                            if (!dictionaryResult.ContainsKey(check_type))
                                            {
                                                dictionaryResult.Add(check_type, indexAndResult);
                                            }
                                            wordsToRemove.Add(word);

                                            counter++;
                                        }
                                           
                                            
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    
                    wordIndex++;
                }

               

               

               

                Dictionary<string, int> remainingStrings = new Dictionary<string, int>();
                Dictionary<string, int> remainingIntegers = new Dictionary<string, int>();
                Dictionary<string, int> crc32 = new Dictionary<string, int>();
                Dictionary<string, int> season = new Dictionary<string, int>();
                Dictionary<string, int> episode = new Dictionary<string, int>();


                bool foundSeason = false;
                if (dictionaryResult.ContainsKey("Seasonal"))
                {
                    int indexToRemove = 0;
                    foreach (string word in wordsList)
                    {
                        foreach (KeyValuePair<string, string[]> seasonPrefixes in WeebFileNameTags.SeasonPrefixes)
                        {
                            foreach (string tocheck in seasonPrefixes.Value)
                            {
                                if (word.ToUpper().Contains(tocheck))
                                {
                                    season.Add(seasonPrefixes.Key, 0);
                                    wordsToRemove.Add(word);
                                    foundSeason = true;
                                    break;
                                }
                            }
                            if (foundSeason)
                            {
                                break;
                            }
                        }
                        if (foundSeason)
                        {
                            break;
                        }
                        indexToRemove++;
                    }
                    if (foundSeason)
                    {

                        wordsList.RemoveAt(indexToRemove);
                    }

                }
               
                foreach (string word in wordsToRemove)
                {
                    try
                    {
                        wordsList.Remove(word);
                    }
                    catch
                    {

                    }
                }

                for (int i = 0; i < wordsList.Count; i++)
                {
                    if (Array.IndexOf(Splitters, wordsList[i]) == -1)
                    {
                        if (wordsList[i].Any(c => char.IsDigit(c)))
                        {

                            string numberword = wordsList[i];

                            if (numberword.Contains('v'))
                            {
                                string[] numbers = wordsList[i].Split('v');
                                numberword = new string(numbers[0].Where(x => char.IsDigit(x)).ToArray());


                                remainingIntegers.Add(numberword, i);
                            }

                            if (numberword.Contains('S'))
                            {
                                remainingIntegers.Add(numberword, i);
                            }
                            else
                            {
                                if (i == 0 && !RomanNumbers.ContainsKey(numberword))
                                {
                                    remainingStrings.Add(numberword, i);
                                }
                                else if (wordsList[i].Length < 5)//>= 5 possibly crc32
                                {

                                    remainingIntegers.Add(numberword, i);
                                }
                                else if (wordsList[i].Length > 5)//>= 5 possibly crc32
                                {
                                    crc32.Add(wordsList[i], i);
                                }
                                else if (i == 0 && RomanNumbers.ContainsKey(wordsList[i]))
                                {
                                    int value = RomanNumbers[numberword];
                                    if (value != 1)
                                    {

                                        remainingIntegers.Add(numberword, i);
                                    }
                                    else
                                    {
                                        remainingStrings.Add(numberword, i);
                                    }
                                }
                            }

                         
                        }
                        else
                        {
                            if (wordsList[i].Trim().Length >= 1 && !wordsList[i].Contains(" "))
                            {
                                if (!RomanNumbers.ContainsKey(wordsList[i]))
                                {
                                    remainingStrings.Add(wordsList[i], i);
                                }
                                else
                                {
                                    int value = RomanNumbers[wordsList[i]];
                                    if (value != 1)
                                    {
                                        if (wordsList[i].Contains('v'))
                                        {
                                            string[] numbers = wordsList[i].Split('v');
                                            remainingIntegers.Add(numbers[0], i);
                                        }
                                        else
                                        {
                                            remainingIntegers.Add(wordsList[i], i);
                                        }
                                    }
                                    else
                                    {
                                        remainingStrings.Add(wordsList[i], i);
                                    }
                                }
                            }
                        }
                    }
                }



                if (resultparser.Key == "notencapsulated")
                {
                    int indexcounter = 0;
                    foreach (KeyValuePair<string, int> remainingInteger in remainingIntegers)
                    {
                        if (remainingIntegers.Count == 1)
                        {
                            if (remainingInteger.Key.ToUpper().Contains("S") && remainingInteger.Key.ToUpper().Contains('E'))
                            {
                                string[] splitted = remainingInteger.Key.Split('E');
                                string episodestring = splitted[1];
                                string seasonstring = splitted[0].Substring(1);

                                seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());
                                episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                episode.Add(episodestring, remainingInteger.Value);

                                if (!foundSeason)
                                {
                                    season.Add(seasonstring, remainingInteger.Value);
                                }
                            }
                            else if (remainingInteger.Key.ToUpper().Contains("S"))
                            {
                                string seasonstring = remainingInteger.Key.Substring(1);
                                seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());

                                if (!foundSeason)
                                {
                                    season.Add(seasonstring, remainingInteger.Value);
                                }
                            }
                            else if (remainingInteger.Key.ToUpper().Contains("S"))
                            {
                                string episodestring = remainingInteger.Key.Substring(1);
                                episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                episode.Add(episodestring, remainingInteger.Value);
                            }
                            else
                            {
                                string episodestring = new string(remainingInteger.Key.Where(x => char.IsDigit(x)).ToArray());
                                episode.Add(episodestring, remainingInteger.Value);
                            }

                        }
                        else
                        {
                            string numbercontainingstring = remainingInteger.Key;

                            if (remainingIntegers.Count == 2 && indexcounter > 0)
                            {
                                if (remainingInteger.Key.ToUpper().Contains("S") && remainingInteger.Key.ToUpper().Contains('E'))
                                {
                                    string[] splitted = remainingInteger.Key.Split('E');
                                    string episodestring = splitted[1];
                                    string seasonstring = splitted[0].Substring(1);
                                    seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());
                                    episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                    episode.Add(episodestring, remainingInteger.Value);

                                    if (!foundSeason)
                                    {
                                        season.Add(seasonstring, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                }else if (remainingInteger.Key.ToUpper().Contains("S"))
                                {
                                    string seasonstring = remainingInteger.Key.Substring(1);
                                    seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());

                                    if (!foundSeason)
                                    {
                                        season.Add(seasonstring, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                } else  if (remainingInteger.Key.ToUpper().Contains('E'))
                                {
                                    string episodestring = remainingInteger.Key.Substring(1);
                                    episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                    episode.Add(episodestring, remainingInteger.Value);
                                }
                                else
                                {
                                    string episodestring = new string(remainingInteger.Key.Where(x => char.IsDigit(x)).ToArray());
                                    episode.Add(episodestring, remainingInteger.Value);
                                }
                            }
                            else if (remainingIntegers.Count == 2 && indexcounter == 0)
                            {
                                if (remainingInteger.Key.ToUpper().Contains("S") && remainingInteger.Key.ToUpper().Contains('E'))
                                {
                                    string[] splitted = remainingInteger.Key.Split('E');
                                    string episodestring = splitted[1];
                                    string seasonstring = splitted[0].Substring(1);
                                    seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());
                                    episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                    episode.Add(episodestring, remainingInteger.Value);

                                    if (!foundSeason)
                                    {
                                        season.Add(seasonstring, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                }
                                else if (remainingInteger.Key.ToUpper().Contains("S"))
                                {
                                    string seasonstring = remainingInteger.Key.Split('S')[1];

                                    if (!foundSeason)
                                    {
                                        season.Add(seasonstring, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                }
                                else if (remainingInteger.Key.ToUpper().Contains('E'))
                                {
                                    string episodestring = remainingInteger.Key.Substring(1);
                                    episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                    episode.Add(episodestring, remainingInteger.Value);
                                }
                                else
                                {
                                    if (!foundSeason)
                                    {
                                        string seasonstring = new string(remainingInteger.Key.Where(x => char.IsDigit(x)).ToArray());
                                        season.Add(remainingInteger.Key, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                    remainingStrings.Add(remainingInteger.Key, remainingInteger.Value);
                                }
                            }
                            else
                            {
                                if (remainingIntegers.Count == 3 && indexcounter == 1)
                                {
                                    if (!foundSeason)
                                    {
                                        string seasonstring = new string(remainingInteger.Key.Where(x => char.IsDigit(x)).ToArray());
                                        season.Add(remainingInteger.Key, remainingInteger.Value);
                                        foundSeason = true;
                                    }
                                }
                                else if (remainingIntegers.Count == 3 && indexcounter == 2)
                                {
                                    if (remainingInteger.Key.ToUpper().Contains("S") && remainingInteger.Key.ToUpper().Contains('E'))
                                    {
                                        string[] splitted = remainingInteger.Key.Split('E');
                                        string episodestring = splitted[1];
                                        string seasonstring = splitted[0].Substring(1);
                                        seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());
                                        episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                        episode.Add(episodestring, remainingInteger.Value);
                                        if (!foundSeason)
                                        {
                                            season.Add(seasonstring, remainingInteger.Value);
                                            foundSeason = true;
                                        }
                                    }
                                    else if (remainingInteger.Key.ToUpper().Contains("S"))
                                    {
                                        string seasonstring = remainingInteger.Key.Substring(1);
                                        seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());

                                        if (!foundSeason)
                                        {
                                            season.Add(seasonstring, remainingInteger.Value);
                                            foundSeason = true;
                                        }
                                    }
                                    else if (remainingInteger.Key.ToUpper().Contains("S"))
                                    {
                                        string episodestring = remainingInteger.Key.Substring(1);

                                        episodestring = new string(episodestring.Where(x => char.IsDigit(x)).ToArray());
                                        episode.Add(episodestring, remainingInteger.Value);
                                    }
                                    else
                                    {
                                        string episodestring = new string(remainingInteger.Key.Where(x => char.IsDigit(x)).ToArray());
                                        episode.Add(episodestring, remainingInteger.Value);
                                    }
                                }
                            }
                        }

                        indexcounter++;
                    }



                    Dictionary<string, int> title = new Dictionary<string, int>();

                    string combined = "";
                    foreach (KeyValuePair<string, int> wordremaining in remainingStrings)
                    {
                        combined += wordremaining.Key + " ";
                    }
                    title.Add(combined, 0);
                    if (episode.Count == 0)
                    {
                        episode.Add("-1", 0);
                    }
                    

                    dictionaryResult.Add("Title", title);
                    dictionaryResult.Add("Episode", episode);

                    if (dictionaryResult.ContainsKey("Season"))
                    {
                        if (season.Count != 0)
                        {
                            dictionaryResult["Season"] = season;
                        }
                    }
                    else
                    {
                        if (season.Count == 0)
                        {
                            season.Add("0", 0);
                        }
                        dictionaryResult.Add("Season", season);
                    }
                    //dictionaryResult.Add("Remaining_Words_Not_Encapsulated", remainingStrings);
                    //dictionaryResult.Add("Remaining_Numbers_Not_Encapsulatedd", remainingIntegers);

                }
                else
                {
                    Dictionary<string, int> possibleReleaseGroup = new Dictionary<string, int>();
                    int indexcounter = 0;

                    string combinedSubGroupName = "";
                    foreach (KeyValuePair<string, int> wordremaining in remainingStrings)
                    {

                        combinedSubGroupName += wordremaining.Key + " ";
                        indexcounter++;
                    }

                    possibleReleaseGroup.Add(combinedSubGroupName, 0);
                    dictionaryResult.Add("Release_Group", possibleReleaseGroup);
                    dictionaryResult.Add("CRC32", crc32);

                    if (!foundSeason)
                    {
                        foreach (string word in wordsList)
                        {
                            if (word.Any(c => char.IsDigit(c))){
                                if (word.Contains("S"))
                                {
                                    if (word.Contains("E"))
                                    {
                                        string seasonstring = word.Split('S')[1].Split('E')[0];
                                        seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());

                                        season.Add(seasonstring, 0);
                                        foundSeason = true;
                                    }
                                    else
                                    {
                                        string seasonstring = word.Split('S')[1];
                                        seasonstring = new string(seasonstring.Where(x => char.IsDigit(x)).ToArray());

                                        season.Add(seasonstring, 0);
                                        foundSeason = true;
                                    }
                                }
                            }
                           
                        }

                        if (season.Count == 0)
                        {
                            season.Add("0", 0);
                        }


                        if (dictionaryResult.ContainsKey("Season"))
                        {
                            dictionaryResult["Season"] = season;
                        }
                        else
                        {
                            dictionaryResult.Add("Season", season);
                        }
                    }


                    //dictionaryResult.Add("Remaining_Words_Encapsulated", remainingStrings);
                    //dictionaryResult.Add("Remaining_Numbers_Encapsulated", remainingIntegers);
                }


              

            }

            return dictionaryResult;

        }
    }
}
