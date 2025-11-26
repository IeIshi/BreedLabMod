using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public float mouseSensitivity = 10f;

	public Transform target;

	private float dstFromTarget;

	public Vector2 pitchMinMax = new Vector2(-40f, 85f);

	public float rotationSmoothTime = 0.12f;

	private Vector3 rotationSmoothVelocity;

	private Vector3 currentRotation;

	private float yaw;

	private float pitch;

	private float zoom = 0.5f;

	public float smooth = 10f;

	private Vector3 dollyDir;

	public Vector3 dollyDirAdjusted;

	public float distance;

	private void Start()
	{
		dstFromTarget = 2f;
	}

	private void Update()
	{
		Rotate();
		Zoom();
		dstFromTarget = Mathf.Clamp(dstFromTarget, 0.5f, 4f);
		//target.position = new Vector3(target.position.x, target.position.y + 0.1f, target.position.z);
		base.transform.position = target.position - base.transform.forward * dstFromTarget;
	}

	private void Rotate()
	{
		if (Input.GetMouseButton(0))
		{
			yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
			pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
		}
		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		base.transform.eulerAngles = currentRotation;
	}

	private void Zoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			dstFromTarget -= zoom;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			dstFromTarget += zoom;
		}
	}
}
