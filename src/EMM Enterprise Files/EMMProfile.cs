using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{

    public enum intent { Available, Compliant, Create };
    public enum compliancestate { Compliant, NonCompliant };

    public interface IEMMProfile
    {
        string Name { get; set; }
        string URL { get; set; }
        string Path { get; set; }
        //ImageSource Icon { get; }
        intent Intent { get; set; }
        compliancestate IsCompliant { get; }
        string Base64IconString { get; set; }

        public void InitializeFileEnforcement();

        public ImageSource Icon { get; }

    }


    public class EMMProfile
    {

        public static List<IEMMProfile> All { get; private set; }

        static EMMProfile()
        {

            ConfigurationProvider myEMMFileSync = ConfigurationProvider.GetInstance();
            All = new List<IEMMProfile>();
            All.AddRange(myEMMFileSync.GetEMMBase64());
            All.AddRange(myEMMFileSync.GetEMMFileData());
            

        }

    }
}
