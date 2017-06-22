using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollHelper : MonoBehaviour {

    public ScrollRect scroll;
    public Scrollbar scrollbar;

	// Use this for initialization
	void Start () {
        scrollbar.value = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if(scrollbar.value != 1)
        {
            scrollbar.value = Mathf.Lerp(0, 1, Time.deltaTime);
        }
        */
        
        /*
	    if(scroll.enabled == false)
        {
            scrollbar.value = 1;
        }
        */
	}
}
