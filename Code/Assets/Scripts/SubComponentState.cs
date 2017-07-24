using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class SubComponentState : IEnvironmentState
{
    private readonly StatePatternEnvironment envi;

    public SubComponentState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
    }

    public void ToDiagramState()
    {
        envi.currentState = envi.diagramState;
    }

    public void OnTriggerPressed(Collider other)
    {
        throw new NotImplementedException();
    }

    public void ToMainState()
    {
        envi.currentState = envi.mainState;
    }

    public void ToMajorComponentState()
    {
        envi.currentState = envi.majorComponentState;
    }

    public void ToSubComponentState()
    {
        Debug.Log("Can't transition to same state.");
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
			OnTriggerClicked();
		}
		#endif

		#if UNITY_IOS && !UNITY_EDITOR
		if(Input.touches.Length > 0)
		{
		OnTriggerClicked();
		}
		#endif

        envi.timer += Time.deltaTime;
        if (envi.timer > 5f)
        {
            Debug.Log(envi.currentState);
            envi.timer = 0;
        }
    }

	//Layers: 8: Components, 9: Button, 10: ExpandIcon, 11: Exterior, 12: 
    //Fix This
    public void OnTriggerClicked()
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();
		Ray myRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		//Debug.DrawLine (Camera.main.transform.position, Camera.main.transform.position + new Vector3 (0, 0, 100f), Color.cyan);

		//if raycast hits
		//if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))

		//if raycast hits
		//if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11))))
		if (Physics.Raycast(myRay, out hit, Mathf.Infinity, ((1 << 9) | (1 << 10))))// | (1 << 12))))        
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);

                if (collidertag.Equals("Return"))
                {
                    //Moves Back the players position
                    //envi.currentPlayerPos = Camera.main.transform;
                    //Organized.Instance.player.transform.position = envi.currentPlayerPos.position; //This does not work with the new setup, Fix with new ExpandIcon
                    Organized.Instance.player.transform.position = envi.tempPlayerPos; //It was this before I removed tempPlayer from envi

                    //get the diableOnIsolate list for the system
                    if (Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders == null)
                    {
                        Debug.Log("disableOnIsolateColliders is empty");
                    }
                    List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders;

                    //disable the disableOnIso colliders for the system
                    envi.enable(toBeDisabled);

                    //get isolation objects
                    List<GameObject> objectsToHide = new List<GameObject>();
                    List<GameObject> objectsToShow = new List<GameObject>();

                    foreach (BuildingSystem s in Organized.Instance.systemsList)
                    {
                        if (s._name != (envi.currentSystemName + " (Trans)"))
                        {
                            objectsToHide.Add(s._root.gameObject);
                        }
                    }

                    //add any additional objects
                    objectsToHide.Add(Organized.Instance.environment);
                    objectsToHide.Add(Organized.Instance.animations);
                    objectsToHide.Add(Organized.Instance.diagramButtons);

                    if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent != null)
                    {
                        objectsToHide.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent);
                    }
                    objectsToHide.AddRange(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].extraHideOnIsolate);

                    //Hudes SubComponentExpandIcons
                    Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subExpandIcons);

                    //Unhides MajorExpandIcons
                    Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorExpandIcons);
                    //foreach (ExpandIcon e in Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorExpandIcons)
                    //{
                    //    e.expand();
                    //}
                    

                    //Isolate the trans system
                    Organized.Instance.unHide(objectsToHide);
                    Organized.Instance.changeSkybox();

                    //Get the objects to be shown in the subComponent State
                    objectsToShow.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);

                    if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent != null)
                    {
                        objectsToShow.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent);
                    }

                    //Show the subComponent data
                    Organized.Instance.toggle(objectsToShow);

                    //Debug.Log("We reached here!");

                    //Disable the disableOnIsolation Colliders
                    List<Collider> tempDisableColliders = new List<Collider>();
                    tempDisableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentColliders;
                    //Debug.Log(tempDisableColliders.Count);
                    //envi.disable(tempDisableColliders);
                    foreach (Collider c in tempDisableColliders)
                    {
                        c.enabled = true;
                    }
                    envi.disable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnIsolateColliders);

                    //Activate enabledOnIsoColliders
                    List<Collider> tempEnableColliders = new List<Collider>();
                    tempEnableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders;
                    //Debug.Log(tempEnableColliders.Count);
                    //envi.enable(tempEnableColliders);
                    foreach (Collider c in tempEnableColliders)
                    {
                        c.enabled = false;
                    }

                    envi.activateFadeCoroutine((Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentsTransforms));

                    //Add collider tag as an event
                    tempEventNames.Add(collidertag);

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                    ToMajorComponentState();
                }
            }
        }
    }
}
