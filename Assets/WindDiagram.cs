using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WindDiagram : MonoBehaviour {

    public GameObject west;
    public GameObject north;
    public GameObject east;
    public GameObject south;
    public GameObject southEast;

    //public GameObject panel;

    private List<GameObject> directions = new List<GameObject>();

    public int sliderW; //start South
    public Slider windSlider;
    //public Toggle prevailingToggle;

    // Use this for initialization
    void Start ()
    {
        directions.Add(west);
        directions.Add(north);
        directions.Add(east);
        directions.Add(south);
        directions.Add(southEast);

        windSlider.wholeNumbers = true;

        windSlider.onValueChanged.AddListener(delegate { onValueChange(); });
    }
	
	// Update is called once per frame
	void Update ()
    {
        windDirection(windSlider.value);

        windPos();
    }

    /*
    public void returnToGround()
    {
        toggleCam(hideCams);
        toggle(hideObjects);
        changeSkybox();
    }
    */

    public void windDirection(float sliderw)
    {
        sliderW = (int)sliderw;
    }

    public void onValueChange()
    {
        //Debug.Log(sliderW);
    }

    public void windPos()
    {
        if (windSlider.transform.parent.transform.parent.gameObject.activeSelf)
        {
            if (sliderW == 0)
            {
                deactivateUnused(south.name);
            }

            if (sliderW == 1)
            {
                deactivateUnused(west.name);
            }

            if (sliderW == 2)
            {
                deactivateUnused(north.name);
            }

            if (sliderW == 3)
            {
                deactivateUnused(east.name);
            }
            if (sliderW == 4)
            {
                deactivateUnused(southEast.name);
            }
        }

        else
        {
            deactivateUnused("empty");
        }
    }

    public void deactivateUnused(string currentDir)
    {
        for(int i = 0; i<directions.Count; i++)
        {
            if (directions[i].name.Equals(currentDir) == false)
            {
                directions[i].SetActive(false);
            }
            else
            {
                directions[i].SetActive(true);
            }
        }
    }
}
