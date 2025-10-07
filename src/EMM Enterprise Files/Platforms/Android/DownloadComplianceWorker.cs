using Android.Content;
using Android.Content.PM;
using AndroidX.Work;
using Java.Net;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMM_Enterprise_Files
{
    internal class DownloadComplianceWorker : Worker
    {

        public DownloadComplianceWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {

        }

        private void EnforceCompliance ()
        {
            EMMFileSync eMMFileSync = new EMMFileSync();
            DownloadJobManager djm = new DownloadJobManager();
            List<EMMFile> eMMFiles = eMMFileSync.GetEMMFileData();
            foreach (var file in eMMFiles)
            {
                if (file.IsCompliant == EMMFile.compliancestate.NonCompliant)
                    djm.AddDownloadJob(file);
            }
            Progress<double> progress = new Progress<double>();
            djm.StartDownloadJobsAsync(progress);
        }

        public override Result DoWork()
        {
  //          EnforceCompliance();
            Android.Util.Log.Debug("CalculatorWorker", $"we are here");
            return Result.InvokeSuccess();
        }



    }
}
