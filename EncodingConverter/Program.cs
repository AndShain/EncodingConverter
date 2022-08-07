using System.Text;

namespace EncodingConverter
{
    public class Program
    {
        static void Main(string[] args) // Кодировка исходного файла не определяется автоматически
                                        // Если у файла кодировка не соотвествует sourceEncoding, выходной файл повреждается(кириллица не отображается)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Encoding sourceEncoding = Encoding.GetEncoding(1251);
            Encoding destinationEncoding = Encoding.UTF8;
            string directoryPath = @"D:\TestUTF";
            string[] extensions = { "txt", "cpp" };
            string backupDirectoryPath = @"D:\TestUTF_copy";

            FileManager fileManager = new FileManager(directoryPath, extensions, sourceEncoding, destinationEncoding, backupDirectoryPath);
            if (fileManager.FilesWithSuchExtensionExsist())
            {
                fileManager.ChangeFilesEncoding();
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Files with such extensions not found");
            }
        }
    }
}