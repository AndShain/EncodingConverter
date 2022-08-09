using System;
using System.Text;

namespace EncodingConverter
{
    // Проводит операции с кодировками файлов
    public class Converter
    {
        // Преобразует текст DecodableFile из исходной кодировки в требуемую 
        public static DecodableFile ConvertTextEncoding(DecodableFile decodableFile, Encoding destinationEncoding)
        {
            // Если исходная кодировка является требуемой, возвращается DecodableFile с исходным текстом
            if (decodableFile.Encoding.Equals(destinationEncoding))
            {
                decodableFile.EncodingСhanged = false;
                return decodableFile;
            }

            // Производит конвертацию строки в исходной кодировке в массив байтов
            byte[] sourceTextInBytes = decodableFile.Encoding.GetBytes(decodableFile.Text);

            // Преобразует массив байтов в исходной кодировке в массив байтов с требуемой кодировкой.
            decodableFile.Text = destinationEncoding.GetString(Encoding.Convert(decodableFile.Encoding, destinationEncoding, sourceTextInBytes));

            // Записываем факт наличия изменений в исходной кодировке
            decodableFile.EncodingСhanged = true;

            // Возвращает DecodableFile с текстом в требуемой кодировке
            return decodableFile;
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
