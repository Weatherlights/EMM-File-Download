using Java.Net;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    internal class SyncBackground : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var timer = new Timer(async =>
                {
                    try
                    {
                        Task.Run(() =>
                        {
                            EMMFileSync eMMFileSync = new EMMFileSync();
                            List<EMMFile> eMMFiles = eMMFileSync.GetEMMFileData();
                            foreach (var file in eMMFiles)
                            {
                                Progress<double> progress = new Progress<double>();
                                DownloadManager.DownloadAsync(file.Path, file.URL, progress);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);

                        // SentrySdk.CaptureException(ex);

                        // await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    }
                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            }
        }

    }
}
