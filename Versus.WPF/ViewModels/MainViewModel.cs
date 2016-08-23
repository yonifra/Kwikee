using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kwikee.Portable.Data;
using Kwikee.Portable.Entities;

namespace Kwikee.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _editCommand;
        private ObservableCollection<FiveMinVideo> _videos;
        private ObservableCollection<Category> _categories;
        private VideoViewModel _newVideo;
        private RelayCommand _addVideo;
        private CategoryViewModel _newCategory;
        private RelayCommand _addCategory;
        private RelayCommand _refreshCommand;
        private Category _selectedCategory;
        private FiveMinVideo _selectedVideo;
        private string _consoleText;

        public MainViewModel()
        {
            InitializeCollections();

            AddVideo = new RelayCommand(AddVideoExecute);
            AddCategory = new RelayCommand(AddCategoryExecute);
            RefreshCommand = new RelayCommand(RefreshAll);
            NewVideo = new VideoViewModel();
            NewCategory = new CategoryViewModel();
        }
        
        private void RefreshAll()
        {
            PopulateVideos();
            PopulateCategories();
        }

        private async void AddCategoryExecute()
        {
            if (NewCategory != null)
            {
                var category = new Category
                {
                    Name = NewCategory.Name,
                    Description = NewCategory.Description,
                    ImageUrl = NewCategory.BackdropUrl,
                    DateAdded = DateTime.Now,
                    IsWatched = false,
                    Keywords = NewCategory.KeywordsFormatted
                };

                await FirebaseManager.Instance.AddCategory(category);

                Categories.Add(category);

                NewCategory = new CategoryViewModel();
            }
        }
        
        private async void AddVideoExecute()
        {
            if (NewVideo != null)
            {
                var video = new FiveMinVideo
                {
                    Name = NewVideo.Name,
                    Categories = new List<string> { SelectedCategory.Name },
                    Description = NewVideo.Description,
                    ImageUrl =  NewVideo.BackdropUrl,
                    DateAdded =  DateTime.Now,
                    Likes = 0,
                    Dislikes = 0,
                    WatchCount = 0,
                    IsWatched = false,
                    VideoId = NewVideo.VideoUrl,
                    Keywords = NewVideo.KeywordsFormatted,
                    Length = NewVideo.LengthFormatted
                };

                await FirebaseManager.Instance.AddVideo(video);

                Videos.Add(video);

                NewVideo = new VideoViewModel();
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        private void InitializeCollections()
        {
            _videos = new ObservableCollection<FiveMinVideo>();
            _categories = new ObservableCollection<Category>();

            RefreshAll();
        }

        private async void PopulateVideos()
        {
            if (_videos != null)
            {
                _videos.Clear();

                var videos = await FirebaseManager.Instance.GetAllVideos();

                // Videos will be null if no video has yet to be added
                if (videos != null)
                {
                    foreach (var c in videos)
                    {
                        Videos.Add(c.Value);
                    }
                }
            }
        }

        private async void PopulateCategories()
        {
            if (_categories != null)
            {
                _categories.Clear();

                var categories = await FirebaseManager.Instance.GetAllCategories();

                if (categories != null)
                {
                    foreach (var c in categories)
                    {
                        Categories.Add(c.Value);
                    }
                }
            }
        }
      
        public RelayCommand EditCommand
        {
            get { return _editCommand; }
            set
            {
                _editCommand = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<FiveMinVideo> Videos
        {
            get { return _videos; }
            set
            {
                _videos = value;
                RaisePropertyChanged();
            }
        }

        public FiveMinVideo SelectedVideo
        {
            get { return _selectedVideo; }
            set
            {
                _selectedVideo = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged();
            }
        }
        
        public VideoViewModel NewVideo
        {
            get { return _newVideo; }
            set
            {
                _newVideo = value;
                RaisePropertyChanged();
            }
        }
        
        public RelayCommand RefreshCommand
        {
            get { return _refreshCommand; }
            set
            {
                _refreshCommand = value;
                RaisePropertyChanged();
            }
        }

        public CategoryViewModel NewCategory
        {
            get { return _newCategory; }
            set
            {
                _newCategory = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand AddVideo
        {
            get { return _addVideo; }
            set
            {
                _addVideo = value;
                RaisePropertyChanged();
            }
        }

        public string ConsoleText
        {
            get { return _consoleText; }
            set
            {
                _consoleText = value;
                RaisePropertyChanged();
            }
        }
        
        public RelayCommand AddCategory
        {
            get { return _addCategory; }
            set
            {
                _addCategory = value;
                RaisePropertyChanged();
            }
        }
    }
}
