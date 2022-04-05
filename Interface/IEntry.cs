using System;

namespace RecUber.Interface
{
    public interface IEntry : IRecord
    {
        double Duration { get; set; }
        float Distance { get; set; }
    }
}
