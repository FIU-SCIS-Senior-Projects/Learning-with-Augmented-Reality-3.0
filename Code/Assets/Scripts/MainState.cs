using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class MainState : IEnvironmentState
{
    //Interface State Machine Variable//
    private readonly StatePatternEnvironment envi;

    //Constructor//
    public MainState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
    }

    public void OnTriggerPressed(Collider other)
    {
        //if (other.gameObject.CompareTag("Transparent1"))
        //    ToMajorComponentState();
    }

    public void ToMainState()
    {
        Debug.Log("Can't transition to same state.");
    }

    public void ToDiagramState()
    {
        //Debug.Log("Diagram State Activated");
        envi.currentState = envi.diagramState;
    }

    public void ToMajorComponentState()
    {
        envi.currentState = envi.majorComponentState;
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
            //Debug.Log(envi.currentState);
            envi.timer = 0;
        }

    }

    void OnEnable()
    {
        //EventManager.StartListening("MajorComponent", ToMajorComponentState);
        //EventManager.StartListening("SubComponent", ToSubComponentState);
    }

    void OnDisable()
    {
        //EventManager.StopListening("MajorComponent", ToMajorComponentState);
        //EventManager.StopListening("SubComponent", ToSubComponentState);
    }

	public void OnTriggerClicked(Ray myRay)
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();
		//Ray myRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);

		Debug.DrawLine (Camera.main.transform.position, Camera.main.transform.position + new Vector3 (0, 0, 100f), Color.cyan);
        //if raycast hits
        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11))))
		if (Physics.Raycast(myRay, out hit, Mathf.Infinity, ((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11))))
		{
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);

                if (collidertag.Equals("RValue") || collidertag.Equals("GeneratorVideo") || collidertag.Equals("WMechanicalVideo") || collidertag.Equals("MechanicalVideo")
                    || collidertag.Equals("EE") || collidertag.Equals("ShadingCo"))
                {
                    tempEventNames.Add(collidertag);

                    //Organized.Instance.panelDictionary["Panel_Diagram"].SetActive(true);

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                   // ToDiagramState();
                }

                    //If its a Diagram
                if (collidertag.Equals("Sun") || collidertag.Equals("Wind") || collidertag.Equals("Soil") || collidertag.Equals("Drainage"))
                {
                    //EventManager.TriggerEvent(collidertag);

                    //Add collider tag as an event
                    tempEventNames.Add(collidertag);

                    if (collidertag != "Soil" && collidertag != "Drainage")
                    {
                        //Add any additional events
                        tempEventNames.Add("AerialView");
                    }

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                    ToDiagramState();
                }

                if (collidertag.Equals("SoilAnimation"))
                {
                    tempEventNames.Add(collidertag);

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                    ToDiagramState();
                }

                //If its a Mechanical Room
                if (collidertag.Equals("M1"))
                {
                    envi.currentSystemName = collidertag;

                    envi.DisableComponents(envi.mechanicalRoomDoors);

                    //get the diableOnIsolate list for the system
                    if (Organized.Instance.systemDictionary[envi.currentSystemName].disableOnTransparentColliders != null)
                    {
                        List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName].disableOnTransparentColliders;

                        //Debug.Log(toBeDisabled.Count);

                        //disable the disableOnTrans colliders for the system
                        envi.disable(toBeDisabled);
                    }

                    envi.disable(Organized.Instance.systemDictionary[envi.currentSystemName].disableOnTransparentColliders);

                    try
                    {
                        //Adds the hit colliders system and its trans system
                        systemsHit.Add(Organized.Instance.systemDictionary[collidertag]._root.gameObject);
                        systemsHit.Add(Organized.Instance.systemDictionary[collidertag]._root.gameObject);
                        envi.toggle(systemsHit);
                    }

                    catch
                    {
                        Debug.Log("Error: the hit collider's tag does not match any current systems.");
                    }

                    tempEventNames.Add(collidertag);
                    envi.BroadcastEvents(tempEventNames);
                    ToMechanicalRoomState();
                }

                //If its a MajorComponent
                if (collidertag == "E1" || collidertag == "E2" || collidertag == "E3" || collidertag == "E4" || collidertag == "E5")
                {
                    envi.currentSystemName = collidertag;
                    //Debug.Log(envi.currentSystemName);

                    //Test
                    //ExpandIcon icon = new ExpandIcon(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].layers[4], Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"]._name);

                    //Creates an Expand Icon for each MajorComponent
                    //foreach (MajorComponent t in Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponents)
                    //{
                    //    //Debug.Log(t.hasIcon);
                    //    if (t.hasIcon)
                    //    {
                    //        envi.expandIcons.Add(new ExpandIcon(t._root, t._name));
                    //    }
                    //}

                    Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorExpandIcons);
                    //Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconGameObjects);

                    //Hides the exterior walls in VR_Heavy
                    //if (Organized.Instance.systemDictionary ["VR_Heavy"].hideOnTransparent != null)
                    //{
                    //  Organized.Instance.hide (Organized.Instance.systemDictionary ["VR_Heavy"].hideOnTransparent);
                    //}

                    if (envi.lockOn)
                    {
                        Organized.Instance.panelDictionary["Panel_Locked"].SetActive(true);
                    }
                    else
                    {
                        //Hides E()Detail
                        if (Organized.Instance.systemDictionary[envi.currentSystemName].hideOnTransparent != null)
                        {
                            Organized.Instance.hide(Organized.Instance.systemDictionary[envi.currentSystemName].hideOnTransparent);
                        }
                        else
                        {
                            Debug.Log("There are no hideOnTransparent objects in this system");
                        }
                    }

                    //get the diableOnIsolate list for the system
                    if (Organized.Instance.systemDictionary[envi.currentSystemName].disableOnTransparentColliders != null)
                    {
                        //foreach (var item in Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnTransparentColliders)
                        //{
                        //    Debug.Log(item);
                        //}
                        
                        List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnTransparentColliders;

                        //Debug.Log(toBeDisabled.Count);

                        //disable the disableOnTrans colliders for the system
                        envi.disable(toBeDisabled);
                    }

                    //Enable the subcomponent colliders for E5
                    if(envi.currentSystemName == "E5" || envi.currentSystemName == "E1")
                    {
                        envi.enable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders);
                    }


                    //					if(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnTransparentColliders != null)
                    //					{
                    //                    	envi.disable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnTransparentColliders);
                    //					}

                    try
                    {
                        //Adds the hit colliders system and its trans system
                        //systemsHit.Add(Organized.Instance.systemDictionary[collidertag]._root.gameObject);
                        systemsHit.Add(Organized.Instance.systemDictionary[collidertag + " (Trans)"]._root.gameObject);
                        //systemsHit.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);
                        envi.toggle(systemsHit);
                    }

                    catch
                    {
                        Debug.Log("Error: the hit collider's tag does not match any current systems.");
                    }

                    tempEventNames.Add(collidertag);
                    envi.BroadcastEvents(tempEventNames);

                    //Overide MajorComponent State for Subcomponets to be higlighted (Must tag subcomponents as major components)
                    if (collidertag.Equals("E5"))
                    {
                        //Organized.Instance.toggle(Organized.Instance.systemDictionary["M1"]._root);
                        Organized.Instance.systemDictionary["M1"]._root.gameObject.SetActive(true);
                        envi.enable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders);
						Organized.Instance.toggle (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subExpandIcons);
                    }

                    
                    ToMajorComponentState();
                }

                //if(collidertag.Equals("ExpandIcon"))
                //{
                //    envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.parent.gameObject;
                //    envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

                //    Debug.Log("ExpandIcon hit!");
                //    Debug.Log(hit.collider.gameObject.name);

                //    tempEventNames.Add("ExpandIcon");
                //    envi.BroadcastEvents(tempEventNames);

                //    ToIconState();
                //}
            }
        }
    }
}