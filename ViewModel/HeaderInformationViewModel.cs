using Microsoft.Extensions.Configuration;
using RecUber.Interface;
using RecUber.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecUber.ViewModel
{
    public sealed class HeaderInformationViewModel : ViewModelBase
    {
        private int _totalTrips;
        public int TotalTrips => _totalTrips;

        private double _totalHours;
        public double TotalHours => _totalHours;
        
        private float _totalDistance;
        public float TotalDistance => _totalDistance;
        
        private float _remainingMileage;
        public float RemainingMileage => _remainingMileage;
        
        private float _remainingFuel;
        public float RemainingFuel => _remainingFuel;
        
        private decimal _totalBalance;
        public decimal TotalBalance => _totalBalance;
        
        private float _fuelPercentage;
        public float FuelPercentage => _fuelPercentage;

        private readonly Config _config;

        public HeaderInformationViewModel(IConfiguration config)
        {
            _config = new();
            config.Bind(_config);
        }

        public void UpdateTotalTrips(int value)
        {
            _totalTrips = value;
            NotifyPropertyChanged(nameof(TotalTrips));
        }
        public void UpdateTotalHours(double value)
        {
            _totalHours = value;
            NotifyPropertyChanged(nameof(TotalHours));
        }
        public void UpdateTotalDistance(float value)
        {
            _totalDistance = value;
            NotifyPropertyChanged(nameof(TotalDistance));
        }
        public void UpdateRemainingMileage(float value)
        {
            _remainingMileage = value;
            NotifyPropertyChanged(nameof(RemainingMileage));
        }
        public void UpdateRemainingFuel(float value)
        {
            _remainingFuel = value;
            _fuelPercentage = (value / _config.FuelTankCapacity) * 100;

            NotifyPropertyChanged(nameof(RemainingFuel));
            NotifyPropertyChanged(nameof(FuelPercentage));
        }
        public void UpdateTotalBalance(decimal value)
        {
            _totalBalance = value;
            NotifyPropertyChanged(nameof(TotalBalance));
        }

        public void UpdateData(IEnumerable<IRecord> list)
        {
            if (list is null || !list.Any()) { Clear(); return; }

            var filteredEntry = list.OfType<IEntry>();
            var filteredEgress = list.OfType<IEgress>();

            // Cantidad de viajes.
            UpdateTotalTrips(filteredEntry.Count());
            
            // Horas totales on-line.
            UpdateTotalHours(filteredEntry.Sum(entry => entry.Duration));

            // Distancia total recorrida.
            var totalDistance = filteredEntry.Sum(entry => entry.Distance);
            UpdateTotalDistance(totalDistance);

            // Litros restantes estimados.
            var remaining = EstimateRemainingMileageAndFuel(EstimateRemainingFuel(filteredEgress.Sum(egress => egress.TotalValue)), totalDistance);
            UpdateRemainingFuel(remaining.fuel);

            // Kilometraje restante estimado.
            UpdateRemainingMileage(remaining.mileage);

            // Balance total.
            UpdateTotalBalance(filteredEntry.Sum(en => en.Profit) + filteredEgress.Sum(eg => eg.TotalValue)/*list.Sum(entry => entry.TotalValue)*/);
        }

        // Limpia todos los valores
        public void Clear()
        {
            UpdateTotalTrips(0);
            UpdateTotalHours(0d);
            UpdateTotalDistance(0f);
            UpdateRemainingFuel(0f);
            UpdateRemainingMileage(0f);
            UpdateTotalBalance(0m);
        }

        // Estima el combustible restante.
        private float EstimateRemainingFuel(decimal amount)
        {
            var fuelPricePerLiter = _config.FuelPricePerLiter;
            var literCharged = Math.Abs(amount) / fuelPricePerLiter;

            return (float)literCharged;
        }

        // Estima el kilometraje y el combustible restante.
        private (float fuel, float mileage) EstimateRemainingMileageAndFuel(float liters, float totalDistance)
        {
            var totalMileageFullTank = _config.TotalMileageFullTank;
            var fuelTankCapacity = _config.FuelTankCapacity;

            var remainingMileage = ((totalMileageFullTank / fuelTankCapacity) * liters) - totalDistance;

            var remainingFuel = remainingMileage / (totalMileageFullTank / fuelTankCapacity);

            return (remainingFuel, remainingMileage);
        }
    }
}
