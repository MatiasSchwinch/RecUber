using RecUber.Interface;
using System;

namespace RecUber.Model
{
    public class Entry : IEntry
    {
        public int EntryId { get; set; }
        public RecordType Type => RecordType.Entry;

        public DateTime Date { get; set; }
        public string Details { get; set; } = string.Empty;
        public double Duration { get; set; }
        public float Distance { get; set; }

        public decimal Fee { get; set; }
        public decimal Profit { get; set; }
        public decimal TotalValue { get; set; }

        public Entry(DateTime date, string details, double duration, float distance, decimal fee, decimal profit, decimal totalValue)
        {
            Date = date;
            Details = details;
            Duration = duration;
            Distance = distance;
            Fee = fee;
            Profit = profit;
            TotalValue = totalValue;
        }
    }
}
