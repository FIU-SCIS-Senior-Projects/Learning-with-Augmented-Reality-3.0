using System.Xml;
using System.Xml.Serialization;

public class SubDetail
{
    [XmlAttribute("MajorDetail")]
    public string majorComponent;

    [XmlElement("Name")]
    public string name;

    [XmlElement("Material")]
    public string material;

    [XmlElement("Size")]
    public string size;

    [XmlElement("Description")]
    public string description;

    [XmlElement("RValue")]
    public string rValue;

    [XmlElement("Attach")]
    public string attach;
}
