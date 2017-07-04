using UnityEngine;

public class GyroCamera : MonoBehaviour {

	private float initialYAngle = 0f;
	private float appliedGyroYAngle = 0f;
	private float calibrationYAngle = 0f;
	private bool gyroSupported;
	private bool wantsToStopGyro;

	// Use this for initialization
	void Start () {
		gyroSupported = SystemInfo.supportsGyroscope;
		wantsToStopGyro = false;

		if (gyroSupported)
		{
			Input.gyro.enabled = true;          //needed for android
			Application.targetFrameRate = 60;
			initialYAngle = transform.eulerAngles.y;
		}
		else
		{
			AndroidNativeFunctions.ShowAlert("We're sorry, your device doesn't support gyroscope. You won't be able to get the full experience of the app", "WARNING", "Ok", "Back", "Later", ShowAlertAction);
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (gyroSupported)
		{
			ApplyGyroRotation();
			ApplyCalibration();
		}
	}

	public void CalibrateYAngle()
	{
		calibrationYAngle = appliedGyroYAngle - initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
	}

	void ApplyGyroRotation()
	{
		transform.rotation = Input.gyro.attitude;
		transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
		transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
		appliedGyroYAngle = transform.eulerAngles.y; // Save the angle around y axis for use in calibration.
	}

	void ApplyCalibration()
	{
		transform.Rotate(0f, -calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
	}

	void toggleGyro()
	{
		if (gyroSupported)
		{
			Input.gyro.enabled = wantsToStopGyro;
			wantsToStopGyro = !wantsToStopGyro;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, 108, 150, 208));
		if (GUILayout.Button("Calibrate", GUILayout.Width(150), GUILayout.Height(100)))
		{
			CalibrateYAngle();
		}
		if (GUILayout.Button("ToggleGyro", GUILayout.Width(150), GUILayout.Height(100)))
		{
			toggleGyro();
		}
		GUILayout.EndArea();
	}

	void ShowAlertAction(DialogInterface w)
	{
		AndroidNativeFunctions.ShowToast(w.ToString());
	}
}