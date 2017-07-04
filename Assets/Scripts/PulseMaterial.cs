/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <PulseMaterial. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

public class PulseMaterial : MonoBehaviour {

    //Variables//

    //Inspector
    public Transform triggerObj;
    public Camera cam;
    public Material originalMaterial;
    public Material pulseColor;
    public float glowRadius;

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


    // Use this for initialization
    void Start ()
    {
        //Gets GameObjects MeshRenderer
        pulseRenderer = GetComponent<MeshRenderer>();

        cam = Camera.main;

        if(triggerObj == null)
        {
            triggerObj = transform;
        }
        //Sets trigger
        //trigger = gameObject.GetComponent<Transform>();

        //Gets the objects original material
        //getOriginalMaterial();
    }
	
	// Update is called once per frame
	void Update ()
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
        if (Vector3.Distance(cam.transform.position, triggerObj.position) < glowRadius)
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
            if (Vector3.Distance(cam.transform.position, triggerObj.position) < glowRadius)
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
