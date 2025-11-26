using UnityEngine;

public class TPC : MonoBehaviour
{
	[SerializeField]
	private Vector2 cameraSensitivity = Vector2.one;

	[SerializeField]
	private float zoomSensitivity = 10f;

	[SerializeField]
	private float pitchClamp = 85f;

	[SerializeField]
	private Vector2 zoomClamp = new Vector2(0.5f, 25f);

	[SerializeField]
	private float zoomDamp = 0.05f;

	[SerializeField]
	private float cameraRadius = 0.5f;

	private float pitch;

	private float yaw;

	private float targetZoom = 5f;

	private float zoom = 5f;

	private float zoomVelocity;

	private void Update()
	{
		Rotate();
		Zoom();
	}

	private void Rotate()
	{
		if (Input.GetMouseButton(1))
		{
			pitch -= Input.GetAxis("Mouse Y") * cameraSensitivity.y;
			yaw += Input.GetAxis("Mouse X") * cameraSensitivity.x;
		}
		pitch = Mathf.Clamp(pitch, 0f - pitchClamp, pitchClamp);
		base.transform.parent.rotation = Quaternion.Euler(pitch, yaw, 0f);
	}

	private void Zoom()
	{
		targetZoom += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
		targetZoom = Mathf.Clamp(targetZoom, zoomClamp.x, zoomClamp.y);
		if (Physics.Raycast(new Ray(base.transform.parent.position, -base.transform.parent.forward), out var hitInfo, targetZoom))
		{
			zoom = Mathf.SmoothDamp(zoom, hitInfo.distance, ref zoomVelocity, zoomDamp);
			zoom -= cameraRadius;
		}
		else
		{
			zoom = Mathf.SmoothDamp(zoom, targetZoom, ref zoomVelocity, zoomDamp);
		}
		zoom = Mathf.Clamp(zoom, zoomClamp.x, zoomClamp.y);
		base.transform.localPosition = Vector3.back * zoom;
	}
}
