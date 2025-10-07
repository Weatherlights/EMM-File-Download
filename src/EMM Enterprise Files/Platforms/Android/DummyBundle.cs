using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    internal class DummyBundle
    {

        public static Bundle getDummyBundle()
        {
            Bundle myBundle = new Bundle();
            myBundle.PutString("name", "CompanyPortal.apk");
            myBundle.PutString("url", "https://download.microsoft.com/download/2f1157ff-c775-44cc-a80a-b328603a8ab9/com.microsoft.windowsintune.companyportal.apk");
            myBundle.PutString("path", "/storage/emulated/0/Download/CompanyPortal.apk");
            myBundle.PutString("intent", "Available");
            Bundle innerBundle1 = new Bundle();
            innerBundle1.PutString("name", "Maulwurf.jpg");
            innerBundle1.PutString("url", "https://i.imgflip.com/5dk643.jpg");
            innerBundle1.PutString("path", "/storage/emulated/0/Download/meme.jpg");
            innerBundle1.PutString("intent", "Require");
            Bundle innerBundle2 = new Bundle();
            innerBundle2.PutString("name", "InnerFile2.png");
            innerBundle2.PutString("url", "https://de.wikipedia.org/wiki/Datei:Henri_Rousseau_-_Portrait_de_Monsieur_X.jpg");
            innerBundle2.PutString("path", "/storage/emulated/0/Download/monsieur.jpg");
            innerBundle2.PutString("intent", "Create");
            Bundle[] bundleArray = new Bundle[] { myBundle, innerBundle1 };
            myBundle.PutParcelableArray("profiles", bundleArray);
            return myBundle;
        }
    }
}
