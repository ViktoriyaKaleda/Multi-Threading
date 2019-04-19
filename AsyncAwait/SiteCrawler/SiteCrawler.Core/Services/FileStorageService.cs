using SiteCrawler.Core.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace SiteCrawler.Core.Services
{
    public class FileStorageService : IStorageService
    {
        public async Task SaveStringAsync(string content, string name, string filePath)
        {
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string resultPath = Path.Combine(filePath, NormalizeName(name));
            using (TextWriter writer = new StreamWriter(resultPath))
            {
                await writer.WriteLineAsync(content);
            }
        }

        private string NormalizeName(string name)
        {
            name = name
                .Replace('<', '_')
                .Replace('>', '_')
                .Replace('*', '_')
                .Replace('|', '_')
                .Replace(':', '_')
                .Replace('?', '_')
                .Replace(@"//", "_")
                .Replace('/', '_')
                .Replace(@"\\", "_")
                .Replace(@"\", "_");

            if (name.Length > 255)
            {
                name = name.Substring(0, 250);
            }

            return name + ".html";
        }
    }
}
