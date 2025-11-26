using System.Collections;
using UnityEngine;

public class ActivateAmbient : MonoBehaviour
{
	public AudioSource sound;

	private float transitionTime = 7f;

	private float t;

	private bool volumeFull;

	private void OnTriggerEnter(Collider other)
	{
		if (!volumeFull)
		{
			StartCoroutine(Activate());
		}
	}

	private IEnumerator Activate()
	{
		if (!sound.isPlaying)
		{
			sound.Play();
			t += Time.deltaTime / transitionTime;
			sound.volume = Mathf.SmoothStep(0f, 0.2f, t);
		}
		if (sound.volume >= 0.2f)
		{
			yield return new WaitForSeconds(0.1f);
			t = 0f;
			volumeFull = true;
		}
	}
}
