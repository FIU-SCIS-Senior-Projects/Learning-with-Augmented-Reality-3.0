/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <MajorDetail. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
following conditions are met:

	Redistributions of source code must retain the above copyright notice, this list of conditions and the
    following disclaimer.
    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
    following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUISNESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


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
