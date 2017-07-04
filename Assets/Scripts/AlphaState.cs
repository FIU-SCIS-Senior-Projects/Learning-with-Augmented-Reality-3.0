/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <AlphaState. Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Collections.Generic;

public class AlphaState : IARState
{
    private readonly StatePatternAR envi;

    public AlphaState(StatePatternAR statePatternAR)
    { envi = statePatternAR; }

    public void UpdateState()
    {
        if (envi.isGetSubPanelCalled == true)
        {
            Description.Instance.setDescript(envi.currentSubPanelName);
            envi.isGetSubPanelCalled = false;
        }

        //returns collider.tag on click
        if (Input.GetMouseButtonDown(0))
        {
            OnTriggerClicked();
        }
    }

    public void OnTriggerClicked()
    {
        //if(IOS or Android device)
        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();

		//#if UNITY_ANDROID
		//Debug.Log("Test");
		//#endif

		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        Debug.Log("Stand Alone or Webplayer");

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 9) | (1 << 10) | (1 << 12)))) //no componet layer anymore becuase we are relying on ExpandIcons to go to Subcomponent State
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);
                if (envi.isMoving == false)
                {
                    if (collidertag.Equals("ExpandIcon"))
                    {
                        envi.priorToIconState = envi.AlphaState;

                        //Saves players position
                        //envi.currentPlayerPos = Camera.main.transform;
                        envi.tempPlayerPos = Organized.Instance.player.transform.position; //before last change
						//envi.currentCameraHead = Camera.main.gameObject;
						//envi.tempPlayerPos = envi.currentCameraHead.transform.position;

                        envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        //envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

                        //Moves camera to zoomPosition
                        envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
                        //envi.cameraHead.transform.position = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentDictionary[envi.currentExpandIcon.name].iconControl.zoomPos.position;

						//Highlight//

						envi.selectedComponent = hit.collider.gameObject;

						//If its a expandIcon
						if (hit.collider.tag == "ExpandIcon")
						{
							//Highlight SelectedComponent
							//selectedComponent.GetComponent<Renderer>().material.shader = highlightShader;
							hit.collider.gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.cyan);
						}
							
						envi.previousSelectedComponent = envi.selectedComponent;

                        //Disables Physics and Movement for Camera while zoomed
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
                        //envi.cameraHead.GetComponent<Rigidbody>().isKinematic = true;

                        Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

                        //Debug.Log("From MajorComponentState " + envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().window);
                        //envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().expand();

                        Debug.Log("ExpandIcon hit!");
                        //Debug.Log(hit.collider.gameObject.name);

                        tempEventNames.Add("ExpandIcon");
                        envi.BroadcastEvents(tempEventNames);

                        //ToIconState();
						envi.currentState = envi.iconStateAR;
						GameObject skopeButton = GameObject.Find("Panel_MechanicalA").transform.FindChild("SKOPE").gameObject;
						Organized.Instance.toggle(skopeButton);
                    }

                    //If it hits a return button
                    if (collidertag.Equals("Return"))
                    {

                        Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIcons);
                        //Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIconGameObjects);
                        //Debug.Log (Organized.Instance.systems[15].name); //worked
                        //Debug.Log(Organized.Instance.systemDictionary["VR_Heavy"]); //did not work
                        //Debug.Log(Organized.Instance.systemsList[15].name); //did not work
                        //Debug.Log(Organized.Instance.systemDictionary[envi.currentSystemName]._name);

                        try
                        {
                            //Adds the hit colliders system and its trans system
                            //systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName]._root.gameObject); //before exterior walls were clickable
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName].layerDictionary[envi.currentSystemName + "Detail"].gameObject);
                            //Debug.Log(Organized.Instance.systemsList[15].layers.Count); //19
                            //						for(int y=0; y<Organized.Instance.systemsList[15].layers.Count; y++)
                            //						{
                            //							if(Organized.Instance.systemsList[15].layers[y].Equals("E4Deatil"))
                            //							{
                            //								systemsHit.Add(Organized.Instance.systemsList[15].layers[y].gameObject);
                            //								Debug.Log(Organized.Instance.systemsList[15].layers[y].name);
                            //							}
                            //						}

                            //						systemsHit.Add(Organized.Instance.systemDictionary["VR_Heavy"].layerDictionary[envi.currentSystemName + "Detail"].gameObject); //dictionary does not work if stuff is hidden
                            //systemsHit.Add(Organized.Instance.systemsList[15].layerDictionary[envi.currentSystemName + "Deatil"]);

                            //Debug.Log(Organized.Instance.systemsList[15].name);
                            //						foreach(GameObject s in Organized.Instance.systemDictionary)
                            //						{
                            //							Debug.Log(s.name);
                            //						}
                            //Debug.Log(Organized.Instance.systemDictionary["VR_Heavy"].name);
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"]._root.gameObject);
                            //systemsHit.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);
                            envi.toggle(systemsHit);
                        }

                        catch
                        {
                            Debug.Log("Error: the hit collider's tag does not match any current systems.");
                        }

                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);

                        //Broadcast the events
                        envi.BroadcastEvents(tempEventNames);
                        envi.currentState = envi.mainStateAR;
                    }

                    
                }
            }
        }
	#endif


	#if UNITY_IOS

    Debug.Log("Mobile IOS");
	
		if(Input.touches.Length > 0)
		{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out hit, Mathf.Infinity, ((1 << 9) | (1 << 10) | (1 << 12)))) //no componet layer anymore becuase we are relying on ExpandIcons to go to Subcomponent State
        {
            if (hit.collider != null)
            {
                collidertag = hit.collider.tag;
                Debug.Log(collidertag);
                if (envi.isMoving == false)
                {
                    if (collidertag.Equals("ExpandIcon"))
                    {
                        envi.priorToIconState = envi.AlphaState;

                        //Saves players position
                        //envi.currentPlayerPos = Camera.main.transform;
                        envi.tempPlayerPos = Organized.Instance.player.transform.position;

                        envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        //envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

                        //Moves camera to zoomPosition
                        envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
                        //envi.cameraHead.transform.position = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentDictionary[envi.currentExpandIcon.name].iconControl.zoomPos.position;

                        //Disables Physics and Movement for Camera while zoomed
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
                        //envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
                        //envi.cameraHead.GetComponent<Rigidbody>().isKinematic = true;

                        Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

                        //Debug.Log("From MajorComponentState " + envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().window);
                        //envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().expand();

                        Debug.Log("ExpandIcon hit!");
                        //Debug.Log(hit.collider.gameObject.name);

                        tempEventNames.Add("ExpandIcon");
                        envi.BroadcastEvents(tempEventNames);

                        //ToIconState();
                        envi.currentState = envi.iconStateAR;
						GameObject skopeButton = GameObject.Find("Panel_MechanicalA").transform.FindChild("SKOPE").gameObject;
						Organized.Instance.toggle(skopeButton);
                    }

                    //If it hits a return button
                    if (collidertag.Equals("Return"))
                    {

                        Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIcons);

                        try
                        {
                            //Adds the hit colliders system and its trans system
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName].layerDictionary[envi.currentSystemName + "Detail"].gameObject);
                           
                            systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"]._root.gameObject);
                            //systemsHit.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);
                            envi.toggle(systemsHit);
                        }

                        catch
                        {
                            Debug.Log("Error: the hit collider's tag does not match any current systems.");
                        }

                        //Add collider tag as an event
                        tempEventNames.Add(collidertag);

                        //Broadcast the events
                        envi.BroadcastEvents(tempEventNames);
                        envi.currentState = envi.mainStateAR;
                    }
                }
            }
        }
		}
#endif

		#if UNITY_ANDROID

		Debug.Log("Mobile Android");

		if(Input.touchCount > 0)
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, Mathf.Infinity, ((1 << 9) | (1 << 10) | (1 << 12)))) //no componet layer anymore becuase we are relying on ExpandIcons to go to Subcomponent State
			{
				if (hit.collider != null)
				{
					collidertag = hit.collider.tag;
					Debug.Log(collidertag);
					if (envi.isMoving == false)
					{
						if (collidertag.Equals("ExpandIcon"))
						{
							envi.priorToIconState = envi.AlphaState;

							//Saves players position
							//envi.currentPlayerPos = Camera.main.transform;
							envi.tempPlayerPos = Organized.Instance.player.transform.position;

							envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
							//envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

							//Moves camera to zoomPosition
							envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);
							//envi.cameraHead.transform.position = Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].majorComponentDictionary[envi.currentExpandIcon.name].iconControl.zoomPos.position;

							//Disables Physics and Movement for Camera while zoomed
							//envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_GravityMultiplier = 0;
							//envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
							//envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
							//envi.cameraHead.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = 0;
							//envi.cameraHead.GetComponent<Rigidbody>().isKinematic = true;

							Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

							//Debug.Log("From MajorComponentState " + envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().window);
							//envi.currentExpandIconGameObject.GetComponent<ExpandIcon>().expand();

							Debug.Log("ExpandIcon hit!");
							//Debug.Log(hit.collider.gameObject.name);

							tempEventNames.Add("ExpandIcon");
							envi.BroadcastEvents(tempEventNames);

							//ToIconState();
							envi.currentState = envi.iconStateAR;
						}

						//If it hits a return button
						if (collidertag.Equals("Return"))
						{

							Organized.Instance.toggle(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"].expandIcons);

							try
							{
								//Adds the hit colliders system and its trans system
								systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName].layerDictionary[envi.currentSystemName + "Detail"].gameObject);

								systemsHit.Add(Organized.Instance.systemDictionary[envi.currentSystemName + " (Trans)"]._root.gameObject);
								//systemsHit.Add(Organized.Instance.panelDictionary["Panel_Annotation"]);
								envi.toggle(systemsHit);
							}

							catch
							{
								Debug.Log("Error: the hit collider's tag does not match any current systems.");
							}

							//Add collider tag as an event
							tempEventNames.Add(collidertag);

							//Broadcast the events
							envi.BroadcastEvents(tempEventNames);
							envi.currentState = envi.mainStateAR;
						}
					}
				}
			}
		}
		#endif
    }
}
