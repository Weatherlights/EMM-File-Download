namespace EMM_Enterprise_Files
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DownloadJobManager downloadJobManager = new DownloadJobManager();

        public MainPage()
        {
            InitializeComponent();
        }


        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            
            EMMFile item = args.SelectedItem as EMMFile;

                downloadJobManager.AddDownloadJob(item);
                downloadJobManager.Label = StatusLabel;
                downloadJobManager.EMMFilesListView = EMMFilesListView;
                downloadJobManager.DownloadProgressBar = DownloadProgressBar;
            Progress<double> progress = new Progress<double>(i => DownloadProgressBar.Progress = i);
            downloadJobManager.StartDownloadJobsAsync(progress);


            //DownloadManager.DownloadAsync(item.Path, item.URL);

        }

    }

}
