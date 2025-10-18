using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.Work;
using CommunityToolkit.Mvvm.Messaging;
using Java.Lang;

namespace EMM_Enterprise_Files
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        private NotificationManagerCompat _notificationManager;

        public MainActivity()
        {
            //PermissionStatus status = Permissions.RequestAsync<NotificationPermission>().GetAwaiter().GetResult();

            var messenger = MauiApplication.Current.Services.GetService<IMessenger>();

            messenger.Register<MessageData>(this, (recipient, message) =>
            {
                if (message.Channel == 1)
                {
                    SendOnChannel1(message.Title, message.Message);
                }
                else
                {
                    SendOnChannel2(message.Title, message.Message);
                }
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _notificationManager = NotificationManagerCompat.From(this);

            ConfigurationProvider configurationProvider = ConfigurationProvider.GetInstance();

           PeriodicWorkRequest backgroundEMMFileSyncTaskRequest = PeriodicWorkRequest.Builder.From<DownloadComplianceWorker>(TimeSpan.FromMinutes(configurationProvider.GetSyncInterval())).Build();

          WorkManager.GetInstance(this).EnqueueUniquePeriodicWork("EMMFileSyncWork", ExistingPeriodicWorkPolicy.Keep, backgroundEMMFileSyncTaskRequest);
        }

        private void SendOnChannel1(string title, string message)
        {
            var notification = new NotificationCompat.Builder(this, MainApplication.Channel1Id)
                .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetCategory(NotificationCompat.CategoryMessage)
                .Build();

            _notificationManager.Notify(1, notification);
        }

        private NotificationCompat.Builder SendOnChannel2(string title, string message)
        {
            const int progressMax = 100;
            var notification = new NotificationCompat.Builder(this, MainApplication.Channel2Id)
               .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
               .SetContentTitle("Download")
               .SetContentText("Download in progress")
               .SetPriority(NotificationCompat.PriorityLow)
               .SetOngoing(true)
               .SetOnlyAlertOnce(true) // with high priority, stops the popup on every update
               .SetProgress(progressMax, 0, false);
      
            _notificationManager.Notify(2, notification.Build());
            return notification;
            // simulate progress, such as a download
            Task.Run(() =>
            {
              

                for (var progress = 0; progress <= progressMax; progress += 10)
                {
                    notification.SetProgress(progressMax, progress, false);
                    // same id (2) to ensure overwrite/updates existing
                    _notificationManager.Notify(2, notification.Build());

                   
                }

                notification.SetContentText("Download finished")
                   .SetOngoing(false)
                   .SetProgress(0, 0, false);

                // same id (2) to ensure overwrite/updates existing
                _notificationManager.Notify(2, notification.Build());
            });
        }
    }
}
