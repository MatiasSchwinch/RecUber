using RecUber.Model;
using System;

namespace RecUber.Interface
{
    public interface IRecord
    {
        RecordType Type { get; }
        DateTime Date { get; set; }
        string Details { get; set; }
        decimal TotalValue { get; set; }
    }
}
