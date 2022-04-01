using System;

namespace RecUber.Interface
{
    public interface IEntry : IRecord
    {
        TimeSpan Duration { get; set; }
        float Distance { get; set; }
    }
}
