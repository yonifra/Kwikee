using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;

namespace Kwikee.WPF.ViewModels
{
    public class VideoViewModel : ViewModelBase
    {
        private string _name;
        private string _category;
        private string _description;
        private string _backdropUrl;
        private string _startedBy;
        private string _keywords;
        private string _length;
        private TimeSpan _lengthFormatted;
        private List<string> _keywordsFormatted;
        private DateTime _endingDate;
        private string _videoUrl;

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

        public string Length
        {
            get { return _length; }
            set
            {
                _length = value;

                if (!string.IsNullOrWhiteSpace(_length))
                {
                    _lengthFormatted = TimeSpan.Parse(_length);
                }

                RaisePropertyChanged();
            }
        }

        public TimeSpan LengthFormatted => _lengthFormatted;
        public List<string> KeywordsFormatted => _keywordsFormatted;

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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string VideoUrl
        {
            get { return _videoUrl; }
            set
            {
                _videoUrl = value;
                RaisePropertyChanged();
            }
        }
    }
}
