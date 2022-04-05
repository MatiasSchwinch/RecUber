using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using RecUber.Interface;
using RecUber.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace RecUber.ViewModel
{
    public class AddMenuViewModel : ViewModelBase
    {
        // Propiedades para ingresar a la base de datos un Ingreso o Egreso.
        public InsertMode Mode { get; set; }

        #region Propiedades de los Controles.
        private bool _isOpenAddMenu = false;
        public bool IsOpenAddMenu
        {
            get => _isOpenAddMenu;
            set
            {
                _isOpenAddMenu = value;
                NotifyPropertyChanged();
            }
        }

        private string _desc = string.Empty;
        public string Desc
        {
            get => _desc;
            set
            {
                _desc = value;
                NotifyPropertyChanged();
                ConfirmInsertData.NotifyCanExecuteChanged();
            }
        }

        private DateTime _initTime = DateTime.Now;
        public DateTime InitTime
        {
            get => _initTime;
            set
            {
                _initTime = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _finalizeTime = DateTime.Now;
        public DateTime FinalizeTime
        {
            get => _finalizeTime;
            set
            {
                _finalizeTime = value;
                NotifyPropertyChanged();
            }
        }

        private double _distanceTotal;
        public double DistanceTotal
        {
            get => _distanceTotal;
            set
            {
                _distanceTotal = value;
                NotifyPropertyChanged();
                ConfirmInsertData.NotifyCanExecuteChanged();
            }
        }

        private double _totalValue;
        public double TotalValue
        {
            get => _totalValue;
            set
            {
                _totalValue = value;
                NotifyPropertyChanged();
                ConfirmInsertData.NotifyCanExecuteChanged();
            }
        }

        private bool? _applyFee = true;
        public bool? ApplyFee
        {
            get => _applyFee;
            set
            {
                _applyFee = value;
                NotifyPropertyChanged();
            }
        }

        public bool DisableControl { get; set; } = true;
        #endregion

        private readonly IRepository<Entry>? _repositoryEntry;
        private readonly IRepository<Egress>? _repositoryEgress;
        private DateTime _date;
        private readonly RecordCollection _currentListToAdd;

        public AddMenuViewModel(InsertMode mode, DateTime date, RecordCollection currentListToAdd)
        {
            Mode = mode;
            _date = date;
            _currentListToAdd = currentListToAdd;
            
            IsOpenAddMenu = true;

            switch (mode)
            {
                case InsertMode.Entry:
                    _repositoryEntry = App.Current.Services!.GetService<IRepository<Entry>>()!;
                    break;
                case InsertMode.Egress:
                    _repositoryEgress = App.Current.Services!.GetService<IRepository<Egress>>()!;
                    break;
                default:
                    break;
            }
        }

        public bool VerifyData()
        {
            if (string.IsNullOrWhiteSpace(Desc)) return false;
            if (DistanceTotal <= 0 && Mode == InsertMode.Entry) return false;
            if (TotalValue <= 0) return false;

            return true;
        }

        public async Task<IRecord> InsertData(DateTime currentDate)
        {
            IRecord record;
            switch (Mode)
            {
                case InsertMode.Entry:
                    record = new Entry(currentDate, Desc, FinalizeTime.Subtract(InitTime).TotalHours, (float)DistanceTotal, (decimal)TotalValue);
                    await _repositoryEntry!.Insert((Entry)record);
                    await _repositoryEntry.Save();
                    _currentListToAdd.Add(record);
                    break;
                case InsertMode.Egress:
                    record = new Egress(currentDate, Desc, (decimal)TotalValue);
                    await _repositoryEgress!.Insert((Egress)record);
                    await _repositoryEgress.Save();
                    _currentListToAdd.Add(record);
                    break;
                default:
                    throw new Exception("Error al insertar datos");
            }
            
            return record;
        }

        private IAsyncRelayCommand? _confirmInsertData;
        public IAsyncRelayCommand ConfirmInsertData => _confirmInsertData ??= new AsyncRelayCommand(async () =>
        {
            try
            {
                await InsertData(_date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "¡Se ha producido un error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsOpenAddMenu = false;
            }
            
        }, () => VerifyData());
    }

    public enum InsertMode
    {
        Entry,
        Egress
    }
}
