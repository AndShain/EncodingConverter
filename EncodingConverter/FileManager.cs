using System.Text;

namespace EncodingConverter
{
    public class FileManager
    {
        public string DirectoryPath { get; set; }
        public string BackupDirectoryPath { get; set; }
        public Encoding SourceEncoding { get; set; }
        public Encoding DestinationEncoding { get; set; }
        public string[] Extensions { get; set; }
        public List<string> FileNames { get; set; } = new List<string>();

        public FileManager(string directoryPath, string[] extensions, Encoding sourceEncoding, Encoding destinationEncoding)
        {
            DirectoryPath = directoryPath;
            Extensions = extensions;
            SourceEncoding = sourceEncoding;
            DestinationEncoding = destinationEncoding;
            BackupDirectoryPath = directoryPath;
        }

        public FileManager(string directoryPath, string[] extensions, Encoding sourceEncoding, Encoding destinationEncoding, string backupDirectoryPath)
            : this(directoryPath, extensions, sourceEncoding, destinationEncoding)
        {
            BackupDirectoryPath = backupDirectoryPath;
        }

        public void GetFileNamesWithExtension(string directory, string extension)
        {
            foreach (string fileName in Directory.GetFiles(directory, "*." + extension)) // Нужно обработать исключение, когда указанный в настройках каталог не существует
            {
                FileNames.Add(fileName);
            }
            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                GetFileNamesWithExtension(subdirectory, extension);
            }
        }

        public bool FilesWithSuchExtensionExsist()
        {
            foreach(string extension in Extensions)
            {
                GetFileNamesWithExtension(DirectoryPath, extension);
            }
            return FileNames.Count > 0;
        }

        public void WriteTextToFile(Encoding fileEncoding, string fileName, string text)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, fileEncoding))
            {
                sw.Write(text);
            }
        }

        public string ReadAllTextFromFile(Encoding fileEncoding, string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, fileEncoding))
            {
                return sr.ReadToEnd();
            }
        }

        public void ChangeFilesEncoding()
        {
            foreach (string fileName in FileNames)
            {
                string decodedText = Converter.ConvertTextEncoding(SourceEncoding, DestinationEncoding, ReadAllTextFromFile(SourceEncoding, fileName));
                string destinationFileName = CreatePathToBackupDirectory(fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFileName)); // Может вернуть null
                WriteTextToFile(DestinationEncoding, destinationFileName, decodedText);
            }
        }

        public string CreatePathToBackupDirectory(string fileName)
        {
            return fileName.Replace(DirectoryPath, BackupDirectoryPath);
        }
    }
}
