using System.Text;

namespace EncodingConverter
{
    public class Converter
    {
        public static string ConvertTextEncoding(Encoding sourceEncoding, Encoding destinationEncoding, string text)
        {
            if (sourceEncoding.Equals(destinationEncoding))
            {
                return text;
            }
            return destinationEncoding.GetString(Encoding.Convert(sourceEncoding, destinationEncoding, sourceEncoding.GetBytes(text)));
        }

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
            return Encoding.Default; // Заглушка, нужно бросать исключение
        }
    }
}
