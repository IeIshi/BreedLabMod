using UnityEngine;

public class FreeCamera : MonoBehaviour
{
	public GameObject cam;

	private float yaw;

	private float pitch;

	private float mouseSensitivity = 500f;

	private Vector3 currentRotation;

	private Vector3 rotationSmoothVelocity;

	private float rotationSmoothTime = 0.12f;

	public static bool attaching;

	private void Start()
	{
		GetComponent<AudioListener>().enabled = false;
		GetComponent<Camera>().enabled = false;
	}

	private void Update()
	{
		if (GetComponent<Camera>().enabled)
		{
			CamRot();
		}
	}

	private void CamRot()
	{
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
		pitch = Mathf.Clamp(pitch, -12f, 25f);
		yaw = Mathf.Clamp(yaw, 50f, 130f);
		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		cam.transform.eulerAngles = currentRotation;
	}
}
