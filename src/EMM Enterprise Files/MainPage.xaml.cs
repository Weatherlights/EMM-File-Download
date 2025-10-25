using CommunityToolkit.Mvvm.Messaging;
using Android.OS;
using Android.Content;

namespace EMM_Enterprise_Files
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly IMessenger _messenger;

        public MainPage(IMessenger messenger)
        {
            InitializeComponent();
            // Permissions.RequestAsync<NotificationPermission>();
            this.OnAppearing();
            _messenger = messenger;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var status = await Permissions.CheckStatusAsync<NotificationPermission>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<NotificationPermission>();
            }

            
                var intent = new Intent(Android.Provider.Settings.ActionManageAllApplicationsSettings);
            var activity = Platform.CurrentActivity;
            activity?.StartActivity(intent);

            if (status == PermissionStatus.Granted)
            {
               
            }
            else
            {
                // TODO: not granted  permission
            }
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
