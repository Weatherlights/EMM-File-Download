using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;

namespace EMM_Enterprise_Files
{
    internal class DownloadJobManager
    {

        List<EMMFile> PayloadEMMFile = new List<EMMFile>();
        public Label Label { get; set; }
        public ListView EMMFilesListView { get; set; }
        public ProgressBar DownloadProgressBar { get; set; }
        private int BufferSize = 2048;
        private DownloadManager DownloadManager = new DownloadManager();


        public DownloadJobManager() { }
        public DownloadJobManager(int bufferSize)
        {
            this.BufferSize = bufferSize;
        }

        public void AddDownloadJob(EMMFile file)
        {
            if (file is null)
                throw new ArgumentNullException($"The {nameof(file)} can't be null.");

            PayloadEMMFile.Add(file);
        }

        public static string GetTemporaryFileLocation()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.GetRandomFileName());//address
            return fileName;
        }

        public async Task StartDownloadJobsAsync(Progress<Double> progress, IProgress<string> progressText)
        {
           //EMMFilesListView.IsEnabled = false;
            int i = 0;
            int maxi = PayloadEMMFile.Count;
            
           foreach (var file in PayloadEMMFile)
            {
                try
                {
                    //string temporaryFilePath = DownloadJobManager.GetTemporaryFileLocation();
# if DEBUG
                    Android.Util.Log.Debug("DownloadJobManager", $"Downloading {file.URL} to {file.Path}.");
#endif
                    progressText.Report($"Downloading {file.Name} ({i + 1}/{maxi})");
                    //Label.Text = $"Downloading {file.Name} ({i}/{maxi})";

                    await DownloadManager.DownloadAsync(file.Path, file.URL, progress, BufferSize);
                    
                    if (file.IsCompliant == EMMFile.compliancestate.NonCompliant) // file has wrong hash
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
                    progressText.Report($"Download failed {e.HResult}");
                }

            }
            progressText.Report($"Download completed.");
            PayloadEMMFile.Clear();

            //EMMFilesListView.IsEnabled = true;
        }
    }
}
