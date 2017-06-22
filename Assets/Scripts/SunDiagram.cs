using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class SunDiagram : MonoBehaviour
{
    //private UnityAction someListener;

    //create a empty gameobject for each of the variables

    public Transform summerMorn;
    public Transform summerNoon;
    public Transform summerAfter;
    public Transform fallMorn;
    public Transform fallNoon;
    public Transform fallAfter;
    public Transform winterMorn;
    public Transform winterNoon;
    public Transform winterAfter;
    public Transform springMorn;
    public Transform springNoon;
    public Transform springAfter;

    public GameObject louversVerticle;
    public GameObject louversHorizontal;

    public GameObject Pngs;
    public List<GameObject> pngs = new List<GameObject>();

    //public List<Material> exteriors = new List<Material>();
    
    /*
    public List<GameObject> summerExt = new List<GameObject>();
    public List<GameObject> fallExt = new List<GameObject>();
    public List<GameObject> winterExt = new List<GameObject>();
    public List<GameObject> springExt = new List<GameObject>();
    */

    public GameObject sunRays;
    
    public GameObject summerSolstice;
    public GameObject winterSolstice;
    public GameObject springEquinox;
    public GameObject fallEquinox;
    

    public GameObject sunPanel;
    //public Toggle existing;
    public Toggle modified;

    //public TextMesh tSummerMorn;
    //add the rest here

    public Transform buildingCenter;

    public int sliderSeason = 0; //start summer
    public int sliderTime = 1; //start morning

    public Slider seaSlider;
    public Slider timSlider;

    private GameObject[] tempPNG; //temporary container

    private List<GameObject> arcs = new List<GameObject>();

    
    // Use this for initialization
    void Start () {

        //INITIALIZERS//
        //existing.isOn = true;
        modified.isOn = false;

        //sunPanel = GameObject.Find("Panel_Sun");
        //modified = sunPanel.GetComponentInChildren<Toggle>(modified);
        

        //Sliders
        seaSlider.wholeNumbers = true;
        timSlider.wholeNumbers = true;
        seaSlider.onValueChanged.AddListener(delegate { onValueChange(); });
        timSlider.onValueChanged.AddListener(delegate { onValueChange(); });
        sliderSeason = 1;
        sliderTime = 2;
        gameObject.transform.position = fallNoon.position;
        //Debug.Log(sliderSeason + " " + sliderTime);

        //ARCS//
        //adds arcs to the arc list
        arcs.Add(summerSolstice);
        arcs.Add(winterSolstice);
        arcs.Add(fallEquinox);
        arcs.Add(springEquinox);
        
        //PNGS//
        //initializes an array from png layer parent
        tempPNG = new GameObject[Pngs.transform.childCount];
        //Debug.Log("pngs count: " + Pngs.transform.childCount);
        //gets each png store in the parent layer gameobject
        for (int i = 0; i < Pngs.transform.childCount; i++)
        {
            tempPNG[i] = Pngs.transform.GetChild(i).gameObject;
            //Debug.Log(Pngs.transform.GetChild(i).gameObject);
        }
        //adds each png to the list
        for(int i=0; i< tempPNG.Length; i++)
        {
            pngs.Add(tempPNG[i]);
            //Debug.Log(pngs[i]);
        }

        //deactivates all world sun data at start
        deactivateUnusedPng("");
        deactivateUnusedArc("");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //seaSlider.onValueChanged.AddListener(delegate { sunTime(timSlider.value); });
        //timSlider.onValueChanged.AddListener(delegate { sunSeason(seaSlider.value); });

        //Debug.Log(seaSlider.value + " " + timSlider.value);

        //Updates the suns position
        //sunPos();

        //Keeps the sun aiming at building
        gameObject.transform.LookAt(buildingCenter);
       
        

        //Sun Data local Trigger
        if (sunPanel.activeSelf)
        {
            EventManager.TriggerEvent("SunActive");
            
            //Updates values from time and season sliders
            sunTime(timSlider.value);
            sunSeason(seaSlider.value);

            //if modified is on, activate louvres
            if (modified.isOn)
            {
                louversHorizontal.SetActive(true);
                louversVerticle.SetActive(true);
            }
            else
            {
                louversHorizontal.SetActive(false);
                louversVerticle.SetActive(false);
            }

            /*
            //Sucessfull Test For Seasons
            if (sliderSeason == 0)
            {
                EventManager.TriggerEvent("Summer");
            }
           */
        }
        else
        {
            louversHorizontal.SetActive(false);
            louversVerticle.SetActive(false);
        }        

        //to do:
        //set event for modified
        //set event for existing
        //set event for hot climate
        //set event for cold climate
    }

    //MUTATORS//

    void OnEnable()
    {
        //EventManager.DiagramButtonPressed += activateData;
        EventManager.StartListening("SunDiagramActive", activateData);
        EventManager.StartListening("SunActive", sunPos);
        //EventManager.StartListening("SecondaryInterfaceMode", deactivateData);
        //EventManager.StartListening("Summer", summerState);
    }

    void OnDisable()
    {
        //EventManager.DiagramButtonPressed -= activateData;
        EventManager.StopListening("SunDiagramActive", activateData);
        EventManager.StopListening("SunActive", sunPos);
        //EventManager.StopListening("SecondaryInterfaceMode", deactivateData);
        //EventManager.StopListening("Summer", summerState);
    }

    //Activates sun world data if diagrambuttonpressed
    public void activateData()
    {
        Debug.Log("SunDiagramActive was clicked.");
        sunRays.SetActive(true);

        //makes sure panel is active first
        if(sunPanel.activeSelf == false)
        {
            sunPanel.SetActive(true);
        }
        //activates current arc
        StartCoroutine(updateArcs());
    }

    public void deactivateData()
    {
        //Debug.Log("Event was clicked.");
        sunRays.SetActive(false);
        deactivateUnusedArc("");
        deactivateUnusedPng("");

    }

    public void onValueChange()
    {
        //Debug.Log(sliderTime + " " + sliderSeason);
    }

    //Gets the time value and converts it to an int
    public void sunTime(float sliderT)
    {
        sliderTime = (int)sliderT;
    }

    public void sunSeason(float sliderS)
    {
        sliderSeason = (int)sliderS;
    }

    IEnumerator updateArcs()
    {
        while (sunPanel.activeSelf)
        {
            if (sliderSeason == 0)
            {
                deactivateUnusedArc(summerSolstice.name);
            }

            if (sliderSeason == 1)
            {
                deactivateUnusedArc(fallEquinox.name);
            }

            if (sliderSeason == 2)
            {
                deactivateUnusedArc(winterSolstice.name);
            }

            if (sliderSeason == 3)
            {
                deactivateUnusedArc(springEquinox.name);
            }

            yield return null;
        }

        //print("Sun panel closed.");

        //yield return new WaitForSeconds(3f);

        //print("MyCoroutine is now finished.");
    }

    /*
    IEnumerator updatePng()
    {
        while (sunPanel.activeSelf)
        {
            if (sliderSeason == 0)
            {
                deactivateUnusedArc(summerSolstice.name);
            }

            if (sliderSeason == 1)
            {
                deactivateUnusedArc(fallEquinox.name);
            }

            if (sliderSeason == 2)
            {
                deactivateUnusedArc(winterSolstice.name);
            }

            if (sliderSeason == 3)
            {
                deactivateUnusedArc(springEquinox.name);
            }

            yield return null;
        }

        //print("Sun panel closed.");

        //yield return new WaitForSeconds(3f);

        //print("MyCoroutine is now finished.");
    }
    */


    public void sunPos()
    {
        //if Season slider equals 1 and time slider equals 1
        //then sun moves to summer solitice morning tranform #,#,#
        //Debug.Log("Sunpos is updating");

        //Summer
        if (sliderSeason == 0 && sliderTime == 0)
        {
            deactivateUnusedPng(pngs[0].name);
            //gameObject.transform.position = summerMorn.position;
            //tSummerMorn.gameObject.SetActive(true);
            //add this to all of them

            //gameObject.transform.rotation = summerMorn.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, summerMorn.position, Time.deltaTime);
        }
        if (sliderSeason == 0 && sliderTime == 1)
        {
            deactivateUnusedPng(pngs[1].name);
            //gameObject.transform.position = summerNoon.position;
            //tSummerNoon.gameObject.SetActive(true);
            //gameObject.transform.rotation = summerNoon.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, summerNoon.position, Time.deltaTime);
        }
        if (sliderSeason == 0 && sliderTime == 2)
        {
            deactivateUnusedPng(pngs[2].name);
            //gameObject.transform.position = summerAfter.position;
            //tSummerAfter.gameObject.SetActive(true);
            //gameObject.transform.rotation = summerAfter.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, summerAfter.position, Time.deltaTime);
        }
        //Fall
        if (sliderSeason == 1 && sliderTime == 0)
        {
            deactivateUnusedPng(pngs[3].name);
            //gameObject.transform.position = fallMorn.position;
            //gameObject.transform.rotation = fallMorn.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, fallMorn.position, Time.deltaTime);
        }
        if (sliderSeason == 1 && sliderTime == 1)
        {
            deactivateUnusedPng(pngs[4].name);
            //gameObject.transform.position = fallNoon.position;
            //gameObject.transform.rotation = fallNoon.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, fallNoon.position, Time.deltaTime);
        }
        if (sliderSeason == 1 && sliderTime == 2)
        {
            deactivateUnusedPng(pngs[5].name);
            //gameObject.transform.position = fallAfter.position;
            //gameObject.transform.rotation = fallAfter.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, fallAfter.position, Time.deltaTime);
        }
        //Winter
        if (sliderSeason == 2 && sliderTime == 0)
        {
            deactivateUnusedPng(pngs[6].name);
            //gameObject.transform.position = winterMorn.position;
            //gameObject.transform.rotation = winterMorn.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, winterMorn.position, Time.deltaTime);
        }
        if (sliderSeason == 2 && sliderTime == 1)
        {
            deactivateUnusedPng(pngs[7].name);
            //gameObject.transform.position = winterNoon.position;
            //gameObject.transform.rotation = winterNoon.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, winterNoon.position, Time.deltaTime);
        }
        if (sliderSeason == 2 && sliderTime == 2)
        {
            deactivateUnusedPng(pngs[8].name);
            //gameObject.transform.position = winterAfter.position;
            //gameObject.transform.rotation = winterAfter.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, winterAfter.position, Time.deltaTime);
        }
        //Spring
        if (sliderSeason == 3 && sliderTime == 0)
        {
            deactivateUnusedPng(pngs[9].name);
            //gameObject.transform.position = springMorn.position;
            //gameObject.transform.rotation = springMorn.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, springMorn.position, Time.deltaTime);
        }
        if (sliderSeason == 3 && sliderTime == 1)
        {
            deactivateUnusedPng(pngs[10].name);
            //gameObject.transform.position = springNoon.position;
            //gameObject.transform.rotation = springNoon.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, springNoon.position, Time.deltaTime);
        }
        if (sliderSeason == 3 && sliderTime == 2)
        {
            deactivateUnusedPng(pngs[11].name);
            //gameObject.transform.position = springAfter.position;
            //gameObject.transform.rotation = springAfter.rotation;
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, springAfter.position, Time.deltaTime);
        }

        if(sunPanel.activeSelf == false)
        {
            //gameObject.transform.position = fallNoon.position;

            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, fallNoon.position, Time.deltaTime);
        }
    }

    public void deactivateUnusedPng(string currentDir)
    {
        for (int i = 0; i < pngs.Count; i++)
        {
            //Debug.Log(currentDir);
            if (pngs[i].name.Equals(currentDir) == false)
            {
                pngs[i].SetActive(false);
            }
            else
            {
                pngs[i].SetActive(true);
            }
        }
    }

    public void deactivateUnusedArc(string currentDir)
    {
        for (int i = 0; i < arcs.Count; i++)
        {
            //Debug.Log(currentDir);
            if (arcs[i].name.Equals(currentDir) == false)
            {
                arcs[i].SetActive(false);
            }
            else
            {
                arcs[i].SetActive(true);
            }
        }
    }

    public int getTime()
    {
        return sliderTime;
    }

    public int getSeason()
    {
        return sliderSeason;
    }

    public void summerState()
    {
        Debug.Log("Its Summer!");
    }
}
