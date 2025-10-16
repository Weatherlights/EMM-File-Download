using Android.Text.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public partial class EMMBase64 : IEMMProfile
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string base64 { get; set; }
        private string _Path;
        public string Path { get { return VariableHandler.ResolveVariables(this._Path); } set { this._Path = value; } }
        public intent Intent { get; set; }
       
        public bool isChecked { get; set; }
        public bool isEnabled { get
            {
                if (this.IsCompliant == compliancestate.Compliant)
                    return false;
                return true;
            }
        }
        public EMMProfileViewModel eMMProfileViewModel { get; } = new EMMProfileViewModel();

        public compliancestate IsCompliant
        {
            get
            {
                return GetComplianceState(this.Path, this.base64, this.Intent);
            }
        }

        public ImageSource Icon
        {
            get
            {
                MemoryStream stream;
                if (this.Base64IconString != null)
                    stream = new MemoryStream(Convert.FromBase64String(Base64IconString));
                else
                    stream = new MemoryStream(Convert.FromBase64String("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"));
                ImageSource image = ImageSource.FromStream(() => stream);
                return image;
            }
        }

        public string Base64IconString { get; set; }

        

        public static compliancestate GetComplianceState(string Path, string base64, intent Intent)
        {
            if (Intent == intent.Create)
            {
                if (File.Exists(Path))
                    return compliancestate.Compliant;
            }
            else if (Intent == intent.Compliant)
            {
                if (File.Exists(Path))
                {
                    if (base64 == null)
                        return compliancestate.Compliant;
                    if (EMMBase64.GetBase64String(Path) == base64)
                        return compliancestate.Compliant;
                }
            }
            else
            {
                return compliancestate.Compliant;
            }
            return compliancestate.NonCompliant;
        }

        public void InitializeFileEnforcement()
        {

            if (GetComplianceState(Path, base64, Intent) == compliancestate.NonCompliant) {
                this.eMMProfileViewModel.Status = profilestatusvalue.Enforcing;
                this.eMMProfileViewModel.IsAvailable = false;
                // Convert Base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64);

                // Specify the path to save the image

                // Write the byte array to a file
                File.WriteAllBytes(Path, imageBytes);
                this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
            }
        }


        static private string GetBase64String(string filePath)
        {
            
            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(fileBytes);
#if DEBUG
            Android.Util.Log.Debug("EMMBase64", base64String);
#endif
            return base64String;
        }
    }
}
