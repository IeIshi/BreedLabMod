using System.Collections;
using UnityEngine;

public class AmbientSoundEnter : MonoBehaviour
{
	public AudioSource ambientSound;

	public float maxVolume = 0.2f;

	public float volumeRaiseSpeed = 1f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			StartCoroutine(StartAmbient());
		}
	}

	private IEnumerator StartAmbient()
	{
		Debug.Log(ambientSound.volume);
		if (!ambientSound.isPlaying)
		{
			ambientSound.Play();
			for (float volume = 0f; volume <= maxVolume; volume += Time.deltaTime * volumeRaiseSpeed)
			{
				ambientSound.volume = volume;
				yield return null;
			}
		}
	}
}
