using GalaSoft.MvvmLight;

namespace FiveMin.WPF.ViewModels
{
    public class EntityViewModel : ViewModelBase
    {
        private string _wikiLink;
        private string _description;
        private string _imageUrl;
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

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
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

        public string WikiLink
        {
            get { return _wikiLink; }
            set
            {
                _wikiLink = value;
                RaisePropertyChanged();
            }
        }
    }
}
