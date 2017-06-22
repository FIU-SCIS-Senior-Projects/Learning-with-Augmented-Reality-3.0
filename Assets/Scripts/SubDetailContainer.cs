using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("DetailCollection")]
public class SubDetailContainer
{
    [XmlArray("Details")]
    [XmlArrayItem("MajorDetail")]
    public List<SubDetail> subDetails = new List<SubDetail>();

    public static SubDetailContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer seralizer = new XmlSerializer(typeof(SubDetailContainer));

        StringReader reader = new StringReader(_xml.text);

        SubDetailContainer subDetails = seralizer.Deserialize(reader) as SubDetailContainer;

        reader.Close();

        return subDetails;
    }
}
