using RecUber.Interface;
using System;

namespace RecUber.Model
{
    public class Egress : IEgress
    {
        public int EgressId { get; set; }
        public RecordType Type => RecordType.Egress;

        public DateTime Date { get; set; }
        public string Details { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }

        public Egress(DateTime date, string details, decimal totalValue)
        {
            Date = date;
            Details = details;
            TotalValue = Math.Abs(totalValue) == totalValue ? totalValue * (-1) : totalValue;
        }
    }
}
