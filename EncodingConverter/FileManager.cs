using System.Text;
using System.Xml;

namespace EncodingConverter
{
    public class FileManager
    {
        private string DirectoryPath { get; set; } = string.Empty;
        private string BackupDirectoryPath { get; set; } = string.Empty;
        private Encoding SourceEncoding { get; set; } = Encoding.Default;
        private Encoding DestinationEncoding { get; set; } = Encoding.Default;
        private List<string> Extensions { get; set; } = new List<string>();
        private List<string> FileNames { get; set; } = new List<string>();

        public FileManager()
        {
            ReadSettingsFromXml();
        }
        public FileManager(string directoryPath, string[] extensions, Encoding sourceEncoding, Encoding destinationEncoding)
        {
            DirectoryPath = directoryPath;
            Extensions = extensions.ToList();
            SourceEncoding = sourceEncoding;
            DestinationEncoding = destinationEncoding;
            BackupDirectoryPath = directoryPath;
        }

        public FileManager(string directoryPath, string[] extensions, Encoding sourceEncoding, Encoding destinationEncoding, string backupDirectoryPath)
            : this(directoryPath, extensions, sourceEncoding, destinationEncoding)
        {
            BackupDirectoryPath = backupDirectoryPath;
        }

        private void GetFileNamesWithExtension(string directory, string extension)
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

        private string CreatePathToBackupDirectory(string fileName)
        {
            return fileName.Replace(DirectoryPath, BackupDirectoryPath);
        }

        private void ReadSettingsFromXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("settings.xml");
            XmlElement? xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null)
            {
                foreach (XmlElement xmlNode in xmlRoot)
                {
                    if (xmlNode.Name == "directoryPath")
                    {
                        DirectoryPath = xmlNode.InnerText;
                    }
                    if (xmlNode.Name == "backupDirectoryPath")
                    {
                        BackupDirectoryPath = xmlNode.InnerText;
                    }
                    if (xmlNode.Name == "sourceEncoding")
                    {
                        SourceEncoding = Converter.ConverTextToEncoding(xmlNode.InnerText);
                    }
                    if (xmlNode.Name == "destinationEncoding")
                    {
                        DestinationEncoding = Converter.ConverTextToEncoding(xmlNode.InnerText);
                    }
                    if (xmlNode.Name == "extensions")
                    {
                        foreach (XmlNode childNode in xmlNode.ChildNodes)
                        {
                            if (childNode.Name == "extension")
                            {
                                Extensions.Add(childNode.InnerText);
                            }
                        }
                    }
                }
            }
        }
    }
}
