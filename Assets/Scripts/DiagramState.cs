/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <DiagramState. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class DiagramState : IEnvironmentState
{
    private readonly StatePatternEnvironment envi;

    public DiagramState(StatePatternEnvironment statePatternEnvi)
    {
        envi = statePatternEnvi;
    }

    public void OnTriggerPressed(Collider other)
    {

    }

    public void ToDiagramState()
    {
        Debug.Log("Can't transition to same state.");
    }

    public void ToMainState()
    {
        Debug.Log("Main State Activated");
        envi.currentState = envi.mainState;
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

    public void UpdateState()
    {
        envi.timer += Time.deltaTime;
        if (envi.timer > 5f)
        {
            Debug.Log(envi.currentState);
            envi.timer = 0;
        }

        //returns collider.tag on click
        if (Input.GetMouseButtonDown(0))
        {
            OnTriggerClicked();
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);

                //If its a SoilAnimation
                if (collidertag.Equals("SoilAnimation") || collidertag.Equals("Drainage"))
                {
                    //EventManager.TriggerEvent(collidertag);
                    envi.currentPlayerPos = Camera.main.transform;

                    //Add collider tag as an event
                    tempEventNames.Add(collidertag);

                    //Organized.Instance.panelDictionary["Panel_Soil"].SetActive(false);

                    //Broadcast the events
                    envi.BroadcastEvents(tempEventNames);
                    ToMainState();
                }
            }
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("Return", ToMainState);
        //ReturnButton.OnClicked += ToMainState;
    }

    void OnDisable()
    {
        EventManager.StopListening("Return", ToMainState);
        //ReturnButton.OnClicked -= ToMainState;
    }
}