using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using RecUber.Interface;
using RecUber.Model;
using System;
using System.Collections.Specialized;
using System.Windows.Input;

namespace RecUber.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private IRepository<Entry> _repository;

        public MainViewModel()
        {
            // Repositorio.
            _repository = App.Current.Services!.GetService<IRepository<Entry>>()!;

            // Instanciar el objeto que contiene la información del Header de la Home.
            _header = App.Current.Services!.GetService<HeaderInformationViewModel>()!;

            // Cargar Registros del día actual.
            RecordCollection = new();
            RecordCollection.CollectionChanged += RecordCollection_CollectionChanged;
        }

        private void RecordCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _header.LoadInitialData(RecordCollection);
        }

        private HeaderInformationViewModel _header;
        public HeaderInformationViewModel Header
        {
            get => _header;
            set => _header = value;
        }

        private AddMenuViewModel? _addMenu;
        public AddMenuViewModel AddMenu
        {
            get => _addMenu!;
            set
            {
                _addMenu = value;
                NotifyPropertyChanged();
            }
        }

        public RecordCollection RecordCollection { get; set; }

        private IRecord? _currentRecord;
        public IRecord CurrentRecord
        {
            get => _currentRecord!;
            set
            {
                _currentRecord = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isOpenAddPopup = false;
        public bool IsOpenAddPopup
        {
            get => _isOpenAddPopup;
            set
            {
                _isOpenAddPopup = value;
                NotifyPropertyChanged();
            }
        }
    }

    public partial class MainViewModel
    {
        private ICommand? _openAddPopup;
        public ICommand OpenAddPopup => _openAddPopup ??= new RelayCommand(() =>
        {
            IsOpenAddPopup = !IsOpenAddPopup;
        });

        private ICommand? _openAddMenu;
        public ICommand OpenAddMenu => _openAddMenu ??= new AsyncRelayCommand<string>(async (mode) =>
        {
            IsOpenAddPopup = false;
            
            switch (mode)
            {
                case "entry":
                    AddMenu = new AddMenuViewModel(InsertMode.Entry, SelectedDate, RecordCollection);
                    break;
                case "egress":
                    AddMenu = new AddMenuViewModel(InsertMode.Egress, SelectedDate, RecordCollection) { DisableControl = false };
                    break;
                default:
                    break;
            }

            while (AddMenu.IsOpenAddMenu) { await System.Threading.Tasks.Task.Delay(10); }

            AddMenu = null!;
        });

        
    }
    
    
}
