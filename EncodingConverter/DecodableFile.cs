using System.Text;

namespace EncodingConverter
{
    public class DecodableFile
    {
        // Кодировка файла
        public Encoding Encoding { get; set; } = Encoding.Default;

        // Текст файла в текущей кодировке
        public string Text { get; set; } = string.Empty;

        // Флаг, который показывает, менялась кодировка или нет
        public bool EncodingСhanged { get; set; } = false;

        public DecodableFile(Encoding encoding, string text)
        {
            Encoding = encoding;
            Text = text;
        }
    }
}
