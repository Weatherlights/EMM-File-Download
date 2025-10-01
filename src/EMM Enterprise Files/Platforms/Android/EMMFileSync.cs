using Android.App;
using Android.App.Admin;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Android.App.Admin.DevicePolicyManager;
using static System.Net.Mime.MediaTypeNames;
using Application = Android.App.Application;

namespace EMM_Enterprise_Files
{
    public partial class EMMFileSync
    {
        public partial List<EMMFile> GetEMMFileData();

        // DevicePolicyManager devicePolicyManager = (DevicePolicyManager)Application.Context.GetSystemService(Context.DevicePolicyService);
        ManagedConfigurationProvider myMCP = new ManagedConfigurationProvider();



        public partial List<EMMFile> GetEMMFileData()
        {
            List<Bundle> bundles = myMCP.GetBundleArrayList("profiles");
            List<EMMFile> eMMFiles = new List<EMMFile>();
            foreach (Bundle bundle in bundles)
            {
                EMMFile eMMFile = new EMMFile();
                eMMFile.Name = bundle.GetString("name");

                eMMFile.URL = bundle.GetString("url");
                eMMFile.Path = bundle.GetString("path");
                eMMFiles.Add(eMMFile);
            }
  
            return eMMFiles;

        }

    }
}
