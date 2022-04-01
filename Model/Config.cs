using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecUber.Model
{
    public class Config
    {
        public const string SectionName = "UserConfig";

        public string Username { get; set; } = string.Empty;
        public float UberFee { get; set; }
        public float FuelTankCapacity { get; set; }
        public float TotalMileageFullTank { get; set; }
        public decimal FuelPricePerLiter { get; set; }
    }
}
