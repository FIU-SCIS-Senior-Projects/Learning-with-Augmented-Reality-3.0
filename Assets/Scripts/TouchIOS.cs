using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchIOS : MonoBehaviour {

	public TouchIOS()
	{

	}

	public Ray UpdateTouch () 
	{
		int nbTouches = Input.touchCount;

		Ray returnRay = new Ray();

		Debug.Log ("nbTouches: " + nbTouches);

		if(nbTouches > 0)
		{
			for (int i = 0; i < nbTouches; i++)
			{
				Touch touch = Input.GetTouch(i);

				TouchPhase phase = touch.phase;

				switch(phase)
				{
				case TouchPhase.Began:
					print("New touch detected at position " + touch.position + " , index " + touch.fingerId);
					break;
				case TouchPhase.Moved:
					print("Touch index " + touch.fingerId + " has moved by " + touch.deltaPosition);
					break;
				case TouchPhase.Stationary:
					print("Touch index " + touch.fingerId + " is stationary at position " + touch.position);
					break;
				case TouchPhase.Ended:
					print ("Touch index " + touch.fingerId + " ended at position " + touch.position);
					//returnRay = Camera.main.ScreenPointToRay (Input.touches [0].position);
					returnRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
					break;
				case TouchPhase.Canceled:
					print("Touch index " + touch.fingerId + " cancelled");
					break;
				}
			}
		}

		return returnRay;
	}
}
