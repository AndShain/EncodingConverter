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
    }
}
