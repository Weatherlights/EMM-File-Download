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
            if(bundle.GetString("icon") != "0x0")
                this.IconSource = bundle.GetString("icon");
            //eMMFile.Base64 = bundle.GetString("base64");

            if (bundle.GetString("hash") != "0x0")
                this.Hash = bundle.GetString("hash");
            this.eMMProfileViewModel.Description = bundle.GetString("description");

            if(bundle.GetBoolean("hiddeninportal"))
                this.Visibility = visibility.Invisible;
            else
                this.Visibility = visibility.Visible;

            if (bundle.GetString("intent") == "Create")
                this.Intent = intent.Create;
            else if (bundle.GetString("intent") == "Compliant")
                this.Intent = intent.Compliant;
            else
                this.Intent = intent.Available;
            
            this.eMMProfileViewModel.Status = profilestatusvalue.Available;

            if (ValidateHash(Path, Hash) != validationresult.Invalid)
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
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path + $"/.{context.ApplicationInfo.PackageName}/{Name}";
        }


        private partial void ProcessDownload()
        {
            DownloadManagerWrapper downloadManagerWrapper = DownloadManagerWrapper.GetDownloadManager();

            long downloadId = downloadManagerWrapper.Download(this);
        }
    }
}
