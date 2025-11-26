using UnityEngine;

public class RunAmbient : MonoBehaviour
{
	public AudioSource sound;

	public bool soundPlaying;

	private bool reset;

	private float startValue;

	private float endValue = 0.2f;

	private float transitionTime = 7f;

	public float t;

	private void Start()
	{
		GetComponent<RunAmbient>().enabled = false;
	}

	private void Update()
	{
		if (reset)
		{
			t = 0f;
			soundPlaying = false;
			reset = false;
		}
		if (!soundPlaying)
		{
			sound.Play();
			soundPlaying = true;
		}
		if (sound.volume == endValue)
		{
			reset = true;
			GetComponent<RunAmbient>().enabled = false;
		}
		else
		{
			t += Time.deltaTime / transitionTime;
			sound.volume = Mathf.SmoothStep(startValue, endValue, t);
		}
	}
}
