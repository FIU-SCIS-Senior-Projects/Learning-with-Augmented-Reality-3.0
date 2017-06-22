using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MajorDetailLoader : MonoBehaviour {

    public const string path = "Components032017_3";

    private static MajorDetailLoader instance;

    public MajorDetailContainer dc;
    //public Dictionary<string, ExpandIcon> expandIcons = new Dictionary<string, ExpandIcon>();
    //public int excelNum;

    //Singleton Constructor
    public static MajorDetailLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MajorDetailLoader>();

                if (instance == null)
                {
                    GameObject go = new GameObject("MajorDetailLoader");
                    instance = go.AddComponent<MajorDetailLoader>();
                    Debug.Log("Singleton created!");
                }
            }
            return instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Instance.dc = MajorDetailContainer.Load(path);
        //Debug.Log("Awake dc");
        //for (int i = 2; i < dc.majorDetails.Count; i++)
        //{
        //    //Debug.Log("MajorDetailLoader: " + dc.majorDetails[i]);
        //    Debug.Log("MajorDetailLoader: " + dc.majorDetails[i].name);
        //}
    }

    //public string getData(int number, int excelNum)
    //{
    //    int num = number;
    //    string value = "";

    //    switch (num)
    //    {

    //        case 1:
    //            value = getName(excelNum);

    //            break;

    //        case 2:
    //            value = getMaterial(excelNum);

    //            break;

    //        case 3:
    //            value = getSize(excelNum);

    //            break;

    //        case 4:
    //            value = getDescription(excelNum);

    //            break;

    //        case 5:
    //            double u = getRValue(excelNum);
    //            value = "" + u.ToString();

    //            break;

    //        case 6:
    //            double w = getWieght(excelNum);
    //            value = "" + w.ToString();

    //            break;

    //        case 7:
    //            double c = getCost(excelNum);
    //            value = "$" + c.ToString();

    //            break;

    //        default:
    //            value = "Field Missing";

    //            break;
    //    }
    //    return value;
    //}

    //public string getName(int excelNum)
    //{
    //    //for (int i = 2; i < dc.majorDetails.Count; i++)
    //    //{
    //    //    //Debug.Log("MajorDetailLoader " + dc.majorDetails[i].material);
    //    //    Debug.Log("MajorDetailLoader " + dc.majorDetails[i]);
    //    //}
        
    //    return dc.majorDetails[excelNum - 2].name;
    //}

    //public string getMaterial(int excelNum)
    //{
    //    //for (int i = 2; i < dc.majorDetails.Count; i++)
    //    //{
    //    //    Debug.Log("MajorDetailLoader " + dc.majorDetails[i]);
    //    //    //Debug.Log("MajorDetailLoader " + dc.majorDetails[i].name);
    //    //}
    //    Debug.Log(excelNum);
    //    Debug.Log(dc);
    //    Debug.Log(dc.majorDetails[7].material);
    //    return dc.majorDetails[excelNum - 2].material;
    //}

    //public string getSize(int excelNum)
    //{
    //    return dc.majorDetails[excelNum - 2].size;
    //}

    //public string getDescription(int excelNum)
    //{
    //    return dc.majorDetails[excelNum - 2].description;
    //}

    //public double getRValue(int excelNum)
    //{
    //    return dc.majorDetails[excelNum - 2].rValue;
    //}

    //public double getWieght(int excelNum)
    //{
    //    return dc.majorDetails[excelNum - 2].wieght;
    //}

    //public double getCost(int excelNum)
    //{
    //    return dc.majorDetails[excelNum - 2].cost;
    //}
}
