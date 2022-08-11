using System.IO;
using System.Text;

namespace EncodingConverter
{
    public class Logger
    {
        // Название файла для логирования
        private static string logFileName = "Logs.txt";

        // Записывает текст в файл для логирования
        public static void WriteTextToLog(string text)
        {
            using (StreamWriter sw = new StreamWriter(logFileName, true, Encoding.UTF8))
            {
                sw.Write(text);
            }
        }
    }
}
