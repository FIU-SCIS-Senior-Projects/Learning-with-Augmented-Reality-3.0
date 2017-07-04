/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <Description. Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using UnityEngine.UI;
using System.Collections.Generic;

public class Description : MonoBehaviour {

    //Links the scroll view text to the databse description field

    

    // public GameObject scroll;
    // private Text text;
    // public Slider wind;
    // public Slider sunSeason;
    // public Slider sunTime;
    // public GameObject windPanel;
    // public GameObject sunPanel;

    //private string summerNine = "The building receives significant heat on the east and west sides " +
    //    "in the summer months. In hot and humid climates, summer morning sun can produce overheating " +
    //    "of the east façade. As a result, it is best practice to minimize the building dimension on "
    //    + "the east side and provide verticle shading on this facade.  The high summer sun does not impact " +
    //    "the north façade directly. The indirect light on the north side often provides a great source " +
    //    "of diffused natural lighting.";
    //private string summerNoon = "Sun angle durning Summer at 12:00pm.";
    //private string summerFive = "The building receives significant heat on the east and west directions " +
    //    "in summer months. In hot and humid climates, the summer afternoon sun can produce overheating " +
    //    "of the west façade. As a result, it is best practice to minimize the building dimensions on " +
    //    "the west side and provide verticle shading on this facade. The high summer sun does not impact the " +
    //    "north façade directly. The indirect light provides a great source of diffused natural light for " +
    //    "illumination of the interior façade on the north side.";

    //private string fallNine = "Sun angle during Fall at 9:00am.";
    //private string fallNoon = "Sun angle during Fall at 12:00pm";
    //private string fallFive = "Sun angle during Fall at 5:00pm";

    //private string winterNine = "In hot and humid climates, the low sun angles of the winter mornings impact " +
    //    "the southern side penetrating deep into the building. Using horizontal overhangs will reduce direct " +
    //    "heat gain and reduce glare inside the building.";
    //private string winterNoon = "Sun angle during Winter at 12:00pm";
    //private string winterFive = "In hot and humid climates, afternoon sun angles can overheat the west side of " +
    //    "the building. Using shading devices such as horizontal lovers or overhangs can block the sun rays from entering " +
    //    "the building thus reducing the interior temperature.";

    //private string springNine = "Sun angle during Spring at 9:00am";
    //private string springNoon = "Sun angle durning Spring at 12:00pm";
    //private string springFive = "Sun angle during Spring at 5:00pm";
    //public Toggle prevailingToggle;

    private string startingText;
    [HideInInspector]
    public string currentSubPanel;

    
    //Dictionary to store titles and descriptions of each sub panel
    private Dictionary<string, string> descriptDict = new Dictionary<string, string>();

    //The instance of the description for the singleton pattern
    private static Description instance;

    /* 
        Singleton pattern to ensure only one instance of the Description object exist.
    */
    public static Description Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Description>();

                if (instance == null)
                {
                    GameObject go = new GameObject("Description");
                    instance = go.AddComponent<Description>();
                    Debug.Log("Singleton created!");
                }
            }
            return instance;
        }
    }

    /*
        collectSubPanels parses a text file which has lines which are formatted as follows:
        
        "panelname=title:description"

        For each line in the text file we will add a new entry into the dictionary with
        it's title and description.

     */
    private void collectSubPanels(){foreach(string line in Resources.Load<TextAsset>("titleDescription").text.Split('\n')){descriptDict.Add(line.Split('=')[0], line.Split('=')[1]);}}

    /*
        setDescript(string currentSubP) sets the title and discription of the Title_Description panel
        based on which subpanel was clicked by the user.
    */
    public void setDescript(string currentSubP)
    {
        //Set the currentSubPanel
        currentSubPanel = currentSubP;
        //Debug.Log(currentSubPanel);

        //Create an array to store the title and description which is parsed from the dictionary value
        //of the current subpanel  
        string[] str = descriptDict[currentSubPanel].Split(':');

        //For every Child object of Title_Description that is of type Text
        foreach (Text textObj in Organized.Instance.panelDictionary["Title_Description"].GetComponentsInChildren<Text>())
        { 
            if (textObj.name.Equals("Description"))
                textObj.text = str[1]; //Set the text for the description          
            else
                textObj.text = str[0]; //Set the text for the title
        } 
              
     }

    //Use this for initialization
    void Start() { collectSubPanels(); }
	
	// Update is called once per frame
	void Update() 
    {
	 //if(windPanel.activeSelf)
     //   {
     //       if(wind.value == 0)
     //       {
     //           text.text = "Wind is coming from the South.";
     //       }
     //       if (wind.value == 1)
     //       {
     //           text.text = "Wind is coming from the West.";
     //       }
     //       if (wind.value == 2)
     //       {
     //           text.text = "Wind is coming from the North.";
     //       }
     //       if (wind.value == 3)
     //       {
     //           text.text = "Wind is coming from the East.";
     //       }
     //       if(wind.value == 4)
     //       {
     //           text.text = "The prevailing winds in Miami, Florida, generally come from the Southeast.";
     //       }
     //   }

     //   if(sunPanel.activeSelf)
     //   {
     //       if(sunSeason.value == 0 && sunTime.value == 0)
     //       {
     //           text.text = summerNine;
     //       }
     //       if (sunSeason.value == 0 && sunTime.value == 1)
     //       {
     //           text.text = summerNoon;
     //       }
     //       if (sunSeason.value == 0 && sunTime.value == 2)
     //       {
     //           text.text = summerFive;
     //       }


     //       if (sunSeason.value == 1 && sunTime.value == 0)
     //       {
     //           text.text = fallNine;
     //       }
     //       if (sunSeason.value == 1 && sunTime.value == 1)
     //       {
     //           text.text = fallNoon;
     //       }
     //       if (sunSeason.value == 1 && sunTime.value == 2)
     //       {
     //           text.text = fallFive;
     //       }


     //       if (sunSeason.value == 2 && sunTime.value == 0)
     //       {
     //           text.text = winterNine;
     //       }
     //       if (sunSeason.value == 2 && sunTime.value == 1)
     //       {
     //           text.text = winterNoon;
     //       }
     //       if (sunSeason.value == 2 && sunTime.value == 2)
     //       {
     //           text.text = winterFive;
     //       }


     //       if (sunSeason.value == 3 && sunTime.value == 0)
     //       {
     //           text.text = springNine;
     //       }
     //       if (sunSeason.value == 3 && sunTime.value == 1)
     //       {
     //           text.text = springNoon;
     //       }
     //       if (sunSeason.value == 3 && sunTime.value == 2)
     //       {
     //           text.text = springFive;
     //       }
     //   }

     //   if(!windPanel.activeSelf && !sunPanel.activeSelf)
     //   {
     //       text.text = startingText;
     //   }
	}
}
