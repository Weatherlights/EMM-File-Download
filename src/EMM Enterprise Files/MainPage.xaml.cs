using CommunityToolkit.Mvvm.Messaging;
using Android.OS;

namespace EMM_Enterprise_Files
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly IMessenger _messenger;

        public MainPage(IMessenger messenger)
        {
            InitializeComponent();
            Permissions.RequestAsync<NotificationPermission>();
            _messenger = messenger;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as IEMMProfile;
            if (item == null)
                return;
            if (item.eMMProfileViewModel.IsSelected && item.eMMProfileViewModel.IsAvailable)
                            {
                item.eMMProfileViewModel.IsSelected = false;
            }
            else if (item.eMMProfileViewModel.IsAvailable)
            {
                item.eMMProfileViewModel.IsSelected = true;
            }
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            List<IEMMProfile> profiles = EMMProfile.All;
            ProfileJobManager djm = new ProfileJobManager();
            
            foreach ( IEMMProfile profile in profiles )
            {
                if ( profile.eMMProfileViewModel.IsSelected && profile.eMMProfileViewModel.IsAvailable)
                {
                    djm.AddProfileJob(profile);
                }
            }
            djm.StartProfileJobs();
        }
    }

}
