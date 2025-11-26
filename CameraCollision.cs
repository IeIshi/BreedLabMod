using UnityEngine;

public class CameraCollision : MonoBehaviour
{
	public float minDistance = 1f;

	public float maxDistance = 4f;

	private Vector3 dollyDir;

	public Vector3 dollyDirAjdusted;

	public float distance;

	private float shootingDist = 1.8f;

	private float zoom = 0.1f;

	public float maxCamDistance = 2.3f;

	private LayerMask mask;

	public Transform followedObj;

	private void Awake()
	{
		dollyDir = base.transform.localPosition.normalized;
		distance = base.transform.localPosition.magnitude;
	}

	private void Start()
	{
		mask = LayerMask.GetMask("CamObst");
	}

	private void Update()
	{
		if (CameraFollow.shootingMode)
		{
			distance = shootingDist;
			base.transform.localPosition = dollyDir * distance;
			return;
		}
		Vector3 end = followedObj.TransformPoint(dollyDir * maxDistance);
		if (Physics.Linecast(followedObj.position, end, out var hitInfo, mask))
		{
			distance = Mathf.Clamp(hitInfo.distance * 0.9f, 0.1f, maxDistance);
		}
		else
		{
			distance = maxDistance;
		}
		maxDistance = Mathf.Clamp(maxDistance, minDistance, maxCamDistance);
		base.transform.localPosition = dollyDir * distance;
		Zoom();
	}

	private void Zoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if (PlayerController.iFalled)
			{
				minDistance = 0.1f; //1.3f;
			}
			else
			{
				minDistance = 0.1f; //1f;
			}
			maxDistance -= zoom;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			maxDistance += zoom;
		}
	}
}
