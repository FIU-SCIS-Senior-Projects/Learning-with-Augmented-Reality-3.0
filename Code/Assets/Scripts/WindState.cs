using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class WindState : IDiagramState
{

    private readonly StatePatternDiagram dia;

    public WindState(StatePatternDiagram statePatternDia)
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
        EventManager.StartListening("Wind", activateData);
    }

    void OnDisable()
    {
        EventManager.StopListening("Wind", activateData);
    }

    public void activateData()
    {
        Debug.Log("Wind State Activated.");
    }
}