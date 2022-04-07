using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.Input;
using RecUber.Model;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RecUber.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly Config _cfgRecord = new();
        private Config _cfgCurrent = new();

        public ConfigurationViewModel(IConfiguration cfg)
        {
            cfg.Bind(_cfgRecord);
            cfg.Bind(_cfgCurrent);
        }

        private IRelayCommand? _save;
        public IRelayCommand Save => _save ??= _save = new RelayCommand(() =>
        {
            try
            {
                var serialized = JsonSerializer.Serialize(_cfgCurrent);
                File.WriteAllText("config.json", serialized);

                IsOpen = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }, () =>
        {
            return !_cfgCurrent.Equals(_cfgRecord);
        });


        private bool _isOpen = false;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                NotifyPropertyChanged();
            }
        }

        public string UserName
        {
            get => _cfgCurrent.Username;
            set
            {
                _cfgCurrent.Username = value;
                NotifyPropertyChanged();
                Save.NotifyCanExecuteChanged();
            }
        }

        public float FuelTankCapacity
        {
            get => _cfgCurrent.FuelTankCapacity;
            set
            {
                _cfgCurrent.FuelTankCapacity = value;
                NotifyPropertyChanged();
                Save.NotifyCanExecuteChanged();
            }
        }

        public float TotalMileageFullTank
        {
            get => _cfgCurrent.TotalMileageFullTank;
            set
            {
                _cfgCurrent.TotalMileageFullTank = value;
                NotifyPropertyChanged();
                Save.NotifyCanExecuteChanged();
            }
        }

        public decimal FuelPricePerLiter
        {
            get => _cfgCurrent.FuelPricePerLiter;
            set
            {
                _cfgCurrent.FuelPricePerLiter = value;
                NotifyPropertyChanged();
                Save.NotifyCanExecuteChanged();
            }
        }

        public float UberFee
        {
            get => _cfgCurrent.UberFee;
            set
            {
                _cfgCurrent.UberFee = value;
                NotifyPropertyChanged();
                Save.NotifyCanExecuteChanged();
            }
        }
    }
}
