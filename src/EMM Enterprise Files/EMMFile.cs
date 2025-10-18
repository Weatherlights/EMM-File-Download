using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;
using static Android.Provider.ContactsContract;

namespace EMM_Enterprise_Files
{
    public enum validationresult { Valid, Invalid, NoHash };

    public partial class EMMFile : IEMMProfile
    {

        public string Name { get; set; }
        public string URL { get; set; }
        protected string _Path;
        public string Path { get { return VariableHandler.ResolveVariables(this._Path); } set { this._Path = value; } }
        public intent Intent { get; set; }
        private string TemporaryPath => GetTemporaryPath();
        public string IconSource { get; set; }
        public visibility Visibility { get; set; }

        public bool isEnabled
        {
            get
            {
                if (this.IsCompliant == compliancestate.Compliant)
                    return false;
                return true;
            }
        }
        public EMMProfileViewModel eMMProfileViewModel { get; } = new EMMProfileViewModel();


        public EMMFile()
        {
            this.eMMProfileViewModel.Status = profilestatusvalue.Available;
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
                    }
                    catch (Exception ex)
                    {
                        return "file.png";
                    }
                }
            }
        }


        private partial string GetTemporaryPath();
        private partial void ProcessDownload();

        public string Hash { get; set; }

        public string Base64 { get; set; }
        public long DownloadJobId = 0;


        public compliancestate IsCompliant { get
            {
                return GetComplianceState(this.Path, this.Hash, this.Intent);
            }
        }


        public void InitializeFileEnforcement() {
            this.eMMProfileViewModel.Status = profilestatusvalue.Enforcing;
            this.eMMProfileViewModel.IsAvailable = false;

            if (EMMFile.GetComplianceState(this.TemporaryPath, this.Hash, this.Intent) == compliancestate.Compliant)
            {
                try
                {
                    System.IO.File.Move(TemporaryPath, Path, true);
                    this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
                    this.eMMProfileViewModel.IsAvailable = true;
                }
                catch (Exception ex)
                {
                    this.eMMProfileViewModel.Status = this.eMMProfileViewModel.Status = $"{ex.HResult}: {ex.Message}";
                    this.eMMProfileViewModel.IsAvailable = true;

                }
            }
            else
            {
                try
                {
                    this.ProcessDownload();
                }
                catch (Exception ex)
                {
                    this.eMMProfileViewModel.Status = $"{ex.HResult}: {ex.Message}";
                    this.eMMProfileViewModel.IsAvailable = true;
                }
            }


        }

        public void ProcessDownloadedFile()
        {
            if (EMMFile.ValidateHash(TemporaryPath, this.Hash) != validationresult.Invalid)
            {
                var destFile = this.Path;
                try
                {
                    System.IO.File.Move(TemporaryPath, destFile, true);
                    this.eMMProfileViewModel.Status = profilestatusvalue.Completed;
                }
                catch (Exception ex)
                {
                    this.eMMProfileViewModel.Status = $"{ex.HResult}: {ex.Message}";
                }
            }
            else
            {
                System.IO.File.Delete(TemporaryPath);
                this.eMMProfileViewModel.Status = "Failed Hash validation";
            }

            if (File.Exists(Path))
                if (IsCompliant == compliancestate.NonCompliant && this.Hash != null) // file has wrong hash
                {
                    //File.Delete(Path);
                    this.eMMProfileViewModel.Status = profilestatusvalue.Available;
                    this.eMMProfileViewModel.IsAvailable = true;
                    
                }
            this.eMMProfileViewModel.IsAvailable = true;
            this.eMMProfileViewModel.IsSelected = false;
        }


        public static validationresult ValidateHash(string Path, string Hash)
        {
            if (File.Exists(Path))
            {
                if (Hash == null)
                    return validationresult.NoHash;
                if (EMMFile.GetChecksum(Path) == Hash)
                    return validationresult.Valid;
            }
            return validationresult.Invalid;
        }

        public static compliancestate GetComplianceState(string Path, string Hash, intent Intent)
        {
            if (Intent == intent.Create)
            {
                if (File.Exists(Path))
                    return compliancestate.Compliant;
            }
            else if (Intent == intent.Compliant)
            {
                if ( ValidateHash(Path, Hash) == validationresult.Valid )
                    return compliancestate.Compliant;
            }
            else
            {
                return compliancestate.Compliant;
            }
            return compliancestate.NonCompliant;
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
