using System;
using System.Xml.Serialization;

namespace Graphs.IO.GraphML
{
    [XmlRoot("data")]
    public sealed class GmlData
    {
        internal GmlData() { }

        internal GmlData(Guid key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [XmlAttribute("key")]
        public Guid Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
