using System.Threading.Tasks;

namespace SiteCrawler.Core.Interfaces
{
    public interface IStorageService
    {
        Task SaveStringAsync(string content, string contentName, string path);
    }
}
