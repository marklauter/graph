using Graph.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public abstract class GmlElement
    {
        protected readonly Element Element;

        protected GmlElement() { }

        protected GmlElement(
            Element element,
            Dictionary<string, GmlKey> keys)
        {
            this.Element = element;
            this.Id = element.Id;

            var classData = element.Classes
                .Select(c => new GmlData(keys["class"].Id, c));

            var attributeData = element.Attributes
                .Select(kvp => new GmlData(keys[kvp.Key].Id, kvp.Value));

            this.Data = classData
                .Union(attributeData)
                .ToList();
        }

        [XmlAttribute("id")]
        public Guid Id { get; set; }

        [XmlElement("data")]
        public List<GmlData> Data { get; set; }
    }
}
