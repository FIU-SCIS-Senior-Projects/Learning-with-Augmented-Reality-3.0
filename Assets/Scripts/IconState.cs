/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <IconState. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

public class IconState : IEnvironmentState
{

    //Interface State Machine Variable//
    private readonly StatePatternEnvironment envi;

    //Constructor//
    public IconState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
    }

    public void ToMainState()
    {
        envi.currentState = envi.mainState;
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
        Debug.Log("Can't transition to same state.");
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

    public void OnTriggerClicked()
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();

        //if raycast hits
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 10) | (1 << 14))))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                //Debug.Log(collidertag);

                if (envi.isMoving == false)
                {

                    if (collidertag.Equals("SubComponent"))
                    {
                        //Saves players position

                        //envi.currentPlayerPos = Camera.main.transform; //last
                        //envi.tempPlayerPos = Organized.Instance.player.transform.position;

                        envi.activateCameraCoroutine(envi.tempPlayerPos);

                        //Details with SubComponent States
                        if (envi.currentSystemName.Equals("E2"))
                        {
                            //get the diableOnIsolate list for the system
                            if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnIsolateColliders == null)
                            {
                                Debug.Log("disableOnIsolateColliders is empty");
                            }

                            List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnIsolateColliders;

                            //disable the disableOnIso colliders for the system
                            envi.disable(toBeDisabled);

                            //get isolation objects
                            List<GameObject> objectsToHide = new List<GameObject>();
                            List<GameObject> objectsToShow = new List<GameObject>();

                            //Hide Objects//

                            //adds all systems except the subcomonent of the current system
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

                            //adds the majorcomponent buttons
                            if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent != null)
                            {
                                objectsToHide.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorButtonsParent);
                            }
                            //adds any extraHideOnIsolate objects
                            objectsToHide.AddRange(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].extraHideOnIsolate);

                            //Hide Everything but Isolated System
                            Organized.Instance.hide(objectsToHide);
                            
                            //Change Skybox
                            Organized.Instance.changeSkybox();

                            //Closes the currentExpandIcon
                            envi.currentExpandIcon = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconDictionary[envi.currentExpandIconGameObject.name];
                            envi.currentExpandIcon.expand();

                            //Hide MajorExpandIcons
                            Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorExpandIcons);

                            //Show Objects//

                            //Get the objects to be shown in the subComponent State
                            objectsToShow.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);

                            if (Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent != null)
                            {
                                objectsToShow.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subButtonsParent);
                            }

                            //Show the SubComponent data
                            Organized.Instance.toggle(objectsToShow);

                            //Show the SubExpandIcons
                            Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subExpandIcons);

                            //Disable the disableOnIsolation Colliders
                            List<Collider> tempDisableColliders = new List<Collider>();
                            tempDisableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentColliders;
                            //Debug.Log(tempDisableColliders.Count);
                            //envi.disable(tempDisableColliders);
                            foreach (Collider c in tempDisableColliders)
                            {
                                c.enabled = false;
                            }
                            envi.disable(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].disableOnIsolateColliders);

                            //Activate enabledOnIsoColliders
                            List<Collider> tempEnableColliders = new List<Collider>();
                            tempEnableColliders = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].subComponentColliders;
                            //Debug.Log(tempEnableColliders.Count);
                            //envi.enable(tempEnableColliders);
                            foreach (Collider c in tempEnableColliders)
                            {
                                c.enabled = true;
                            }

                            //Major Componets are switched to a transparent shader
                            //foreach (var item in Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentsTransforms)
                            //{
                            //    Debug.Log(item.name);
                            //}
                            envi.activateFadeCoroutine((Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentsTransforms));

                            //Add collider tag as an event
                            tempEventNames.Add(collidertag);

                            //Broadcast the events
                            envi.BroadcastEvents(tempEventNames);
                            ToSubComponentState();
                        }
                    }

                    if (collidertag.Equals("ExpandIcon"))
                    {
                        //if its the same icon or envi.currentExpandIcon -> back to majorcomponent state
                        if(hit.collider.gameObject.transform.parent.parent.name.Equals(envi.currentExpandIconGameObject.name))
                        {
                            //Debug.Log("Same ExpandIcon selected");
                            
//							if (envi.currentSystemName == "E5") 
//							{
//								Debug.Log ("First");
//								envi.activateCameraCoroutine (Organized.Instance.systemDictionary [envi.currentSystemName + " (Trans)"].subComponentDictionary [envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
//							} 
//							else 
							{
								//Debug.Log ("Second");
								//Moves camera between zoomPosition and cameraPosition
								envi.activateCameraCoroutine (Organized.Instance.systemDictionary [envi.currentSystemName + " (Trans)"].majorComponentDictionary [envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
							}
							//Debug.Log ("Third");
                            //close the window //might be able to add it as one line above
                            Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

                            tempEventNames.Add("ExpandIcon");
                            envi.BroadcastEvents(tempEventNames);
                        }
                        //else envi.currenticon = the new icon and move to that icons zoomPos  
                        else
                        {
                            //Debug.Log("New ExpandIcon selected");

//							if (envi.currentSystemName == "E5") 
//							{
//								Debug.Log ("Fourth");
//								Debug.Log (envi.currentSystemName);
//								Debug.Log (hit.collider.gameObject.transform.parent.parent.name);
//								Debug.Log (Organized.Instance.systemDictionary [envi.currentSystemName + " (Trans)"].subComponentDictionary [hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
//								envi.activateCameraCoroutine (Organized.Instance.systemDictionary [envi.currentSystemName + " (Trans)"].majorComponentDictionary [hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
//							} 
//							else 
							{
								//Debug.Log ("Fifth");
								//Moves camera between zoomPosition and new zoomPosition
								envi.activateCameraCoroutine (Organized.Instance.systemDictionary [envi.currentSystemName + " (Trans)"].majorComponentDictionary [hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
							}
                            //Closes old icon window
                            //envi.currentExpandIcon.expand(); //old
                            //envi.currentExpandIconGameObject; //new

                            //close the window //might be able to add it as one line above
							//Debug.Log ("Sixth");
                            Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

							//Debug.Log ("Seventh");
                            //Saves the new Current ExpandIcon
                            envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                            //envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

							//Debug.Log ("Eight");
                            //Opens new icon window
                            //envi.currentExpandIcon.expand(); //old
                            Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();
                        }


                        //Debug.Log("ExpandIcon hit!");                       
                    }

                    //Add if subComponent button

                }
            }
        }
    }
}
