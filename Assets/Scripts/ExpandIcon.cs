/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <ExpandIcon. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

//Create a majorComponentClass, that holds an option for an ExpandIcon
//subcomponent need a zoomTo transform position
//We can leave the highlight and annotate on highlight but disable the click on major component
//add a toSubcomponent button to the expand icon
//highlight associated MajorComponent on icon click
//add text to expand icon (zoom state)

public class ExpandIcon : MonoBehaviour {

    public string _name;
    public Transform _transform;
    public string _systemName;

    public int excelNum;
    public MajorAnnotate mj;
    public SubAnnotate sj;

    public GameObject icon;
    public GameObject geometry;
    public GameObject window;
    public float width;
    public float height;
    public Vector3 leftCorner;
    GameObject cornerMarker;
    public GameObject lCornerPivot;
    public GameObject textGameObject;
    public Text text;
    public Canvas canvas;
    public GameObject canvasGameObject;
    public GameObject panelGameObject;
    public GameObject buttonGameObject;
    public Button button;
    public GameObject buttonGameObjectChild;
    public GameObject buttonMovieGameObjectChild;
    public Text buttonGameObjectText;
    public Text buttonMovieGameObjectText;
	public GameObject imagePanelGameObject;
	public GameObject imageGameObject;

    public ExpandIcon(Transform transform, string name)
    {
        //Add Icon clone in that location 
        icon = Instantiate(Resources.Load<GameObject>("Location") as GameObject);

        //window = icon.GetComponent<ExpandIcon>().window;

        //Clones parent location and name
        _transform = transform;
        //Debug.Log(name);
        _name = name;

        Transform tParent = _transform.parent;
        if (tParent.tag == "BaseRoot")
        {
            _systemName = _transform.name;
        }
        else
        {
            while (tParent.parent.tag != "BaseRoot")
            {
                tParent = tParent.parent;
            }
            _systemName = tParent.name;
        }
        
        //Debug.Log("Constructor" + _name);
        //excelNum = excelNumber;

        //Debug.Log(_name);

        icon.transform.position = _transform.position;
        icon.transform.Translate(0, 0, .5f);
        icon.name = _name;
        icon.transform.parent = _transform;

        formatExpandIcon();
    }

    //public string getName()
    //{
    //    Debug.Log("getName" + _name);
    //    return _name;
    //}

    // Use this for initialization
    void formatExpandIcon()
    {
        //_name = name;
        //_transform = transform;
        //Debug.Log("Start" + _name);

        //getName();

        //if(!lCornerPivot)
        //{
        //    lCornerPivot = new GameObject();
        //    lCornerPivot.transform.parent = lCornerPivot.transform;
        //    lCornerPivot.gameObject.name = "LeftCornerPivot";
        //}

        //if(!geometry)
        //{
        //    geometry = new GameObject();
        //}

        lCornerPivot = icon.transform.FindChild("LeftCornerPivot").gameObject;
        geometry = icon.transform.FindChild("Geometry").gameObject;

        //Debug.Log(lCornerPivot);

        //Creates Window
        if (!window)
        {
            window = GameObject.CreatePrimitive(PrimitiveType.Cube);
            window.gameObject.name = "Window";
            window.transform.parent = lCornerPivot.transform;
            //Debug.Log("Window 1st " + window);
        }

        else
        {
            //Debug.Log("Window 2nd " + window);
            window = icon.transform.FindChild("Window").gameObject;
        }


        //Sets the Icon as parent
        window.transform.parent = lCornerPivot.transform;

        window.GetComponent<MeshRenderer>().material = Resources.Load<Material>("IconBlack") as Material;
        window.layer = 13;

        //Creates Left Corner Pivot
        lCornerPivot.transform.Translate(icon.transform.localPosition);

        //Resets the Left Corners local position
        lCornerPivot.transform.localPosition = new Vector3(0, 0, 0);

        ////Initializes it's local position and scale
        window.transform.localPosition = new Vector3(0, 0, 0);
        window.transform.localScale = new Vector3(4, 4, .01f);


        ////Initializes Variables
        width = window.transform.localScale.x;
        height = window.transform.localScale.y;

        //Initialize Text
        //Debug.Log("Name" + _name);


        //Adds Canvas
        canvasGameObject = new GameObject("Canvas");
        canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.transform.parent = lCornerPivot.transform;
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        CanvasScaler cs = canvasGameObject.AddComponent<CanvasScaler>();
        cs.scaleFactor = 10f;
        cs.dynamicPixelsPerUnit = 10f;
        GraphicRaycaster gr = canvasGameObject.AddComponent<GraphicRaycaster>();

        RectTransform rect = canvas.GetComponent<RectTransform>() as RectTransform;
        rect.localPosition = new Vector3(-width/2, 0, width/2);
        rect.sizeDelta = new Vector2(width/2, height/2);
        canvasGameObject.layer = 13;

        //Adds Panel
        panelGameObject = new GameObject("Panel");
        RectTransform panelRect = panelGameObject.AddComponent<RectTransform>();
        //panelGameObject.name = "Panel";
        panelGameObject.transform.parent = canvasGameObject.transform;
        panelRect.localPosition = new Vector3(0, 0, 0);
        panelRect.sizeDelta = new Vector3(width - (width/4), height - (height/4));
        panelGameObject.layer = 13;

        //Adds Button
        GameObject buttonGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buttonGameObject.name = "Button";
        buttonGameObject.transform.parent = panelGameObject.transform;
        buttonGameObject.transform.localPosition = new Vector3(0, 0, 10);
        buttonGameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("IconButton") as Material;
        //Debug.Log(width);
        //Debug.Log(height);
        buttonGameObject.transform.localScale = new Vector3((width / 4), (height / 8), 1);
        float widthOfButton = (width / 4);
        float heightOfButton = (height / 8);
        buttonGameObject.transform.localPosition = new Vector3(-(width / 2) + widthOfButton - .25f, (height / 2) - heightOfButton, 2); //2 - the width of the button //2 - the height of the button
        buttonGameObject.layer = 14;
        //buttonGameObject.tag = "SubComponent";

        //Add Button Text
        buttonGameObjectChild = new GameObject("Text");
        buttonGameObjectChild.layer = 15;
        buttonGameObjectChild.transform.parent = buttonGameObject.transform;
        buttonGameObjectChild.transform.localPosition = new Vector3(0,0,0);
        buttonGameObjectText = buttonGameObjectChild.AddComponent<Text>();
        buttonGameObjectText.rectTransform.localPosition = new Vector3(0,0,10);
        buttonGameObjectText.rectTransform.localScale = new Vector3(.10f, .10f, .10f);
        buttonGameObjectText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 9);
        buttonGameObjectText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 5);
        buttonGameObjectText.rectTransform.localRotation = new Quaternion(0, 180, 0, 0);
        buttonGameObjectText.alignment = TextAnchor.MiddleCenter;
        buttonGameObjectText.horizontalOverflow = HorizontalWrapMode.Wrap;
        buttonGameObjectText.verticalOverflow = VerticalWrapMode.Truncate;
        Font ArialFont = (Font)Resources.Load("HELVETICANEUELTSTD-ROMAN") as Font;
        buttonGameObjectText.font = ArialFont;
        buttonGameObjectText.fontSize = 8;
        buttonGameObjectText.resizeTextForBestFit = true;
        buttonGameObjectText.resizeTextMaxSize = 8;
        buttonGameObjectText.resizeTextMinSize = 1;

        buttonGameObjectText.text = "Components";



        //Adds Button
        GameObject buttonMovieGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buttonMovieGameObject.name = "Button";
        buttonMovieGameObject.transform.parent = panelGameObject.transform;
        buttonMovieGameObject.transform.localPosition = new Vector3(0, 0, 10);
        buttonMovieGameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("IconButton") as Material;
        //Debug.Log(width);
        //Debug.Log(height);
        buttonMovieGameObject.transform.localScale = new Vector3((width / 4), (height / 8), 1);
        float widthOfMovieButton = (width / 4);
        float heightOfMovieButton = (height / 8);
        buttonMovieGameObject.transform.localPosition = new Vector3((width / 2) - widthOfMovieButton + .25f, (height / 2) - heightOfMovieButton, 2); //2 - the width of the button //2 - the height of the button
        buttonMovieGameObject.layer = 14;
        //buttonGameObject.tag = "SubComponent";

        //Add Button Text
        buttonMovieGameObjectChild = new GameObject("Text");
        buttonMovieGameObjectChild.layer = 15;
        buttonMovieGameObjectChild.transform.parent = buttonMovieGameObject.transform;
        buttonMovieGameObjectChild.transform.localPosition = new Vector3(0, 0, 0);
        buttonMovieGameObjectText = buttonMovieGameObjectChild.AddComponent<Text>();
        buttonMovieGameObjectText.rectTransform.localPosition = new Vector3(0, 0, 10);
        buttonMovieGameObjectText.rectTransform.localScale = new Vector3(.10f, .10f, .10f);
        buttonMovieGameObjectText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 9);
        buttonMovieGameObjectText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 5);
        buttonMovieGameObjectText.rectTransform.localRotation = new Quaternion(0, 180, 0, 0);
        buttonMovieGameObjectText.alignment = TextAnchor.MiddleCenter;
        buttonMovieGameObjectText.horizontalOverflow = HorizontalWrapMode.Wrap;
        buttonMovieGameObjectText.verticalOverflow = VerticalWrapMode.Truncate;
        buttonMovieGameObjectText.font = ArialFont;
        buttonMovieGameObjectText.fontSize = 8;
        buttonMovieGameObjectText.resizeTextForBestFit = true;
        buttonMovieGameObjectText.resizeTextMaxSize = 8;
        buttonMovieGameObjectText.resizeTextMinSize = 1;

        buttonGameObjectText.text = "Components";


        //If placed here it just stretches the button with text now
        //int maxTextLenght = 5;
        //if (buttonGameObjectText.text.Length > maxTextLenght)
        //{
        //    int addToButtonSize = buttonGameObjectText.text.Length - 5;
        //    //Adjusts Button for text size
        //    buttonGameObject.transform.localScale = new Vector3((width / 4) + (addToButtonSize / (width)), (height / 8), 1);
        //}

        //Adds Image Panel
        imagePanelGameObject = new GameObject("Image Panel");
		RectTransform imagePanelRect = imagePanelGameObject.AddComponent<RectTransform>();
		imagePanelGameObject.transform.parent = canvasGameObject.transform;
		imagePanelRect.localPosition = new Vector3(0, 0, 0);
		imagePanelRect.sizeDelta = new Vector3();
		imagePanelGameObject.layer = 15;
		//Add Image
		imageGameObject = new GameObject("Image");
		RectTransform imageRect = imageGameObject.AddComponent<RectTransform>();
		imageGameObject.transform.parent = imagePanelGameObject.transform;
		imageRect.localPosition = new Vector3(0, 0, 0);


        //Adds Text
        textGameObject = new GameObject("Text");
        text = textGameObject.AddComponent<Text>();
        textGameObject.transform.parent = panelGameObject.transform;
        textGameObject.transform.localPosition = new Vector3(0, 0, 60);
        text.rectTransform.localScale = new Vector3(.05f, .05f, .05f);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
        text.rectTransform.localRotation = new Quaternion(0, 180, 0, 0);
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        //Font ArialFont = (Font)Resources.Load("HELVETICANEUELTSTD-ROMAN") as Font;
        text.font = ArialFont;
        text.fontSize = 5;
        text.resizeTextForBestFit = true;
        text.resizeTextMaxSize = 5;
        text.resizeTextMinSize = 1;

        text.text = "Information to be displayed for this component.";
        //if(excelNum == 0)
        //{
        //    excelNum = 6;
        //}
        //excelNum = Organized.Instance.systemDictionary["E4 (Trans)"].expandIconDictionary[_name];

        IconControl ic = _transform.parent.GetComponent<IconControl>();

        Debug.Log(_transform.parent.tag);

        if(_transform.parent.tag == "Mechanical")
        {
            excelNum = _transform.parent.GetComponent<MajorAnnotate>().excelNum;
            mj = _transform.parent.GetComponent<MajorAnnotate>();

            //text.text = "Mechanical Systems";
            text.text = mj.getData(2) + " " + mj.getData(1) + "\n" + mj.getData(3) + "\n" + "\n" + mj.getData(4); //Material + Name, Size, Description

			if (_transform.parent.name.Equals ("ERV")) 
			{
				buttonGameObjectText.text = "Diagram";
				buttonGameObject.tag = "Diagram";
                buttonMovieGameObjectText.text = "Movie";
                buttonMovieGameObject.tag = "Movie";
			} 
            else if (_transform.parent.name.Equals("Generator (1)"))
            {
                buttonGameObject.SetActive(false);
                buttonMovieGameObjectText.text = "Movie";
                buttonMovieGameObject.tag = "Movie";
            }
            else if (_transform.parent.name.Equals("AHU (2)"))
            {
                buttonMovieGameObject.SetActive(false);
                buttonGameObjectText.text = "Diagram";
                buttonGameObject.tag = "Diagram";
            }
			else
			{
				buttonGameObject.SetActive (false);
                buttonMovieGameObject.SetActive(false);
			}
        }

        if (_transform.parent.tag == "MajorComponent")
        {
            excelNum = _transform.parent.GetComponent<MajorAnnotate>().excelNum;
            mj = _transform.parent.GetComponent<MajorAnnotate>();

            text.text = mj.getData(2, excelNum) + " " + mj.getData(1, excelNum) + "\n" + "Size: " + mj.getData(3, excelNum) + "\n" + "\n" + mj.getData(4, excelNum);
            //Debug.Log("From ExpanIcon, SystemName: " + _systemName);
            //Debug.Log("From ExpanIcon, ExpandIconName: " + _name);
            //Debug.Log("From ExpanIcon, MajorComponentName: " + );
            //Debug.Log("From ExpanIcon, " + _systemName + " OrganizedSystemDictionary Count: " + Organized.Instance.systemDictionary.Count);
            //Debug.Log("From ExpanIcon, " + _systemName + " MajorComponentDictionaryCount: " + Organized.Instance.systemDictionary[_systemName].majorComponentDictionary.Count);// + " SubComponet Count: " + Organized.Instance.systemDictionary[_systemName].majorComponentDictionary[_name].subcomponents.Count);
            if (Organized.Instance.systemDictionary[_systemName].majorComponentDictionary[_name].subcomponents.Count != 0)
            {
                buttonGameObject.tag = "SubComponent";
            }
            else
            {
                buttonGameObject.SetActive(false);
            }
        }

        if (_transform.parent.tag == "SubComponent")
        {
            excelNum = _transform.parent.GetComponent<SubAnnotate>().excelNum;
            sj = _transform.parent.GetComponent<SubAnnotate>();
            text.text = sj.getData(3) + " " + sj.getData(2) + "\n" + "Size: " + sj.getData(4) + "\n" + "\n" + sj.getData(5);
        }
        //text.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);


        //Debug.Log(_transform.parent.name);
        //Debug.Log(_transform.parent.GetComponent<MajorAnnotate>().excelNum);
        //Debug.Log(dc);

        
        
        //excelNum = _transform.parent.GetComponent<MajorAnnotate>().excelNum;

        

        //Debug.Log(mj.globalMaterial);
        //Debug.Log("ExpandIcon " + excelNum);
        //Debug.Log("ExpandIcon " + ic.getData(2, excelNum));
        //Debug.Log(mj.getData(1, mj.excelNum));
        //mj.getData(2, excelNum);

        //Debug.Log("MajorDetails" + dc.majorDetails[0]);
        //Debug.Log(getData(2, excelNum));

        //text.text = getName(Organized.Instance.systemDictionary[]icon.GetComponent<IconControl>().excelNum);
        //add a script that extends majorannotate and add it to the major component gameobject that has iconControl script and getComponent<>().excel number

        text.enabled = true;
        text.color = Color.white;
        textGameObject.layer = 13;


        //Aligns Pivot to left corner
        window.transform.Translate(new Vector3(-width / 2, -height / 2, 0));

        //icon.transform.localScale = new Vector3(4f, 4f, 4f);
        icon.transform.localScale = new Vector3(.15f, .15f, .15f);

        //window.layer = 10;

        //icon.SetActive(false);
        //window.SetActive(false);
        //canvasGameObject.SetActive(false);
        lCornerPivot.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (icon)
    //    //{
    //            scaleExpandIcons();
    //    //}

    //    ////Updates Scale
    //    ////For just Pivot
    //    //lCornerPivot.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
    //    //lCornerPivot.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);

    //    ////For Icon
    //    //geometry.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
    //    //geometry.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);
    //}

    public void scaleExpandIcons()
    {
        //Updates Scale
        //For just Pivot
        lCornerPivot.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
        lCornerPivot.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);

        //For Icon
        geometry.transform.localScale = new Vector3((Vector3.Distance(Camera.main.transform.position, icon.transform.position)), .01f, (Vector3.Distance(Camera.main.transform.position, icon.transform.position)));
        geometry.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);
    }


    //void OnEnable()
    //{
    //    EventManager.StartListening("ExpandIcon", expand);
    //}

    //void OnDisable()
    //{
    //    EventManager.StopListening("ExpandIcon", expand);
    //}

    //public void callData()
    //{
    //    text.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
    //}

    public void expand()
    {
        //Debug.Log("lCornerPivot = " + lCornerPivot);

        Organized.Instance.toggle(lCornerPivot);
        //Organized.Instance.toggle(window);
        //Organized.Instance.toggle(canvasGameObject);
    }
}
