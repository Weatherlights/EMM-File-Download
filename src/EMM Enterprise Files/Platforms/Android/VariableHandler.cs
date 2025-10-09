using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    internal partial class VariableHandler
    {

        public static partial string GetDownloadFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;
        }

        public static partial string GetDocumentsFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
        }

        public static partial string GetPicturesFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;
        }

        public static partial string GetExternalStorageFolder()
        {
            return Android.OS.Environment.ExternalStorageDirectory.Path;
        }

        public static partial string GetDataFolder()
        {
            return Android.OS.Environment.DataDirectory.Path;
        }
    }
}
