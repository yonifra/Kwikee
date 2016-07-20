using System;
using GalaSoft.MvvmLight;

namespace FiveMin.WPF.ViewModels
{
    public class CompetitionViewModel : ViewModelBase
    {
        private string _name;
        private string _category;
        private string _description;
        private string _backdropUrl;
        private string _startedBy;
        private DateTime _endingDate;

        public DateTime EndingDate
        {
            get { return _endingDate; }
            set
            {
                _endingDate = value;
                RaisePropertyChanged();
            }
        }

        public string StartedBy
        {
            get { return _startedBy; }
            set
            {
                _startedBy = value;
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

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
    }
}
