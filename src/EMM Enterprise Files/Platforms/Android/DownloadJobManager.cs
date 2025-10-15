using Android.App;
using Android.Content;
using Android.Widget;
using AndroidX.Core.App;
using Dalvik.SystemInterop;
using EMM_Enterprise_Files.Platforms;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Android.Provider.ContactsContract;
using static System.Net.Mime.MediaTypeNames;

namespace EMM_Enterprise_Files
{
    partial class DownloadJobManager
    {
        private Context context = Platform.AppContext;
        private DownloadManager downloadManager = DownloadManager.FromContext(Platform.AppContext);
        public static Dictionary<string, long> downloadTracker = new Dictionary<string, long>();



        /*
        public partial async Task StartDownloadJobsAsync(Progress<Double> progress, IProgress<string> progressText)
        {
           //EMMFilesListView.IsEnabled = false;
            int i = 0;
            int maxi = PayloadEMMProfiles.Count;
            DownloadManager2 dm = new DownloadManager2();
            if (maxi > 0)
            {
                foreach (var file in PayloadEMMProfiles)
                {
                    try
                    {
                        //string temporaryFilePath = DownloadJobManager.GetTemporaryFileLocation();
#if DEBUG
                        Android.Util.Log.Debug("DownloadJobManager", $"Downloading {file.URL} to {file.Path}.");
#endif
                        progressText.Report($"Downloading {file.Name} ({i + 1}/{maxi})");
                        //Label.Text = $"Downloading {file.Name} ({i}/{maxi})";

                        var text = $"Downloading {file.Name}...";


                        dm.DownloadAsync(file.Path, file.URL, progress, BufferSize);

                        if (file.IsCompliant == compliancestate.NonCompliant) // file has wrong hash
                        {
                            File.Delete(file.Path);
                        }
                        i++;
#if DEBUG
                        Android.Util.Log.Debug("DownloadJobManager", $"Download completed.");
#endif

                    }
                    catch (Exception e)
                    {
#if DEBUG
                        Android.Util.Log.Debug("DownloadJobManager", $"Download failed {e.HResult}: {e.Message}");
#endif
                        progressText.Report($"Download failed {e.HResult}: {e.Message}");
                    }

                }
                
            }
            PayloadEMMFile.Clear();

            //EMMFilesListView.IsEnabled = true;
        }*/



    }
}
