using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

public class MajorComponentState : IEnvironmentState
{
    private readonly StatePatternEnvironment envi;

    public MajorComponentState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
    }

    public void OnTriggerPressed(Collider other)
    {
        throw new NotImplementedException();
    }

    public void ToDiagramState()
    {
        envi.currentState = envi.diagramState;
    }

    public void ToMainState()
    {
        envi.currentState = envi.mainState;
    }

    public void ToMajorComponentState()
    {
        Debug.Log("Can't transition to same state.");
    }

    public void ToSubComponentState()
    {
        envi.currentState = envi.subComponentState;
    }

    public void ToBuildState()
    {
        envi.currentState = envi.buildState;
    }

    public void ToMechanicalState()
    {
        envi.currentState = envi.mechanicalState;
    }

    public void ToMechanicalRoomState()
    {
        envi.currentState = envi.mechanicalRoomState;
    }

    public void ToIconState()
    {
        envi.currentState = envi.iconState;
    }

    public void UpdateState()
    {
		//returns collider.tag on click
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			Ray myRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			OnTriggerClicked(myRay);
		}
		#endif

		#if UNITY_IOS && !UNITY_EDITOR
		/*
		if(Input.touches.Length > 0)
		{
		OnTriggerClicked();
		}
		*/
		if(Input.touches.Length > 0)
		{
		Debug.Log("touch");
		Ray myRay = envi.myTouch.UpdateTouch();
		//Fix this to center!!!!
		//OnTriggerClicked(Camera.main.ScreenPointToRay(Input.touches[0].position));
		OnTriggerClicked(myRay);
		}
		#endif

        envi.timer += Time.deltaTime;
        if (envi.timer > 5f)
        {
            Debug.Log(envi.currentState);
            envi.timer = 0;
        }
    }

	//Layers: 8: Components, 9: Button, 10: ExpandIcon, 11: Exterior
    //Fix This
	public void OnTriggerClicked(Ray myRay)
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
		RaycastHit hit = new RaycastHit();
		//myRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		//Debug.DrawLine (Camera.main.transform.position, Camera.main.transform.position + new Vector3 (0, 0, 100f), Color.cyan);

		//if raycast hits
		//if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11))))
		if (Physics.Raycast(myRay, out hit, Mathf.Infinity, ((1 << 9) | (1 << 10) | (1 << 11) | (1 << 14))))
		{
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);
                if (envi.isMoving == false)
                {
                    //If its a BuildConstrcution
                    if (collidertag.Equals("Build"))
                    {
                        //EventManager.TriggerEvent(collidertag);
                        //envi.currentPlayerPos = Camera.main.transform; //This does nothing, use tempPlayer Position instead

                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);

                        //Broadcast the events
                        envi.BroadcastEvents(tempEventNames);
                        ToBuildState();
                    }

                    //If its a BuildConstrcution
                    if (collidertag.Equals("Mechanical"))
                    {
                        //EventManager.TriggerEvent(collidertag);
                        envi.currentPlayerPos = Camera.main.transform;

                        Organized.Instance.toggle(Organized.Instance.systemDictionary["M1"]._root);
                        Organized.Instance.toggle(Organized.Instance.systemDictionary["M1 (T)"]._root);

                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);

                        //Broadcast the events
                        envi.BroadcastEvents(tempEventNames);
                        ToMechanicalState();
                    }

                    if (collidertag.Equals("ExpandIcon"))
                    {
                        //Saves players position
                        //envi.currentPlayerPos = Camera.main.transform;
                        //envi.tempPlayerPos = Organized.Instance.player.transform.position;
                        envi.tempPlayerPos = Camera.main.transform.position;

                        envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        //envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

                        //Moves camera to zoomPosition
						if (envi.currentSystemName == "E5") 
						{
							envi.activateCameraCoroutine(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
						}
						else
						{
	                        envi.activateCameraCoroutine(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
	                        //envi.cameraHead.transform.position = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentDictionary[envi.currentExpandIcon.name].iconControl.zoomPos.position;
						}
                        //Disables Physics and Movement for Camera while zoomed
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
                        //envi.cameraHead.GetComponent<Rigidbody>().isKinematic = true;

                        Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

                        //Debug.Log("From MajorComponentState " + envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().window);
                        //envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().expand();

                        Debug.Log("ExpandIcon hit!");
                        //Debug.Log(hit.collider.gameObject.name);

                        tempEventNames.Add("ExpandIcon");
                        envi.BroadcastEvents(tempEventNames);

                        //ToIconState();
                    }

                    //If it hits a return button
                    if (collidertag.Equals("Return"))
                    {
                        
                        Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIcons);
                        //Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconGameObjects);
                        //Debug.Log (Organized.Instance.systems[15].name); //worked
                        //Debug.Log(Organized.Instance.systemDictionary["VR_Heavy"]); //did not work
                        //Debug.Log(Organized.Instance.systemsList[15].name); //did not work
                        //Debug.Log(Organized.Instance.systemDictionary[envi.currentSystemName]._name);

                        try
                        {
                            //Adds the hit colliders system and its trans system
                            //systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName]._root.gameObject); //before exterior walls were clickable
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName].layerDictionary[envi.currentSystemName + "Detail"].gameObject);
                            //Debug.Log(Organized.Instance.systemsList[15].layers.Count); //19
                            //						for(int y=0; y<Organized.Instance.systemsList[15].layers.Count; y++)
                            //						{
                            //							if(Organized.Instance.systemsList[15].layers[y].Equals("E4Deatil"))
                            //							{
                            //								systemsHit.Add(Organized.Instance.systemsList[15].layers[y].gameObject);
                            //								Debug.Log(Organized.Instance.systemsList[15].layers[y].name);
                            //							}
                            //						}

                            //						systemsHit.Add(Organized.Instance.systemDictionary["VR_Heavy"].layerDictionary[envi.currentSystemName + "Detail"].gameObject); //dictionary does not work if stuff is hidden
                            //systemsHit.Add(Organized.Instance.systemsList[15].layerDictionary[envi.currentSystemName + "Deatil"]);

                            //Debug.Log(Organized.Instance.systemsList[15].name);
                            //						foreach(GameObject s in Organized.Instance.systemDictionary)
                            //						{
                            //							Debug.Log(s.name);
                            //						}
                            //Debug.Log(Organized.Instance.systemDictionary["VR_Heavy"].name);
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"]._root.gameObject);
                            //systemsHit.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);
                            envi.toggle(systemsHit);
                        }

                        catch
                        {
                            Debug.Log("Error: the hit collider's tag does not match any current systems.");
                        }

                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);

                        //Broadcast the events
                        envi.BroadcastEvents(tempEventNames);
                        ToMainState();
                    }

                    //if (collidertag.Equals("MajorComponent"))
                    //{
                    //    //Saves players position
                    //    envi.currentPlayerPos = Camera.main.transform;
                    //    //envi.tempPlayerPos = Organized.Instance.player.transform.position;

                    //    //Details with SubComponent States
                    //    if (envi.currentSystemName.Equals("E2"))
                    //    {
                    //        //get the diableOnIsolate list for the system
                    //        if (Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders == null)
                    //        {
                    //            Debug.Log("disableOnIsolateColliders is empty");
                    //        }

                    //        List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders;

                    //        //disable the disableOnIso colliders for the system
                    //        envi.disable(toBeDisabled);

                    //        //get isolation objects
                    //        List<GameObject> objectsToHide = new List<GameObject>();
                    //        List<GameObject> objectsToShow = new List<GameObject>();

                    //        foreach (BuildingSystem s in Organized.Instance.systemsList)
                    //        {
                    //            if (s._name != (envi.currentSystemName + " (Trans)"))
                    //            {
                    //                objectsToHide.Add(s._root.gameObject);
                    //            }
                    //        }

                    //        //add any additional objects
                    //        objectsToHide.Add(Organized.Instance.environment);
                    //        objectsToHide.Add(Organized.Instance.animations);
                    //        objectsToHide.Add(Organized.Instance.diagramButtons);

                    //        if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent != null)
                    //        {
                    //            objectsToHide.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent);
                    //        }

                    //        objectsToHide.AddRange(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].extraHideOnIsolate);

                    //        //Isolate the trans system
                    //        Organized.Instance.hide(objectsToHide);
                    //        Organized.Instance.changeSkybox();

                    //        //Get the objects to be shown in the subComponent State
                    //        objectsToShow.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);

                    //        if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent != null)
                    //        {
                    //            objectsToShow.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent);
                    //        }

                    //        //Show the subComponent data
                    //        Organized.Instance.toggle(objectsToShow);

                    //        //Debug.Log("We reached here!");

                    //        //Disable the disableOnIsolation Colliders
                    //        List<Collider> tempDisableColliders = new List<Collider>();
                    //        tempDisableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentColliders;
                    //        //Debug.Log(tempDisableColliders.Count);
                    //        //envi.disable(tempDisableColliders);
                    //        foreach (Collider c in tempDisableColliders)
                    //        {
                    //            c.enabled = false;
                    //        }
                    //        envi.disable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnIsolateColliders);

                    //        //Activate enabledOnIsoColliders
                    //        List<Collider> tempEnableColliders = new List<Collider>();
                    //        tempEnableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders;
                    //        //Debug.Log(tempEnableColliders.Count);
                    //        //envi.enable(tempEnableColliders);
                    //        foreach (Collider c in tempEnableColliders)
                    //        {
                    //            c.enabled = true;
                    //        }

                    //        //Major Componets are switched to a transparent shader

                    //        envi.activateFadeCoroutine((Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentsTransforms));

                    //        //Add collider tag as an event
                    //        tempEventNames.Add(collidertag);

                    //        //Broadcast the events
                    //        envi.BroadcastEvents(tempEventNames);
                    //        ToSubComponentState();
                    //    }
                }
            }
        }
    }
}