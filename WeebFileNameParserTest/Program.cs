using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WeebFileNameParserLibrary;

namespace ConsoleApp1
{
    class Program
    {

      

        static void Main(string[] args)
        {

            


            WeebFileNameParser filenameParser = new WeebFileNameParser();

            while (true)
            {

                Console.WriteLine("Episode Parser Test! Type Episode Name:");
                string filename = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Parsing Epidose File Name: " + filename);




                Dictionary<string, string[]> resultofprocessing = filenameParser.ProcessFileName(filename);

                
                foreach (KeyValuePair<string, string[]> resultparser in resultofprocessing)
                {

                    Console.WriteLine(resultparser.Key + ":");
                    foreach (string value in resultparser.Value)
                    {
                        Console.WriteLine("     " + value);
                    }

                }



                Dictionary<string, Dictionary<string, int>> perWord = filenameParser.ParseStaticWords(resultofprocessing);

                Console.WriteLine("--------------RESULT---------------");

                foreach (KeyValuePair<string, Dictionary<string, int>> resultparser in perWord)
                {

                    Console.WriteLine(resultparser.Key + ":");
                    foreach (KeyValuePair<string, int> value in resultparser.Value)
                    {
                        Console.WriteLine("     " + value.Key);
                    }

                }
                Console.ReadLine();
            }
        }

      


     

       
    }
}
