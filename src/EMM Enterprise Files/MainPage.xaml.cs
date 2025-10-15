using CommunityToolkit.Mvvm.Messaging;
using Android.OS;

namespace EMM_Enterprise_Files
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DownloadJobManager downloadJobManager = new DownloadJobManager();
        private readonly IMessenger _messenger;

        public MainPage(IMessenger messenger)
        {
            InitializeComponent();
            Permissions.RequestAsync<NotificationPermission>();
            _messenger = messenger;
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

            IEMMProfile item = args.SelectedItem as IEMMProfile;

            // Check if notifications are enabled
     


            downloadJobManager.AddDownloadJob(item);
                downloadJobManager.Label = StatusLabel;

                downloadJobManager.DownloadProgressBar = DownloadProgressBar;
            Progress<double> progress = new Progress<double>(i => DownloadProgressBar.Progress = i);
            Progress<string> progressText = new Progress<string>(i => StatusLabel.Text = i);
            downloadJobManager.StartDownloadJobs(progress, progressText);


            //DownloadManager.DownloadAsync(item.Path, item.URL);

        }

    }

}
