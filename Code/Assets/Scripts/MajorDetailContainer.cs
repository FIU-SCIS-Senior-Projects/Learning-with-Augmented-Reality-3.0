using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("DetailCollection")]   
public class MajorDetailContainer
{
    [XmlArray("Details")]       
    [XmlArrayItem("SystemNumber")]    
    public List<MajorDetail> majorDetails = new List<MajorDetail>(); 

	public static MajorDetailContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer seralizer = new XmlSerializer(typeof(MajorDetailContainer));

        StringReader reader = new StringReader(_xml.text);

        MajorDetailContainer majorDetails = seralizer.Deserialize(reader) as MajorDetailContainer;

        reader.Close();

        return majorDetails;
    }
}
