/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <IconControl. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using UnityEngine.UI;

[RequireComponent(typeof(MajorAnnotate))]
[ExecuteInEditMode]
public class IconControl : MonoBehaviour
{
    public GameObject zoomMarker;
    public GameObject iconMarker;
    public Transform zoomPos;
    public Transform iconPos;
    public int excelNum;

    //public ExpandData expandData;
    //public ExpandIcon expandIcon;

    //public class ExpandData
    //{
    //    public string _name;
    //    public Transform _transform;

    //    public GameObject icon;
    //    public GameObject geometry;
    //    public GameObject window;
    //    public float width;
    //    public float height;
    //    public Vector3 leftCorner;
    //    GameObject cornerMarker;
    //    public GameObject lCornerPivot;
    //    public GameObject textGameObject;
    //    public Text text;
    //    public Canvas canvas;
    //    public GameObject canvasGameObject;
    //    public GameObject panelGameObject;
    //    public GameObject buttonGameObject;
    //    public Button button;

    //    public ExpandData(Transform transform, string name)
    //    {
    //        //Add Icon clone in that location 
    //        icon = Instantiate(Resources.Load<GameObject>("Location") as GameObject);

    //        //Clones parent location and name
    //        _transform = transform;
    //        //Debug.Log(name);
    //        _name = name;
    //        Debug.Log("Constructor" + _name);
    //        //excelNum = excelNumber;

    //        //Debug.Log(_name);

    //        icon.transform.position = _transform.position;
    //        icon.transform.Translate(0, 0, .5f);
    //        icon.name = _name;


    //        //Creates Window
    //        if (!window)
    //        {
    //            window = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //            window.gameObject.name = "Window";
    //            window.transform.parent = lCornerPivot.transform;
    //        }

    //        //Sets the Icon as parent
    //        window.transform.parent = lCornerPivot.transform;

    //        window.GetComponent<MeshRenderer>().material = Resources.Load<Material>("IconBlack") as Material;
    //        window.layer = 13;

    //        //Creates Left Corner Pivot
    //        lCornerPivot.transform.Translate(icon.transform.localPosition);

    //        //Resets the Left Corners local position
    //        lCornerPivot.transform.localPosition = new Vector3(0, 0, 0);

    //        ////Initializes it's local position and scale
    //        window.transform.localPosition = new Vector3(0, 0, 0);
    //        window.transform.localScale = new Vector3(4, 4, .01f);


    //        ////Initializes Variables
    //        width = window.transform.localScale.x;
    //        height = window.transform.localScale.y;

    //        //Initialize Text
    //        //Debug.Log("Name" + _name);


    //        //Adds Canvas
    //        canvasGameObject = new GameObject("Canvas");
    //        canvas = canvasGameObject.AddComponent<Canvas>();
    //        canvas.transform.parent = lCornerPivot.transform;
    //        canvas.renderMode = RenderMode.WorldSpace;
    //        canvas.worldCamera = Camera.main;
    //        CanvasScaler cs = canvasGameObject.AddComponent<CanvasScaler>();
    //        cs.scaleFactor = 10f;
    //        cs.dynamicPixelsPerUnit = 10f;
    //        GraphicRaycaster gr = canvasGameObject.AddComponent<GraphicRaycaster>();

    //        RectTransform rect = canvas.GetComponent<RectTransform>() as RectTransform;
    //        rect.localPosition = new Vector3(-2, 0, 2);
    //        rect.sizeDelta = new Vector2(4, 4);
    //        canvasGameObject.layer = 13;

    //        //Adds Panel
    //        panelGameObject = new GameObject("Panel");
    //        RectTransform panelRect = panelGameObject.AddComponent<RectTransform>();
    //        //panelGameObject.name = "Panel";
    //        panelGameObject.transform.parent = canvasGameObject.transform;
    //        panelRect.localPosition = new Vector3(0, 0, 0);
    //        panelRect.sizeDelta = new Vector3(4, 4);
    //        panelGameObject.layer = 13;

    //        //Adds Button
    //        GameObject buttonGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //        buttonGameObject.transform.parent = panelGameObject.transform;
    //        buttonGameObject.transform.localPosition = new Vector3(0, 0, 40);
    //        //Debug.Log(width);
    //        //Debug.Log(height);
    //        buttonGameObject.transform.localScale = new Vector3((width / 4), (height / 8), 1);
    //        float widthOfButton = (width / 4);
    //        float heightOfButton = (height / 8);
    //        buttonGameObject.transform.localPosition = new Vector3(-(width / 2) + widthOfButton - .25f, (height / 2) - heightOfButton, 2); //2 - the width of the button //2 - the height of the button
    //        buttonGameObject.layer = 13;
    //        buttonGameObject.tag = "SubComponent";

    //        //Add Button Text
    //        buttonGameObject.AddComponent<Text>();


    //        //Adds Text
    //        textGameObject = new GameObject("Text");
    //        text = textGameObject.AddComponent<Text>();
    //        textGameObject.transform.parent = panelGameObject.transform;
    //        textGameObject.transform.localPosition = new Vector3(0, 0, 60);
    //        text.rectTransform.localScale = new Vector3(.05f, .05f, .05f);
    //        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75);
    //        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
    //        text.rectTransform.localRotation = new Quaternion(0, 180, 0, 0);
    //        text.alignment = TextAnchor.MiddleCenter;
    //        text.horizontalOverflow = HorizontalWrapMode.Wrap;
    //        text.verticalOverflow = VerticalWrapMode.Truncate;
    //        Font ArialFont = (Font)Resources.Load("HELVETICANEUELTSTD-ROMAN") as Font;
    //        text.font = ArialFont;
    //        text.fontSize = 5;
    //        text.resizeTextForBestFit = true;
    //        text.resizeTextMaxSize = 5;
    //        text.resizeTextMinSize = 1;

    //        //text.text = "Information to be displayed for this component.";
    //        //if(excelNum == 0)
    //        //{
    //        //    excelNum = 6;
    //        //}
    //        //excelNum = Organized.Instance.systemDictionary["E4 (Trans)"].expandIconDictionary[_name];

    //        //text.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
    //        //text.text = 
    //        //text.text = getName(Organized.Instance.systemDictionary[]icon.GetComponent<IconControl>().excelNum);
    //        //add a script that extends majorannotate and add it to the major component gameobject that has iconControl script and getComponent<>().excel number

    //        text.enabled = true;
    //        text.color = Color.white;
    //        textGameObject.layer = 13;


    //        //Aligns Pivot to left corner
    //        window.transform.Translate(new Vector3(-width / 2, -height / 2, 0));

    //        icon.transform.localScale = new Vector3(.15f, .15f, .15f);

    //        //window.layer = 10;

    //        //icon.SetActive(false);
    //        //window.SetActive(false);
    //        //canvasGameObject.SetActive(false);
    //        //lCornerPivot.SetActive(false);
    //    }


    //    // Use this for initialization
    //    //void Start()
    //    //{
            
    //    //}

    //    // Update is called once per frame
    //    //void Update()
    //    //{
    //    //    //Updates Scale
    //    //    //For just Pivot
    //    //    lCornerPivot.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
    //    //    lCornerPivot.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);

    //    //    //For Icon
    //    //    geometry.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
    //    //    geometry.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);
    //    //}

    //    //void OnEnable()
    //    //{
    //    //    EventManager.StartListening("ExpandIcon", callData);
    //    //}

    //    //void OnDisable()
    //    //{
    //    //    EventManager.StopListening("ExpandIcon", callData);
    //    //}

    //    //public void callData()
    //    //{
    //    //    text.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
    //    //}

    //    public void expand()
    //    {
    //        Organized.Instance.toggle(window);
    //        Organized.Instance.toggle(canvasGameObject);
    //    }
    //}

    void Start()
    {
        //Debug.Log("IconControl " + dc);

        //zoomPos = gameObject.transform;
        //if(expandData == null)
        //{
        //    expandData = new ExpandData(gameObject.transform, gameObject.name);
        //}
        //if(!expandIcon)
        //{
        //    expandIcon = new ExpandIcon(gameObject.transform, gameObject.name);
        //}

        //if zoom marker does not exist from manual placement in inspector
        if (!zoomMarker)
        {
            zoomMarker = new GameObject("ZoomMarker");
            zoomMarker.transform.parent = gameObject.transform;
            zoomMarker.transform.localPosition = new Vector3(0, 0, 0);

            zoomPos = zoomMarker.transform;
        }

        if (!iconMarker)
        {
            iconMarker = new GameObject("IconMarker");
            iconMarker.transform.parent = gameObject.transform;
            iconMarker.transform.localPosition = new Vector3(0, 0, 0);

            iconPos = iconMarker.transform;

            //iconMarker.AddComponent<ExpandIcon>();
        }

        if (gameObject.tag == "MajorComponent")
        {
            excelNum = gameObject.GetComponent<MajorAnnotate>().excelNum;
        }
        if(gameObject.tag == "SubComponent")
        {
            excelNum = gameObject.GetComponent<SubAnnotate>().excelNum;
        }
        //Organized.Instance.systemDictionary[gameObject.transform.parent.parent.name].majorComponentDictionary[gameObject.transform.name].icon.text.text = getName(excelNum);

        //expandIcons.Add(gameObject.GetComponent<ExpandIcon>()._name, gameObject.GetComponent<ExpandIcon>());
    }

    void Update()
    {
        //if (expandData == null)
        //{
        //    expandData = new ExpandData(gameObject.transform, gameObject.name);
        //}

        if (!zoomMarker)
        {
            zoomMarker = new GameObject("ZoomMarker");
            zoomMarker.transform.parent = gameObject.transform;
            zoomMarker.transform.localPosition = new Vector3(0, 0, 0);

            zoomPos = zoomMarker.transform;
        }

        if (!iconMarker)
        {
            //Debug.Log("Does not exist.");
            iconMarker = new GameObject("IconMarker");
            iconMarker.transform.parent = gameObject.transform;
            iconMarker.transform.localPosition = new Vector3(0, 0, 0);

            iconPos = iconMarker.transform;
        }

        if (iconPos)
        {
            iconPos.position = iconMarker.transform.position;
        }

        if (!iconPos)
        {
            iconPos = iconMarker.transform;
        }

        if (zoomPos)
        {
            zoomPos.position = zoomMarker.transform.position;
        }

        if(!zoomPos)
        {
            zoomPos = zoomMarker.transform;
        }
    }

}
