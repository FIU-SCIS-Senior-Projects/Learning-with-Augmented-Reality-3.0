using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SoilState : IDiagramState
{

    private readonly StatePatternDiagram dia;

    public SoilState(StatePatternDiagram statePatternDia)
    {
        dia = statePatternDia;
    }

    //public void ToMainState()
    //{
    //    dia.currentDiaState = dia.mainState;
    //}

    public void ToSoilState()
    {
        Debug.Log("Can't transition to same state.");
    }

    public void ToSunState()
    {
        dia.currentDiaState = dia.sunState;
    }

    public void ToWindState()
    {
        dia.currentDiaState = dia.windState;
    }

    public void UpdateState()
    {
        dia.timer += Time.deltaTime;
        if (dia.timer > 5f)
        {
            Debug.Log(dia.currentDiaState);
            dia.timer = 0;
        }

        ////returns collider.tag on click
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OnTriggerClicked();
        //}
    }

    void OnEnable()
    {
        EventManager.StartListening("Soil", activateData);
        EventManager.StartListening("SoilAnimation", activateData);
    }

    void OnDisable()
    {
        EventManager.StopListening("Soil", activateData);
        EventManager.StopListening("SoilAnimation", activateData);
    }

    public void activateData()
    {
        Debug.Log("Soil State Activated.");
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
                //Debug.Log (collidertag);

                //If it hits a return button
                if (collidertag.Equals("Return"))
                {
                    //Add collider tag as an event
                    tempEventNames.Add(collidertag);

                    //Broadcast the events
                    dia.BroadcastEvents(tempEventNames);

                    //ToMainState ();
                }
            }
        }

    }

}