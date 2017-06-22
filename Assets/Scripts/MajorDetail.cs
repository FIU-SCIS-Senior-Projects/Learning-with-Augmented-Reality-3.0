using System.Xml;
using System.Xml.Serialization;

public class MajorDetail
{
    [XmlElement("SystemNumber")]
    public string systemNumber;

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

    [XmlElement("ShadingCoefficient")]
    public string shadingCoefficient;

    [XmlElement("EmbodiedEnergy")]
    public string embodiedEnergy;

    [XmlElement("Attach")]
    public string attach;
}
