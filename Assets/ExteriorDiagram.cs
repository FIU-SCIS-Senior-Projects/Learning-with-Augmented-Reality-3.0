using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExteriorDiagram : MonoBehaviour {

    GameObject canvasGameObject;
    Canvas canvas;

    void Start()
    {
        canvasGameObject = gameObject.transform.Find("Canvas_Diagram").gameObject;
        canvas = canvasGameObject.GetComponent<Canvas>();

        //Debug.Log(canvas);
        canvasGameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        
	}

    void togglePanel()
    {
        Organized.Instance.toggle(canvasGameObject);
    }

    void openPanel()
    {
        canvasGameObject.SetActive(true);
    }

    void closePanel()
    {
        canvasGameObject.SetActive(false);
    }

    void OnEnable()
    {
        if(gameObject.name.Equals("RValue"))
            EventManager.StartListening("RValue", togglePanel);

        if (gameObject.name.Equals("EmbodiedEnergy"))
            EventManager.StartListening("EE", togglePanel);

        if (gameObject.name.Equals("ShadingCo"))
            EventManager.StartListening("ShadingCo", togglePanel);
    }

    void OnDisable()
    {

    }
    
}
