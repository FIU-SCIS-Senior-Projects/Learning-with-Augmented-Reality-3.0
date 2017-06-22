using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppearWithProx : MonoBehaviour {

    public Transform user;
    public List<Transform> buttons = new List<Transform>();
    public GameObject aerialCam;
    public GameObject returnButton;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Organized.Instance.isolationActive);
        //if (Organized.Instance.isolationActive == false) //this is not updating
        {
            //Debug.Log("AerialCam " + aerialCam.activeSelf);
            //Debug.Log("ReturnButton " + returnButton.activeSelf);
            if (!aerialCam.activeSelf) //&& !returnButton.activeSelf
            {
                if (!Organized.Instance.panelDictionary["Panel_Soil"].activeSelf && !Organized.Instance.panelDictionary["Panel_Drainage"].activeSelf)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        if (Vector3.Distance(buttons[i].transform.position, user.transform.position) < 12)
                        {
                            {
                                buttons[i].gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            buttons[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
