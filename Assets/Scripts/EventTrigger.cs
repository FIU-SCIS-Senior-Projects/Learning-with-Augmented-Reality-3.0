/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <EventTrigger. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using UnityEngine.UI;
using System.Collections.Generic;

public class EventTrigger : MonoBehaviour {

    //public List<GameObject> secondaryInterfaces = new List<GameObject>();
    //int secondaryInterfacesNum = 0;

    // Use this for initialization
    void Start () {

        /*
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Build");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Return");
        GameObject[] temp3 = GameObject.FindGameObjectsWithTag("FloatingButton");

        for (int i=0; i<temp.Length; i++)
        {
            secondaryInterfaces.Add(temp[i]);
        }
        for (int i = 0; i < temp2.Length; i++)
        {
            secondaryInterfaces.Add(temp2[i]);
        }
        for (int i = 0; i < temp3.Length; i++)
        {
            secondaryInterfaces.Add(temp3[i]);
        }
        */
    }
	
	// Update is called once per frame
	void Update ()
    {
        //ray();
        /*
        if (ray().Equals("MajorComponent"))
        {
            //EventManager.TriggerEvent("IsolationView");
        }

         if(ray().Equals("FloatingButton"))
        {
            //EventManager.TriggerEvent("MainView");
        }

        if (ray().Equals("Transparent1"))
        {
            //EventManager.TriggerEvent("MajorComponentView");
        }
        */

        //Sun Button Trigger
        /*
        if (ray().Equals("Sun"))
        {
            Debug.Log("Ray hit sun! -> SunDiagramActive");
            EventManager.TriggerEvent("SunDiagramActive");
        }
        */

        /*
        //Secondary Interface Mode
        for (int i = 0; i < secondaryInterfaces.Count; i++)
        {
            if (secondaryInterfaces[i].activeSelf == true)
            {
                EventManager.TriggerEvent("SecondaryInterfaceMode");
            }
        }
        */

    }

    public string ray()
    {
        //if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            //if raycast hit
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) //pulse.maxDistance))
            {
                /*
                if(hit.collider.tag.Equals("SubComponent"))
                {
                    Debug.Log("EVent");
                    EventManager.TriggerEvent("ParentComponent");
                }
                */

                /*
                if (hit.collider.tag == "Sun")
                {
                    //Debug.Log("Sun Hit!");
                    return "Sun";
                }

                if (hit.collider.tag == "Wind")
                {
                    //Debug.Log("Wind Hit!");
                    return "Wind";
                }

                if (hit.collider.tag == "Soil")
                {
                    //Debug.Log("Soil Hit!");
                    return "Soil";
                }
                */

                //Debug.Log("EventTrigger.ray(): " + hit.collider.tag);
                return hit.collider.tag;
            }
        }
        return "";
    }
}
