/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <EventManager. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    //VARIABLES//
    private Dictionary <string, UnityEvent> eventDictionary;//Dictionary of Methods for Events

    //Class Variables
    private static EventManager eventManager;               //An instance of this class EventManager
    public static EventManager instance                     //A getter for the instance (eventManager)
    {
        get
        {
            //if there is no eventManager Linked to script then look for one
            if(!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                //if there is no event manager component attached anywhere we need to add it somewhere
                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject" +
                        " in your scene.");
                }
                //if there is a floating eventManager, initialize it
                else
                {
                    eventManager.Init();
                }
            }
            //return current event manager
            return eventManager;
        }
    }
    
    /*
    public delegate void ClickAction();                     //Method type
    public static event ClickAction DiagramButtonPressed;   //Variable of Method type

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        if(ray().Equals("Sun"))
        {
            if (DiagramButtonPressed != null)
            {
                DiagramButtonPressed();
            }
        }
    }
    */

    //initializes the event manager by creating a new dictionary for it
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    //Adds a listner to the event called
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        //if event already exists in dictionary, add a listner to it
        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //add this listener
            thisEvent.AddListener(listener);
        }

        //else create a new event, add it to dictionary, and add a listner to it
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    //Removes the listner from the event called back
    public static void StopListening(string eventName, UnityAction listener)
    {
        //if destroyed for some reason, terminate
        if (eventManager == null) return;

        UnityEvent thisEvent = null;
        //removes the listener from the event
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //triggers the event being called
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        //if event exists trigger it using invoke
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    //triggers the event being called
    public static GameObject TriggerEvent(string eventName, GameObject passItem)
    {
        UnityEvent thisEvent = null;
        //if event exists trigger it using invoke
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }

        return passItem;
    }

    /*
    public string ray()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            //if raycast hit
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) //pulse.maxDistance))
            {
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
            }
        }
        return "";
    }
    */
}
