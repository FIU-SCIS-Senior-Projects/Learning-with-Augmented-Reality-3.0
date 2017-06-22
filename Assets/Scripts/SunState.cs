using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class SunState : IDiagramState
{
    private readonly StatePatternDiagram dia;

    public SunState(StatePatternDiagram statePatternDia)
    {
        dia = statePatternDia;
    }

    public void ToSoilState()
    {
        dia.currentDiaState = dia.soilState;
    }

    public void ToSunState()
    {
        Debug.Log("Can't transition to same state.");
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
    }
}
