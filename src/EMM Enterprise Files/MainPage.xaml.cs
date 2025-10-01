namespace EMM_Enterprise_Files
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            EMMFile item = args.SelectedItem as EMMFile;
            EMMFilesListView.IsEnabled = false;
            StatusLabel.Text = $"Status: Downloading {item.Name}";
            Progress<double> progress = new Progress<double>(i => DownloadProgressBar.Progress = i);
            DownloadManager.DownloadAsync(item.Path, item.URL, progress);
            StatusLabel.Text = $"Status: Finished {item.Name}";
            EMMFilesListView.IsEnabled = true;
        }

    }

}
