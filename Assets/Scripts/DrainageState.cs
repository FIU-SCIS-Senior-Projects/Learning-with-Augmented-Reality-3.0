using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DrainageState : IDiagramState
{
    private readonly StatePatternDiagram dia;

    public DrainageState(StatePatternDiagram statePatternDia)
    {
       dia = statePatternDia;
    }

    public void ToSoilState()
    {
        dia.currentDiaState = dia.soilState;
    }

    public void ToSunState()
    {
        dia.currentDiaState = dia.sunState;
    }

    public void ToWindState()
    {
        dia.currentDiaState = dia.windState;
    }

    public void ToDrainageState()
    {
        Debug.Log("Can't transition to same state.");
    }

    public void UpdateState()
    {
        dia.timer += Time.deltaTime;

        if (dia.timer > 5f)
        {
            Debug.Log(dia.currentDiaState);
            dia.timer = 0;
        }

    }
    
    void OnEnable()
    {
        EventManager.StartListening("Drainage", activateData);
    }

    void OnDiasable()
    {
        EventManager.StopListening("Drainage", activateData);
    }

    public void activateData()
    {
        Debug.Log("Drainage State Activated.");
    }

    public void OnTriggerClicked()
    {
        string collidertag = null;
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;

                if (collidertag.Equals("Return"))
                {
                    tempEventNames.Add(collidertag);

                    dia.BroadcastEvents(tempEventNames);
                }
            }
        }
    }
}
