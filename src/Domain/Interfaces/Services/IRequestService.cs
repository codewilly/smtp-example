using System.IO;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IRequestService
    {
        Task<(Stream stream, string fileName)> DownloadAsStream(string url);
    }
}
