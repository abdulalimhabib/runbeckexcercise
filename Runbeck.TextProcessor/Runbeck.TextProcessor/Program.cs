using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runbeck.TextProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;


            Console.WriteLine("Welcome to Runbeck Text Processor!");

            while (isRunning)
            {
                Console.WriteLine(
                    "**************************************" + Environment.NewLine
                    + "Please select an option: " + Environment.NewLine
                    + "1- Start the Text Processor tool" + Environment.NewLine
                    + "2- Quit application" + Environment.NewLine
                    + "**************************************");

                switch (Console.ReadLine())
                {
                    case "1":
                        try
                        {
                            var textProcessor = new TextProcessor(
                                GetUserInput("What is the full path of the file to process?"),
                                GetUserInput("What is the file fomat? (c for csv or t for tsv)"),
                                GetUserInput("How many fields should each record contain?"));

                            textProcessor.Run();

                            Console.WriteLine("SUCCESS!!");

                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(string.Format("Failed to process file. {0}", exception.Message));
                        }
                        break;
                    case "2":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option entered.");
                        break;
                }

                Console.WriteLine();

            }
        }

        private static string GetUserInput(string instructions)
        {
            while (true)
            {
                Console.WriteLine(instructions);

                string response = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(response))
                {
                    return response;
                }
                else
                {
                    Console.WriteLine("You must enter a response.");
                }
            }
        }
    }
}
