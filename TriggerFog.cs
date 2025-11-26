using UnityEngine;

public class TriggerFog : MonoBehaviour
{
	public GameObject gangBangScene;

	public GameObject heroine;

	private float startValue;

	private float endValue = 0.3f;

	private float transitionTime = 10f;

	private float t;

	public float lustGain = 1f;

	private void FixedUpdate()
	{
		if (GetComponent<ActivateGangBang>().triggerFogy)
		{
			if (t > 1f)
			{
				gangBangScene.SetActive(value: true);
				GetComponent<ActivateGangBang>().enabled = false;
			}
			if (t < 1f)
			{
				t += Time.deltaTime / transitionTime;
				RenderSettings.fogDensity = Mathf.SmoothStep(startValue, endValue, t);
			}
		}
		heroine.GetComponent<HeroineStats>().GainLust(lustGain);
	}
}
