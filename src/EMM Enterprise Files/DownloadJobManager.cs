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




        public async Task StartDownloadJobsAsync()
        {
           EMMFilesListView.IsEnabled = false;
            
           foreach (var file in PayloadEMMFile)
            {
                
                Label.Text = $"Status: Downloading {file.Name}";
                Progress<double> progress = new Progress<double>(i => DownloadProgressBar.Progress = i);
                await DownloadManager.DownloadAsync(file.Path, file.URL, progress);
                Label.Text = $"Status: Finished {file.Name}";
                
            }
           PayloadEMMFile.Clear();

            EMMFilesListView.IsEnabled = true;
        }
    }
}
