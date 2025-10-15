using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Provider.ContactsContract;

namespace EMM_Enterprise_Files
{
    
    internal class EMMFileBroadcastReceiver :  BroadcastReceiver
    {
        private readonly long _downloadId;
        private EMMFile _profile;

        public EMMFileBroadcastReceiver(long downloadId, EMMFile profile)
        {
            _downloadId = downloadId;
            _profile = profile;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            // Only handle download broadcasts
            Thread.Sleep(250);
            if (intent.Action == DownloadManager.ActionDownloadComplete)
            {
                var downloadId = intent.GetLongExtra(DownloadManager.ExtraDownloadId, 0);
                // Only handle specific download ID
                if (downloadId == _downloadId)
                {
                    ProcessFinishedDownload(context, downloadId);
                    context.UnregisterReceiver(this);
                }
            }
        }

        private void ProcessFinishedDownload(Context context, long downloadId)
        {
            var downloadManager = (DownloadManager)context.GetSystemService(Context.DownloadService);
            
            var fileUri = downloadManager.GetUriForDownloadedFile(downloadId);

            var fileMimeType = downloadManager.GetMimeTypeForDownloadedFile(downloadId);
  
            if (fileUri == null || fileMimeType == null)
            {
                return;
            }
            _profile.ProcessDownloadedFile(); /*
            else if (EMMFile.GetComplianceState(srcFile, _file.Hash, _file.Intent) == compliancestate.Compliant)
            {
                var destFile = _file.Path;
                try
                {
                    System.IO.File.Move(srcFile, destFile, true);
                    Toast.MakeText(context, $"{_file.Name} downloaded successfully.", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(context, $"Error saving file: {ex.Message}", ToastLength.Long).Show();
                }
            }
            else
            {
                System.IO.File.Delete(srcFile);
                Toast.MakeText(context, $"{_file.Name} failed integrity check.", ToastLength.Long).Show();
            }*/
        }
    }
}
