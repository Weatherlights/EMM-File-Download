using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace EMM_Enterprise_Files
{
    

    public class EMMFile
    {
        public enum intent { Available, Compliant, Create };
        public enum compliancestate {  Compliant, NonCompliant  };

        public string Name { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }
        public string Base64 { get; set; }
        public string Hash { get; set; }
        public intent Intent { get; set; }
        public compliancestate IsCompliant { get
            {
                if (this.Intent == EMMFile.intent.Create)
                {
                    if (File.Exists(this.Path))
                        return compliancestate.Compliant;
                }
                else if (this.Intent == EMMFile.intent.Compliant)
                {
                    if (File.Exists(this.Path))
                    {
                        if (this.Hash == null)
                            return compliancestate.Compliant;
                        if (EMMFile.GetChecksum(this.Path) == this.Hash)
                            return compliancestate.Compliant;
                    }
                }
                else
                {
                    return compliancestate.Compliant;
                }
                return compliancestate.NonCompliant;
            }
        }




        //public static IEnumerable<EMMFile> All { get; private set; }
        public static List<EMMFile> All { get; private set; }

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

        private static string GetChecksum(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                var sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }
    }
}
