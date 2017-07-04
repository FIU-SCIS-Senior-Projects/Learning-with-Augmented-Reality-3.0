/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <Description. Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class StatePatternAR : MonoBehaviour
{

    [HideInInspector]
    public AlphaState AlphaState;
    [HideInInspector]
    public BetaState BetaState;
    [HideInInspector]
    public mainStateAR mainStateAR;
    [HideInInspector]
    public IconStateAR iconStateAR;
    [HideInInspector]
    public float timer = 0f;

	OpenClose oc = new OpenClose();

    public GameObject currentSubPanel;
    public GameObject previousSubPanel;
    public string currentSubPanelName;
    public string previousSubPanelName;
    public bool isGetSubPanelCalled;
    public IARState currentState;

    public IARState priorToIconState;

    //System//
    public string currentSystemName;

    //MajorComponent
    public GameObject currentMajorComponentGameObject;

    //ExpandIcon//
    public ExpandIcon currentExpandIcon;
    public GameObject currentExpandIconGameObject;

    //Player Camera//
    public Transform currentPlayerPos;
	public GameObject currentCameraHead;
	public GameObject cameraHead; //old
    public GameObject cameraHeadA;
	public GameObject cameraHeadB;
    public Vector3 cameraHeadSavedPos;
    public Vector3 tempPlayerPos;
    public Transform cameraPositions;


    ///Highlight Variables///

    public Shader highlightShader;
    public Shader subHighlightShader;
    public Material expandIconMaterial;
    public Material expandIconMaterialH;
    Shader expandIconShader;
    Shader expandIconShaderH;
    public GameObject selectedComponent;
    public GameObject previousSelectedComponent;

	public Sprite ERV;
	public Sprite AHU;

    [HideInInspector]
    public bool isMoving;


    private void Awake()
    {
        AlphaState = new AlphaState(this);
        BetaState = new BetaState(this);
        mainStateAR = new mainStateAR(this);
        iconStateAR = new IconStateAR(this);
        isGetSubPanelCalled = false;

		ERV = Resources.Load<Sprite> ("HeatRecoveryVentilators");
		AHU = Resources.Load<Sprite>("AHU");
    }

    // Use this for initialization
    void Start()
    {
        cameraPositions = GameObject.Find("Positions").transform;

        currentState = mainStateAR;

        List<BuildingSystem> hideSystemsAtStart = new List<BuildingSystem>();
        //hideSystemsAtStart.Add(Organized.Instance.systemDictionary["VR_Heavy"]);
        //hideSystemsAtStart.Add(Organized.Instance.systemDictionary["VR_Light"]);
        //hideSystemsAtStart.Add(Organized.Instance.systemDictionary["VR_Light (Trans)"]);
        foreach (BuildingSystem s in Organized.Instance.systemsList)
        {
            hideSystemsAtStart.Add(s);
        }

		foreach (GameObject p in Organized.Instance.panels) 
		{
			if (!p.name.Equals ("Opening")) 
			{
				Organized.Instance.toggle(p);
			}
		}

        Organized.Instance.toggle(hideSystemsAtStart);

        //Update Variables//
		//cameraHead = GameObject.Find("ARCamera");
		cameraHeadA = GameObject.Find("ARCameraA");
		cameraHeadB = GameObject.Find ("ARCameraB");
        isMoving = false;

        //Highlight Variables//
        highlightShader = Resources.Load("Final") as Shader;
        subHighlightShader = Resources.Load("Nada") as Shader;
        expandIconMaterial = Resources.Load("IconBlack") as Material;
        expandIconShader = expandIconMaterial.shader;
        expandIconMaterialH = Resources.Load("IconBlackHighlight") as Material;
        expandIconShaderH = expandIconMaterialH.shader;
    }

    public void OnEnable()
    {
        EventManager.StartListening("MainStateAR", ToggleMainStateAR);
        EventManager.StartListening("AlphaState", ToggleAlpha);
        EventManager.StartListening("BetaState", ToggleBeta);
		//EventManager.StartListening("Locked", ToggleBeta);
    }

    public void OnDisable()
    {
        EventManager.StopListening("MainSTateAR", ToggleMainStateAR);
        EventManager.StopListening("AlphaState", ToggleAlpha);
        EventManager.StopListening("BetaState", ToggleBeta);
    }

    public void getSubPanel(GameObject subPanel)
    {
        currentSubPanel = subPanel;
        currentSubPanelName = subPanel.name;
        isGetSubPanelCalled = true;
    }

    public void goToPreviusSubPanel()
    {
        Organized.Instance.toggle(previousSubPanel);
        Organized.Instance.toggle(GameObject.Find("Title_Description"));
    }

    void ToggleMainStateAR()
    {
        if (currentState == AlphaState || currentState == BetaState)
            currentState = mainStateAR;
    }

    void ToggleAlpha()
    {
        //Camera.main.transform.position = cameraPositions.transform.FindChild("APosition").position;
        //Camera.main.transform.rotation = cameraPositions.transform.FindChild("APosition").rotation;
		Organized.Instance.cameraA.SetActive(true);
		//rganized.Instance.cameraB.SetActive(false);
		currentState = AlphaState;
    }
    void ToggleBeta()
    {
        //Camera.main.transform.position = cameraPositions.transform.FindChild("BPosition").position;
        //Camera.main.transform.rotation = cameraPositions.transform.FindChild("BPosition").rotation;
		Organized.Instance.cameraB.SetActive(true);
		//Organized.Instance.cameraA.SetActive(false);
        currentState = BetaState;
    }
    public void getPreviousSubPanel(GameObject subPanel) { previousSubPanel = subPanel; }

	IEnumerator callLockCo()
	{
		float timeSinceStarted = 0f;

		if (currentState == AlphaState) 
		{
			GameObject a = GameObject.Find ("Panel_AlignmentFilterA");
			if (a != null) 
			{
				oc.close(a);
			}

		}
		if (currentState == AlphaState) 
		{
			GameObject b = GameObject.Find ("Panel_AlignmentFilterB");
			if (b != null) 
			{
				oc.close (b);
			}
		}

		EventManager.TriggerEvent("Lock");
		EventManager.TriggerEvent("Calibrate");
		Debug.Log ("Calibrate 1");

		yield return new WaitForSeconds(2);

		EventManager.TriggerEvent("Calibrate");
		Debug.Log("Calibrate 2");
	}

	public void callLock()
	{
		StartCoroutine (callLockCo());

//		if (currentState == AlphaState) 
//		{
//			GameObject a = GameObject.Find ("Panel_AlignmentFilterA");
//			if (a != null) 
//			{
//				oc.close(a);
//			}
//
//		}
//		if (currentState == AlphaState) 
//		{
//			GameObject b = GameObject.Find ("Panel_AlignmentFilterB");
//			if (b != null) 
//			{
//				oc.close (b);
//			}
//		}
//		EventManager.TriggerEvent("Lock");
	}

    public void callAlphaState()
    {
        foreach (var item in Organized.Instance.systemDictionary["M1"].expandIcons)
        {
            //Debug.Log(item._name);
        }
        
        EventManager.TriggerEvent("AlphaState");
    }
    public void callBetaState()
    {
        foreach (var item in Organized.Instance.systemDictionary["M1"].expandIcons)
        {
            //Debug.Log(item._name);
        }
        EventManager.TriggerEvent("BetaState");
    }

    public void callMainStateAR()
    {
        EventManager.TriggerEvent("MainStateAR");
    }

//	public void callMechancial()
//	{
//		EventManager.TriggerEvent ("Mechanical");
//	}

    public void toggle(List<GameObject> selectedSystems)
    {
        //if (lockOn == false)
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

    public void activateCameraCoroutine(Vector3 zoomPos)
    {
        if (currentState == AlphaState || currentState == BetaState)
        {
            Debug.Log("ZoomIn");

			if (currentState == AlphaState) 
			{
				Debug.Log("cameraHeadA");
				//Debug.Log (Organized.Instance.cameraA);
				//Debug.Log (currentCameraHead);
				currentCameraHead = Organized.Instance.cameraA;
				Debug.Log (currentCameraHead);
			}

			if (currentState == BetaState) 
			{
				Debug.Log("cameraHeadB");
				//Debug.Log (currentCameraHead);
				currentCameraHead = Organized.Instance.cameraB;
				Debug.Log (currentCameraHead);
			}

			StartCoroutine(cameraZoomIn(zoomPos));
        }

        if (currentState == iconStateAR)
        {
            //ToSubComponentState
            if (zoomPos == tempPlayerPos)
            {
                Debug.Log("ZoomOut tempPlayerPos");
                StartCoroutine(cameraZoomOut(tempPlayerPos));
            }

            //ToMainState
            if (zoomPos == Organized.Instance.systemDictionary["M1"].majorComponentDictionary[currentExpandIconGameObject.name].iconControl.zoomPos.position)
            {
                Debug.Log("ZoomOut");
                StartCoroutine(cameraZoomOut(zoomPos));
            }

            //ToNextIconState
            if (zoomPos != Organized.Instance.systemDictionary["M1"].majorComponentDictionary[currentExpandIconGameObject.name].iconControl.zoomPos.position && zoomPos != tempPlayerPos)
            {
                Debug.Log("ZoomNext");
                StartCoroutine(cameraZoomNext(zoomPos));
            }
        }
    }

    IEnumerator cameraZoomIn(Vector3 zoomPos)
    {
        float t = 4;
		Debug.Log ("currentCameraHead: " + currentCameraHead.name);
        cameraHeadSavedPos = currentCameraHead.transform.position;

		if (currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
        {
			currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
			currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
			currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
			currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
			currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_StickToGroundForce = 0;
        }
		if (currentCameraHead.GetComponent<Rigidbody>())
        {
			currentCameraHead.GetComponent<Rigidbody>().isKinematic = false;
        }
        isMoving = true;

		while (Vector3.Distance(currentCameraHead.transform.position, zoomPos) > .05f)
        {
			currentCameraHead.transform.position = Vector3.Slerp(currentCameraHead.transform.position, zoomPos, t * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Camera Reached");
		if (Vector3.Distance(currentCameraHead.transform.position, zoomPos) < .05f)
        {
            currentState = iconStateAR;
            isMoving = false;
        }
    }

    IEnumerator cameraZoomNext(Vector3 zoomPos)
    {
        float t = 4;
        isMoving = true;

		while (Vector3.Distance(currentCameraHead.transform.position, zoomPos) > .05f)
        {
			currentCameraHead.transform.position = Vector3.Slerp(currentCameraHead.transform.position, zoomPos, t * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Camera Reached");
		if (Vector3.Distance(currentCameraHead.transform.position, zoomPos) < .05f)
        {
            currentState = iconStateAR;
            isMoving = false;
        }
    }

    IEnumerator cameraZoomOut(Vector3 zoomPos)
    {
        float t = 4;
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

		while (Vector3.Distance(currentCameraHead.transform.position, zoomTo) > .05f)
        {
			currentCameraHead.transform.position = Vector3.Slerp(currentCameraHead.transform.position, zoomTo, t * Time.deltaTime);
            yield return null;
        }

		if (Vector3.Distance(currentCameraHead.transform.position, zoomTo) < .05f)
        {
			if (currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
            {
                //Debug.Log("Camera Reached");
				currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 1;
				currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 2.5f;
				currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 5f;
				currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 5f;
				currentCameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_StickToGroundForce = 10f;
            }
			if (currentCameraHead.GetComponent<Rigidbody>())
            {
				currentCameraHead.GetComponent<Rigidbody>().isKinematic = true;
            }
            currentState = priorToIconState;
            isMoving = false;
        }
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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            Debug.Log(currentState);
            timer = 0;
        }

        currentState.UpdateState();

		if (currentState == AlphaState || currentState == BetaState || currentState == iconStateAR) 
		{
			foreach (ExpandIcon item in Organized.Instance.expandIcons) 
			{
				item.scaleExpandIcons ();
			}
		}

        //highlight();
    }

    public void highlight()
    {
        RaycastHit hit = new RaycastHit();
        //Shader originalShader = null;

        //if a component is not hit
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8 | 1 << 10)))
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

                if (previousSelectedComponent.tag == "MechanicalComponent")
                {
                    //Unhighlight the previousSelectedComponent
                    previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary["M1"].originalShaders[previousSelectedComponent.name];
                }

                //If its an ExpandIcon
                if (previousSelectedComponent.tag == "ExpandIcon")
                {
                    //previousSelectedComponent.GetComponent<Renderer>().material.shader = expandIconShader;
                    previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0, 0, 0, 200));
                }

                previousSelectedComponent = null;
            }
        }

        //if a component is hit
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8 | 1 << 10)))
        {
            if (hit.collider.tag == "MajorComponent" || hit.collider.tag == "SubComponent" || hit.collider.tag == "ExpandIcon" || hit.collider.tag == "MechanicalComponent")
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
                        previousSelectedComponent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color32(0, 0, 0, 200));
                    }
                    if (previousSelectedComponent.tag == "MechanicalComponent")
                    {
                        //Unhighlight the previousSelectedComponent
                        previousSelectedComponent.GetComponent<Renderer>().material.shader = Organized.Instance.systemDictionary["M1"].originalShaders[previousSelectedComponent.name];
                    }

                    ///Highlight selectedComponent///

                    //If its a component
                    if (selectedComponent.tag == "MajorComponent" || selectedComponent.tag == "SubComponent" || selectedComponent.tag == "MechanicalComponent")
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
                    previousSelectedComponent = selectedComponent;
                }
            }
        }
    }
}
