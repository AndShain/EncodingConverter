using System.Text;

namespace EncodingConverter
{
    public class Program
    {
        static void Main(string[] args)
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
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}