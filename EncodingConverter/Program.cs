using System.Text;

namespace EncodingConverter
{
    public class Program
    {
        static void Main(string[] args) // Кодировка исходного файла не определяется автоматически
                                        // Если у файла кодировка не соотвествует sourceEncoding, выходной файл повреждается(кириллица не отображается)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            FileManager fileManager = new FileManager();
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