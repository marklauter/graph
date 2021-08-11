using System;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("key")]
    public sealed class GmlKey
        : GmlElement
    {
        internal GmlKey() : base() { }

        internal GmlKey(
            GmlKeyTarget target,
            string name,
            string type,
            string defaultValue)
        {
            this.Target = target;
            this.Name = name;
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Id = Guid.NewGuid();
        }

        [XmlAttribute("for")]
        public GmlKeyTarget Target { get; set; }

        [XmlAttribute("attr.name")]
        public string Name { get; set; }

        [XmlAttribute("attr.type")]
        public string Type { get; set; }

        [XmlText]
        public string DefaultValue { get; set; }
    }
}
