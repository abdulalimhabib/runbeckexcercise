using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbeck.TextProcessor
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    bool isRunning = true;

        //    Console.WriteLine("Welcome to Runbeck Text Processor!");

        //    while (isRunning)
        //    {
        //        Console.WriteLine("Please select an option: " + Environment.NewLine
        //            + "1- Start the Text Processor tool" + Environment.NewLine
        //            + "2- Quit application");
        //        string inputFilePath;
        //        switch (Console.ReadLine())
        //        {
        //            case "1":
        //                Console.WriteLine("What is the full path of the file to process?");
        //                inputFilePath = Console.ReadLine();
        //                if (string.IsNullOrWhiteSpace(inputFilePath))
        //                {
        //                    Console.WriteLine("File path cannot be empty");
        //                }
        //                else if (!IsValidPath(inputFilePath))
        //                {
        //                    Console.WriteLine("Invalid path selected");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("What is the file fomat? (c for csv or t for tsv)");

        //                    string inputDelimiter = Console.ReadLine();

        //                    MyClass.DelimiterType? delimiterType = null;
        //                    switch (inputDelimiter.ToLower())
        //                    {
        //                        case "c":
        //                            delimiterType = MyClass.DelimiterType.Comma;
        //                            break;
        //                        case "t":
        //                            delimiterType = MyClass.DelimiterType.Tab;
        //                            break;
        //                        default:
        //                            Console.WriteLine("Invalid selection made.");
        //                            break;
        //                    }

        //                    if (delimiterType.HasValue)
        //                    {
        //                        Console.WriteLine("How many fields should each record contain?");

        //                        string inputFieldsCount = Console.ReadLine();


        //                        if (!int.TryParse(inputFieldsCount, out int fieldCount) || fieldCount == 0)
        //                        {
        //                            Console.WriteLine("Invalid field count entered. Please try again");
        //                        }
        //                        else
        //                        {
        //                            var textProcessor = new MyClass(inputFilePath, delimiterType.Value, fieldCount);

        //                            textProcessor.Run();
        //                        }

        //                    }

        //                }
        //                break;
        //            case "2":
        //                isRunning = false;
        //                break;
        //            default:
        //                Console.WriteLine("Invalid option entered.");
        //                break;
        //        }



        //    }
        //}
       
        static bool IsValidPath(string path)
        {
            try
            {
                return System.IO.File.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    class MyClass
    {
        #region " Enums "
        internal enum DelimiterType
        {
            Comma, Tab
        }
        private enum OutputType
        {
            Correct, Incorrect
        } 
        #endregion

        #region " Variables "
        private char delimiter; 
        #endregion

        #region " Constructors "
        public MyClass(string inputFilePath, DelimiterType delimiterType, int fieldsCount)
        {
            InputFilePath = inputFilePath;
            FieldsCount = fieldsCount;
            InitializeDelimiter(delimiterType);
            ValidateParameters();
        } 
        #endregion

        #region " Properties "
        public string InputFilePath { get; private set; }
        public DelimiterType Delimiter { get; private set; }
        public int FieldsCount { get; private set; }
        #endregion

        #region " Run "
        internal void Run()
        {
            try
            {
                if (TryGetFile(InputFilePath, out string[] fileContent))
                {
                    // We're skipping the first row as it contains headers
                    // which aren't relevant to this solution.
                    var correctFormatBuilder = new StringBuilder();
                    var incorrectFormatBuilder = new StringBuilder();

                    fileContent.Skip(1).ToList().ForEach(a =>
                    {
                        if (a.Split(delimiter).Length == FieldsCount)
                        {
                            correctFormatBuilder.AppendLine(a);
                        }
                        else
                        {
                            incorrectFormatBuilder.AppendLine(a);
                        }
                    });

                    CreateOutputFile(InputFilePath, correctFormatBuilder.ToString(), OutputType.Correct);
                    CreateOutputFile(InputFilePath, incorrectFormatBuilder.ToString(), OutputType.Incorrect);
                }
                else
                {
                    throw new Exception("Error: failed to read the supplied input file.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region " Functions "
        private void InitializeDelimiter(DelimiterType delimiterType)
        {
            switch (delimiterType)
            {
                case DelimiterType.Tab:
                    delimiter = '\t';
                    break;
                default:
                    delimiter = ',';
                    break;
            }
        }
        private void ValidateParameters()
        {
            try
            {
                ValidatePath(InputFilePath);
                ValidateFieldsCount(this.FieldsCount);
            }
            catch (Exception exception)
            {
                throw new Exception("Invalid parameter detected:" + exception.Message);
            }
        }
        private void ValidatePath(string path)
        {
            try
            {
                System.IO.Path.GetFullPath(path);
            }
            catch (Exception)
            {
                throw new Exception("Invalid file path entered.");
            }
        }
        private void ValidateFieldsCount(int fieldsCount)
        {
            if (fieldsCount == 0)
            {
                throw new Exception("Invalid field count entered. Field count must be a numeric value greater than 0.");
            }
        }
        private void CreateOutputFile(string basePath, string content, OutputType outputType)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    string outputFileName = outputType == OutputType.Correct
                        ? basePath + ".correct"
                        : basePath + ".incorrect";

                    System.IO.File.WriteAllText(outputFileName, content);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to create an output file");
                }
            }
        }
        private bool TryGetFile(string path, out string[] fileContent)
        {
            fileContent = null;
            try
            {
                fileContent = System.IO.File.ReadAllLines(path);
                return fileContent.Any();
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion
    }
}
