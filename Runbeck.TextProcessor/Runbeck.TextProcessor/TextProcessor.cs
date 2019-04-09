using System;
using System.Linq;
using System.Text;

namespace Runbeck.TextProcessor
{
    public class TextProcessor
    {
        #region " Enums "
        private enum OutputType
        {
            Correct, Incorrect
        }
        #endregion

        #region " Constructors "
        public TextProcessor(string inputFilePath, string inputDelimiter, string inputFieldsCount)
        {
            InitializeFilePath(inputFilePath);
            InitializeDelimiter(inputDelimiter);
            InitializeFieldsCount(inputFieldsCount);
        }
        #endregion

        #region " Properties "
        public string InputFilePath { get; private set; }
        public char Delimiter { get; private set; }
        public int FieldsCount { get; private set; }
        #endregion

        #region " Run "
        public void Run()
        {
            try
            {
                if (TryGetFile(InputFilePath, out string[] fileContent))
                {
                    var correctFormatBuilder = new StringBuilder();
                    var incorrectFormatBuilder = new StringBuilder();

                    // skipping the first line as it contains the headers which aren't relevant
                    fileContent.Skip(1).ToList().ForEach(a =>
                    {
                        if (a.Split(Delimiter).Length == FieldsCount)
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

        #region " Initializers "
        private void InitializeFieldsCount(string inputFieldsCount)
        {
            if (int.TryParse(inputFieldsCount.Trim(), out int fieldsCount))
            {
                FieldsCount = fieldsCount;
            }
            else
            {
                throw new Exception("Invalid field count entered.");
            }
        }

        private void InitializeDelimiter(string inputDelimiter)
        {
            switch (inputDelimiter.Trim().ToLower())
            {
                case "c":
                    Delimiter = ',';
                    break;
                case "t":
                    Delimiter = '\t';
                    break;
                default:
                    throw new Exception("Invalid delimiter entered.");
            }
        }

        private void InitializeFilePath(string inputFilePath)
        {
            try
            {
                if (!IsValidPath(inputFilePath))
                {
                    throw new Exception("Input file path entered is invalid.");
                }
                if (!System.IO.File.Exists(inputFilePath))
                {
                    throw new Exception("Input file entered does not exist.");
                }
                else
                {
                    InputFilePath = inputFilePath;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Failed to initialize File Path. {0}", exception.Message));
            }
        }
        #endregion

        #region " Functions "
        
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
        private bool IsValidPath(string path)
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
        #endregion
    }
}
