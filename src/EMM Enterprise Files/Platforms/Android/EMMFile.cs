using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Provider.ContactsContract;

namespace EMM_Enterprise_Files
{
    public partial class EMMFile
    {
        Context context = Android.App.Application.Context;
        private DownloadManager downloadManager = DownloadManager.FromContext(Platform.AppContext);


        private string TemporaryFileName => $"{this.Name}.tmp";

        public EMMFile(Bundle bundle)
        {
            this.Name = bundle.GetString("name");

            this.URL = bundle.GetString("url");
            this.Path = bundle.GetString("path");
            this.Base64IconString = bundle.GetString("icon");
            //eMMFile.Base64 = bundle.GetString("base64");

            this.Hash = bundle.GetString("hash");

            if (bundle.GetString("intent") == "Create")
                this.Intent = intent.Create;
            else if (bundle.GetString("intent") == "Compliant")
                this.Intent = intent.Compliant;
            else
                this.Intent = intent.Available;
            
            this.eMMProfileViewModel.Status = profilestatusvalue.Available;

            if (this.IsCompliant == compliancestate.Compliant)
            {
                this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
                this.eMMProfileViewModel.IsAvailable = true; //  or false
            }
            else
            {
                this.eMMProfileViewModel.Status = profilestatusvalue.Available;
                this.eMMProfileViewModel.IsAvailable = true;
            }
        }

        private partial string GetTemporaryPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path + $"/{context.ApplicationInfo.PackageName}/{TemporaryFileName}";
        }


        private partial void ProcessDownload()
        {
            var request = new DownloadManager.Request(global::Android.Net.Uri.Parse(this.URL));

            var text = $"Downloading {this.Name}...";
            request.SetDescription(text);
 

                request.SetTitle(Name).SetVisibleInDownloadsUi(false).SetNotificationVisibility(Android.App.DownloadVisibility.Visible);
        
                request.SetDestinationInExternalPublicDir(global::Android.OS.Environment.DirectoryDownloads, $"{context.ApplicationInfo.PackageName}/{TemporaryFileName}");
                //request.SetDestinationInExternalPublicDir(FileSystem.Current.CacheDirectory, file.Name);

                var downloadId = downloadManager.Enqueue(request);
                //downloadTracker.Add(tmpFilePath, downloadId);
                context.RegisterReceiver(new EMMFileBroadcastReceiver(downloadId, this), new IntentFilter(DownloadManager.ActionDownloadComplete), Android.Content.ReceiverFlags.Exported);
                



            

        }
    }
}
