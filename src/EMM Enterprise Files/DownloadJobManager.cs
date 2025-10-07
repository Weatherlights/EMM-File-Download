using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    internal class DownloadJobManager
    {

        List<EMMFile> PayloadEMMFile = new List<EMMFile>();
        public Label Label { get; set; }
        public ListView EMMFilesListView { get; set; }
        public ProgressBar DownloadProgressBar { get; set; }

        public DownloadJobManager() { }

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

        public async Task StartDownloadJobsAsync(Progress<Double> progress)
        {
           //EMMFilesListView.IsEnabled = false;
            int i = 0;
            int maxi = PayloadEMMFile.Count;
            
           foreach (var file in PayloadEMMFile)
            {
                try
                {
                    //string temporaryFilePath = DownloadJobManager.GetTemporaryFileLocation();
                    Android.Util.Log.Debug("DownloadJobManager", $"Downloading {file.URL} to {file.Path}.");
                    //Label.Text = $"Downloading {file.Name} ({i}/{maxi})";

                    await DownloadManager.DownloadAsync(file.Path, file.URL, progress);
                    if (file.IsCompliant == EMMFile.compliancestate.NonCompliant) // file has wrong hash
                    {
                        File.Delete(file.Path);  
                    }
                    i++;
                    Android.Util.Log.Debug("DownloadJobManager", $"Download completed.");
                    //Label.Text = $"Finished {file.Name}";
                }
                catch (Exception e)
                {
                    Android.Util.Log.Debug("DownloadJobManager", $"Download failed {e.HResult}: {e.Message}");
                    //          Label.Text += $"{e.HResult}: {e.Message}";
                }
                
            }
           PayloadEMMFile.Clear();

            //EMMFilesListView.IsEnabled = true;
        }
    }
}
