using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public class EMMProfileViewModel : ObservableObject
    {
        private string name;
        private string status;
        private bool _isAvailable;
        private bool _isSelected;


        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
