using UnityEngine;

public class DeactivateAmbient : MonoBehaviour
{
	public AudioSource sound;

	private float endValue;

	private float transitionTime = 7f;

	public float t;

	private bool reset;

	private void Start()
	{
		GetComponent<DeactivateAmbient>().enabled = false;
	}

	private void Update()
	{
		if (sound.volume == endValue)
		{
			reset = true;
			sound.Stop();
			GetComponent<DeactivateAmbient>().enabled = false;
		}
		else
		{
			float volume = sound.volume;
			t += Time.deltaTime / transitionTime;
			sound.volume = Mathf.SmoothStep(volume, endValue, t);
		}
		if (reset)
		{
			t = 0f;
			reset = false;
		}
	}
}
