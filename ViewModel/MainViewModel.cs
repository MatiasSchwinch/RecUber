using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using RecUber.Interface;
using RecUber.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RecUber.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private IRepository<Entry> _repositoryEntries;
        private IRepository<Egress> _repositoryEgress;

        public MainViewModel()
        {
            // Repositorio.
            _repositoryEntries = App.Current.Services!.GetService<IRepository<Entry>>()!;
            _repositoryEgress = App.Current.Services!.GetService<IRepository<Egress>>()!;

            // Instanciar el objeto que contiene la información del Header de la Home.
            _header = App.Current.Services!.GetService<HeaderInformationViewModel>()!;

            // Cargar Registros del día actual.
            RecordCollection = new();
            RecordCollection!.CollectionChanged += RecordCollection_CollectionChanged;

            Task.Run(async () =>
            {
                await LoadDbRecords();
            }).Wait();
        }

        private void RecordCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _header.UpdateData(RecordCollection);
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
                DeleteRecord.NotifyCanExecuteChanged();
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

                App.Current.Dispatcher.Invoke(async () =>
                {
                    await LoadDbRecords();
                });
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

            while (AddMenu.IsOpenAddMenu) { await Task.Delay(10); }

            AddMenu = null!;
        });

        private IAsyncRelayCommand? _deleteRecord;
        public IAsyncRelayCommand DeleteRecord => _deleteRecord ??= new AsyncRelayCommand(async () =>
        {
            if (MessageBox.Show("¿Esta seguro que desea eliminar este registro de la base de datos?", "¿Estas seguro?", MessageBoxButton.YesNo, MessageBoxImage.Question) is MessageBoxResult.Yes)
            {
                try
                {
                    switch (CurrentRecord)
                    {
                        case Entry entry:
                            await _repositoryEntries.Delete(entry.EntryId);
                            await _repositoryEntries.Save();
                            RecordCollection.Remove(entry);
                            break;
                        case Egress egress:
                            await _repositoryEgress.Delete(egress.EgressId);
                            await _repositoryEgress.Save();
                            RecordCollection.Remove(egress);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "¡Error al intentar eliminar el registro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }, () =>
        {
            return CurrentRecord != null;
        });

        private async Task LoadDbRecords()
        {
            try
            {
                RecordCollection.Clear();

                List<IRecord> list = new();
                list.AddRange(await _repositoryEntries.GetAll(obj => obj.Date.Day == SelectedDate.Day));
                list.AddRange(await _repositoryEgress.GetAll(obj => obj.Date.Day == SelectedDate.Day));

                foreach (var item in list)
                {
                    RecordCollection.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "¡Error al intentar obtener los registro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
    
    
}
