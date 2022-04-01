using RecUber.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecUber.Model
{
    public class RecordCollection : ObservableCollection<IRecord>
    {
        public RecordCollection() { }
        public RecordCollection(IEnumerable<IRecord> records) : base(records) { }
        public RecordCollection(IList<IRecord> records) : base(records) { }
    }
}
