using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;

namespace Kwikee.WPF.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        private string _backdropUrl;
        private string _description;
        private string _name;
        private string _keywords;
        private List<string> _keywordsFormatted;

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

        public string Keywords
        {
            get { return _keywords; }
            set
            {
                _keywords = value;
                if (!string.IsNullOrWhiteSpace(_keywords))
                {
                    _keywordsFormatted = _keywords.Split(',').ToList();
                }

                RaisePropertyChanged();
            }
        }

        public List<string> KeywordsFormatted => _keywordsFormatted; 
    }
}
