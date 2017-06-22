using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

public class MechanicalState : IEnvironmentState
{
    private readonly StatePatternEnvironment envi;

    public MechanicalState(StatePatternEnvironment statePatternEnvi)
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
        Debug.Log("Can't transition to same state.");
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

                //If its a ReturnButton
                if (collidertag.Equals("Mechanical"))
                {
                    //EventManager.TriggerEvent(collidertag);
                    Debug.Log("Returned from Mechanical State");

                    Organized.Instance.toggle(Organized.Instance.systemDictionary["M1"]._root);
                    Organized.Instance.toggle(Organized.Instance.systemDictionary["M1 (T)"]._root);

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
