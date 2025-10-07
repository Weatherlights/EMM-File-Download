using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Work;
using Java.Lang;

namespace EMM_Enterprise_Files
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            PeriodicWorkRequest taxWorkRequest = PeriodicWorkRequest.Builder.From<DownloadComplianceWorker>(TimeSpan.FromMinutes(1)).Build();

            WorkManager.Instance.Enqueue(taxWorkRequest);
        }
    }
}
