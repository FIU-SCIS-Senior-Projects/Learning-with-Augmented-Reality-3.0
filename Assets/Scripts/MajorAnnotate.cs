using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Annotates Major Components
//Put this script on each model to be annotated.
//Assign each model a value.

//[RequireComponent(typeof(LineRenderer))]
//[RequireComponent(typeof(MeshCollider))]
public class MajorAnnotate : MonoBehaviour
{
    //Global Class Variables
    private static int NUM_OF_FIELDS = 7;
    private static int FONT_SIZE = 80;

    ///Local Instance Variables///

    //For Annotation TextMesh//
    private bool created = false;
    private bool createdExpandIcon = false;

    //For Lines//
    private LineRenderer line;
    private Vector3[] points = new Vector3[2];
    private Material lineMaterial;
    private Texture tex;
    private Color pColorA = Color.cyan;
    private Color pColorB = Color.clear;
    private List<LineRenderer> lines = new List<LineRenderer>();
    private List<Vector3> oldPos = new List<Vector3>();
    private List<Vector3> newPos = new List<Vector3>();
    private bool activate = false;
    private bool moved = false;
    private float width = .03f;

    //Local Instance In Game Object Variables
    private static GameObject annotatePanel;
    private static GameObject majorTextArea;
    private static Text majorTextAreaText;

    private static Text majorComponentText;

    //Not sure yet
    //private List<Collider> colliders = new List<Collider>();
    private bool first = true;
    public Collider currentCollider;
    public Collider previousCollider;
    //private List<Collider> subColliders = new List<Collider>();
    //private List<Transform> disableColliders = new List<Transform>();

    //For Text
    private TextMesh text;
    private Text textUI;
    public int excelNum;
    private List<TextMesh> texts = new List<TextMesh>();
    private Rigidbody r;
    //private List<Transform> models = new List<Transform>();

    //For ExpandIcon
    public string exName;
    public Transform exTransform;

    public string globalName;
    public string globalMaterial;

    public GameObject icon;
    public GameObject geometry;
    public GameObject window;
    public float exWidth;
    public float exHeight;
    public Vector3 leftCorner;
    GameObject cornerMarker;
    public GameObject lCornerPivot;
    public GameObject textGameObject;
    public Text exText;
    public Canvas canvas;
    public GameObject canvasGameObject;
    public GameObject panelGameObject;
    public GameObject buttonGameObject;
    public Button button;

    public List<ExpandIcon> expandIcons = new List<ExpandIcon>();
    public ExpandIcon expandIcon;

    //SubComponent subComp;

    private GameObject player;

    //private OrganizedDetail od;

    // Use this for initialization
    void Start()
    {
        if (excelNum == 0)
        {
            //Debug.Log("Fix ExcelNumber: " + gameObject.name);
            excelNum = 3;
        }

        //Initializes Text Mesh Instance to be cloned
        text = GameObject.Find("Annotation").GetComponent<TextMesh>();
        //expandIcon = GameObject.Find("Location").GetComponent<ExpandIcon>();
        //if(gameObject.GetComponent<IconControl>())
        //{
        //    expandIcon = new ExpandIcon(gameObject.GetComponent<IconControl>().iconPos, gameObject.name);

        //    //if (createdExpandIcon == false)
        //    //{
        //    //    formatExpandIcon();
        //    //}
        //}
        majorTextArea = Organized.Instance.panelDictionary["Panel_Annotation"].transform.Find("MajorTextArea").gameObject;
        majorTextAreaText = majorTextArea.GetComponentInChildren<Text>();



        //Initializes Text material
        tex = (Texture)Resources.Load("White Aluminum");

        //Initializes Line material
        lineMaterial = (Material)Resources.Load("Lines");

        //Gets the player
        player = GameObject.FindGameObjectWithTag("User"); //might have to switch this to how the ExpandIcons Rotate later
    }

    // Update is called once per frame
    void Update()
    {
        //Annotation Type//
        //Debug.Log(currentCollider);

        //ray();
        rayUI();


        //Draw Lines//

        //updates the lines pt2 to follow the positions of the texts
        //for (int i = 0; i < lines.Count; i++)
        //{
        //Debug.Log(lines.Count);
        //lines[i].SetPositions(addLines(texts[i], i));
        //}

        if (icon)
        {
            scaleExpandIcons();
        }

        //Rotate Annotation with Player//
        followData();
    }

    public string rayUI()
    {
        //RaycastHit hit;
        RaycastHit hit = new RaycastHit();

        //if raycast hit
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8) | (1 << 16)))
        {
            //Debug.Log(hit.collider.tag);

            if (hit.collider.tag.Equals("MajorComponent") || hit.collider.tag.Equals("Exterior") || hit.collider.tag.Equals("Clickable"))
            {
                if (hit.collider == gameObject.GetComponent<MeshCollider>() || hit.collider == gameObject.GetComponent<Collider>() || hit.collider == gameObject.GetComponent<BoxCollider>())
                {
                    //Debug.Log("Detail Hit!");
                    //Debug.Log(hit.collider);
                    //currentCollider = hit.collider;
                    //Debug.Log(currentCollider);
                    //Debug.Log(Organized.Instance.globalCurrentState.ToString());
                    if (hit.collider.tag.Equals("MajorComponent"))
                    {
                        if (created == false)
                        {
                            formatTextMesh();
                        }

                        else
                        {
                            //Debug.Log("Not First Click");
                            //activateExplode();
                            //drawLines();
                            for (int i = 0; i < texts.Count; i++)
                            {
                                texts[i].transform.gameObject.SetActive(true);
                            }
                            //for (int i = 0; i < expandIcons.Count; i++)
                            //{
                            //    expandIcons[i].expand();
                            //}
                        }
                    }
                    if (Organized.Instance.globalCurrentState.ToString().Equals("MainState"))
                    {
                        if (hit.collider.tag.Equals("Exterior"))// || hit.collider.tag.Equals("Clickable"))
                        {
                            //Debug.Log ("Exterior Hit");
                            //formatExteriorTextUI ();

                            if (created == false)
                            {
                                //currentCollider = hit.collider;
                                formatExteriorTextMesh();
                            }

                            else
                            {
                                //Debug.Log("Not First Click");
                                //activateExplode();
                                //drawLines();
                                for (int i = 0; i < texts.Count; i++)
                                {
                                    texts[i].transform.gameObject.SetActive(true);
                                }
                                //for (int i = 0; i < expandIcons.Count; i++)
                                //{
                                //    expandIcons[i].expand();
                                //}
                            }
                            //if (Organized.Instance.panelDictionary["Panel_Diagram"].activeSelf)
                            //{
                            //    Organized.Instance.panelDictionary["Panel_Diagram"].SetActive(true);
                            //}
                        }
                    }

                    return "activate2";
                }

                else
                {
                    //currentCollider = null;

                    for (int i = 0; i < texts.Count; i++)
                    {
                        texts[i].transform.gameObject.SetActive(false);
                    }

                    //Organized.Instance.panelDictionary["Panel_Diagram"].SetActive(false);
                }

                //if (previousCollider != null)
                //{
                //if (previousCollider != hit.collider)
                //{
                //for (int i = 0; i < texts.Count; i++)
                //{
                //texts[i].transform.gameObject.SetActive(false);
                //}
                //}
                //}
                //previousCollider = currentCollider;                
            }
        }
        else
        {
            //currentCollider = null;

            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].transform.gameObject.SetActive(false);
            }

            //Organized.Instance.panelDictionary["Panel_Diagram"].SetActive(false);
        }

        return "";
    }

    //For drawing annotation with lines
    public string ray()
    {
        //if (Input.GetMouseButtonDown(0))
        {
            //RaycastHit hit;
            RaycastHit hit = new RaycastHit();

            //if raycast hit
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider == gameObject.GetComponent<MeshCollider>() || hit.collider == gameObject.GetComponent<Collider>() || hit.collider == gameObject.GetComponent<BoxCollider>())
                {
                    //THIS SUBANNOTATE SCRIPT HAS TO BE ON EVERY SINGLE INDIVIDUAL SUBCOMPONET THAT WILL BE CLICKED
                    //NO IT DOSNT IF THEY ARE ALL THE SAME
                    //BUT THE FIRST OPTION IS FAR EASIER

                    //Debug.Log("Detail Hit!");
                    /*
                    currentCollider = hit.collider;
                        if(subColliders.Contains(currentCollider) == false)
                        {
                            subColliders.Add(currentCollider);
                            SubComponent subComp = new SubComponent(currentCollider.name, currentCollider.transform.position, true, currentCollider);
                            formatTextMesh(subComp);
                            getPositions();
                            drawLines();
                            activateExplode();
                        }
                        */

                    

                    if (created == false)
                    {
                        //Debug.Log("First Click");
                        formatTextMesh();

                        //formatTextMeshOnUI();
                        //getPositions();
                        //drawLines();
                        //activateExplode();

                    }


                    else
                    {
                        //Debug.Log("Not First Click");
                        //activateExplode();
                        //drawLines();
                        for (int i = 0; i < texts.Count; i++)
                        {
                            texts[i].transform.gameObject.SetActive(true);
                        }
                    }

                   

                    return "activate2";
                }

                else
                {
                    for (int i = 0; i < texts.Count; i++)
                    {
                        texts[i].transform.gameObject.SetActive(false);
                    }

                }
            }
        }
        return "";
    }

    //Creates a new text mesh for each field
    public void createTextMesh()
    {
        for (int i = 1; i <= NUM_OF_FIELDS; i++)
        {
            text = Instantiate(text, gameObject.transform.position + new Vector3(0, i, 0), Quaternion.identity) as TextMesh;
            text.text = getData(i, excelNum);
            text.fontSize = FONT_SIZE;
            texts.Add(text);
        }
        created = true;
    }

    //Formats a Standard text mesh for the MajorComponent that was clicked
    public void formatTextMesh()//GameObject gameObjectToAnnotate)
    {
        text = Instantiate(text, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity) as TextMesh;
        //expandIcons[]
        text.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
        text.fontSize = FONT_SIZE;
        texts.Add(text);
        created = true;
        r = texts[0].transform.gameObject.AddComponent<Rigidbody>();
        r.useGravity = false;
    }

	//Formats a Standard text mesh for the MajorComponent that was clicked
	public void formatExteriorTextMesh()//GameObject gameObjectToAnnotate)
	{
		text = Instantiate(text, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity) as TextMesh;
		//expandIcons[]
		text.text = getData(1, excelNum) + "\n" + "R-Value: " + getData(5, excelNum) + "\n" + "Shading Coefficient: " + getData(6, excelNum) + "\n" + "Embodied Energy: " + getData(7, excelNum);
		text.fontSize = FONT_SIZE;
		texts.Add(text);
		created = true;
		r = texts[0].transform.gameObject.AddComponent<Rigidbody>();
		r.useGravity = false;
	}

    public void formatExteriorTextUI()//GameObject gameObjectToAnnotate)
    {
        Debug.Log(majorTextAreaText);
        majorTextAreaText.text = "R-Value: " + getData(5, excelNum) + "\n" + "Shading Coefficient: " + getData(6, excelNum) + "\n" + "Embodied Energy: " + getData(7, excelNum);

        majorTextAreaText.fontSize = FONT_SIZE;
        //texts.Add(text);
        //created = true;
        //r = texts[0].transform.gameObject.AddComponent<Rigidbody>();
        //r.useGravity = false;
    }

    public GameObject createExpandIcon(Transform inTransform, string inName)
    {
        //expandIcon = null;
        //icon = null;

        //Add Icon clone in that location 
        icon = Instantiate(Resources.Load<GameObject>("Location") as GameObject);

        //Clones parent location and name
        exTransform = inTransform;
        //Debug.Log(name);
        exName = inName;

        lCornerPivot = icon.transform.Find("LeftCornerPivot").gameObject;
        geometry = icon.transform.Find("Geometry").gameObject;

        icon.transform.position = inTransform.position;
        icon.transform.Translate(0, 0, .5f);
        icon.name = inName;

        icon.transform.parent = gameObject.transform.Find("IconMarker");

        formatExpandIcon();

        return icon;
    }

    public void formatExpandIcon()
    {
        //Creates Window
        if (!window)
        {
            window = GameObject.CreatePrimitive(PrimitiveType.Cube);
            window.gameObject.name = "Window";
            window.transform.parent = lCornerPivot.transform;
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
        exWidth = window.transform.localScale.x;
        exHeight = window.transform.localScale.y;

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
        rect.localPosition = new Vector3(-2, 0, 2);
        rect.sizeDelta = new Vector2(4, 4);
        canvasGameObject.layer = 13;

        //Adds Panel
        panelGameObject = new GameObject("Panel");
        RectTransform panelRect = panelGameObject.AddComponent<RectTransform>();
        //panelGameObject.name = "Panel";
        panelGameObject.transform.parent = canvasGameObject.transform;
        panelRect.localPosition = new Vector3(0, 0, 0);
        panelRect.sizeDelta = new Vector3(4, 4);
        panelGameObject.layer = 13;

        //Adds Button
        GameObject buttonGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buttonGameObject.transform.parent = panelGameObject.transform;
        buttonGameObject.transform.localPosition = new Vector3(0, 0, 40);
        //Debug.Log(width);
        //Debug.Log(height);
        buttonGameObject.transform.localScale = new Vector3((exWidth / 4), (exWidth / 8), 1);
        float widthOfButton = (exWidth / 4);
        float heightOfButton = (exWidth / 8);
        buttonGameObject.transform.localPosition = new Vector3(-(exWidth / 2) + widthOfButton - .25f, (exWidth / 2) - heightOfButton, 2); //2 - the width of the button //2 - the height of the button
        buttonGameObject.layer = 13;
        buttonGameObject.tag = "SubComponent";

        //Add Button Text
        buttonGameObject.AddComponent<Text>();


        //Adds Text
        textGameObject = new GameObject("Text");
        exText = textGameObject.AddComponent<Text>();
        textGameObject.transform.parent = panelGameObject.transform;
        textGameObject.transform.localPosition = new Vector3(0, 0, 60);
        exText.rectTransform.localScale = new Vector3(.05f, .05f, .05f);
        exText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75);
        exText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
        exText.rectTransform.localRotation = new Quaternion(0, 180, 0, 0);
        exText.alignment = TextAnchor.MiddleCenter;
        exText.horizontalOverflow = HorizontalWrapMode.Wrap;
        exText.verticalOverflow = VerticalWrapMode.Truncate;
        Font ArialFont = (Font)Resources.Load("HELVETICANEUELTSTD-ROMAN") as Font;
        exText.font = ArialFont;
        exText.fontSize = 5;
        exText.resizeTextForBestFit = true;
        exText.resizeTextMaxSize = 5;
        exText.resizeTextMinSize = 1;

        exText.text = "Information to be displayed for this component.";
        //if(excelNum == 0)
        //{
        //    excelNum = 6;
        //}
        //excelNum = Organized.Instance.systemDictionary["E4 (Trans)"].expandIconDictionary[_name];

        //exText.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
        //text.text = 
        //text.text = getName(Organized.Instance.systemDictionary[]icon.GetComponent<IconControl>().excelNum);
        //add a script that extends majorannotate and add it to the major component gameobject that has iconControl script and getComponent<>().excel number

        exText.enabled = true;
        exText.color = Color.white;
        textGameObject.layer = 13;


        //Aligns Pivot to left corner
        window.transform.Translate(new Vector3(-exWidth / 2, -exHeight / 2, 0));

        icon.transform.localScale = new Vector3(.15f, .15f, .15f);

        //window.layer = 10;

        //icon.SetActive(false);
        window.SetActive(false);
        canvasGameObject.SetActive(false);
        //lCornerPivot.SetActive(false);
        createdExpandIcon = true;
    }

    public void formatTextMeshOnUI()//SubComponent sub)
    {
        //text = Instantiate(text, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity) as TextMesh;
        //gets the MajorAnnotates text componet
        textUI = majorComponentText;
        //sets the text
        textUI.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
        //textUI.fontSize = FONT_SIZE;
        //texts.Add(text);
        //created = true;
        //r = texts[0].transform.gameObject.AddComponent<Rigidbody>();
        //r.useGravity = false;
        first = false;
    }

    public void formatTextMeshInWorld()//SubComponent sub)
    {
        //text = Instantiate(text, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity) as TextMesh;
        //gets the MajorAnnotates text componet
        textUI = majorComponentText;
        
        //sets the text
        textUI.text = getData(2, excelNum) + " " + getData(1, excelNum) + "\n" + "Size: " + getData(3, excelNum);
        //textUI.fontSize = FONT_SIZE;
        //texts.Add(text);
        //created = true;
        //r = texts[0].transform.gameObject.AddComponent<Rigidbody>();
        //r.useGravity = false;
        first = false;
    }

    /*
    //Creates a new LineRenderers each TextMesh
    public void createLineRenderers()
    {
        for (int i = 1; i <= texts.Count; i++)
        {
            line = Instantiate(line, gameObject.transform.position + new Vector3(0, i - 1, 0), Quaternion.identity) as LineRenderer;
            lines.Add(line);
        }
    }
    */

    public void toggleData()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            if (texts[i].transform.gameObject.activeSelf == true)
            {
                texts[i].transform.gameObject.SetActive(false);
            }
            else
            {
                texts[i].transform.gameObject.SetActive(true);
            }
        }
    }

    //public void toggleColliders()
    //{
    //    for (int i = 0; i < colliders.Count; i++)
    //    {
    //        if (colliders[i].enabled == true)
    //        {
    //            colliders[i].enabled = false;
    //        }
    //        else
    //        {
    //            colliders[i].enabled = true;
    //        }
    //    }
    //}

    public void drawLines()
    {
        //for each text mesh
        for (int i = 0; i < texts.Count; i++)
        {
            //if it dosn't have a line renderer, add one
            if (texts[i].transform.gameObject.GetComponent<LineRenderer>() == null)
            {
                Debug.Log("TextMesh " + i + ": has no LineRenderer, so one was added.");
                line = texts[i].transform.gameObject.AddComponent<LineRenderer>();
            }
            else
            {
                line = texts[i].transform.gameObject.GetComponent<LineRenderer>();
            }
            line.SetPositions(addLines(texts[i], i));
            line.SetWidth(width, width);
            line.material.SetTexture("_MainTex", tex);
            line.material = lineMaterial;
            line.SetColors(pColorA, pColorB);
            if (lines.Count == 0)
            {
                lines.Add(line);
            }
        }
    }

    //sets textMesh origin points as pt1, and the current position of the textMesh as pt2
    private Vector3[] addLines(TextMesh t, int i)
    {
        points[0] = oldPos[i];
        //points[1] = gameObject.transform.position + new Vector3(0, i - 1, 0);
        points[1] = texts[i].transform.position;
        return points;
    }

    /*
    public void explodeData()
    {
        for(int i = 0; i < texts.Count; i++)
        {
            //texts[i].transform.Translate(Vector3.Slerp(p)
        }
    }
    */

    public void getPositions()
    {
        //for each TextMesh
        for (int i = 0; i < texts.Count; i++)
        {
            //gets the original position of the texts 
            oldPos.Add(texts[i].transform.position);

            //for straight lined labels
            //Vector3 tempPos = (oldPos[i] + new Vector3(-5, 0, 0));

            //for labels come to player
            Vector3 tempPos = Vector3.Lerp(oldPos[i], (player.transform.position + new Vector3(0, i, 0)), 0.5f);
            newPos.Add(tempPos);

            //Debug.Log("old: " + oldPos[i] + "new: " + newPos[i]);
            //With Physics (not working yet)
            //bodies[i].AddForce(Vector3.forward);
        }
    }

    public void updatePositions()
    {
        newPos.Clear();

        for (int i = 0; i < texts.Count; i++)
        {
            //Straight lined labels
            //Vector3 tempPos = (oldPos[i] + new Vector3(-5, 0, 0));

            //Labels come to player
            Vector3 tempPos = Vector3.Lerp(oldPos[i], (player.transform.position + new Vector3(0, i, 0)), 0.5f);
            newPos.Add(tempPos);
        }
        //Debug.Log("Positions Updated");
    }

    public void activateExplode()
    {
        if (moved == false)
        {
            StartCoroutine(explodeData());
            moved = true;
        }
        else if (moved == true)
        {
            StartCoroutine(explodeBackData());
            moved = false;
        }
        else
        {
            Debug.Log("Text still moving, please click after it has stopped.");
        }
    }


    //Move layers transform
    IEnumerator explodeData()
    {
        for (int t = 0; t < texts.Count; t++)
        {
            if (texts[t].transform.gameObject.activeSelf == false)
            {
                toggleData();
            }
        }
        float timeSinceStarted = 0f;

        while (Vector3.Distance(texts[0].transform.position, newPos[0]) > 0f)
        {
            timeSinceStarted += Time.deltaTime;
            for (int i = 0; i < texts.Count; i++)
            {
                //Debug.Log("Explode[" + i + "]: newPos: " + newPos[i] + " currentPos: " + texts[i].transform.position + " oldPos: " + oldPos[i]);
                texts[i].transform.position = Vector3.Lerp(oldPos[i], newPos[i], timeSinceStarted);
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (texts[i].transform.position == newPos[i])
                {
                    //Debug.Log("moved");
                    moved = true;
                    yield break;
                }
                yield return null;
            }
        }
    }

    IEnumerator explodeBackData()
    {
        float timeSinceStarted = 0f;

        while (Vector3.Distance(texts[0].transform.position, oldPos[0]) > 0f)
        {
            timeSinceStarted += Time.deltaTime;
            for (int i = 0; i < texts.Count; i++)
            {
                //Debug.Log("Explode[" + i + "]: newPos: " + newPos[i] + " currentPos: " + texts[i].transform.position + " oldPos: " + oldPos[i]);
                texts[i].transform.position = Vector3.Lerp(newPos[i], oldPos[i], timeSinceStarted);
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (texts[i].transform.position == oldPos[i])
                {
                    //Debug.Log("moved back");
                    moved = false;
                    toggleData();
                    updatePositions();
                    yield break;
                }
                yield return null;
            }
        }
    }

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

    public void followData()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            //Debug.Log(texts[i].name + "rotating");
            r.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            r.transform.LookAt(player.transform.parent.position);
            r.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    //ACCESSORS//    
    public string getData(int number)
    {
        int num = number;
        string value = "";

        switch (num)
        {

            case 1:
                value = getName();

                break;

            case 2:
                value = getMaterial();

                break;

            case 3:
                value = getSize();

                break;

            case 4:
                value = getDescription();
                if (getDescription().Equals("null"))
                {
                    value = "";
                }

                break;

            case 5:
                string u = getRValue();
                value = "" + u.ToString();

                break;

            case 6:
                string sc = getShadingCoefficient(excelNum);
                value = "" + sc.ToString();

                break;

            case 7:
                string ee = getEmbodiedEnergy(excelNum);
                value = "$" + ee.ToString();

                break;

            default:
                value = "Field Missing";

                break;
        }
        return value;
    }

    public string getName()
    {
        globalName = MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].name;
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].name;
    }

    public string getMaterial()
    {
        globalMaterial = MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].material;
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].material;
    }

    public string getSize()
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].size;
    }

    public string getDescription()
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].description;
    }

    public string getRValue()
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].rValue;
    }

    public string getShadingCoefficient()
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].shadingCoefficient;
    }

    public string getEmbodiedEnergy()
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].embodiedEnergy;
    }

    void OnEnable()
    {
        //EventManager.DiagramButtonPressed += activateData;
        EventManager.StartListening("MajorComponentView", activateData);
        EventManager.StartListening("ExpandIcon", expand);
    }

    void OnDisable()
    {
        //EventManager.DiagramButtonPressed -= activateData;
        EventManager.StopListening("MajorComponentView", activateData);
        EventManager.StopListening("ExpandIcon", expand);
    }

    public void expand()
    {
        //Debug.Log("Window = " + window);
        //Organized.Instance.toggle(window);
        //Organized.Instance.toggle(canvasGameObject);
    }

    public void activateData()
    {

    }

    public string getData(int number, int excelNum)
    {
        int num = number;
        string value = "";

        switch (num)
        {

            case 1:
                value = getName(excelNum);

                break;

            case 2:
                value = getMaterial(excelNum);

                break;

            case 3:
                value = getSize(excelNum);

                break;

            case 4:
                value = getDescription(excelNum);
                if (getDescription().Equals("null"))
                {
                    value = "";
                }

                break;

            case 5:
                string u = getRValue(excelNum);
                value = "" + u.ToString();

                break;

            case 6:
                string sc = getShadingCoefficient(excelNum);
                value = "" + sc.ToString();

                break;

            case 7:
                string ee = getEmbodiedEnergy(excelNum);
                value = ee.ToString();

                break;

            default:
                value = "Field Missing";

                break;
        }
        return value;
    }

    public string getName(int excelNum)
    {
        globalName = MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].name;
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].name;
    }

    public string getMaterial(int excelNum)
    {
        globalMaterial = MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].material;
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].material;
    }

    public string getSize(int excelNum)
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].size;
    }

    public string getDescription(int excelNum)
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].description;
    }

    public string getRValue(int excelNum)
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].rValue;
    }

    public string getShadingCoefficient(int excelNum)
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].shadingCoefficient;
    }

    public string getEmbodiedEnergy(int excelNum)
    {
        return MajorDetailLoader.Instance.dc.majorDetails[excelNum - 2].embodiedEnergy;
    }
}

//organizeData()

//explodeData() eg. move to the right


//drawLines() (1d)

//remove collider from transparent parts that are in the way and not going to be clickable (1/2d)

//add rim shader to show what you are clicking on (1d)

//use a sphere collider for pulse instead of a distance(1/2d)

//pulse for tansparent parts to return to opaque (maybe a button since the meshcollider will be removed) (1d)

//cut smaller hidden floor for foundation, as well as the ceiling (1d)

//switch everything to FPS camera instead of AR (1/2d)

//Add exterior landscape (1d) Hadi (AR)

//Rotate skybox slowly (1/2d)

//Heat transfer diagrams (convection, radiation, conduction) (1week) (AR)

//Wind diagram (2d) Hadi (AR)

//9days + 7days = 16 days (Due August 11th)


//Today:
//get excel sheet to shahin
//drawlines

