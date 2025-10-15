using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Text.Style;
using AndroidX.Core.App;
using AndroidX.Work;
using CommunityToolkit.Mvvm.Messaging;
using Android.App;
using Java.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;


namespace EMM_Enterprise_Files
{
    internal class DownloadComplianceWorker : Worker
    {
        private int notificationId = 2;
        private Context _context;
        public DownloadComplianceWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
       
            _context = context;
        }

        private ForegroundInfo createForegroundInfo(String progress, int i)
        {
           
            Notification notification = new NotificationCompat.Builder(_context, MainApplication.Channel1Id)
                    .SetContentTitle("EMM FileSync")
                    .SetTicker("EMM FileSync")
                    .SetContentText(progress)
                    .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                    .SetOngoing(true)
                    .SetOnlyAlertOnce(true)
                    // Add the cancel action to the notification which can
                    // be used to cancel the worker
                    //.SetProgress(100, i, false)
                    .Build();
            
    
            return new ForegroundInfo(notificationId, notification, 1);
        }

        async void EnforceCompliance()
        {


            ConfigurationProvider eMMFileSync = ConfigurationProvider.GetInstance();
            DownloadJobManager djm = new DownloadJobManager();
            List<IEMMProfile> eMMFiles = EMMProfile.All;
            var messenger = MauiApplication.Current.Services.GetService<IMessenger>();
            //messenger.Send(new MessageData(2, "EMM Enterprise Files", "Enforcing compliance..."));

            Progress<double> progress = new Progress<double>(i => {

                //notification.SetContentText($"Download in progress {i * 100:0.##}%");
                
                if ((int)(i * 100) % 5 == 0)
                {
                    SetForegroundAsync(createForegroundInfo($"Download in progress", (int)(i * 100)));
                }

                // _notificationManager.Notify(2, notification.Build());
            });
            Progress<string> progressText = new Progress<string>(i => {
                    SetForegroundAsync(createForegroundInfo(i, 100));
            });
            //NotificationManagerCompat _notificationManager = NotificationManagerCompat.From(_context);
            SetForegroundAsync(createForegroundInfo("Hello", 100));
            foreach (var file in eMMFiles)
            {
                if (file.IsCompliant == compliancestate.NonCompliant)
                {
                    djm.AddDownloadJob(file);
#if DEBUG
                    Android.Util.Log.Debug("DownloadComplianceWorker", $"Created download job for {file.Name}.");
#endif
                }
                else
                {
#if DEBUG
                    Android.Util.Log.Debug("DownloadComplianceWorker", $"Skipping {file.Name}.");
#endif
                }
            }
            //  Progress<double> progress = new Progress<double>();

            djm.StartDownloadJobs(progress, progressText);

            //_notificationManager.Cancel(2);
            Android.Util.Log.Debug("DownloadComplianceWorker", $"Download jobs started.");
        }

        async void EnforceCompliance2()
        {


            ConfigurationProvider eMMFileSync = ConfigurationProvider.GetInstance();
            DownloadJobManager djm = new DownloadJobManager();
            List<EMMFile> eMMFiles = eMMFileSync.GetEMMFileData();
            var messenger = MauiApplication.Current.Services.GetService<IMessenger>();
            //messenger.Send(new MessageData(2, "EMM Enterprise Files", "Enforcing compliance..."));

            var notification = new NotificationCompat.Builder(_context, MainApplication.Channel2Id)
   .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
   .SetContentTitle("EMM FileSync")
   .SetPriority(NotificationCompat.PriorityLow)
   .SetOngoing(true)
   .SetOnlyAlertOnce(true) // with high priority, stops the popup on every update
   .SetProgress(100, 0, false);

            NotificationManagerCompat _notificationManager = NotificationManagerCompat.From(_context);

            Progress<double> progress = new Progress<double>(i => {                 

                    //notification.SetContentText($"Download in progress {i * 100:0.##}%");
                    notification.SetProgress(100, (int)(i * 100), false);
                    if ( (int)(i*100) % 5 == 0 )
                    {
                        _notificationManager.Notify(2, notification.Build());
                    }

               // _notificationManager.Notify(2, notification.Build());
            } );
            Progress<string> progressText = new Progress<string>(i => {
                notification.SetContentText(i);
                Thread.Sleep(250);
                _notificationManager.Notify(2, notification.Build());
            });
            foreach (var file in eMMFiles)
            {
                if (file.IsCompliant == compliancestate.NonCompliant)
                {
                    djm.AddDownloadJob(file);
#if DEBUG
                    Android.Util.Log.Debug("DownloadComplianceWorker", $"Created download job for {file.Name}.");
#endif
                }
                else
                {
#if DEBUG
                    Android.Util.Log.Debug("DownloadComplianceWorker", $"Skipping {file.Name}.");
#endif
                }
            }
          //  Progress<double> progress = new Progress<double>();

           // djm.StartDownloadJobsAsync(progress, progressText);

            //_notificationManager.Cancel(2);
            Android.Util.Log.Debug("DownloadComplianceWorker", $"Download jobs started.");
        }

        public override Result DoWork()
        {
            //SetForegroundAsync(createForegroundInfo("Hallo", 100));
            //Thread.Sleep(2000);
            EnforceCompliance();
            
            return Result.InvokeSuccess();
        }



    }
}
