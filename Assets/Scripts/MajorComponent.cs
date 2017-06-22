using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MajorComponent : MonoBehaviour {

    public string _name;
    public Transform _root;

    public bool hasIcon;
    public IconControl iconControl;
    //public ExpandIcon icon;
    //public Transform zoomPos;

    public List<SubComponent> subcomponents = new List<SubComponent>();
    public Dictionary<string, SubComponent> subcomponentDictionary = new Dictionary<string, SubComponent>();

    public MajorComponent(string name, Transform root)
    {
        _name = name;
        _root = root;

        collectIcon();
    }

    //Checks to see if this component has an ExpandIcon
    public void collectIcon()
    {
        if (_root.gameObject.GetComponent<IconControl>() == null)
        {
            hasIcon = false;
        }
        else
        {
            hasIcon = true;
            iconControl = _root.gameObject.GetComponent<IconControl>();
            if (!iconControl.zoomPos)
            {
                //GameObject z = new GameObject();
                //Instantiate(z);
                //z.transform.parent = _root;
                //iconControl.zoomPos.position = _root.gameObject.transform.position;
                //iconControl.zoomPos.Translate(0, 5f, 5f);
                //Debug.Log(iconControl.zoomPos.localPosition);
            }
            //Debug.Log(iconControl.zoomPos.localPosition);
            //Debug.Log(hasIcon);
        }
        
    }

    //public ExpandIcon getExpandIcon()
    //{
    //    return icon;
    //}

    void Start()
    {
        
    }

}
