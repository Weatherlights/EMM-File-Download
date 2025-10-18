using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public partial class EMMBase64
    {

        public EMMBase64(Bundle bundle)
        {
            
            this.Name = bundle.GetString("name");

            this.base64 = bundle.GetString("base64");
            this.Path = bundle.GetString("path");
            this.Base64IconString = bundle.GetString("icon");
            //eMMFile.Base64 = bundle.GetString("base64");


            if (bundle.GetString("intent") == "Create")
                this.Intent = intent.Create;
            else if (bundle.GetString("intent") == "Compliant")
                this.Intent = intent.Compliant;
            else
                this.Intent = intent.Available;

            if (this.IsCompliant == compliancestate.Compliant && this.Intent != intent.Available)
            {
                this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
                this.eMMProfileViewModel.IsAvailable = true;
            }
            else
            {
                this.eMMProfileViewModel.Status = profilestatusvalue.Available;
                this.eMMProfileViewModel.IsAvailable = true;
            }
        }
    }
}
