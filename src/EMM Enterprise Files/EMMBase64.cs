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
        public visibility Visibility { get; set; }


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
                if (this.IconSource == null)
                    return "file.png";
                else if (this.IconSource.StartsWith("http") || this.IconSource.StartsWith("/"))
                    return this.IconSource;
                else
                {
                    try
                    {
                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(IconSource));
                        return ImageSource.FromStream(() => stream);
                    } catch (Exception ex)
                    {
                        return "file.png";
                    }
                }
            }
        }

        public string IconSource { get; set; }

        

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

            //if (GetComplianceState(Path, base64, Intent) == compliancestate.NonCompliant) {
                this.eMMProfileViewModel.Status = profilestatusvalue.Enforcing;
                this.eMMProfileViewModel.IsAvailable = false;
                // Convert Base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64);

                // Specify the path to save the image

                // Write the byte array to a file
                File.WriteAllBytes(Path, imageBytes);
                this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
                this.eMMProfileViewModel.IsSelected = false;
                this.eMMProfileViewModel.IsAvailable = true;
            //}
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
