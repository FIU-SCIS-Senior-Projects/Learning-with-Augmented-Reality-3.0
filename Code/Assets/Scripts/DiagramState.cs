/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <DiagramState.  Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
    public RaycastHit hit = new RaycastHit();

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

    public void ToIconState()
    {
        envi.currentState = envi.iconState;
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

        //if raycast hits
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);

                //If its a SoilAnimation
                if (collidertag.Equals("SoilAnimation") || collidertag.Equals("Drainage") || collidertag.Equals("RValue") || collidertag.Equals("GeneratorVideo") || collidertag.Equals("WMechanicalVideo") || collidertag.Equals("MechanicalVideo")
                    || collidertag.Equals("EE") || collidertag.Equals("ShadingCo"))
                {
                    //EventManager.TriggerEvent(collidertag);
                    envi.currentPlayerPos = Camera.main.transform;

                    if (!collidertag.Equals("RValue") || !collidertag.Equals("EE") || !collidertag.Equals("ShadingCo") 
                        || !collidertag.Equals("GeneratorVideo") || !collidertag.Equals("WMechanicalVideo") || !collidertag.Equals("MechanicalVideo"))
                    {
                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);
                    }

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
        if (hit.collider.tag.Equals("MechanicalVideo"))
            EventManager.StartListening("MechanicalVideo", ToMainState);

        if (hit.collider.tag.Equals("GeneratorVideo"))
            EventManager.StartListening("GeneratorVideo", ToMainState);

        if (hit.collider.tag.Equals("WMechanicalVideo"))
            EventManager.StartListening("WMechanicalVideo", ToMainState);

        if (hit.collider.tag.Equals("RValue"))
            EventManager.StartListening("RValue", ToMainState);

        if (hit.collider.tag.Equals("EmbodiedEnergy"))
            EventManager.StartListening("EE", ToMainState);

        if (hit.collider.tag.Equals("ShadingCo"))
            EventManager.StartListening("ShadingCo", ToMainState);
        
    }

    void OnDisable()
    {
        if (hit.collider.tag.Equals("MechanicalVideo"))
            EventManager.StartListening("MechanicalVideo", ToMainState);

        if (hit.collider.tag.Equals("GeneratorVideo"))
            EventManager.StartListening("GeneratorVideo", ToMainState);

        if (hit.collider.tag.Equals("WMechanicalVideo"))
            EventManager.StartListening("WMechanicalVideo", ToMainState);

        if (hit.collider.tag.Equals("RValue"))
            EventManager.StartListening("RValue", ToMainState);

        if (hit.collider.tag.Equals("EmbodiedEnergy"))
            EventManager.StartListening("EE", ToMainState);

        if (hit.collider.tag.Equals("ShadingCo"))
            EventManager.StartListening("ShadingCo", ToMainState);
      
    }
}