using GalaSoft.MvvmLight;

namespace FiveMin.WPF.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        private string _smallIconUrl;
        private string _backdropUrl;
        private string _description;
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public string BackdropUrl
        {
            get { return _backdropUrl; }
            set
            {
                _backdropUrl = value;
                RaisePropertyChanged();
            }
        }

        public string SmallIconUrl
        {
            get { return _smallIconUrl; }
            set
            {
                _smallIconUrl = value;
                RaisePropertyChanged();
            }
        }
    }
}
