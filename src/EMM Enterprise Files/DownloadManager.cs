using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 MIT License

Copyright (c) 2019 Pedro Jesus

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

https://github.com/pictos/DownloadManager
*/

namespace EMM_Enterprise_Files
{
    public static class Extentions
    {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static bool IsNullOrWhiteSpaces(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool IsValidString(this string s) => !(IsNullOrEmpty(s) && IsNullOrWhiteSpaces(s));
    }

    public class DownloadManager
    {
        public HttpClient Client = new HttpClient();

        public void UseCustomHttpClient(HttpClient client)
        {
            if (client is null)
                throw new ArgumentNullException($"The {nameof(client)} can't be null.");

           Client.Dispose();
           Client = null;
           Client = client;
        }

        public async Task<string> DownloadAsync(
            string file,
            string url,
            IProgress<double> progress = default(IProgress<double>),
            int bufferSize = 2048,
            CancellationToken token = default(CancellationToken))
        {
            if (!(file.IsValidString() && url.IsValidString()))
                throw new ArgumentNullException($"the {nameof(file)} and {nameof(url)} parameters can't be null.");

            //TODO colocar isso dentro de alguma pasta
            //var path = PlataformFolder();

            using (var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error in download: {response.StatusCode}");

                var total = response.Content.Headers.ContentLength ?? -1L;
                
                using (var streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    var totalRead = 0L;
                    var buffer = new byte[bufferSize];
                    var isMoreToRead = true;
                    var fileWriteTo = file;//Path.Combine(path, file);
                    var output = new FileStream(fileWriteTo, FileMode.Create);
                    do
                    {
                        token.ThrowIfCancellationRequested();

                        var read = await streamToReadFrom.ReadAsync(buffer, 0, buffer.Length, token);

                        if (read == 0)
                            isMoreToRead = false;

                        else
                        {
                            await output.WriteAsync(buffer, 0, read);

                            totalRead += read;

#if DEBUG
                            Android.Util.Log.Debug("DownloadManager", $"Total read {totalRead}");
#endif
                            double progressToReport = (totalRead * 1d) / (total * 1d);
#if DEBUG
                            Android.Util.Log.Debug("DownloadManager", $"Progress {progressToReport}");
#endif
                            progress.Report(progressToReport);
                        }

                    } while (isMoreToRead);
                    progress.Report(1.0);
                    output.Close();
                    return fileWriteTo;
                }
            }
        }
    }
}
