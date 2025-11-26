using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;

	[Range(0f, 360f)]
	public float viewAngle;

	public LayerMask targetMask;

	public LayerMask obstacleMask;

	private PlayerController controller;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public static Transform target;

	public GameObject questionMarkPrefab;

	public Transform questionMarkTarget;

	private Transform questionMarkUI;

	private Transform cam;

	private void Start()
	{
		controller = GetComponent<PlayerController>();
		cam = Camera.main.transform;
		Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				questionMarkUI = UnityEngine.Object.Instantiate(questionMarkPrefab, canvas.transform).transform;
				break;
			}
		}
	}

	private void LateUpdate()
	{
		FindVisibleTargets();
		if (visibleTargets.Count >= 1)
		{
			target = visibleTargets[0];
			questionMarkUI.gameObject.SetActive(value: true);
			if (questionMarkUI != null)
			{
				questionMarkUI.position = questionMarkTarget.position;
				questionMarkUI.forward = -cam.forward;
			}
		}
		if (visibleTargets.Count == 0)
		{
			target = null;
			questionMarkUI.gameObject.SetActive(value: false);
		}
	}

	private void FindVisibleTargets()
	{
		visibleTargets.Clear();
		Collider[] array = Physics.OverlapSphere(base.transform.position, viewRadius, targetMask);
		for (int i = 0; i < array.Length; i++)
		{
			Transform transform = array[i].transform;
			Vector3 normalized = (transform.position - base.transform.position).normalized;
			if (Vector3.Angle(base.transform.forward, normalized) < viewAngle / 2f)
			{
				float maxDistance = Vector3.Distance(base.transform.position, transform.position);
				if (!Physics.Raycast(base.transform.position, normalized, maxDistance, obstacleMask))
				{
					visibleTargets.Add(transform);
				}
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += base.transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * ((float)Math.PI / 180f)), 0f, Mathf.Cos(angleInDegrees * ((float)Math.PI / 180f)));
	}
}
