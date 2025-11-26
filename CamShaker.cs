using System;
using System.Collections;
using UnityEngine;

public class CamShaker : MonoBehaviour
{
	[Serializable]
	public class Properties
	{
		public float angle;

		public float strength;

		public float maxSpeed;

		public float minSpeed;

		public float duration;

		[Range(0f, 1f)]
		public float noisePercent;

		[Range(0f, 1f)]
		public float dampingPercent;

		[Range(0f, 1f)]
		public float rotationPercent;

		public Properties(float angle, float strength, float maxSpeed, float minSpeed, float duration, float noisePercent, float dampingPercent, float rotationPercent)
		{
			this.angle = angle;
			this.strength = strength;
			this.maxSpeed = maxSpeed;
			this.minSpeed = minSpeed;
			this.duration = duration;
			this.noisePercent = Mathf.Clamp01(noisePercent);
			this.dampingPercent = Mathf.Clamp01(dampingPercent);
			this.rotationPercent = Mathf.Clamp01(rotationPercent);
		}
	}

	private const float maxAngle = 10f;

	private IEnumerator currentShakeCoroutine;

	public void StartShake(Properties properties)
	{
		if (currentShakeCoroutine != null)
		{
			StopCoroutine(currentShakeCoroutine);
		}
		currentShakeCoroutine = Shake(properties);
		StartCoroutine(currentShakeCoroutine);
	}

	private IEnumerator Shake(Properties properties)
	{
		float completionPercent = 0f;
		float movePercent = 0f;
		float angle_radians = properties.angle * ((float)Math.PI / 180f) - (float)Math.PI;
		Vector3 previousWaypoint = Vector3.zero;
		Vector3 currentWaypoint = Vector3.zero;
		float moveDistance = 0f;
		float speed = 0f;
		Quaternion targetRotation = Quaternion.identity;
		Quaternion previousRotation = Quaternion.identity;
		do
		{
			if (movePercent >= 1f || completionPercent == 0f)
			{
				float num = DampingCurve(completionPercent, properties.dampingPercent);
				float num2 = (UnityEngine.Random.value - 0.5f) * (float)Math.PI;
				angle_radians += (float)Math.PI + num2 * properties.noisePercent;
				currentWaypoint = new Vector3(Mathf.Cos(angle_radians), Mathf.Sin(angle_radians)) * properties.strength * num;
				previousWaypoint = base.transform.localPosition;
				moveDistance = Vector3.Distance(currentWaypoint, previousWaypoint);
				targetRotation = Quaternion.Euler(new Vector3(currentWaypoint.y, currentWaypoint.x).normalized * properties.rotationPercent * num * 10f);
				previousRotation = base.transform.localRotation;
				speed = Mathf.Lerp(properties.minSpeed, properties.maxSpeed, num);
				movePercent = 0f;
			}
			completionPercent += Time.deltaTime / properties.duration;
			movePercent += Time.deltaTime / moveDistance * speed;
			base.transform.localPosition = Vector3.Lerp(previousWaypoint, currentWaypoint, movePercent);
			base.transform.localRotation = Quaternion.Slerp(previousRotation, targetRotation, movePercent);
			yield return null;
		}
		while (moveDistance > 0f);
	}

	private float DampingCurve(float x, float dampingPercent)
	{
		x = Mathf.Clamp01(x);
		float p = Mathf.Lerp(2f, 0.25f, dampingPercent);
		float num = 1f - Mathf.Pow(x, p);
		return num * num * num;
	}
}
