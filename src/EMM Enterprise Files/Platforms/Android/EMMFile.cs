using Android.App;
using Android.Content;
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

        private partial string GetTemporaryPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path + $"/{context.ApplicationInfo.PackageName}/{TemporaryFileName}";
        }


        public partial void InitializeFileEnforcement()
        {
            var request = new DownloadManager.Request(global::Android.Net.Uri.Parse(this.URL));

            var text = $"Downloading {this.Name}...";
            request.SetDescription(text);

            if (EMMFile.GetComplianceState(this.TemporaryPath, this.Hash, this.Intent) == compliancestate.Compliant)
            {
                try
                {
                    System.IO.File.Move(TemporaryPath, Path, true);
                    Toast.MakeText(context, $"{Name} processed successfully.", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(context, $"{Name} processed failed: {ex.Message}", ToastLength.Long).Show();
                }
            }
            else
            {
                request.SetTitle(Name).SetVisibleInDownloadsUi(false).SetNotificationVisibility(Android.App.DownloadVisibility.Hidden);
                request.SetDestinationInExternalPublicDir(global::Android.OS.Environment.DirectoryDownloads, $"{context.ApplicationInfo.PackageName}/{TemporaryFileName}");
                //request.SetDestinationInExternalPublicDir(FileSystem.Current.CacheDirectory, file.Name);

                var downloadId = downloadManager.Enqueue(request);
                //downloadTracker.Add(tmpFilePath, downloadId);
                context.RegisterReceiver(new EMMFileBroadcastReceiver(downloadId, this), new IntentFilter(DownloadManager.ActionDownloadComplete), Android.Content.ReceiverFlags.Exported);
            }

            if (IsCompliant == compliancestate.NonCompliant) // file has wrong hash
            {
                File.Delete(Path);
            }

            

        }
    }
}
