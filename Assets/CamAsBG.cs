using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CamAsBG : MonoBehaviour {

    private RawImage image;
    private WebCamTexture cam;
	private AspectRatioFitter arf;
	private bool wantsToStopCamera;

	// Use this for initialization
	void Start () {
        arf = GetComponent<AspectRatioFitter>();
		image = GetComponent<RawImage>();
		wantsToStopCamera = false;
        cam = new WebCamTexture(Screen.width, Screen.height);
		image.texture = cam;
        cam.Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if(cam.width < 100)
        {
            Debug.Log("Waiting for correct frames...");
            return;
        }

        float cwNeeded = -cam.videoRotationAngle;
        if (cam.videoVerticallyMirrored)
            cwNeeded += 180;

        image.rectTransform.localEulerAngles = new Vector3(0f, 0f, cwNeeded);
        float videRatio = (float)cam.width / (float)cam.height;
        arf.aspectRatio = videRatio;

        if (cam.videoVerticallyMirrored)
        {
            image.uvRect = new Rect(1, 0, -1, 1);
        }
        else
        {
            image.uvRect = new Rect(0, 0, 1, 1);
        }
	}

	void toogleCamera(){
		if (wantsToStopCamera) {
			cam.Pause ();
		} else {
			cam.Play ();
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, 4, 150, 104));

		if (GUILayout.Button("Toggle Camera", GUILayout.Width(150), GUILayout.Height(100)))
		{
			wantsToStopCamera = !wantsToStopCamera;
			toogleCamera();
		}
		GUILayout.EndArea();
	}

	void ShowAlertAction(DialogInterface w)
	{
		AndroidNativeFunctions.ShowToast(w.ToString());
	}
}
