/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <ActivateAnimation. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Collections.Generic;

public class ActivateAnimation : MonoBehaviour
{
    public List<GameObject> animationhideObjects = new List<GameObject>();
    public List<GameObject> animationToggleObjects = new List<GameObject>();
    //private Transform[] all;
    //private Transform isolateObjects;
    //public List<Transform> disableComponents = new List<Transform>();
    private bool hidden = false;
    public List<GameObject> constructionAnimations = new List<GameObject>();
    public List<GameObject> mechanicalAnimations = new List<GameObject>();
    public List<GameObject> soilAnimations = new List<GameObject>();
    public List<GameObject> drainageAnimations = new List<GameObject>();
    private TextMesh text;
    private Camera currentCam;
    private Vector3 tempPlayerPos;

    private readonly StatePatternEnvironment envi;

    // Use this for initialization
    void Start()
    {
        /*
        all = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];

        for (int i = 0; i < all.Length; i++)
        {
            everything.Add(all[i].gameObject);
        }
        */
        //all = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
        //all = (GameObject) GameObject.FindObjectsOfType(GameObject);

        text = gameObject.GetComponentInChildren<TextMesh>();

        tempPlayerPos = new Vector3();

        /*
        for (int i = 0; i < disableComponents.Count; i++)
        {
            Collider col = disableComponents[i].gameObject.GetComponent<Collider>();
            col.enabled = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        EventManager.StartListening("Build", callBuild);
        EventManager.StartListening("Mechanical", callMechanical);
        EventManager.StartListening("SoilAnimation", callSoilAnimation);
        EventManager.StartListening("Drainage", callDrainage);
    }

    void OnDisable()
    {
        EventManager.StopListening("Build", callBuild);
        EventManager.StopListening("Mechanical", callMechanical);
        EventManager.StopListening("SoilAnimation", callSoilAnimation);
        EventManager.StopListening("Drainage", callDrainage);
    }

    public void callBuild()
    {
        actiavteAnimation("Build");
    }

    public void callMechanical()
    {
        actiavteAnimation("Mechanical");
    }

    public void callSoilAnimation()
    {
        actiavteAnimation("SoilAnimation");
    }

    public void callDrainage()
    {
        actiavteAnimation("Drainage");
    }

    public void actiavteAnimation(string state)
    {
        if (gameObject.activeSelf)
        {
            //Debug.Log("Building!");
            if (hidden == false)
            {
                //Debug.Log("Objects active - Hide.");
                //Organized.Instance.hide(animationhideObjects);
                if (text)
                {
                    text.text = "Return";
                }
                //Debug.Log("Before saving position: Organized.Instance.player.transform.position " + Organized.Instance.player.transform.position);

                //Debug.Log("tempPlayerPos.position " + tempPlayerPos.position);
                tempPlayerPos = Organized.Instance.player.transform.position;
                //Debug.Log("tempPlayerPos.position " + tempPlayerPos.position);
                //Debug.Log("After saving position: Organized.Instance.player.transform.position " + Organized.Instance.player.transform.position);
                //Debug.Log("After saving position: tempPlayerPos " + tempPlayerPos);

                //Alignment
                if (state.Equals("Build"))
                {
                    Organized.Instance.hide(animationhideObjects);
                    text.transform.Translate(new Vector3(-.125f, 0, 0));

                    //Start Animation
                    Organized.Instance.toggle(constructionAnimations);
                    constructionAnimations[0].GetComponent<Animation>().Play("Take 001");
                }
                if (state.Equals("Mechanical"))
                {
                    Organized.Instance.hide(animationhideObjects);
                    text.transform.Translate(new Vector3(.35f, 0, 0));
                    //tag = "Return";
                }
                if (state.Equals("SoilAnimation"))
                {
                    Organized.Instance.hide(animationhideObjects);
                    //Debug.Log ();
                    if (text)
                    {
                        text.transform.Translate(new Vector3(-.125f, 0, 0));
                        //Debug.Log ("SoilAnimation is playing");
                        text.transform.Translate(new Vector3(0, 0, 0));
                    }

                    //Start Animation
                    Organized.Instance.toggle(soilAnimations);
                    soilAnimations[0].GetComponent<Animation>().Play("Take 001");

                    //gameObject.tag = "Return";
                    //text.tag = "Return";
                }

                if (state.Equals("Drainage"))
                {
                    Organized.Instance.hide(animationhideObjects);
                    Organized.Instance.toggle(animationToggleObjects);
                    //Debug.Log ();
                    if (text)
                    {
                        text.transform.Translate(new Vector3(0, 0, 0));
                        //Debug.Log ("SoilAnimation is playing");
                        text.transform.Translate(new Vector3(0, 0, 0));
                    }

                    //Start Animation
                    Organized.Instance.toggle(drainageAnimations);
                    //soilAnimations[0].GetComponent<Animation>().Play("Take 001");

                    //gameObject.tag = "Return";
                    //text.tag = "Return";
                }

                hidden = true;
            }

            else
            {
                //Debug.Log("UnHiding!");
                //Organized.Instance.unHide(animationhideObjects);

                //Debug.Log("Before return move: Organized.Instance.player.transform.position " + Organized.Instance.player.transform.position);
                //Debug.Log("Before return move: tempPlayerPos " + tempPlayerPos);
                Organized.Instance.player.transform.position = tempPlayerPos;
                //Debug.Log("After return move: Organized.Instance.player.transform.position " + Organized.Instance.player.transform.position);
                //Debug.Log("After return move: tempPlayerPos " + tempPlayerPos);

                //Alignment
                if (state.Equals("Build"))
                {
                    Organized.Instance.unHide(animationhideObjects);
                    if (text)
                    {
                        text.text = "Build";
                        text.transform.Translate(new Vector3(.125f, 0, 0));
                    }
                    //tag = "Build";

                    //Start Animation
                    Organized.Instance.toggle(constructionAnimations);
                }
                if (state.Equals("Mechanical"))
                {
                    Organized.Instance.unHide(animationhideObjects);
                    if (text)
                    {
                        text.text = "Mechanical";
                        text.transform.Translate(new Vector3(-.35f, 0, 0));
                    }
                    tag = "Mechanical";

                    //Start Animation
                    Organized.Instance.toggle(mechanicalAnimations);
                }
                if (state.Equals("SoilAnimation"))
                {
                    Organized.Instance.unHide(animationhideObjects);
                    if (text)
                    {
                        text.text = "Soil Profile and \n Dewatering";
                        text.transform.Translate(new Vector3(0, 0, 0));
                    }
                    tag = "SoilAnimation";

                    //Start Animation
                    Organized.Instance.toggle(soilAnimations);

                    //call return event
                    EventManager.TriggerEvent("MainState");
                }
                if (state.Equals("Drainage"))
                {
                    Organized.Instance.unHide(animationhideObjects);
                    //Organized.Instance.toggle(animationToggleObjects);
                    if (text)
                    {
                        text.text = "Retention Basin";
                        text.transform.Translate(new Vector3(0, 0, 0));
                    }                    
                    tag = "Drainage";

                    //Start Animation
                    Organized.Instance.toggle(drainageAnimations);

                    //call return event
                    EventManager.TriggerEvent("MainState");
                }

                hidden = false;
            }
            //changeSkybox();
        }
    }
}