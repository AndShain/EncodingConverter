using System;
using System.Text;

namespace EncodingConverter
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Делает доступными дополнительные кодировки символов, которые не поддерживаются по умолчанию
            // (Используется пакет System.Text.Encoding.CodePages)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            FileManager fileManager = new FileManager();

            // Если найдены файлы с требуемым расширением, меняем их кодировку
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