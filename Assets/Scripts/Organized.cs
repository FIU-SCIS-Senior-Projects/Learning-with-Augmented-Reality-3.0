/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <Organized. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

//Singleton
//Collects a list of Systems from a root parent at Runtime
//Loads all needed assets and gameObject references
public class Organized : MonoBehaviour
{
    private static Organized instance;

    //Inspector
    [HideInInspector]
    public Transform baseRoot;

    //Hidden Public Database Arrays
    [HideInInspector]
    public Transform[] allParts;

    //Hidden Public DataBase Lists
    [HideInInspector]
    public List<GameObject> panels = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> returnButtons = new List<GameObject>();
    [HideInInspector]
    public List<Transform> systems = new List<Transform>();
    [HideInInspector]
    public List<BuildingSystem> systemsList = new List<BuildingSystem>();
    [HideInInspector]
    public List<Transform> layers = new List<Transform>();
    [HideInInspector]
    public List<Transform> parts = new List<Transform>();
    [HideInInspector]
    public List<Rigidbody> bodies = new List<Rigidbody>();
    [HideInInspector]
    public List<ExpandIcon> majorExpandIcons = new List<ExpandIcon>();
    [HideInInspector]
    public List<ExpandIcon> subExpandIcons = new List<ExpandIcon>();
    [HideInInspector]
    public List<ExpandIcon> expandIcons = new List<ExpandIcon>();


    //Hidden Public DataBase Dictionaries
    [HideInInspector]
    public Dictionary<string, BuildingSystem> systemDictionary = new Dictionary<string, BuildingSystem>();
    [HideInInspector]
    public Dictionary<string, GameObject> returnButtonDictionary = new Dictionary<string, GameObject>();
    [HideInInspector]
    public Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();
    [HideInInspector]
    public Dictionary<string, ExpandIcon> expandIconDictionary = new Dictionary<string, ExpandIcon>();

    //Hidden Public Assets
    [HideInInspector]
    public Material updateSky;
    [HideInInspector]
    public Material sunnySky;  

    //Hidden Public In-Game Objects
    [HideInInspector]
    public GameObject environment;
    [HideInInspector]
    public GameObject animations;
    [HideInInspector]
    public GameObject worldsCanvas;
    [HideInInspector]
    public GameObject diagramButtons;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Material defaultSky;
	[HideInInspector]
	public IEnvironmentState globalCurrentState;

    //Private Update Variables
    private List<bool> tempActiveSelf = new List<bool>();

    //Singleton Constructor
    public static Organized Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Organized>();

                if (instance == null)
                {
                    GameObject go = new GameObject("Organized");
                    instance = go.AddComponent<Organized>();
                    Debug.Log("Singleton created!");
                }
            }
            return instance;
        }
    }

	public GameObject findGameObjectWithTag(string tagName)
	{
		GameObject go = null;

		if (GameObject.FindGameObjectWithTag (tagName) != null) {
			go = GameObject.FindGameObjectWithTag (tagName);
		} else {
			Debug.Log ("There is no gameobject with the, (" + tagName + ") tag. A Temporary game object was created in its place. " +
				"Please Add the, (" + tagName + ") game object with the (" + tagName + ") tag name.");
			
			go = new GameObject ("Temporary " + tagName);
			go.tag = (tagName);
		}

		return go;
	}

	public GameObject findGameObject(string gameObjectName)
	{
		GameObject go = null;

		if (GameObject.Find (gameObjectName) != null) {
			go = GameObject.Find (gameObjectName);
		} else {
			Debug.Log ("There is no game object named, (" + gameObjectName + "). Please Add a, (" + gameObjectName + ") game object.");
			go = new GameObject ("Temporary " + gameObjectName);
		}

		return go;
	}

    void Awake()
    {
        //Initialize In-Game Objects

        //baseRoot = GameObject.FindGameObjectWithTag("BaseRoot").transform;
		//player = GameObject.FindGameObjectWithTag("Player");
        
		baseRoot = findGameObjectWithTag ("BaseRoot").transform;
		player = findGameObjectWithTag("Player");

//		environment = GameObject.Find("Environment");
//		animations = GameObject.Find("Animations");
//		worldsCanvas = GameObject.Find("WorldCanvas");
//		diagramButtons = GameObject.Find("DiagramButtons");
//		player = GameObject.FindGameObjectWithTag("Player");

		environment = findGameObject("Environment");
		animations = findGameObject("Animations");
		worldsCanvas = findGameObject("WorldCanvas");
		diagramButtons = findGameObject("DiagramButtons");

        //Initializes the Skyboxes
        defaultSky = RenderSettings.skybox;
        //Debug.Log(defaultSky.name);
        updateSky = (Material)Resources.Load("Skybox (Gradient)");
        sunnySky = (Material)Resources.Load("Sunny1 Skybox");
        //Debug.Log(sunnySky.name);

        //Organize
        CollectParts();
        CollectParentSystems();
        CollectPanels();
        CollectReturnButtons();
        setExpandIcons();
    }

    //Get all the parts
    public void CollectParts()
    {
		if (baseRoot != null) {
			allParts = baseRoot.GetComponentsInChildren<Transform> ();

			//Add them to the list parts except the root transform
			for (int i = 0; i < allParts.Length; i++) {
				if (allParts [i].transform.parent != null) {
					parts.Add (allParts [i]);
				}
			}
			//Debug.Log(parts.Count);
		} else {
			Debug.Log ("Please add the (BaseRoot) tag to the parent game object who's children you would like to organize");
		}
    }

    //Get Parent Systems for each part
    public void CollectParentSystems()
    {
        //Creates a List of all the system transforms
        for (int y = 0; y < parts.Count; y++)
        {
            if (parts[y].parent != null)
            {
                if (parts[y].parent.name.Equals(baseRoot.name))
                {
                    //if this layer dosn't exist add it to list
                    if (!systems.Contains(parts[y]))
                    {
                        systems.Add(parts[y]);
                    }
                }
            }
        }
        //Gets all the systems in the environment, and adds them to the dictionary and list
        foreach (Transform s in systems)
        {
            BuildingSystem newSystem = new BuildingSystem(s.gameObject.name, s);
            systemDictionary.Add(newSystem._name, newSystem);
            //Debug.Log("System: " + systemDictionary[newSystem._name]._name + ", has " + systemDictionary[newSystem._name].layers.Count + " layers.");
            systemsList.Add(newSystem);
        }
        //systemDictionary["E2"].layerDictionary["Plane120"].Translate(Vector3.forward * 10f); test for accessing specific information

		addTags ();
    }

    public void CollectPanels()
    {
        GameObject[] panelTemp = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject p in panelTemp)
        {
            panelDictionary.Add(p.name, p);
            panels.Add(p);
        }
    }

    public void CollectReturnButtons()
    {
        Transform rootTest = null;
        GameObject[] returnTemp = GameObject.FindGameObjectsWithTag("Return");
        //Debug.Log(returnTemp.Length);

        //Return Buttons List & Dictionary//
        for (int r = 0; r < returnTemp.Length; r++)
        {
            if (!returnButtons.Contains(returnTemp[r]))
            {
                returnButtons.Add(returnTemp[r]);
            }
            rootTest = returnTemp[r].transform.parent;
            //Debug.Log("Return Button " + r + "'s root: " + returnTemp[r].transform.root.name);
            //Debug.Log("Return Button " + r + "'s parent: " + rootTest);

            while (rootTest != returnTemp[r].transform.root)
            {
                if (rootTest.parent == returnTemp[r].transform.root)
                {
                    //Debug.Log("Return Button " + (r+1) + "'s parent layer: " + rootTest.name);

                    if (!returnButtonDictionary.ContainsKey(rootTest.name))
                    {
                        returnButtonDictionary.Add(rootTest.name, returnTemp[r]);
                        //Debug.Log("Return Button " + (r + 1) + "'s parent layer in dictionary: " + returnButtonDictionary[rootTest.name]);
                    }

                    //Debug.Log(returnButtonDictionary.Count);
                }
                rootTest = rootTest.parent;
            }
        }
    }

    public void setExpandIcons()
    {
        //Debug.Log(_name + " MajorComponent Count: " + majorComponents.Count);

        foreach (BuildingSystem bs in systemsList)
        {
            if (bs.majorComponents.Count > 0)
            {
                //MajorExpandIcons//
                foreach (MajorComponent m in bs.majorComponents)
                {
                    //Debug.Log(_name + " MajorComponent: " + m); //will always be null becuase its not a gameObject
                    //Debug.Log(bs._name + " MajorComponent's Transform: " + m._root.gameObject.name);

                    if (m._root.GetComponent<IconControl>() != null)
                    {
                        //Debug.Log(m._root.gameObject.GetComponent<IconControl>().iconPos);
                        Transform mIconPos = m._root.gameObject.GetComponent<IconControl>().iconPos;

                        ExpandIcon mEx = new ExpandIcon(mIconPos, m._name);
                        bs.expandIconDictionary.Add(m._name, mEx);
                        bs.expandIcons.Add(mEx);
                        bs.majorExpandIcons.Add(mEx);

                        //Adds them to organized
                        expandIconDictionary.Add(m._name, mEx);
                        majorExpandIcons.Add(mEx);
                        expandIcons.Add(mEx);

                        //ExpandIcon ex = Organized.Instance.expandIconDictionary[p.name];
                        //expandIconDictionary.Add(p.name, ex);
                        //expandIcons.Add(ex);
                        //majorExpandIcons.Add(ex);

                        //expandIconDictionary.Add(p.name, Organized.Instance.expandIconDictionary[p.name]);
                        //expandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
                        //majorExpandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
                    }
                }
            }

            if (bs.subComponents.Count > 0)
            {
                //Debug.Log(bs._name + " SubComponent Count: " + bs.subComponents.Count);

                //SubExpandIcons//
                foreach (SubComponent s in bs.subComponents)
                {
                    //Debug.Log(_name + " SubComponent's Transform: " + s._root.gameObject.name);

                    if (s._root.GetComponent<IconControl>() != null)
                    {
                        //Debug.Log("Name of last part: " + s._root.name);
						Transform sIconPos = s._root.gameObject.GetComponent<IconControl>().iconPos;

                        ExpandIcon sEx = new ExpandIcon(sIconPos, s._name);
                        bs.expandIconDictionary.Add(s._name, sEx);
                        bs.expandIcons.Add(sEx);
                        bs.subExpandIcons.Add(sEx);

                        //Adds them to organized
                        expandIconDictionary.Add(s._name, sEx);
                        subExpandIcons.Add(sEx);
                        expandIcons.Add(sEx);

                        //ExpandIcon ex = Organized.Instance.expandIconDictionary[p.name];
                        //expandIconDictionary.Add(p.name, ex);
                        //expandIcons.Add(ex);
                        //subExpandIcons.Add(ex);

                        //expandIconDictionary.Add(p.name, Organized.Instance.expandIconDictionary[p.name]);
                        //expandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);
                        //subExpandIcons.Add(Organized.Instance.expandIconDictionary[p.name]);

                        //expandIconGameObjects.Add(icon);
                        //expandIconGameObjectDictionary.Add(p.name, icon);

                        //Debug.Log(p.name);
                    }
                }
            }
        }        
    }

    public void CollectExpandIcons()
    {
        //Creates a List of all the system transforms
        for (int y = 0; y < parts.Count; y++)
        {
            if (parts[y].GetComponent<IconControl>())
            {
                if (parts[y].tag == "MajorComponent")
                {
                    Transform iconPos = parts[y].GetComponent<IconControl>().iconPos;
                    ExpandIcon ex = new ExpandIcon(iconPos, parts[y].name);

                    expandIconDictionary.Add(parts[y].name, ex);
                    majorExpandIcons.Add(ex);
                    expandIcons.Add(ex);
                }

                if (parts[y].tag == "SubComponent")
                {
                    Transform iconPos = parts[y].GetComponent<IconControl>().iconPos;
                    ExpandIcon ex = new ExpandIcon(iconPos, parts[y].name);

                    expandIconDictionary.Add(parts[y].name, ex);
                    subExpandIcons.Add(ex);
                    expandIcons.Add(ex);
                }
            }
        }
        //Debug.Log(expandIcons.Count);
    }

    //Adds the parent tag to the clickable objects
    public void addTags()
    {
        foreach (BuildingSystem s in systemsList)
        {
            //Debug.Log("SystemName: " + s._name);
            if (s.clickable.Count != 0)
            {
                for (int i = 0; i < s.clickable.Count; i++)
                {
                    //Debug.Log("SystemTag: " + s._root.tag);
                    s.clickable[i].tag = s._root.tag;
                    //Debug.Log(s._root.name); // = VR_Heavy
                    //Debug.Log(s.clickable[i].name);

                    //The expection tags for VR_Heavy Exterior Detail Components
                    if (s._root.name.Equals("VR_Heavy"))
                    {
                        for (int x = 0; x < s.parts.Count; x++)
                        {
                            //If we have some clickable exterior walls that are in VR_Heavy instead of thier appropriate BuildingSystems
                            if (s.parts[x].parent.name.Equals("E1Detail") || s.parts[x].parent.name.Equals("E2Detail") || s.parts[x].parent.name.Equals("E3Detail")
                                || s.parts[x].parent.name.Equals("E4Detail") || s.parts[x].parent.name.Equals("E5Detail"))
                            {
                                s.parts[x].tag = s.parts[x].parent.name.Remove(2);
                            }
                        }
                    }
                    else
                    {
                        s.clickable[i].tag = s._root.tag;
                    }
                }
            }
        }
    }


    ///GLOBAL METHODS///

    //Toggles//

    public void toggle(List<ExpandIcon> hide)
    {
        for (int i = 0; i < hide.Count; i++)
        {
            if (hide[i].icon.activeSelf == true)
            {
                //Debug.Log("active is true");
                hide[i].icon.SetActive(false);
            }
            else
            {
                //Debug.Log("active is false");
                hide[i].icon.SetActive(true);
            }
        }
    }

    public void toggle(List<BuildingSystem> hide)
    {
        for (int i = 0; i < hide.Count; i++)
        {
            if (hide[i]._root.gameObject.activeSelf == true)
            {
                //Debug.Log("active is true");
                hide[i]._root.gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("active is false");
                hide[i]._root.gameObject.SetActive(true);
            }
        }
    }

    public void toggle(List<GameObject> hide)
    {
        for (int i = 0; i < hide.Count; i++)
        {
            if (hide[i].activeSelf == true)
            {
                //Debug.Log("active is true");
                hide[i].SetActive(false);
            }
            else
            {
                //Debug.Log("active is false");
                hide[i].SetActive(true);
            }
        }
    }

    public void toggle(GameObject hide)
    {
        if (hide.activeSelf == true)
        {
            //Debug.Log("active is true");
            hide.SetActive(false);
        }
        else
        {
            //Debug.Log("active is false");
            hide.SetActive(true);
        }
    }


    public void toggle(Transform hide)
    {
        if (hide.gameObject.activeSelf == true)
        {
            //Debug.Log("active is true");
            hide.gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("active is false");
            hide.gameObject.SetActive(true);
        }
    }

    //Hides//
    public void hide(List<Transform> hide)
    {
        tempActiveSelf.Clear();

        //hide all
        for (int i = 0; i < hide.Count; i++)
        {
            tempActiveSelf.Add(hide[i].gameObject.activeSelf);
            hide[i].gameObject.SetActive(false);
        }
    }

    public void hide(List<GameObject> hide)
    {
        tempActiveSelf.Clear();

        //hide all
        for (int i = 0; i < hide.Count; i++)
        {
            tempActiveSelf.Add(hide[i].activeSelf);
            hide[i].SetActive(false);
        }
    }

    //Unhides//

    public void unHide(List<GameObject> hide)
    {
        //unhide all
        for (int i = 0; i < hide.Count; i++)
        {
            hide[i].SetActive(tempActiveSelf[i]);
        }
    }

    public void changeSkybox()
    {
        if (RenderSettings.skybox == sunnySky)
        {
            //Debug.Log("Update Sky");
            RenderSettings.skybox = updateSky;
        }
        else
        {
            //Debug.Log("Sunny Sky");
            RenderSettings.skybox = sunnySky;
        }
    }
}