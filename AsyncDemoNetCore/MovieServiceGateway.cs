using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncDemoNetCore
{
    public static class MovieServiceGateway
    {
        private static readonly string[] sources =
        {
            "https://matlusstorage.blob.core.windows.net/membervideos/action.json",
            "https://matlusstorage.blob.core.windows.net/membervideos/drama.json",
            "https://matlusstorage.blob.core.windows.net/membervideos/thriller.json",
            "https://matlusstorage.blob.core.windows.net/membervideos/scifi.json",
        };

        public static IEnumerable<IEnumerable<Movie>> DownloadData()
        {
            var allMovies = new List<IEnumerable<Movie>>();

            foreach (var url in sources)
            {
                allMovies.Add(DownloadMovies(url));
            }

            return allMovies;
        }

        public static Task<IEnumerable<Movie>[]> DownloadDataAsync(HttpClient httpClient)
        {
            var allMoviesTasks = new List<Task<IEnumerable<Movie>>>();

            foreach (var url in sources)
            {
                allMoviesTasks.Add(DownloadMoviesAsync(httpClient, url));
            }

            return Task.WhenAll(allMoviesTasks);
        }

        public static IEnumerable<IEnumerable<Movie>> DownloadDataParallel()
        {
            var allMovies = new List<IEnumerable<Movie>>();

            Parallel.ForEach(sources, url =>
            {
                allMovies.Add(DownloadMovies(url));
            });

            return allMovies;
        }

        private static IEnumerable<Movie> DownloadMovies(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = httpClient.GetAsync(url).GetAwaiter().GetResult();
                httpResponseMessage.EnsureSuccessStatusCode();
                return httpResponseMessage.Content.ReadAsAsync<IEnumerable<Movie>>().GetAwaiter().GetResult();
            }
        }

        private static async Task<IEnumerable<Movie>> DownloadMoviesAsync(HttpClient httpClient, string url)
        {
            var httpResponseMessage = await httpClient.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsAsync<IEnumerable<Movie>>();
        }
    }
}
