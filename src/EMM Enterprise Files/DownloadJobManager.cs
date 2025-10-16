using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;

namespace EMM_Enterprise_Files
{
    public partial class DownloadJobManager
    {

        List<IEMMProfile> PayloadEMMProfiles = new List<IEMMProfile>();
        public Label Label { get; set; }
        public ListView EMMFilesListView { get; set; }
        public ProgressBar DownloadProgressBar { get; set; }
        private int BufferSize = 2048;
        //private DownloadManager DownloadManager = new DownloadManager();


        public void AddDownloadJob(IEMMProfile profile)
        {
            if (profile is null)
                throw new ArgumentNullException($"The {nameof(profile)} can't be null.");

            PayloadEMMProfiles.Add(profile);
        }



        public static string GetTemporaryFileLocation()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.GetRandomFileName());//address
            return fileName;
        }

        public async Task StartDownloadJobsAsync()
        {
            //EMMFilesListView.IsEnabled = false;
            int i = 0;
            int maxi = PayloadEMMProfiles.Count;
            if (maxi > 0)
            {
                //progressText.Report("Syncing files");
                foreach (var profile in PayloadEMMProfiles)
                {
                    try
                    {
                        //string temporaryFilePath = DownloadJobManager.GetTemporaryFileLocation();
                        profile.InitializeFileEnforcement();
                        i++;

                    }
                    catch (Exception e)
                    {
                        //progressText.Report($"Download failed {e.HResult}: {e.Message}");
                    }

                }

            }
            PayloadEMMProfiles.Clear();

            //EMMFilesListView.IsEnabled = true;
        }

        //public partial Task StartDownloadJobsAsync(Progress<Double> progress, IProgress<string> progressText);
        public void StartDownloadJobs()
        {
            //EMMFilesListView.IsEnabled = false;
            int i = 0;
            int maxi = PayloadEMMProfiles.Count;
            if (maxi > 0)
            {
                //progressText.Report("Syncing files");
                foreach (var profile in PayloadEMMProfiles)
                {
                    try
                    {
                        //string temporaryFilePath = DownloadJobManager.GetTemporaryFileLocation();
                        profile.InitializeFileEnforcement();
                        i++;

                    }
                    catch (Exception e)
                    {
                        //progressText.Report($"Download failed {e.HResult}: {e.Message}");
                    }

                }

            }
            PayloadEMMProfiles.Clear();

            //EMMFilesListView.IsEnabled = true;
        }
    }
}
