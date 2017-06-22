using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class StatePatternDiagram : MonoBehaviour
{

    public GameObject fps;
    public GameObject fps2;
    public GameObject heavy;
    public GameObject light;
    public GameObject lightTrans;
    public GameObject path;
    public Camera cam;
    public Camera cam2;
    public Transform aerialPoint;
    //public GameObject stormAnimation;
    public List<GameObject> hideObjects = new List<GameObject>();
    public List<Camera> hideCams = new List<Camera>();

    public GameObject sunPanel;
    public GameObject windPanel;
    public GameObject soilPanel;
    public GameObject drainagePanel;

    private List<AppearWithProx> appears = new List<AppearWithProx>();

    [HideInInspector]
    public IDiagramState currentDiaState;
    [HideInInspector]
    public SunState sunState;
    [HideInInspector]
    public WindState windState;
    [HideInInspector]
    public SoilState soilState;
    [HideInInspector]
    public DrainageState drainageState;

    public float timer = 0f;

    // Use this for initialization
    void Awake()
    {
		//Old VR
        //fps = GameObject.Find("FPSController");
		fps = GameObject.Find("Player");
        fps2 = GameObject.Find("AerialFirstPersonController");

        heavy = GameObject.Find("VR_Heavy");
        light = GameObject.Find("VR_Light");
        lightTrans = GameObject.Find("VR_Light (Trans)");
        path = GameObject.Find("Paths");
        aerialPoint = GameObject.Find("AerialCamPivot").transform;
        //stormAnimation = GameObject.Find("StormAnimation");
        //stormAnimation.SetActive(false);

		//Old VR
        cam = fps.GetComponentInChildren<Camera>();
        cam2 = fps2.GetComponentInChildren<Camera>(); 

        sunPanel = GameObject.Find("Panel_Sun");
        windPanel = GameObject.Find("Panel_Wind");
        soilPanel = GameObject.Find("Panel_Soil");
        drainagePanel = GameObject.Find("Panel_Drainage");

        sunState = new SunState(this);
        windState = new WindState(this);
        soilState = new SoilState(this);
        drainageState = new DrainageState(this);
    }

    void Start()
    {
        currentDiaState = null;

        hideObjects.Add(fps);
        hideObjects.Add(fps2);
        hideObjects.Add(heavy);
        hideObjects.Add(light);
        hideObjects.Add(path);

        hideCams.Add(cam);
        hideCams.Add(cam2);

        light.SetActive(false);

        //Turns the skycamera off
        cam2.enabled = false;
        //Turns the skyfps off
        fps2.SetActive(false);

        RenderSettings.skybox = Organized.Instance.sunnySky;
    }

    void Update()
    {
        if (currentDiaState != null)
        {
            currentDiaState.UpdateState();
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("AerialView", activateAerialData);
        EventManager.StartListening("Sun", SetSunState);
        EventManager.StartListening("Soil", SetSoilState);
        EventManager.StartListening("Wind", SetWindState);
        EventManager.StartListening("MainState", SetNullState);
        EventManager.StartListening("SoilAnimation", SetSoilState);
        EventManager.StartListening("Return", ReturnOnClick);
        EventManager.StartListening("Drainage", SetDrainageState);
    }

    void OnDisable()
    {
        EventManager.StopListening("AerialView", activateAerialData);
        EventManager.StopListening("Sun", SetSunState);
        EventManager.StopListening("Soil", SetSoilState);
        EventManager.StopListening("Wind", SetWindState);
        EventManager.StopListening("MainState", SetNullState);
        EventManager.StopListening("SoilAnimation", SetSoilState);
        EventManager.StopListening("Return", ReturnOnClick);
        EventManager.StopListening("Drainage", SetDrainageState);
    }

    public void SetNullState()
    {
        currentDiaState = null;
    }

    public void SetSunState()
    {
        activateSunData();
        currentDiaState = sunState;
    }

    public void SetSoilState()
    {
        activateSoilData();
        currentDiaState = soilState;
    }

    public void SetWindState()
    {
        activateWindData();
        currentDiaState = windState;
    }

    public void SetDrainageState()
    {
        activateDrainageData();
        currentDiaState = drainageState;
    }

    public void activateAerialData()
    {
        toggleCam(hideCams);
        Organized.Instance.toggle(hideObjects);
        toggleDefualtSkybox();

        //Turns the ground camera off
        cam.enabled = false;
        //Turns the groundfps off
        fps.SetActive(false);

        //Turns the skyfps on
        fps2.SetActive(true);
        //Turns the skycam on
        cam2.enabled = true;
    }

    public void returnToGround()
    {
        toggleCam(hideCams);
        Organized.Instance.toggle(hideObjects);
        toggleDefualtSkybox();

        if (sunPanel.activeSelf == true)
        {
            sunPanel.SetActive(false);
        }
        if (windPanel.activeSelf == true)
        {
            windPanel.SetActive(false);
        }
    }

    public void ReturnOnClick()
    {
        //if (currentdiastate == soilstate)
        //{
        //    Debug.log("returned to mainstate");
        //    currentDiaState = ;
        //}

        //EventManager.TriggerEvent("MainState");
    }

    public void toggleDefualtSkybox()
    {
        if (RenderSettings.skybox == Organized.Instance.sunnySky)
        {
            RenderSettings.skybox = Organized.Instance.defaultSky;
        }
        else
        {
            RenderSettings.skybox = Organized.Instance.sunnySky;
        }
    }

    //Toggles objects in list
    public void toggleCam(List<Camera> hide)
    {
        for (int i = 0; i < hide.Count; i++)
        {
            if (hide[i].enabled == true)
            {
                //Debug.Log("active is true");
                hide[i].enabled = false;
            }
            else
            {
                //Debug.Log("active is false");
                hide[i].enabled = true;
            }
        }
    }

    public void BroadcastEvents(List<string> eventsName)
    {
        if (eventsName.Count != 0)
        {
            foreach (string e in eventsName)
            {
                Debug.Log("BroadCasted Event: " + e);
                //EventManager.TriggerEvent("EventName");
                EventManager.TriggerEvent(e);
            }
        }
    }

    public void activateSunData()
    {
        sunPanel.SetActive(true);
        EventManager.TriggerEvent("SunDiagramActive");
        Debug.Log("Sun State Activated.");
    }

    public void activateSoilData()
    {
        Organized.Instance.toggle(soilPanel);
        Organized.Instance.toggle(Organized.Instance.panelDictionary["Panel_Main"]);
        Debug.Log("Soil State Activated.");
    }

    public void activateSoilAnimationData()
    {
        //see major component state for build
        Debug.Log("SoilAnimation State Activated.");
    }

    public void activateWindData()
    {
        windPanel.SetActive(true);
        Debug.Log("Wind State Activated.");
    }

    public void activateDrainageData()
    {
        Organized.Instance.toggle(drainagePanel);
        //Organized.Instance.toggle(stormAnimation);
        //Organized.Instance.toggle(heavy);
        Organized.Instance.toggle(lightTrans);
        //foreach(BuildingSystem s in Organized.Instance.systemsList)
        //{
        //    if (!s._name.Equals("VR_Light (Trans)"))
        //    {
        //        s._root.gameObject.SetActive(false);
        //    }
        //}
        
        Debug.Log("Drainage State Activated.");
    }
}