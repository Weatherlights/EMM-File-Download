using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public partial class EMMFileSync
    {
        string test = "as";
        public partial EMMFile GetEMMFileData()
        {

            EMMFile eMMFile = new EMMFile();
            eMMFile.Name = "iOS File";
            eMMFile.URL = "https://example.com/iOSFile.pdf";
            eMMFile.LocalPath = "/local/path/iOSFile.pdf";
            return eMMFile;

        }
    }
