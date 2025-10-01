using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public class EMMFile
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }

       

        public static IEnumerable<EMMFile> All { get; private set; }

       /* public static async Task SaveState(string str) { 
            using FileStream outputStream = System.IO.File.OpenWrite(filePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(str);
        }*/

        static EMMFile()
        {
            
            EMMFileSync myEMMFileSync = new EMMFileSync();

           

            All = myEMMFileSync.GetEMMFileData();

            /*foreach (var file in All)
            {
                Progress<double> progress = new Progress<double>();
                DownloadManager.DownloadAsync(file.Path, file.URL, progress);
            }*/
        }
    }
}
