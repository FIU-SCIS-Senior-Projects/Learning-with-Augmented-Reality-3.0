/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <Transparent. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Linq;
using System.Collections.Generic;

public class Transparent : MonoBehaviour
{
    public Material clearMat;
    public Transform[] objects;
    private Material[] tempMaterials;
    public GameObject whole;
    public bool transparent = false;
    private List<Material> materials = new List<Material>();

    public void ChangeClear(GameObject obj)
    {
        if (transparent == false)
        {
            whole = obj;
            //GetMaterials(whole);
            GetObjects(whole);
            transparent = true;
        }
        if(transparent)
        {
            //GetStoredMaterials
            //ReverseButtonPress
        }
    }

    public void GetObjects(GameObject obj)
    {
        objects = obj.GetComponentsInChildren<Transform>();
        foreach (Transform ob in objects) //(var ob in objects.Where(ob => (ob != transform))) makes sure it dosn't count the parent
        {
            if (ob.gameObject.GetComponent<MeshRenderer>() == null)
            {
                ob.gameObject.AddComponent<MeshRenderer>();
            }
            if (ob != transform) //excludes the parent
            {
                TurnTransparent(ob);
            }
        }
    }

    private void GetMaterials(GameObject obj)
    {
        MeshRenderer[] test = obj.GetComponentsInChildren<MeshRenderer>();
        
        //materials.Add(ob.GetComponent<MeshRenderer>().material);
        for (int i = 0; i < test.Length; i++)
        {
            tempMaterials[i] = test[i].material;
            Debug.Log(tempMaterials[i]);
        }
    }

    private void TurnTransparent(Transform o)
    {
        o.GetComponent<MeshRenderer>().material = clearMat;
    }

    private void ReverseToMaterials(Transform o)
    {
        
        
        //o.GetComponent<MeshRenderer>().material = clearMat;
    }
}
