/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <BuildingSystem. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Collections.Generic;

//Creates a Building System Instance
//Holds data for this systems instance
public class BuildingSystem : MonoBehaviour
{
    ///Public Identifier Variables///
    
    public string _name;
    public Transform _root;


    ///Public Data Variables///

    //This Systems Layers
    public List<Transform> layers = new List<Transform>();
    public Dictionary<string, Transform> layerDictionary = new Dictionary<string, Transform>();

    //This Systems MajorComponents
    public List<MajorComponent> majorComponents = new List<MajorComponent>();
    public Dictionary<string, MajorComponent> majorComponentDictionary = new Dictionary<string, MajorComponent>();
    public List<Transform> majorComponentsTransforms = new List<Transform>();
    public List<Collider> majorComponentColliders = new List<Collider>();

    //This Systems SubComponents
    public List<SubComponent> subComponents = new List<SubComponent>();
    public Dictionary<string, SubComponent> subComponentDictionary = new Dictionary<string, SubComponent>();
    public List<Transform> subComponentsTransforms = new List<Transform>();
    public List<Collider> subComponentColliders = new List<Collider>();

    //This Systems ExpandIcons
    public List<ExpandIcon> majorExpandIcons = new List<ExpandIcon>();
    public List<ExpandIcon> subExpandIcons = new List<ExpandIcon>();
    public List<ExpandIcon> expandIcons = new List<ExpandIcon>();
    public Dictionary<string, ExpandIcon> expandIconDictionary = new Dictionary<string, ExpandIcon>();
    //public List<GameObject> expandIconGameObjects = new List<GameObject>();
    //public Dictionary<string, GameObject> expandIconGameObjectDictionary = new Dictionary<string, GameObject>();

    //This Systems Parts
    public List<Transform> parts = new List<Transform>();
    public Transform[] allParts;

    //This Systems RigidBodies
    public List<Rigidbody> bodies = new List<Rigidbody>();

    //This Systems Material Data
    public Dictionary<string, Shader> originalShaders = new Dictionary<string, Shader>();
    public List<Material> materials = new List<Material>();

    //This Systems Buttons
    //public List<Transform> majorButtons = new List<Transform>();
    //public List<Transform> subButtons = new List<Transform>();
    public GameObject majorButtonsParent;
    public GameObject subButtonsParent;


    ///Public State Dependent Variables///

    //This Systems current Toggle Objects
    public List<GameObject> toggleObjects = new List<GameObject>();

    //This Systems current Hide Objects
    public List<GameObject> hideObjects = new List<GameObject>();
    public List<GameObject> extraHideOnIsolate = new List<GameObject>();
    public List<Transform> hideOnTransparent = new List<Transform>();

    //This Systems Clickable Objects
    public List<GameObject> clickable = new List<GameObject>();

    //This Systems current Able Objects
    //public List<Transform> disabledComponents = new List<Transform>();
    public List<GameObject> disableOnIsolate = new List<GameObject>();
    public List<Collider> disableOnIsolateColliders = new List<Collider>();
    public List<Collider> enableOnIsolateColliders = new List<Collider>();
    public List<GameObject> disableOnTransparent = new List<GameObject>();
    public List<Collider> disableOnTransparentColliders = new List<Collider>();


    //Constructor
    public BuildingSystem(string name, Transform root)
    {
        _name = name;
        _root = root;

        //Organize System
        CollectParts();
        CollectParentLayers();
        addColliders();
        //addRigidBody();

        ///Get Data///

        //Major Componets
        getMajorComponetData();
        getMajorComponentsTransformsAndShaders();

        //SubComponents
        getSubComponentData();
        getSubComponentsTransformsAndShaders();

        //State Dependent
        getClickable();
        getDisabledOnIso();
        setButtonsParent();
        getExtraHideObjectsOnIso();
        getDisableOnTrans();
		getHideOnTransparent();
        getExteriorWalls();
        //getEnabledOnIso();

        //ExpandIcons
        //setExpandIcons();
    }
    
    //Get all the parts
    public void CollectParts()
    {
        allParts = _root.GetComponentsInChildren<Transform>();

        //Add them to the list parts except the root transform
        for (int i = 0; i < allParts.Length; i++)
        {
            parts.Add(allParts[i]);
        }
    }
    
    //Add colliders
    public void addColliders()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if(parts[i].GetComponent<Collider>() == null)
            {
                if(parts[i].GetComponent<MeshRenderer>() != null)
                {
                    if(parts[i].tag != "M1")
                    {
                        MeshCollider collider = parts[i].gameObject.AddComponent<MeshCollider>();
                    }
                }
            }
            //collider.convex = true;
        }
    }
    
   //Add rigidBodies
   public void addRigidBody()
   {
       for (int i = 0; i < layers.Count; i++)
       {
            if(layers[i].GetComponent<Rigidbody>() == null)
            {
                Rigidbody body = layers[i].gameObject.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.isKinematic = false;
                bodies.Add(body);
            }   
       }
   }

   //Get parent layers for each part in this system
   public void CollectParentLayers()
   {
		//Debug.Log(_root.name);

       for (int y = 0; y < allParts.Length; y++)
       {
			if(parts[y].parent.name.Equals(_root.name))
            {
                //if this layer dosn't exist add it to list
                if (!layers.Contains(parts[y]))
                {
                    layers.Add(parts[y]);
                    layerDictionary.Add(parts[y].name, parts[y]);
                }
            }
       }
   }

    //Gets the Clickable Objects in this system
    public void getClickable()
    {
		foreach(Transform p in parts)
        {
            if(p.tag == "Clickable")
            {
                clickable.Add(p.gameObject);
				//Debug.Log (p.name);
                
            }
        }
        //Sets them to the Clickable Layer
		foreach (GameObject c in clickable)
		{
			c.layer = 11;
		}
    }

    public void getDisabledOnIso()
    {
        foreach (Transform p in parts)
        {
            if (p.tag == "DisableOnIso")
            {
                disableOnIsolate.Add(p.gameObject);
                //Debug.Log(p.name);
                if(p.gameObject.GetComponent<Collider>() != null)
                {
                    disableOnIsolateColliders.Add(p.gameObject.GetComponent<Collider>());
                }
            }
        }
    }

    public void getSubComponentData()
    {
        foreach (Transform p in parts)
        {
            if (p.GetComponent<SubAnnotate>() != null)
            {
                if (p.gameObject.GetComponent<Collider>() != null)
                {
                    subComponentColliders.Add(p.GetComponent<Collider>());
                }

                //Creates a new MajorComponent Object for each major component in this system
                subComponentDictionary.Add(p.gameObject.name, new SubComponent(p.gameObject.name, p.gameObject.transform));
                subComponents.Add(new SubComponent(p.gameObject.name, p.gameObject.transform));
                //Creates an Expand Icon for each labeled MajorComponent in this system
//                if (p.GetComponent<IconControl>())
//                {
//					subExpandIcons.Add()
//                }
            //Debug.Log(_root.name + " sub collider count: " + subComponentColliders.Count);
			}
		}

        //Disables the subcomponent colliders
        foreach (var item in subComponentColliders)
        {
            item.enabled = false;
        }
    }

    public void getMajorComponetData()
    {
        foreach(Transform p in parts)
        {
            if(p.GetComponent<MajorAnnotate>())
            {
                if (p.gameObject.GetComponent<Collider>() != null)
                {
                    majorComponentColliders.Add(p.gameObject.GetComponent<Collider>());
                    //Creates a new MajorComponent Object for each major component in this system
                }

                MajorComponent mc = new MajorComponent(p.gameObject.name, p.gameObject.transform);
                majorComponentDictionary.Add(p.gameObject.name, mc);
                majorComponents.Add(mc);

                //foreach (MajorComponent item in majorComponents)
                //{
                //    Debug.Log(item._name);
                //}

                //Creates an Expand Icon for each labeled MajorComponent in this system
                if (p.GetComponent<IconControl>())
                {
                    //SubComponet GameObjects for this MajorComponent
                    List<GameObject> temp = new List<GameObject>();
                    temp.Clear();

                    //SubAnnotates for this MajorComponent
                    List<SubAnnotate> sTemp = new List<SubAnnotate>();
                    sTemp.Clear();

                    sTemp.AddRange(p.GetComponentsInChildren<SubAnnotate>());

                    //Adds this MajorComponent's SubComponents
                    for (int i = 0; i < sTemp.Count; i++)
                    {
                        temp.Add(sTemp[i].gameObject);
                        SubComponent newSub = new SubComponent(temp[i].name, temp[i].transform);
                        majorComponentDictionary[p.name].subcomponentDictionary.Add(temp[i].name, newSub);
                        majorComponentDictionary[p.name].subcomponents.Add(newSub);
                        //Debug.Log(majorComponentDictionary[p.name].subcomponents[i]._name);
                    }
                }
            }
        }

        //Debug.Log(_name + " MajorComponent Count: " + majorComponents.Count);
        //Debug.Log(_name + " MajorComponentDictionary Count: " + majorComponentDictionary.Count);
        //Debug.Log(_root.name + " major collider count: " + majorComponentColliders.Count);
    }

    //public void setExpandIcons()
    //{
    //    Debug.Log(_name + " MajorComponent Count: " + majorComponents.Count);

    //    if (majorComponents.Count > 0)
    //    {
    //        //MajorExpandIcons//
    //        foreach (MajorComponent m in majorComponents)
    //        {
    //            //Debug.Log(_name + " MajorComponent: " + m); //will always be null becuase its not a gameObject
    //            Debug.Log(_name + " MajorComponent's Transform: " + m._root.gameObject.name);

    //            if (m._root.GetComponent<IconControl>() != null)
    //            {
    //                //Debug.Log(m._root.gameObject.GetComponent<IconControl>().iconPos);
    //                Transform mIconPos = m._root.gameObject.GetComponent<IconControl>().iconPos;

    //                ExpandIcon mEx = new ExpandIcon(mIconPos, m._name);
    //                expandIconDictionary.Add(m._name, mEx);
    //                expandIcons.Add(mEx);
    //                majorExpandIcons.Add(mEx);

    //                //Adds them to organized
    //                Organized.Instance.expandIconDictionary.Add(m._name, mEx);
    //                Organized.Instance.majorExpandIcons.Add(mEx);
    //                Organized.Instance.expandIcons.Add(mEx);

    //                //ExpandIcon ex = Organized.Instance.expandIconDictionary[p.name];
    //                //expandIconDictionary.Add(p.name, ex);
    //                //expandIcons.Add(ex);
    //                //majorExpandIcons.Add(ex);

    //                //expandIconDictionary.Add(p.name, Organized.Instance.expandIconDictionary[p.name]);
    //                //expandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
    //                //majorExpandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
    //            }
    //        }
    //    }

    //    if (subComponents.Count > 0)
    //    {
    //        Debug.Log(_name + " SubComponent Count: " + subComponents.Count);

    //        //SubExpandIcons//
    //        foreach (SubComponent s in subComponents)
    //        {
    //            //Debug.Log(_name + " SubComponent's Transform: " + s._root.gameObject.name);

    //            if (s._root.GetComponent<IconControl>() != null)
    //            {
    //                Debug.Log("Name of last part: " + s._root.name);
    //                Transform sIconPos = s.GetComponent<IconControl>().iconPos;

    //                ExpandIcon sEx = new ExpandIcon(sIconPos, s._name);
    //                expandIconDictionary.Add(s._name, sEx);
    //                expandIcons.Add(sEx);
    //                subExpandIcons.Add(sEx);

    //                //Adds them to organized
    //                Organized.Instance.expandIconDictionary.Add(s._name, sEx);
    //                Organized.Instance.subExpandIcons.Add(sEx);
    //                Organized.Instance.expandIcons.Add(sEx);

    //                //ExpandIcon ex = Organized.Instance.expandIconDictionary[p.name];
    //                //expandIconDictionary.Add(p.name, ex);
    //                //expandIcons.Add(ex);
    //                //subExpandIcons.Add(ex);

    //                //expandIconDictionary.Add(p.name, Organized.Instance.expandIconDictionary[p.name]);
    //                //expandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
    //                //subExpandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);

    //                //expandIconGameObjects.Add(icon);
    //                //expandIconGameObjectDictionary.Add(p.name, icon);

    //                //Debug.Log(p.name);
    //            }
    //        }
    //    }
    //}

    public void setButtonsParent()
    {
        foreach(Transform p in parts)
        {
            if(p.name.Equals("MajorButtons"))
            {
                majorButtonsParent = p.gameObject;
            }
            if (p.name.Equals("SubButtons"))
            {
                subButtonsParent = p.gameObject;
            }
        }
    }

    public void getDisableOnTrans()
    {
        foreach (Transform p in parts)
        {
            if(p.tag.Equals("DisableOnTrans"))
            {
                if(p.GetComponent<Collider>() != null)
                {
                    disableOnTransparentColliders.Add(p.GetComponent<Collider>());
                }
            }
        }
    }

    public void getExtraHideObjectsOnIso()
    {
        foreach (Transform p in Organized.Instance.parts)
        {
            if (p.tag.Equals("HideOnIso"))
            {
                extraHideOnIsolate.Add(p.gameObject);
            }
        }
    }

    public void getExteriorWalls()
    {
        foreach (Transform p in parts)
        {
            if (p.tag.Equals("Exterior"))
            {
                if (p.GetComponent<Renderer>() != null)
                {
                    originalShaders.Add(p.name, p.GetComponent<Renderer>().material.shader);
                }
            }
        }
    }

    public void getMajorComponentsTransformsAndShaders()
    {
        foreach (Transform p in parts)
        {
            if (p.tag.Equals("MajorComponent"))
            {
                majorComponentsTransforms.Add(p);

                if (p.GetComponent<Renderer>() != null)
                {
                    originalShaders.Add(p.name, p.GetComponent<Renderer>().material.shader);
                }
            }
        }

		foreach (Transform m in majorComponentsTransforms)
		{
			m.gameObject.layer = 8;
		}
    }

    public void getSubComponentsTransformsAndShaders()
    {
        foreach (Transform p in parts)
        {
            if (p.tag.Equals("SubComponent"))
            {
                subComponentsTransforms.Add(p);

                if (p.GetComponent<Renderer>() != null)
                {
                    originalShaders.Add(p.name, p.GetComponent<Renderer>().material.shader);
                }
            }
        }

		foreach (Transform s in subComponentsTransforms)
		{
			s.gameObject.layer = 8;
		}
    }

	public void getHideOnTransparent()
	{
		foreach (Transform p in parts)
		{
			if (p.tag.Equals ("HideOnTrans"))
			{
				//Debug.Log (p.name + " Parent: " + _root.name);
				hideOnTransparent.Add (p);
				//Debug.Log (p.name);
			}
		}
	}

    //public void get materials
    //materials array of each of the fadeObjects material
    //for each of those materials make a new material with low alpha
    //lerp between both
}
