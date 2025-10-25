using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{

    public enum intent { Available, Compliant, Create };
    public enum compliancestate { Compliant, NonCompliant };
    public enum visibility { Visible, Invisible };

    public static class profilestatusvalue
    {
        public static String Available { get { return "Available"; } }
        public static String Enforcing { get { return "Enforcing"; } }
        public static String Completed { get { return "Completed"; } }
        public static String Failed { get { return "Failed"; } }
    }

    public interface IEMMProfile
    {
        string Name { get; set; }
        string URL { get; set; }
        string Path { get; set; }
        //ImageSource Icon { get; }
        intent Intent { get; set; }
        compliancestate IsCompliant { get; }
        string IconSource { get; set; }
        public bool isEnabled { get;}
        public visibility Visibility { get; set; }
        public EMMProfileViewModel eMMProfileViewModel { get; }


        public void InitializeFileEnforcement();

        public ImageSource Icon { get; }

    }


    public class EMMProfile
    {

        public static List<IEMMProfile> All { get; private set; }
        public static List<IEMMProfile> DisplayList { get
            {
                if (All != null)
                    return All.Where(profile => profile.Visibility == visibility.Visible).ToList();
                else
                    return All;
            }
        }


        static EMMProfile()
        {
     
            ConfigurationProvider myEMMFileSync = ConfigurationProvider.GetInstance();
            All = new List<IEMMProfile>();
            All.AddRange(myEMMFileSync.GetEMMBase64());
            All.AddRange(myEMMFileSync.GetEMMFileData());
            

        }

    }
}
