using Graph.Elements;
using System;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public abstract class GmlElement
    {
        protected readonly Element Element;

        protected GmlElement() { }

        protected GmlElement(Element element)
        {
            this.Element = element;
            this.Id = element.Id;
        }

        [XmlAttribute("id")]
        public Guid Id { get; set; }
    }
}
