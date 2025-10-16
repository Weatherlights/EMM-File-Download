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




        private void Button_Clicked(object sender, EventArgs e)
        {
            List<IEMMProfile> profiles = EMMProfile.All;
            DownloadJobManager djm = new DownloadJobManager();
            
            foreach ( IEMMProfile profile in profiles )
            {
                if ( profile.isChecked && profile.isEnabled)
                {
                    djm.AddDownloadJob(profile);
                }
            }
            djm.StartDownloadJobs();
        }
    }

}
