using SiteCrawler.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SiteCrawler.Core
{
    public class Crawler
    {
        private readonly IStorageService _storageService;

        public Crawler(IStorageService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException("Value can not be undefined.");
        }

        public async Task DownloadAsync(string filePath, List<string> links, CancellationToken cancellationToken)
        {
            foreach (var link in links)
            {
                GuardClauses.IsValidLink(link);
            }

            await Task.WhenAll(links.Select(link => DownloadPagesAsync(filePath, link, cancellationToken)));
        }

        private async Task DownloadPagesAsync(string filePath, string link, CancellationToken cancellationToken)
        {
            string htmlContent = await GetHtmlStringAsync(link, cancellationToken);
            if (htmlContent is null)
                return;

            await _storageService.SaveStringAsync(htmlContent, link, filePath);
        }

        private async Task<string> GetHtmlStringAsync(string uriLink, CancellationToken cancellationToken)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(uriLink, cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return null;
                try
                {
                    return await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
