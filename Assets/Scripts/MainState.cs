/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <MainState. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
        if (Input.GetMouseButtonDown(0))
        {
            OnTriggerClicked();
        }

        envi.timer += Time.deltaTime;
        if (envi.timer > 5f)
        {
            Debug.Log(envi.currentState);
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

    public void OnTriggerClicked()
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();

        //if raycast hits
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11))))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                //Debug.Log(collidertag);


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
                        envi.enable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders);
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