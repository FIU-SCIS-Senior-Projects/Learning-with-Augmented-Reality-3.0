/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <MechanicalRoomState. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

public class MechanicalRoomState : IEnvironmentState
{

    private readonly StatePatternEnvironment envi;

    public MechanicalRoomState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
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
        envi.currentState = envi.majorComponentState;
    }

    public void ToSubComponentState()
    {
        envi.currentState = envi.mechanicalState;
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

    //Fix This
    public void OnTriggerClicked()
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();

        //if raycast hits
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                //Debug.Log(collidertag);

                if (collidertag.Equals("MajorComponent"))
                {
                    //Saves players position
                    envi.currentPlayerPos = Camera.main.transform;
                    envi.tempPlayerPos = Organized.Instance.player.transform.position;

                    //get the diableOnIsolate list for the system
                    if (Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders == null)
                    {
                        Debug.Log("disableOnIsolateColliders is empty");
                    }

                    List<Collider> toBeDisabled = Organized.Instance.systemDictionary[envi.currentSystemName].disableOnIsolateColliders;

                    //disable the disableOnIso colliders for the system
                    envi.disable(toBeDisabled);

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

                    //Isolate the trans system
                    Organized.Instance.hide(objectsToHide);
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

                    envi.activateFadeCoroutine((Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentsTransforms));

                    //Add collider tag as an event
                    tempEventNames.Add(collidertag);

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                    ToSubComponentState();
                }
            }
        }
    }
}
