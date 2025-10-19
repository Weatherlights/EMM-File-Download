using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Java.Util.Jar.Attributes;

namespace EMM_Enterprise_Files
{
    internal class DownloadManagerWrapper
    {
        private DownloadManager downloadManager = DownloadManager.FromContext(Platform.AppContext);
        private static DownloadManagerWrapper _Instance { get; } = new DownloadManagerWrapper();
        private Context context = Android.App.Application.Context;
        private Dictionary<string, long> downloadTracker = new Dictionary<string, long>();

        private DownloadManagerWrapper()
        {
        }

        public static DownloadManagerWrapper GetDownloadManager()
        {
            return _Instance;
        }

        public long Download(EMMFile eMMFile)
        {
            
            if (downloadTracker.ContainsKey(eMMFile.Name))
            {
                var lastDownloadId = downloadTracker[eMMFile.Name];
                var lastFileUri = downloadManager.GetUriForDownloadedFile(lastDownloadId);
                var query = new DownloadManager.Query();
                query.SetFilterById(lastDownloadId);
           
                var cursor = downloadManager.InvokeQuery(query);
                if (cursor.MoveToFirst() && lastFileUri != null)
                {
                    var status = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnStatus));

                    switch(status)
                    {
                        case (int)Android.App.DownloadStatus.Failed:
                            downloadTracker.Remove(eMMFile.Name);
                            break;
                        case (int)Android.App.DownloadStatus.Successful:
                            downloadTracker.Remove(eMMFile.Name);
                            break;
                        default:
                            if (lastFileUri == global::Android.Net.Uri.Parse(eMMFile.URL))
                            {
                                return lastDownloadId;
                            }
                            else
                            {
                                downloadManager.Remove(lastDownloadId);
                            }
                            break;
                    }
                } else
                {
                    downloadTracker.Remove(eMMFile.Name);
                }
                    cursor.Close();



            }

            var request = new DownloadManager.Request(global::Android.Net.Uri.Parse(eMMFile.URL));
            request.SetTitle(eMMFile.Name).SetVisibleInDownloadsUi(true).SetNotificationVisibility(Android.App.DownloadVisibility.Visible);

            request.SetDestinationInExternalPublicDir(global::Android.OS.Environment.DirectoryDownloads, $".{context.ApplicationInfo.PackageName}/{eMMFile.Name}");
            //request.SetDestinationInExternalPublicDir(FileSystem.Current.CacheDirectory, file.Name);

            long downloadId = downloadManager.Enqueue(request);
            //downloadTracker.Add(tmpFilePath, downloadId);
            this.downloadTracker.Add(eMMFile.Name, downloadId);
            context.RegisterReceiver(new EMMFileBroadcastReceiver(downloadId, eMMFile), new IntentFilter(DownloadManager.ActionDownloadComplete), Android.Content.ReceiverFlags.Exported);
            return downloadId;
        }

        public void Confirm (EMMFile eMMFile)
        {
            if (downloadTracker.ContainsKey(eMMFile.Name))
            {
                downloadTracker.Remove(eMMFile.Name);
            }
        }

    }
}
