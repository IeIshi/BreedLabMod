using System.Collections;
using UnityEngine;

public class StartAudioAtCertainTime : MonoBehaviour
{
	private AudioSource audioSource;

	public float startTime = 3f;

	public float targetVolume = 0.5f;

	public float fadeDuration = 3f;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.time = startTime;
		StartCoroutine(FadeInAudio());
	}

	private IEnumerator FadeInAudio()
	{
		audioSource.Play();
		audioSource.volume = 0f;
		float elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
			yield return null;
		}
		audioSource.volume = targetVolume;
	}
}
