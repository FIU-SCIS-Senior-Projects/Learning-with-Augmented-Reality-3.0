using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoilAnimation : MonoBehaviour
{
    public GameObject panel;
    //public List<GameObject> toggleObjects = new List<GameObject>();
    public List<GameObject> hideObjects = new List<GameObject>();
    private bool hidden = false;


    void OnEnable()
    {
        //EventManager.DiagramButtonPressed += activateData;
        EventManager.StartListening("Soil", activateData);
    }

    void OnDisable()
    {
        //EventManager.DiagramButtonPressed -= activateData;
        EventManager.StopListening("Soil", activateData);
    }

    public void activateData()
    {
        if(!hidden)
        {
            panel.SetActive(true);
            if (hideObjects.Count != 0)
            {
                Organized.Instance.hide(hideObjects);
                hidden = true;
            }
        }
        else
        {
            if (hideObjects.Count != 0)
            {
                Organized.Instance.unHide(hideObjects);
                hidden = false;
            }
        }
    }
}
