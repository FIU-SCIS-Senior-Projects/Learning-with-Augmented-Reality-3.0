using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SunRadiation : MonoBehaviour {

    //Inspector
    public Transform triggerObj;
    //public Camera cam;
    public Material originalMaterial;
    public Material pulseColor;
    public float glowRadius;
    public GameObject sunPanel;
    public Slider seasonSlider;
    public Slider timeSlider;
    
    //public Rigidbody player;

    //Hidden variables
    private float duration = 1f;
    private float lerp;
    private MeshRenderer pulseRenderer;

    //For Pulse by raycast
    [HideInInspector]
    public float maxDistance = 10000;
    private Vector3 camDirection;

    //Multi to be developed
    private List<Transform> pulseObjects = new List<Transform>();
    private List<Material> originalMaterials = new List<Material>();
    private List<Material> pulseMaterials = new List<Material>();
    private List<MeshRenderer> pulseRenderers = new List<MeshRenderer>();

    private GameObject player;

    // Use this for initialization
    void Start()
    {
        //Gets GameObjects MeshRenderer
        pulseRenderer = GetComponent<MeshRenderer>();

        player = GameObject.FindGameObjectWithTag("MainCamera");

        //pulseColor = (Material)Resources.Load("CPulse(Detail) 1");

        //Sets trigger
        //trigger = gameObject.GetComponent<Transform>();

        //Gets the objects original material
        //getOriginalMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //Update values    
        lerp = Mathf.PingPong(Time.time, duration);

        singlePulse();
        //multiPulse();
    }

    public void getOriginalMaterial()
    {
        originalMaterial = pulseRenderer.material;
    }

    public void singlePulse()
    {
        if (sunPanel.activeSelf)
        {
            pulseRenderer.material.Lerp(originalMaterial, pulseColor, lerp);
            //Debug.Log(trigger.transform.position);
        }
        else
        {
            pulseRenderer.material = originalMaterial;
            //hide.material = originalColor; 
        }
    }





    //Still Being Developed//

    //Multi
    public void getPulseObjects()
    {
        for (int i = 0; i < pulseObjects.Count; i++)
        {
            pulseRenderers.Add(pulseObjects[i].GetComponent<MeshRenderer>());
        }
    }

    //Multi
    public void getOriginalMaterials()
    {
        //MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();

        //Debug.Log(renderers.Length);

        for (int i = 0; i < pulseRenderers.Count; i++)
        {
            originalMaterials.Add(pulseRenderers[i].material);
        }
    }

    //Multi
    public void multiPulse()
    {
        for (int i = 0; i < pulseRenderers.Count; i++)
        {
            //singlePulse(pulseRenderers[i]);
            if (Vector3.Distance(player.transform.position, triggerObj.position) < glowRadius)
            {
                {
                    //pulseRenderers[i].material.Lerp(originalMaterials[i], pulseColor, lerp);
                    lerpMulti(originalMaterials);
                }
                //Debug.Log(trigger.transform.position);
            }
            else
            {
                pulseRenderers[i].material = originalMaterials[i];
                //hide.material = originalColor;
            }
        }
    }

    public void lerpMulti(List<Material> ogM)
    {
        for (int i = 0; i < pulseRenderers.Count; i++)
        {
            pulseRenderers[i].material.Lerp(ogM[i], pulseColor, lerp);
        }
    }
}
