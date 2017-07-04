/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <SubComponent. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

using UnityEngine;
using System.Collections;

public class SubComponent : MonoBehaviour
{

    public string _name;
    public Transform _root;

    public bool hasIcon;
    public IconControl iconControl;
    //public ExpandIcon icon;
    //public Transform zoomPos;

    public SubComponent(string name, Transform root)
    {
        _name = name;
        _root = root;

        collectIcon();
    }

    //Checks to see if this component has an ExpandIcon
    public void collectIcon()
    {
        if (_root.gameObject.GetComponent<IconControl>() == null)
        {
            hasIcon = false;
        }
        else
        {
            hasIcon = true;
            iconControl = _root.gameObject.GetComponent<IconControl>();
            if (!iconControl.zoomPos)
            {
                //GameObject z = new GameObject();
                //Instantiate(z);
                //z.transform.parent = _root;
                //iconControl.zoomPos.position = _root.gameObject.transform.position;
                //iconControl.zoomPos.Translate(0, 5f, 5f);
                //Debug.Log(iconControl.zoomPos.localPosition);
            }
            //Debug.Log(iconControl.zoomPos.localPosition);
            //Debug.Log(hasIcon);
        }

    }

    //public ExpandIcon getExpandIcon()
    //{
    //    return icon;
    //}

    void Start()
    {

    }

}
