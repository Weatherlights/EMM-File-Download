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
                EMMBase64 eMMBase64 = new EMMBase64(bundle);
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
                EMMFile eMMFile = new EMMFile(bundle);
                eMMFiles.Add(eMMFile);
            }

            return eMMFiles;

        }
    }


}
