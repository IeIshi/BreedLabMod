using UnityEngine;

public class PlayVictory : MonoBehaviour
{
	public AudioSource victory;

	private float startValue;

	private float endValue = 0.4f;

	private float t;

	private float transitionTime = 5f;

	private bool start;

	private void Update()
	{
		t += Time.deltaTime / transitionTime;
		victory.volume = Mathf.SmoothStep(startValue, endValue, t);
		if (!start)
		{
			victory.Play();
			start = true;
		}
		if (startValue == endValue)
		{
			GetComponent<PlayVictory>().enabled = false;
			victory.Stop();
		}
	}
}
