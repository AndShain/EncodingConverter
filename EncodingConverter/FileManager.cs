using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using UtfUnknown;
using System;

namespace EncodingConverter
{
    // Файловый менеджер. Ищет нужные файлы, считывает и записывает их.
    public class FileManager
    {
        // Директория, где будет происходить поиск поддиректорий и файлов с требуемым расширением
        private string DirectoryPath { get; set; } = string.Empty;

        // Кодировка, в которую будут преобразованы исходные файлы
        private Encoding DestinationEncoding { get; set; } = Encoding.UTF8;

        // Список расширений, файлы которых нужно найти
        private List<string> Extensions { get; set; } = new List<string>();

        // Список найденных полных имен файлов с требуемыми расширениями в их текстовом представлении
        private List<string> FileNames { get; set; } = new List<string>();

        // Конструктор без параметров вызывает метод, который получает настройки или задает настройки по умолчанию из xml файла
        public FileManager()
        {
            InitializeSittingsFile();
        }

        // Конструктор, который принимает все параметры. Может быть использован, когда поля инициализируются вручную
        public FileManager(string directoryPath, string[] extensions, Encoding destinationEncoding)
        {
            DirectoryPath = directoryPath;
            Extensions = extensions.ToList();
            DestinationEncoding = destinationEncoding;
        }

        // Выполняет поиск файлов с требуемым расширением и записывает строковое представление их имен в список
        private void GetFileNamesWithExtension(string directory, string extension)
        {
            // Выполняет поиск файлов в текущей директории и записывает их в список
            foreach (string fileName in Directory.GetFiles(directory, "*." + extension)) // Нужно обработать исключение, когда указанный в настройках каталог не существует
            {
                FileNames.Add(fileName);
            }

            // Выполняет поиск поддиректорий в текущей директории и ищет файлы с нужным расширением в этой поддиректории
            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                GetFileNamesWithExtension(subdirectory, extension);
            }
        }

        // Проверяет, были ли найдены файлы с требуемым расширением
        public bool FilesWithSuchExtensionExsist()
        {
            foreach(string extension in Extensions)
            {
                GetFileNamesWithExtension(DirectoryPath, extension);
            }
            // Если список файлов пуст, возращает false
            return FileNames.Count > 0;
        }

        // Записывает текст из строки в файл. Если файл уже существует, он будет перезаписан
        public void WriteTextToFile(Encoding fileEncoding, string fileName, string text)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, fileEncoding))
            {
                sw.Write(text);
            }
        }

        // Считывает весь текст из файла и возвращает его в виде строки
        public string ReadAllTextFromFile(Encoding fileEncoding, string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, fileEncoding))
            {
                return sr.ReadToEnd();
            }
        }

        // Меняет кодировку файла на заданную в DestinationEncoding
        public void ChangeFilesEncoding()
        {
            foreach (string fileName in FileNames)
            {
                // Определяет исходную кодировку файла
                DetectionResult? detectionResult = CharsetDetector.DetectFromFile(fileName);
                if (detectionResult.Detected == null)
                {
                    Console.WriteLine("Unable to determine encoding of {0} file", fileName);
                    continue;
                }
                DetectionDetail resultDetected = detectionResult.Detected;

                // Преобразует полученный результат в Encoding
                Encoding sourceEncoding = resultDetected.Encoding;

                // Преобразует исходную кодировку в заданную в DestinationEncoding, считывая текст из файла
                string decodedText = Converter.ConvertTextEncoding(sourceEncoding, DestinationEncoding, ReadAllTextFromFile(sourceEncoding, fileName));

                // Записывает строку, которая была преобразована в требуемую кодировку в файл, перезаписывая его содержимое
                WriteTextToFile(DestinationEncoding, fileName, decodedText);
            }
        }

        // Проверяет наличие файла настроек. Если файл отсутствует, создает файл с настройками по умолчанию
        private void InitializeSittingsFile()
        {
            if (File.Exists("settings.xml"))
            {
                ReadSettingsFromXml();
            }
            else
            {
                CreateSettingsXmlFile();
                ReadSettingsFromXml();
            }
        }

        // Создает xml файл с настройками по умолчанию
        private void CreateSettingsXmlFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlNode rootNode = xmlDoc.CreateElement("settings");
            xmlDoc.AppendChild(rootNode);

            XmlNode xmlNodePath = xmlDoc.CreateElement("directoryPath");

            // Получаем путь до исполняемого файла
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            xmlNodePath.AppendChild(xmlDoc.CreateTextNode(path));
            rootNode.AppendChild(xmlNodePath);

            XmlNode xmlNodeDestinationEncoding = xmlDoc.CreateElement("destinationEncoding");
            xmlNodeDestinationEncoding.AppendChild(xmlDoc.CreateTextNode("utf8"));
            rootNode.AppendChild(xmlNodeDestinationEncoding);

            XmlNode xmlNodeExtensions = xmlDoc.CreateElement("extensions");
            rootNode.AppendChild(xmlNodeExtensions);

            string[] defaultExtensions = { "cpp", "h" };
            foreach(string extension in defaultExtensions)
            {
                if (!Extensions.Contains(extension))
                {
                    XmlNode subNodeExtension = xmlDoc.CreateElement("extension");
                    subNodeExtension.AppendChild(xmlDoc.CreateTextNode(extension));
                    xmlNodeExtensions.AppendChild(subNodeExtension);
                }
            }
            xmlDoc.Save("settings.xml");
        }

        // Считывет настройки из xml файла
        private void ReadSettingsFromXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("settings.xml");
            XmlElement? xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null)
            {
                foreach (XmlElement? xmlNode in xmlRoot)
                {
                    if (xmlNode != null)
                    {
                        if (xmlNode.Name == "directoryPath")
                        {
                            DirectoryPath = xmlNode.InnerText;
                        }
                        if (xmlNode.Name == "destinationEncoding")
                        {
                            DestinationEncoding = Converter.ConverTextToEncoding(xmlNode.InnerText);
                        }
                        if (xmlNode.Name == "extensions")
                        {
                            foreach (XmlNode? childNode in xmlNode.ChildNodes)
                            {
                                if (childNode != null)
                                {
                                    if (childNode.Name == "extension")
                                    {
                                        if (!Extensions.Contains(childNode.InnerText))
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
        }
    }
}
