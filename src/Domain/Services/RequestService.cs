using Domain.Interfaces.Services;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RequestService : IRequestService
    {
        private IHttpClientFactory _clientFactory;

        public RequestService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<(Stream stream, string fileName)> DownloadAsStream(string url)
        {
            HttpClient _client = _clientFactory.CreateClient("download-file");

            HttpResponseMessage response = await _client.GetAsync(url);

            Stream content = await response.Content.ReadAsStreamAsync();

            string filename =
                response.Content
                        .Headers
                        .GetValues("Content-Disposition")
                        .Select(header =>
                            header.Substring(header.IndexOf("filename") + 9)
                                  .Replace("\"", "")
                                  .Split(";")
                                  .FirstOrDefault())
                        .FirstOrDefault();

            return (content, filename);
        }
    }
}
