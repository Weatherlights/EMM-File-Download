using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMM_Enterprise_Files
{
    public class ConfigurationProvider
    {

        private static ConfigurationProvider _instance = new EMM_Enterprise_Files.ConfigurationProvider();        
        private ManagedConfigurationProvider myMCP = ManagedConfigurationProvider.GetInstance();

        private ConfigurationProvider() { }

        public static ConfigurationProvider GetInstance() { return _instance; }


        public int GetSyncInterval()
        {
            string syncIntervalStr = myMCP.GetStringValue("syncInterval");
            int syncInterval = 20; // default to 60 minutes
            if (syncIntervalStr != null)
            {
                try
                {
                    syncInterval = Int32.Parse(syncIntervalStr);
                }
                catch (FormatException)
                {
                    syncInterval = 20; // default to 60 minutes
                }
            }
            return syncInterval;
        }

        public string GetStringValue(string key)
        {
            return myMCP.GetStringValue(key);
        }

        public List<EMMBase64> GetEMMBase64()
        {
            List<Bundle> bundles = myMCP.GetBundleArrayList("base64files");
            List<EMMBase64> eMMBase64s = new List<EMMBase64>();
            foreach (Bundle bundle in bundles)
            {
                EMMBase64 eMMBase64 = new EMMBase64();
                eMMBase64.Name = bundle.GetString("name");

                eMMBase64.base64 = bundle.GetString("base64");
                eMMBase64.Path = bundle.GetString("path");
                eMMBase64.Base64IconString = bundle.GetString("icon");
                //eMMFile.Base64 = bundle.GetString("base64");


                if (bundle.GetString("intent") == "Create")
                    eMMBase64.Intent = intent.Create;
                else if (bundle.GetString("intent") == "Compliant")
                    eMMBase64.Intent = intent.Compliant;
                else
                    eMMBase64.Intent = intent.Available;
                eMMBase64s.Add(eMMBase64);
            }

            return eMMBase64s;

        }


        public List<EMMFile> GetEMMFileData()
        {
            List<Bundle> bundles = myMCP.GetBundleArrayList("advanceddownloads");
            List<EMMFile> eMMFiles = new List<EMMFile>();
            foreach (Bundle bundle in bundles)
            {
                EMMFile eMMFile = new EMMFile();
                eMMFile.Name = bundle.GetString("name");

                eMMFile.URL = bundle.GetString("url");
                eMMFile.Path = bundle.GetString("path");
                eMMFile.Base64IconString = bundle.GetString("icon");
                //eMMFile.Base64 = bundle.GetString("base64");

                eMMFile.Hash = bundle.GetString("hash");

                if (bundle.GetString("intent") == "Create")
                    eMMFile.Intent = intent.Create;
                else if (bundle.GetString("intent") == "Compliant")
                    eMMFile.Intent = intent.Compliant;
                else
                    eMMFile.Intent = intent.Available;
                eMMFiles.Add(eMMFile);
            }

            return eMMFiles;

        }
    }


}
