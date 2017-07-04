/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <IconStateAR. Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconStateAR : IARState
{

    private readonly StatePatternAR envi;

	public ChangeSprite myCS = new ChangeSprite();

    public IconStateAR(StatePatternAR statePatternAR)
    { envi = statePatternAR; }

    	
	// Update is called once per frame
	public void UpdateState ()
    {
        //returns collider.tag on click
        if (Input.GetMouseButtonDown(0))
        {
            OnTriggerClicked();
        }
    }

    private IEnumerator PlayStreamingVideo(string url)
    {
        Debug.Log("PLAYSTREAMINGVIDEO Be");
        Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
        Debug.Log("PLAYSTREAMINGVIDEO Af");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Debug.Log("Video playback completed.");
    }

    public void OnTriggerClicked()
    {
        string collidertag = null;
        //Debug.Log(collidertag);
        List<string> tempEventNames = new List<string>();
        List<GameObject> systemsHit = new List<GameObject>();
        RaycastHit hit = new RaycastHit();


#if UNITY_IOS
		Debug.Log("Mobile IOS");

//		if(Input.touches == null)
//		{
//			//Debug.Log("Touches is null");
//		}

		if(Input.touches.Length > 0)
		{
			//Debug.Log("1");
			//Debug.Log("Input.touches.Length" + Input.touches.Length);
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out hit, Mathf.Infinity, ((1 << 10) | (1 << 14))))
	        {
				//Debug.Log("2");
	            if (hit.collider != null)
	            {
	                collidertag = hit.collider.tag;
					//Debug.Log("collidertag" + collidertag);

	                if (envi.isMoving == false)
	                {
						
						if(collidertag.Equals("Diagram"))
						{						
							GameObject diagramPanel = Organized.Instance.panelDictionary["Title_Description"];
							//diagramPanel.transform.Find("DiagramCanvas").GetComponent<Image>().sprite;

							Description.Instance.setDescript(hit.collider.gameObject.transform.parent.parent.parent.parent.name);

							if(hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
							{
								Debug.Log("ERV");
								Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
								//envi.currentSubPanel = Organized.Instance.subPanelDictionary["ERV"];
								envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
								if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
								{
									Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
								}
								Debug.Log(Resources.Load<Sprite>("HeatRecoveryVentilators"));
								Sprite tempSprite = Resources.Load<Sprite>("HeatRecoveryVentilators");
								//Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));
								diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;
								//myCS.UpdateSprite(tempSprite);
							}

							if(hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("AHU (2)"))
							{
								Debug.Log("AHU");
								Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
								//envi.currentSubPanel = Organized.Instance.subPanelDictionary["AHU"];
								envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
								if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
								{
									Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
								}
								Debug.Log(Resources.Load<Sprite>("AHU"));
								Sprite tempSprite = Resources.Load<Sprite>("AHU");
								Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));
								diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;	
								//myCS.UpdateSprite((Sprite)Resources.Load("AHU") as Sprite);
							}
						}

						//Debug.Log("Moving is False");
	                    if (collidertag.Equals("ExpandIcon"))
	                    {
							//Debug.Log("collidertag.Equals(\"ExpandIcon\")");
	                        //if its the same icon or envi.currentExpandIcon -> back to majorcomponent state
							if(hit.collider.gameObject.transform.parent.parent == null)
							{
								//Debug.Log("hit.collider.gameObject.transform.parent.parent is null");
							}
							if(envi.currentExpandIconGameObject == null)
							{
								//Debug.Log("envi.currentExpandIconGameObject is null");
							}
	                        if (hit.collider.gameObject.transform.parent.parent.name.Equals(envi.currentExpandIconGameObject.name))
	                        {
								//UnHighlight//
								//Debug.Log ("Got Here: If");
								//Debug.Log(hit.collider.gameObject.transform.parent.parent.name);
								//Debug.Log(envi.currentExpandIconGameObject.name);
								//Debug.Log(hit.collider.gameObject.transform.parent.parent.name.Equals(envi.currentExpandIconGameObject.name));

								if(envi.previousSelectedComponent == null)
								{
									Debug.Log("There is no previous selected component");
								}
								if(envi.expandIconMaterial == null)
								{
									Debug.Log("There is no material");
								}
								if(envi.previousSelectedComponent != null)
								{
									envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterial;
								}
	                            Debug.Log("Same ExpandIcon selected");

	                            //Moves camera between zoomPosition and cameraPosition
	                            envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);

	                            //close the window //might be able to add it as one line above
	                            Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

	                            tempEventNames.Add("ExpandIcon");
	                            envi.BroadcastEvents(tempEventNames);
								GameObject skopeButton = GameObject.Find("Panel_MechanicalA").transform.FindChild("SKOPE").gameObject;
								Organized.Instance.toggle(skopeButton);
	                        }


	                        //else envi.currenticon = the new icon and move to that icons zoomPos  
	                        else
	                        {
								//UnHighlight//
								Debug.Log ("Got Here: Else");
								if(envi.previousSelectedComponent != null)
								{
									envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterial;
								}
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

	                            Debug.Log("New ExpandIcon selected");

	                            //Moves camera between zoomPosition and new zoomPosition
	                            //Debug.Log(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
	                            envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);

	                            //Closes old icon window
	                            //envi.currentExpandIcon.expand(); //old
	                            //envi.currentExpandIconGameObject; //new

	                            //close the window //might be able to add it as one line above
	                            Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();
	                            
	                            //Saves the new Current ExpandIcon
	                            envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
	                            //envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();
	                            
	                            //Opens new icon window
	                            //envi.currentExpandIcon.expand(); //old
	                            Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();
	                        }
	                        //Debug.Log("ExpandIcon hit!");                       
	                    }
        
                       if (collidertag.Equals("Movie"))
                        {
                            if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
                                Handheld.PlayFullScreenMovie("Energy Recovery Machine.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);

                            if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("Generator (1)"))
                                Handheld.PlayFullScreenMovie("S Generator Room.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
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
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, Mathf.Infinity, ((1 << 10) | (1 << 14))))
			{
				if (hit.collider != null)
				{
					collidertag = hit.collider.tag;
					//Debug.Log(collidertag);

					if (envi.isMoving == false)
					{
							if(collidertag.Equals("Diagram"))
							{						
							    GameObject diagramPanel = Organized.Instance.panelDictionary["Title_Description"];
                                //diagramPanel.transform.Find("DiagramCanvas").GetComponent<Image>().sprite;

                                Description.Instance.setDescript(hit.collider.gameObject.transform.parent.parent.parent.parent.name);

                                if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
							    {
						    	    Debug.Log("ERV");
							        Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
						    	    //envi.currentSubPanel = Organized.Instance.subPanelDictionary["ERV"];
							        envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
							        if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
							        {
							            Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
							        }
							        Debug.Log(Resources.Load<Sprite>("HeatRecoveryVentilators"));
							        Sprite tempSprite = Resources.Load<Sprite>("HeatRecoveryVentilators");
							        //Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));
							        diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;
							        //myCS.UpdateSprite(tempSprite);
							    }

							    if(hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("AHU (2)"))
							    {
							        Debug.Log("AHU");
							        Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
							        //envi.currentSubPanel = Organized.Instance.subPanelDictionary["AHU"];
							        envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
							        if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
							        {
							            Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
							        }
							        Debug.Log(Resources.Load<Sprite>("AHU"));
							        Sprite tempSprite = Resources.Load<Sprite>("AHU");
							        Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));
							        diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;	
							        //myCS.UpdateSprite((Sprite)Resources.Load("AHU") as Sprite);
							    }
							}
						
						if (collidertag.Equals("ExpandIcon"))
						{
							//if its the same icon or envi.currentExpandIcon -> back to majorcomponent state
							if (hit.collider.gameObject.transform.parent.parent.name.Equals(envi.currentExpandIconGameObject.name))
							{
								//UnHighlight//
								//Debug.Log ("Got Here");
								envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterialH;

								Debug.Log("Same ExpandIcon selected");

								//Moves camera between zoomPosition and cameraPosition
								envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);

								//close the window //might be able to add it as one line above
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

								tempEventNames.Add("ExpandIcon");
								envi.BroadcastEvents(tempEventNames);
								GameObject skopeButton = GameObject.Find("Panel_MechanicalA").transform.FindChild("SKOPE").gameObject;
								Organized.Instance.toggle(skopeButton);
								
							}
							//else envi.currenticon = the new icon and move to that icons zoomPos  
							else
							{
								//UnHighlight//
								//Debug.Log ("Got Here");
								envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterialH;

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

								Debug.Log("New ExpandIcon selected");

								//Moves camera between zoomPosition and new zoomPosition
								//Debug.Log(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
								envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);

								//Closes old icon window
								//envi.currentExpandIcon.expand(); //old
								//envi.currentExpandIconGameObject; //new

								//close the window //might be able to add it as one line above
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

								//Saves the new Current ExpandIcon
								envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
								//envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

								//Opens new icon window
								//envi.currentExpandIcon.expand(); //old
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();
							}
							//Debug.Log("ExpandIcon hit!");                       
						}

                        if (collidertag.Equals("Movie"))
                        {
                            if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
                                Handheld.PlayFullScreenMovie("Energy Recovery Machine.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);

                            if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("Generator (1)"))
                                Handheld.PlayFullScreenMovie("S Generator Room.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
                        }

                    }
				}
			}
		}
		#endif


		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		Debug.Log("StandAlone or WebPlayer");

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ((1 << 10) | (1 << 14))))
		{
				if (hit.collider != null)
				{
					collidertag = hit.collider.tag;

					if (envi.isMoving == false)
					{

                    if (collidertag.Equals("Diagram"))
					{						
						GameObject diagramPanel = Organized.Instance.panelDictionary["Title_Description"];
                        //diagramPanel.transform.Find("DiagramCanvas").GetComponent<Image>().sprite;
                       
					    Description.Instance.setDescript(hit.collider.gameObject.transform.parent.parent.parent.parent.name);																									 
						if(hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
						{
							Debug.Log("ERV");

							Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
							//envi.currentSubPanel = Organized.Instance.subPanelDictionary["ERV"];
							envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
							if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
							{
								Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
							}

                            Debug.Log(Resources.Load<Sprite>("HeatRecoveryVentilators"));

                            Sprite tempSprite = Resources.Load<Sprite>("HeatRecoveryVentilators");
                            //Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));

                            diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;
    
                            //myCS.UpdateSprite(tempSprite);

                        }
                            
							if(hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("AHU (2)"))
							{
							    Debug.Log("AHU");
								Organized.Instance.toggle(Organized.Instance.panelDictionary["Title_Description"]);
								//envi.currentSubPanel = Organized.Instance.subPanelDictionary["AHU"];
								envi.previousSubPanel = Organized.Instance.panelDictionary["Panel_MechanicalA"];
							if(Organized.Instance.panelDictionary["Panel_MechanicalA"].activeSelf)
							{
								Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_MechanicalA"]);
							}

                            Debug.Log(Resources.Load<Sprite>("AHU"));

                            Sprite tempSprite = Resources.Load<Sprite>("AHU");

                            Debug.Log(diagramPanel.transform.FindChild("DiagramCanvas"));
							diagramPanel.transform.FindChild("DiagramCanvas").GetComponent<Image>().sprite = tempSprite;	
							//myCS.UpdateSprite((Sprite)Resources.Load("AHU") as Sprite);
							}
						}
						
						if (collidertag.Equals("ExpandIcon"))
						{
							//if its the same icon or envi.currentExpandIcon -> back to majorcomponent state
							if (hit.collider.gameObject.transform.parent.parent.name.Equals(envi.currentExpandIconGameObject.name))
							{
								//UnHighlight//
								//Debug.Log ("Got Here");
								envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterialH;

								Debug.Log("Same ExpandIcon selected");

								//Moves camera between zoomPosition and cameraPosition
								envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[envi.currentExpandIconGameObject.name].iconControl.zoomPos.position);

								//close the window //might be able to add it as one line above
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

								tempEventNames.Add("ExpandIcon");
								envi.BroadcastEvents(tempEventNames);
								GameObject skopeButton = GameObject.Find("Panel_MechanicalA").transform.FindChild("SKOPE").gameObject;
								Organized.Instance.toggle(skopeButton);
							}
							//else envi.currenticon = the new icon and move to that icons zoomPos  
							else
							{
								//UnHighlight//
								//Debug.Log ("Got Here");
								envi.previousSelectedComponent.GetComponent<Renderer> ().material = envi.expandIconMaterialH;

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

								Debug.Log("New ExpandIcon selected");

								//Moves camera between zoomPosition and new zoomPosition
								//Debug.Log(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);
								envi.activateCameraCoroutine(Organized.Instance.systemDictionary["M1"].majorComponentDictionary[hit.collider.gameObject.transform.parent.parent.name].iconControl.zoomPos.position);

								//Closes old icon window
								//envi.currentExpandIcon.expand(); //old
								//envi.currentExpandIconGameObject; //new

								//close the window //might be able to add it as one line above
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();

								//Saves the new Current ExpandIcon
								envi.currentExpandIconGameObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
								//envi.currentExpandIcon = envi.currentExpandIconGameObject.GetComponent<ExpandIcon>();

								//Opens new icon window
								//envi.currentExpandIcon.expand(); //old
								Organized.Instance.systemDictionary["M1"].expandIconDictionary[envi.currentExpandIconGameObject.name].expand();
							}
							//Debug.Log("ExpandIcon hit!");                       
						}


                    if (collidertag.Equals("Movie"))
                    {
                        if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("ERV"))
                        {
                            Debug.Log("MOVIE FOR ERV");
                            PlayStreamingVideo("Energy Recovery Machine.mp4");
                            Debug.Log("MOVIE FOR ERV AFTER");
                        }
                        if (hit.collider.gameObject.transform.parent.parent.parent.parent.name.Equals("Generator (1)"))
                        {
                           // PlayStreamingVideo(movPath)

                        }

                    }

                }



            }


        }
            

		#endif
	}
}
