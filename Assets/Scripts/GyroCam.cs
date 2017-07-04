using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCam : MonoBehaviour 
{
	private bool gyroEnabled;
	private Gyroscope gyro;
	private bool locked;

	private GameObject cameraContainer;
	private GameObject rotationViewer;
	private Quaternion rot;

	private void Start()
	{
		locked = false;

		cameraContainer = new GameObject ("Camera Container");
		cameraContainer.transform.position = transform.position;
		//cameraContainer.transform.rotation = transform.rotation; //added //don't need rotation becuase maincamera holds
		transform.SetParent (cameraContainer.transform);

		rotationViewer = new GameObject ("RotationViewer");

		gyroEnabled = EnableGyro();
	}

	private bool EnableGyro()
	{
		rotationViewer.transform.rotation = Quaternion.Euler (0f,0f,0f);

		if(SystemInfo.supportsGyroscope)
		{
			Debug.Log ("Gyro Enabled");

			gyro = Input.gyro;
			gyro.enabled = true;

			//cameraContainer.transform.rotation = Camera.main.transform.rotation;
			//cameraContainer.transform.rotation = GameObject.Find ("APosition").transform.rotation;
			cameraContainer.transform.rotation = Quaternion.Euler (90f,90f,0f);
			//transform.rotation = Quaternion.Euler(90f,0f,0f);
			rot = new Quaternion (0,0,1,0);
			//rot = cameraContainer.transform.localRotation;

			//transform.rotation = GameObject.Find ("APosition").transform.rotation;
			return true;
		}

		return false;
	}

	private void Update()
	{
		//Debug.Log ("CameraContainer" + cameraContainer.transform.localEulerAngles);
		//Debug.Log ("CamerMain" + transform.localEulerAngles);
		Debug.Log ("locked" + locked);

		if (locked) 
		{
			if (gyroEnabled) 
			{
				transform.localRotation = gyro.attitude * rot;
			}
		}
	}

	void toggleGyro()
	{
		if(locked)
		{
			Debug.Log ("togglegyrofalse");
			locked = false;
		}
		else
		{
			Debug.Log ("togglegyrotrue");
			locked = true;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, 0, 300, 300));
		if (GUILayout.Button("ToggleGyro", GUILayout.Width(300), GUILayout.Height(150)))
		{
			Debug.Log ("toggle pressed");
			toggleGyro();
		}
		GUILayout.EndArea();
	}

	void OnEnable()
	{
		EventManager.StartListening ("Lock", setLock);
	}

	void OnDisable()
	{
		EventManager.StopListening ("Lock", setLock);
	}

	void setLock()
	{
		locked = true;
		Debug.Log ("Locked");
	}
}


//	private bool gyroSupported;
//
//	// Use this for initialization
//	void Start () 
//	{
//
//		gyroSupported = SystemInfo.supportsGyroscope;
//
//		if (gyroSupported)
//		{
//			Input.gyro.enabled = true;          //needed for android
//			Application.targetFrameRate = 60;
//		}
//		else
//		{
//			//AndroidNativeFunctions.ShowAlert("We're sorry, your device doesn't support gyroscope. You won't be able to get the full experience of the app", "WARNING", "Ok", "Back", "Later", ShowAlertAction);
//		}
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		transform.Rotate (-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
//	}
//
//	void ApplyGyroRotation()
//	{
//		transform.rotation = Input.gyro.attitude;
//		transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
//		transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
//		//appliedGyroYAngle = transform.eulerAngles.y; // Save the angle around y axis for use in calibration.
//	}
