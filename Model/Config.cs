using System;

namespace RecUber.Model
{
    public class Config : IEquatable<Config>
    {
        public string Username { get; set; } = string.Empty;
        public float UberFee { get; set; }
        public float FuelTankCapacity { get; set; }
        public float TotalMileageFullTank { get; set; }
        public decimal FuelPricePerLiter { get; set; }

        public bool Equals(Config? other)
        {
            if (other is null) return false;

            return Username.Equals(other.Username) &&
                UberFee.Equals(other.UberFee) &&
                FuelTankCapacity.Equals(other.FuelTankCapacity) &&
                TotalMileageFullTank.Equals(other.TotalMileageFullTank) &&
                FuelPricePerLiter.Equals(other.FuelPricePerLiter);
        }
    }
}
