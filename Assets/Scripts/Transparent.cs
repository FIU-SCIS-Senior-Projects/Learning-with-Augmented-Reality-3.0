using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Transparent : MonoBehaviour
{
    //public Material clearMat;
    //public Transform[] objects;
    //private Material[] tempMaterials;
    //public GameObject whole;
    //public bool transparent = false;
    //private List<Material> materials = new List<Material>();

    //public void ChangeClear(GameObject obj)
    //{
    //    if (transparent == false)
    //    {
    //        whole = obj;
    //        //GetMaterials(whole);
    //        GetObjects(whole);
    //        transparent = true;
    //    }
    //    if(transparent)
    //    {
    //        //GetStoredMaterials
    //        //ReverseButtonPress
    //    }
    //}

    //public void GetObjects(GameObject obj)
    //{
    //    objects = obj.GetComponentsInChildren<Transform>();
    //    foreach (Transform ob in objects) //(var ob in objects.Where(ob => (ob != transform))) makes sure it dosn't count the parent
    //    {
    //        if (ob.gameObject.GetComponent<MeshRenderer>() == null)
    //        {
    //            ob.gameObject.AddComponent<MeshRenderer>();
    //        }
    //        if (ob != transform) //excludes the parent
    //        {
    //            TurnTransparent(ob);
    //        }
    //    }
    //}

    //private void GetMaterials(GameObject obj)
    //{
    //    MeshRenderer[] test = obj.GetComponentsInChildren<MeshRenderer>();
        
    //    //materials.Add(ob.GetComponent<MeshRenderer>().material);
    //    for (int i = 0; i < test.Length; i++)
    //    {
    //        tempMaterials[i] = test[i].material;
    //        Debug.Log(tempMaterials[i]);
    //    }
    //}

    //private void TurnTransparent(Transform o)
    //{
    //    o.GetComponent<MeshRenderer>().material = clearMat;
    //}

    //private void ReverseToMaterials(Transform o)
    //{
        
        
    //    //o.GetComponent<MeshRenderer>().material = clearMat;
    //}
}
