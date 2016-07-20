using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;

namespace FiveMin.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _editCommand;
        private ObservableCollection<VsCompetition> _competitions;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<VsEntity> _entities;
        private CompetitionViewModel _newCompetition;
        private RelayCommand _addCompetition;
        private EntityViewModel _newEntity;
        private CategoryViewModel _newCategory;
        private RelayCommand _addEntity;
        private RelayCommand _addCategory;
        private RelayCommand _refreshCommand;
        private Category _selectedCategory;
        private VsCompetition _selectedCompetition;
        private VsEntity _entity1;
        private VsEntity _entity2;
        private string _consoleText;

        public MainViewModel()
        {
            InitializeCollections();

            AddCompetition = new RelayCommand(AddCompetitionExecute);
            AddEntity = new RelayCommand(AddEntityExecute);
            AddCategory = new RelayCommand(AddCategoryExecute);
            RefreshCommand = new RelayCommand(RefreshAll);
            NewCompetition = new CompetitionViewModel();
            NewEntity = new EntityViewModel();
            NewCategory = new CategoryViewModel();
        }
        
        private void RefreshAll()
        {
            PopulateCompetitions();
            PopulateCategories();
            PopulateEntities();
        }

        private async void AddCategoryExecute()
        {
            if (NewCategory != null)
            {
                var category = new Category
                {
                    Name = NewCategory.Name,
                    SmallIconUrl = NewCategory.SmallIconUrl,
                    Description = NewCategory.Description,
                    BackdropUrl = NewCategory.BackdropUrl
                };

                await FirebaseManager.Instance.AddCategory(category);

                Categories.Add(category);

                NewCategory = new CategoryViewModel();
            }
        }

        private async void AddEntityExecute()
        {
            if (NewEntity != null)
            {
                var entity = new VsEntity
                {
                    Name = NewEntity.Name,
                    ImageUrl = NewEntity.ImageUrl,
                    Description = NewEntity.Description,
                    WikiLink = NewEntity.WikiLink
                };

                await FirebaseManager.Instance.AddEntity(entity);

                Entities.Add(entity);

                NewEntity = new EntityViewModel();
            }
        }

        private async void AddCompetitionExecute()
        {
            if (NewCompetition != null)
            {
                var competition = new VsCompetition
                {
                    Name = NewCompetition.Name,
                    Category = SelectedCategory.Name,
                    Description = NewCompetition.Description,
                    BackdropUrl = NewCompetition.BackdropUrl,
                    EndingDate = NewCompetition.EndingDate,
                    CompetitorName1 = Entity1.Name,
                    CompetitorName2 = Entity2.Name,
                    CompetitorScore1 = 0,
                    CompetitorScore2 = 0,
                    StartedBy = NewCompetition.StartedBy
                };

                await FirebaseManager.Instance.AddCompetition(competition);

                Competitions.Add(competition);

                NewCompetition = new CompetitionViewModel();
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

        public VsEntity Entity1
        {
            get { return _entity1; }
            set
            {
                _entity1 = value;
                RaisePropertyChanged();
            }
        }

        public VsEntity Entity2
        {
            get { return _entity2; }
            set
            {
                _entity2 = value;
                RaisePropertyChanged();
            }
        }

        private void InitializeCollections()
        {
            _competitions = new ObservableCollection<VsCompetition>();
            _categories = new ObservableCollection<Category>();
            _entities = new ObservableCollection<VsEntity>();

            RefreshAll();
        }

        private async void PopulateCompetitions()
        {
            if (_competitions != null)
            {
                _competitions.Clear();

                var competitions = await FirebaseManager.Instance.GetAllCompetitions();

                foreach (var c in competitions)
                {
                    Competitions.Add(c.Value);
                }
            }
        }

        private async void PopulateCategories()
        {
            if (_categories != null)
            {
                _categories.Clear();

                var categories = await FirebaseManager.Instance.GetAllCategories();

                foreach (var c in categories)
                {
                    Categories.Add(c.Value);
                }
            }
        }

        private async void PopulateEntities()
        {
            if (_entities != null)
            {
                _entities.Clear();

                var entities = await FirebaseManager.Instance.GetAllEntities();

                foreach (var c in entities)
                {
                    Entities.Add(c.Value);
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

        public ObservableCollection<VsCompetition> Competitions
        {
            get { return _competitions; }
            set
            {
                _competitions = value;
                RaisePropertyChanged();
            }
        }

        public VsCompetition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set
            {
                _selectedCompetition = value;
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

        public ObservableCollection<VsEntity> Entities
        {
            get { return _entities; }
            set
            {
                _entities = value;
                RaisePropertyChanged();
            }
        }

        public CompetitionViewModel NewCompetition
        {
            get { return _newCompetition; }
            set
            {
                _newCompetition = value;
                RaisePropertyChanged();
            }
        }

        public EntityViewModel NewEntity
        {
            get { return _newEntity; }
            set
            {
                _newEntity = value;
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

        public RelayCommand AddCompetition
        {
            get { return _addCompetition; }
            set
            {
                _addCompetition = value;
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

        public RelayCommand AddEntity
        {
            get { return _addEntity; }
            set
            {
                _addEntity = value;
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
