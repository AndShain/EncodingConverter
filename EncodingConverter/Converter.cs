using System.Text;

namespace EncodingConverter
{
    // Проводит операции с кодировками файлов
    public class Converter
    {
        // Преобразует текст из исходной кодировки в требуемую 
        public static string ConvertTextEncoding(Encoding sourceEncoding, Encoding destinationEncoding, string text)
        {
            // Если исходная кодировка является требуемой, возвращается исходный текст
            if (sourceEncoding.Equals(destinationEncoding))
            {
                return text;
            }

            // Производит конвертацию строки в исходной кодировке в массив байтов
            byte[] sourceTextInBytes = sourceEncoding.GetBytes(text);
            // Преобразует массив байтов в исходной кодировке в массив байтов с требуемой кодировкой.
            // Возвращает строку в требуемой кодировке
            return destinationEncoding.GetString(Encoding.Convert(sourceEncoding, destinationEncoding, sourceTextInBytes));
        }

        // Возвращает Encoding, который соответстует текстовому представлению кодировки(текстовое представление берется из xml файла с настройками)
        public static Encoding ConverTextToEncoding(string text)
        {
            if (text.ToLower() == "utf8")
            {
                return Encoding.UTF8;
            }
            if (text.ToLower() == "1251")
            {
                return Encoding.GetEncoding(1251);
            }
            return Encoding.Default; // Заглушка. Если неправильно записать кодировку в настройках, программа должна сообщить пользователю об ошибке
        }
    }
}
