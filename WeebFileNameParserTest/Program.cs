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




                Dictionary<string, string> resultofprocessing = filenameParser.ParseFullString(filename);

                Console.WriteLine("--------------RESULT---------------");

                foreach (KeyValuePair<string, string> resultparser in resultofprocessing)
                {
                    Console.WriteLine(resultparser.Key + ":" + resultparser.Value);
                }


                Console.ReadLine();
            }
        }

      


     

       
    }
}
