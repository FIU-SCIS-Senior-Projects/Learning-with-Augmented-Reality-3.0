using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

//Organizes the Objects in the Environment
//Initializes all the States for the Environment
//Holds all State Current Variables
public class StatePatternEnvironment : MonoBehaviour
{
    ///GamePlay Variables///

    public bool lockOn;

    ///State Variables///
    
    [HideInInspector]
    public IEnvironmentState currentState;
    [HideInInspector]
    public MainState mainState;
    [HideInInspector]
    public MajorComponentState majorComponentState;
    [HideInInspector]
    public SubComponentState subComponentState;
    [HideInInspector]
    public DiagramState diagramState;
    [HideInInspector]
    public BuildState buildState;
    [HideInInspector]
    public MechanicalState mechanicalState;
    [HideInInspector]
    public MechanicalRoomState mechanicalRoomState;
    [HideInInspector]
    public IconState iconState;

    [HideInInspector]
    public MajorAnnotate majorAnnotate;


    ///Static Set at Start Variables///

    //Disbables Colliders with this tag
    public List<Transform> disableComponents = new List<Transform>(); //might have to be moved to BuildingSystem
    //Sets Mechanical Room Doors with tag("M1") -> Should change tag to MechanicalRoomDoors
    public List<Transform> mechanicalRoomDoors = new List<Transform>(); //might have to be moved to BuildingSystem

    
    ///Update Variables///
    
    [HideInInspector]
    public float timer = 0f;
    [HideInInspector]
    public bool isMoving;


    ///Current Variables///
    
    //System//
    public string currentSystemName;

    //MajorComponent
    public GameObject currentMajorComponentGameObject;
        
    //ExpandIcon//
    public ExpandIcon currentExpandIcon;
    public GameObject currentExpandIconGameObject;

    //Player Camera//
    public bool MobileOn;
    public Transform currentPlayerPos;
    public GameObject cameraHead;
    public Vector3 cameraHeadSavedPos;
    public Vector3 tempPlayerPos;
    private GameObject cameraDesktopController;
    private GameObject cameraVRController;
	private GameObject cameraController;
    private GameObject cameraDesktop;
    private GameObject cameraVR;


    ///Highlight Variables///

    Shader highlightShader;
    Shader subHighlightShader;
    Material expandIconMaterial;
    Material expandIconMaterialH;
    Shader expandIconShader;
    Shader expandIconShaderH;
    public GameObject selectedComponent;
    public GameObject previousSelectedComponent;
    public GameObject selectedComponentExterior;
    public GameObject previousSelectedComponentExterior;


    ///FadeMaterial Variables///

    List<Material> currentMainMaterials = new List<Material>();
    Renderer[] renderers;
    Material transMaterial;
    //public float alphaCutOff;

    //public MovieTexturePlayer mechVideo, geneVideo, wMechVideo;

    private void Awake()
    {
        //Initialize States//
        mainState = new MainState(this);
        majorComponentState = new MajorComponentState(this);
        subComponentState = new SubComponentState(this);
        diagramState = new DiagramState(this);
        buildState = new BuildState(this);
        mechanicalState = new MechanicalState(this);
        mechanicalRoomState = new MechanicalRoomState(this);
        iconState = new IconState(this);

        //majorAnnotate = new MajorAnnotate(this);

        //cameraDesktopController = GameObject.Find("FPSController");
        //cameraVRController = GameObject.Find("FPSController (Mobile)");
        cameraDesktop = GameObject.Find("FirstPersonCharacter");
        cameraVR = GameObject.Find("Main Camera");
    }


    void Start()
    {
        ///Initialize Variables///

        //State Variables//
        currentState = mainState;
		Organized.Instance.globalCurrentState = currentState;

        currentSystemName = "VR_Heavy";

        //Highlight Variables//
        highlightShader = Resources.Load("Final") as Shader;
        subHighlightShader = Resources.Load("Nada") as Shader;
        expandIconMaterial = Resources.Load("IconBlack") as Material;
        expandIconShader = expandIconMaterial.shader;
        expandIconMaterialH = Resources.Load("IconBlackHighlight") as Material;
        expandIconShaderH = expandIconMaterialH.shader;

		//Camera variables
		cameraHead = GameObject.Find("Player");
        isMoving = false;

        //Fade Variables//
        transMaterial = Resources.Load("concreteLightTrans") as Material;


        ///Set Starting Environemnt///

        //Disables Unwanted Colliders at Start//
        GameObject[] tempDis = GameObject.FindGameObjectsWithTag("DisableCollider");
        foreach (GameObject d in tempDis)
        {
            disableComponents.Add(d.transform);
        }        
        DisableComponents(disableComponents);

        //Adds Wanted Colliders to Non-Building Systems at Start// 
        GameObject[] tempDoors = GameObject.FindGameObjectsWithTag("M1");
        foreach (GameObject m in tempDoors)
        {
            mechanicalRoomDoors.Add(m.transform);
        }
        foreach(Transform t in mechanicalRoomDoors)
        {
            //Debug.Log(t.name);
            if(t.GetComponent<Collider>() == null)
            {
                t.gameObject.AddComponent<BoxCollider>();
            }
        }

        //Hide all other Unwanted Data//
        DisableSecondaryPanels();
        DisableSubComponentButtons();

        //Hides Transpaent Systems at Start//
        //DisableTransparentSystems();


        ///Hides all Systems at Start Expect for the Ignore List///

        //Ignore List
        List<string> ignore = new List<string>();
		ignore.Add (currentSystemName);

		//Hide at Start List
		List<Transform> hideSystems = new List<Transform> ();

		for (int i = 0; i < Organized.Instance.systems.Count; i++)
		{
            //Hide the expandIcons for each Building System
            //Organized.Instance.toggle(Organized.Instance.systemsList[i].expandIcons);
            //Organized.Instance.toggle(Organized.Instance.systemsList[i].expandIconGameObjects);

            //Add Everything VR_Heavy and Opaque BuildingSystems to hideSystems List
            if (Organized.Instance.systems [i].name.Contains ("Trans") || Organized.Instance.systems [i].name.Contains ("A") || Organized.Instance.systems [i].name.Contains ("M"))
            {//was: if (!Organized.Instance.systems [i].name.Equals ("VR_Heavy"))
                if (!Organized.Instance.systems [i].name.Equals ("VR_Heavy"))
				{
					if (!hideSystems.Contains (Organized.Instance.systems [i]))
					{
						hideSystems.Add (Organized.Instance.systems [i]);
					}
				}
			}
		}
        //Hide Them!
		Organized.Instance.hide(hideSystems);
    }

    void Update()
    {
        currentState.UpdateState();
        highlight();

		//if (currentState != Organized.Instance.globalCurrentState) {
			Organized.Instance.globalCurrentState = currentState;
		//}

        foreach (ExpandIcon item in Organized.Instance.expandIcons)
        {
            item.scaleExpandIcons();
        }

        timer += Time.deltaTime;
        if (timer > 5f)
        {
            //Debug.Log(currentPlayerPos.position);
            Debug.Log(currentState.ToString());
            timer = 0;
        }
    }

    ///State Dependent Methods///

    void callExpand()
    {
        //currentExpandIconGameObject.GetComponent<ExpandIcon>().expand();
        //Debug.Log("Implement Expand");
    }

    public void DisableComponents(List<Transform> _disableComponents)
    {
        for (int i = 0; i < _disableComponents.Count; i++)
        {
            if (_disableComponents[i].gameObject.GetComponent<Collider>() != null)
            {
                Collider col = _disableComponents[i].gameObject.GetComponent<Collider>();
                col.enabled = false;
            }
        }
    }

    public void EnableComponents(List<Transform> _enableComponents)
    {
        for (int i = 0; i < _enableComponents.Count; i++)
        {
            if (_enableComponents[i].gameObject.GetComponent<Collider>() != null)
            {
                Collider col = _enableComponents[i].gameObject.GetComponent<Collider>();
                col.enabled = true;
            }
        }
    }

    public void DisableTransparentSystems()
    {
        foreach (Transform t in Organized.Instance.systems)
        {
            if (t.name.Contains("(Trans)"))
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void DisableSecondaryPanels()
    {
        foreach (GameObject g in Organized.Instance.panels)
        {
            if (g.name != "Panel_Main")
            {
                if (g.name != "Panel_Annotation")
                {
                    g.SetActive(false);
                }
            }
        }
    }

    public void DisableSubComponentButtons()
    {
        foreach (BuildingSystem s in Organized.Instance.systemsList)
        {
            if (s.subButtonsParent != null)
            {
                s.subButtonsParent.SetActive(false);
            }
        }
    }

    public void ReturnOnClick()
    {
        if (currentState == diagramState)
        {
            Debug.Log("Returned to mainState");
            currentState = mainState;
        }

        if (currentState == majorComponentState)
        {
            Debug.Log("Returned to mainState");
            currentState = mainState;
        }

        if (currentState == subComponentState)
        {
            Debug.Log("Returned to majorComponentState");
            currentState = majorComponentState;
        }

        EventManager.TriggerEvent("MainState");
    }

    public void BroadcastEvents(List<string> eventsName)
    {
        if (eventsName.Count != 0)
        {
            foreach (string e in eventsName)
            {
                Debug.Log("BroadCasted Event: " + e);
                //EventManager.TriggerEvent("EventName");
                EventManager.TriggerEvent(e);
            }
        }
    }

    public void toggle(List<GameObject> selectedSystems)
    {
        if (lockOn == false)
        {
            Organized.Instance.toggle(selectedSystems);
        }
    }

    public void disable(List<Collider> colliders)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
    }

    public void enable(List<Collider> colliders)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }
    }

    public void activateFadeCoroutine(List<Transform> objects)
    {
        //Debug.Log(objects.Count);

        if (currentState == iconState)
        {
            StartCoroutine(fadeToTransparent(objects));
        }
        if (currentState == subComponentState)
        {
            StartCoroutine(fadeBackTransparent(objects));
        }
    }

    public void activateCameraCoroutine(Vector3 zoomPos)
    {
        if(currentState == majorComponentState)
        {
            Debug.Log("ZoomIn");
            StartCoroutine(cameraZoomIn(zoomPos));
        }

        if(currentState == iconState)
        {
            //ToSubComponentState
            if (zoomPos == tempPlayerPos)
            {
                Debug.Log("ZoomOut tempPlayerPos");
                StartCoroutine(cameraZoomOut(tempPlayerPos));
            }

            //ToMainState
            if (zoomPos == Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[currentExpandIconGameObject.name].iconControl.zoomPos.position)
            {
                Debug.Log("ZoomOut");
                StartCoroutine(cameraZoomOut(zoomPos));
            }

            //ToNextIconState
            if(zoomPos != Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[currentExpandIconGameObject.name].iconControl.zoomPos.position && zoomPos != tempPlayerPos)
            {
                Debug.Log("ZoomNext");
                StartCoroutine(cameraZoomNext(zoomPos));
            }
        }
        
    }

    //Move layers transform
    IEnumerator fadeToTransparent(List<Transform> objects)
    {
        float timeSinceStarted = 0f;

        if (currentState == iconState)
        {
            currentMainMaterials.Clear();
            renderers = new Renderer[objects.Count];

            for (int i = 0; i < objects.Count; i++)
            {
                renderers[i] = objects[i].GetComponent<Renderer>();
                currentMainMaterials.Add(objects[i].GetComponent<Renderer>().material);
                // Debug.Log(mainMaterials[i].name);
                renderers[i].material = transMaterial;
            }
            yield return null;
        }
    }

    IEnumerator fadeBackTransparent(List<Transform> objects)
    {
        float timeSinceStarted = 0f;

        if (currentState == subComponentState)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                renderers[i].material = currentMainMaterials[i];
            }
            yield return null;
        }
    }

    IEnumerator cameraZoomIn(Vector3 zoomPos)
    {
        float t = 1;
        cameraHeadSavedPos = cameraHead.transform.position;
        /*
		cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
        cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
        cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
        cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
        cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_StickToGroundForce = 0;
        */
        cameraHead.GetComponent<Rigidbody>().isKinematic = true;
        isMoving = true;

        while (Vector3.Distance(cameraHead.transform.position, zoomPos) > .05f)
        {
            cameraHead.transform.position = Vector3.Slerp(cameraHead.transform.position, zoomPos, t * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Camera Reached");
        if(Vector3.Distance(cameraHead.transform.position, zoomPos) < .05f)
        {
            currentState = iconState;
            isMoving = false;
        }
    }

    IEnumerator cameraZoomNext(Vector3 zoomPos)
    {
        float t = 1;
        isMoving = true;

        while (Vector3.Distance(cameraHead.transform.position, zoomPos) > .05f)
        {
            cameraHead.transform.position = Vector3.Slerp(cameraHead.transform.position, zoomPos, t * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Camera Reached");
        if (Vector3.Distance(cameraHead.transform.position, zoomPos) < .05f)
        {
            currentState = iconState;
            isMoving = false;
        }
    }

    IEnumerator cameraZoomOut(Vector3 zoomPos)
    {
        float t = 1;
        isMoving = true;
        Vector3 zoomTo;

        if (zoomPos == tempPlayerPos)
        {
            zoomTo = zoomPos;
        }
        else
        {
            zoomTo = cameraHeadSavedPos;
        }

        while (Vector3.Distance(cameraHead.transform.position, zoomTo) > .05f)
        {
            cameraHead.transform.position = Vector3.Slerp(cameraHead.transform.position, zoomTo, t * Time.deltaTime);
            yield return null;
        }

        if (Vector3.Distance(cameraHead.transform.position, zoomTo) < .05f)
        {
            //Debug.Log("Camera Reached");
			/*
            cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 1;
            cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 2.5f;
            cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 5f;
            cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 5f;
            cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_StickToGroundForce = 10f;
            */
            cameraHead.GetComponent<Rigidbody>().isKinematic = false;

            if(zoomPos == tempPlayerPos)
            {
                currentState = subComponentState;
            }
            else
            {
                currentState = majorComponentState;
            }
            isMoving = false;
        }
    }

	public void highlight()
    {
		#if UNITY_EDITOR

        RaycastHit hit = new RaycastHit();
		Ray myRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		Debug.DrawLine (Camera.main.transform.position, Camera.main.transform.position + new Vector3 (0, 0, 100f), Color.cyan);

        //if a component is not hit
        //if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))// | 1 << 16)))
		//if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))	
		if (!Physics.Raycast(myRay, out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		{
            //Debug.Log("Nothing Hit.");
            selectedComponent = null;

            //if there was a previouslyselectedcomponent
            if (previousSelectedComponent != null)
            {
                //Unhiglight Previous//
                
                //If its a component
                if (previousSelectedComponent.tag == "MajorComponent" || previousSelectedComponent.tag == "SubComponent")
                {
                    //There is a previous selected and it does have a shader
                    //Debug.Log(Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name]);

                    //Unhighlight the previousSelectedComponent
                    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name];
                }

                //If its an ExpandIcon
                if (previousSelectedComponent.tag == "ExpandIcon")
                {
                    //previousSelectedComponent.GetComponent<Renderer>().material.shader = expandIconShader;
                    previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0, 0, 0, 10));
                }

                //If its an ExteriorWall
                //if(previousSelectedComponent.tag == "Exterior")
                //{
                //    //Unhighlight the previousSelectedComponent
                //    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponent.name];
                //}

                previousSelectedComponent = null;
            }
        }

        //if a component is hit
        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))// | 1 << 16)))
		if (Physics.Raycast(myRay, out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		{
            if (hit.collider.tag == "MajorComponent" || hit.collider.tag == "SubComponent" || hit.collider.tag == "ExpandIcon")// || hit.collider.tag == "Exterior")// || hit.collider.tag == "Clickable")
            {
                //Save this component as the new selected component
                selectedComponent = hit.collider.gameObject;

                //If there is a previously selected component
                if (previousSelectedComponent != null)
                {
                    //Unhighlight Previous//
                    //If its a component
                    if (previousSelectedComponent.tag == "MajorComponent" || previousSelectedComponent.tag == "SubComponent")
                    {
                        //Unhighlight the previousSelectedComponent
                        previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name];
                    }
                    //If its an ExpandIcon
                    if (previousSelectedComponent.tag == "ExpandIcon")
                    {
                        //previousSelectedComponent.GetComponent<Renderer>().material.shader = expandIconShader;
                        previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0,0,0,10));
                    }

                    //If its an ExteriorWall
                    //if (previousSelectedComponent.tag == "Exterior")// || previousSelectedComponent.tag == "Clickable")
                    //{
                    //    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponent.name];
                    //}
                }

                ///Highlight selectedComponent///

                //If its a component
                if (selectedComponent.tag == "MajorComponent" || selectedComponent.tag == "SubComponent")
                {
                    selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
                    selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), Color.cyan);
                }

                //If its a expandIcon
                if (selectedComponent.tag == "ExpandIcon")
                {
                    //selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
                    selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.cyan);

                    //if (selectedComponent.transform.parent.parent.parent.parent.tag == "MajorComponent")
                    //{
                    //    //Debug.Log(selectedComponent.transform.parent.parent.parent.parent.name);
                    //    //Debug.Log(Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[selectedComponent.transform.parent.parent.name]._root.name);
                    //    Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[selectedComponent.transform.parent.parent.name]._root.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), Color.cyan);
                    //}
                }

                //If its an ExteriorWall
                //if (selectedComponent.tag == "Exterior")// || selectedComponent.tag == "Clickable")
                //{
                //    selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
                //    selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), new Color32(255, 255, 225, 50));
                //}

                previousSelectedComponent = selectedComponent;
            }
        }

        //if (currentState == mainState)
        //{
        //    //Exterior Walls
        //    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << 16))
        //    {
        //        //Debug.Log("Nothing Hit.");
        //        selectedComponentExterior = null;

        //        //if there was a previouslyselectedcomponent
        //        if (previousSelectedComponentExterior != null)
        //        {
        //            //Unhiglight Previous//

        //            //If its an ExteriorWall
        //            if (previousSelectedComponentExterior.tag == "Exterior")
        //            {
        //                //Unhighlight the previousSelectedComponent
        //                if (Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name])
        //                {
        //                    previousSelectedComponentExterior.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name];
        //                }
        //            }

        //            previousSelectedComponentExterior = null;
        //        }
        //    }

        //    //if a component is hit
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 16)))
        //    {
        //        if (hit.collider.tag == "Exterior")// || hit.collider.tag == "Clickable")
        //        {
        //            //Save this component as the new selected component
        //            selectedComponentExterior = hit.collider.gameObject;

        //            //If there is a previously selected component
        //            if (previousSelectedComponentExterior != null)
        //            {
        //                //Unhighlight Previous//

        //                //If its an ExteriorWall
        //                if (previousSelectedComponentExterior.tag == "Exterior")// || previousSelectedComponent.tag == "Clickable")
        //                {
        //                    if (Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name])
        //                    {
        //                        previousSelectedComponentExterior.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name];
        //                    }
        //                }
        //            }

        //            ///Highlight selectedComponent///

        //            //If its an ExteriorWall
        //            if (selectedComponentExterior.tag == "Exterior")// || selectedComponent.tag == "Clickable")
        //            {
        //                selectedComponentExterior.GetComponent<Renderer>().material.shader = highlightShader;
        //                selectedComponentExterior.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), new Color32(255, 255, 225, 50));
        //            }

        //            previousSelectedComponentExterior = selectedComponentExterior;
        //        }
        //    }
        //}

		#endif

		#if UNITY_IOS && !UNITY_EDITOR

		//Debug.Log("Mobile IOS");
		//Debug.Log(Camera.main);
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red, Mathf.Infinity);

		RaycastHit hit = new RaycastHit();

		Ray myRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		//if(Input.touches.Length > 0)
		//{
			//Debug.Log("1");
			//Debug.Log("Input.touches.Length" + Input.touches.Length);
		//if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		if (!Physics.Raycast(myRay, out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		{
				//Debug.Log("2");

		//if a component is not hit
		//if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))// | 1 << 16)))
		//{
			//Debug.Log("Nothing Hit.");
			selectedComponent = null;

			//if there was a previouslyselectedcomponent
			if (previousSelectedComponent != null)
			{
				//Unhiglight Previous//

				//If its a component
				if (previousSelectedComponent.tag == "MajorComponent" || previousSelectedComponent.tag == "SubComponent")
				{
					//There is a previous selected and it does have a shader
					//Debug.Log(Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name]);

					//Unhighlight the previousSelectedComponent
					previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name];
				}

				//If its an ExpandIcon
				if (previousSelectedComponent.tag == "ExpandIcon")
				{
					//previousSelectedComponent.GetComponent<Renderer>().material.shader = expandIconShader;
					previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0, 0, 0, 10));
				}

				//If its an ExteriorWall
				//if(previousSelectedComponent.tag == "Exterior")
				//{
				//    //Unhighlight the previousSelectedComponent
				//    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponent.name];
				//}

				previousSelectedComponent = null;
			}
		}

		//if a component is hit
		//if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		if (Physics.Raycast(myRay, out hit, Mathf.Infinity, (1 << 8 | 1 << 10 | 1 << 11)))
		{
			if (hit.collider.tag == "MajorComponent" || hit.collider.tag == "SubComponent" || hit.collider.tag == "ExpandIcon")// || hit.collider.tag == "Exterior")// || hit.collider.tag == "Clickable")
			{
				//Save this component as the new selected component
				selectedComponent = hit.collider.gameObject;

				//If there is a previously selected component
				if (previousSelectedComponent != null)
				{
					//Unhighlight Previous//
					//If its a component
					if (previousSelectedComponent.tag == "MajorComponent" || previousSelectedComponent.tag == "SubComponent")
					{
						//Unhighlight the previousSelectedComponent
						previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].originalShaders[previousSelectedComponent.name];
					}
					//If its an ExpandIcon
					if (previousSelectedComponent.tag == "ExpandIcon")
					{
						//previousSelectedComponent.GetComponent<Renderer>().material.shader = expandIconShader;
						previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0,0,0,10));
					}

					//If its an ExteriorWall
					//if (previousSelectedComponent.tag == "Exterior")// || previousSelectedComponent.tag == "Clickable")
					//{
					//    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponent.name];
					//}
				}

				///Highlight selectedComponent///

				//If its a component
				if (selectedComponent.tag == "MajorComponent" || selectedComponent.tag == "SubComponent")
				{
					selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
					selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), Color.cyan);
				}

				//If its a expandIcon
				if (selectedComponent.tag == "ExpandIcon")
				{
					//selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
					selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.cyan);

					//if (selectedComponent.transform.parent.parent.parent.parent.tag == "MajorComponent")
					//{
					//    //Debug.Log(selectedComponent.transform.parent.parent.parent.parent.name);
					//    //Debug.Log(Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[selectedComponent.transform.parent.parent.name]._root.name);
					//    Organized.Instance.systemDictionary[currentSystemName + " (Trans)"].majorComponentDictionary[selectedComponent.transform.parent.parent.name]._root.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), Color.cyan);
					//}
				}

				//If its an ExteriorWall
				//if (selectedComponent.tag == "Exterior")// || selectedComponent.tag == "Clickable")
				//{
				//    selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
				//    selectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), new Color32(255, 255, 225, 50));
				//}

				previousSelectedComponent = selectedComponent;
			}
		}
				//} //end of if

		//if (currentState == mainState)
		//{
		//    //Exterior Walls
		//    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << 16))
		//    {
		//        //Debug.Log("Nothing Hit.");
		//        selectedComponentExterior = null;

		//        //if there was a previouslyselectedcomponent
		//        if (previousSelectedComponentExterior != null)
		//        {
		//            //Unhiglight Previous//

		//            //If its an ExteriorWall
		//            if (previousSelectedComponentExterior.tag == "Exterior")
		//            {
		//                //Unhighlight the previousSelectedComponent
		//                if (Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name])
		//                {
		//                    previousSelectedComponentExterior.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name];
		//                }
		//            }

		//            previousSelectedComponentExterior = null;
		//        }
		//    }

		//    //if a component is hit
		//    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 16)))
		//    {
		//        if (hit.collider.tag == "Exterior")// || hit.collider.tag == "Clickable")
		//        {
		//            //Save this component as the new selected component
		//            selectedComponentExterior = hit.collider.gameObject;

		//            //If there is a previously selected component
		//            if (previousSelectedComponentExterior != null)
		//            {
		//                //Unhighlight Previous//

		//                //If its an ExteriorWall
		//                if (previousSelectedComponentExterior.tag == "Exterior")// || previousSelectedComponent.tag == "Clickable")
		//                {
		//                    if (Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name])
		//                    {
		//                        previousSelectedComponentExterior.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary[currentSystemName].originalShaders[previousSelectedComponentExterior.name];
		//                    }
		//                }
		//            }

		//            ///Highlight selectedComponent///

		//            //If its an ExteriorWall
		//            if (selectedComponentExterior.tag == "Exterior")// || selectedComponent.tag == "Clickable")
		//            {
		//                selectedComponentExterior.GetComponent<Renderer>().material.shader = highlightShader;
		//                selectedComponentExterior.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_FresnelColor"), new Color32(255, 255, 225, 50));
		//            }

		//            previousSelectedComponentExterior = selectedComponentExterior;
		//        }
		//    }
		//}

		#endif

    }
}