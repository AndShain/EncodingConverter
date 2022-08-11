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

            string message = string.Format("{0}\nStart of log\n", DateTime.Now);
            Logger.WriteTextToLog(message);
            
            FileManager fileManager = new FileManager();

            // Если найдены файлы с требуемым расширением, меняем их кодировку
            if (fileManager.FilesWithSuchExtensionExsist())
            {
                fileManager.ChangeFilesEncoding();
            }
            else
            {
                message = "Files with such extensions not found\n";
                Logger.WriteTextToLog(message);
                Console.Write(message);
            }
            Logger.WriteTextToLog("End of log\n\n");
        }
    }
}