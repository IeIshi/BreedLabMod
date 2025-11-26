using System.Collections;
using UnityEngine;

public class AmbientSoundExit : MonoBehaviour
{
	public AudioSource ambientSound;

	public float maxVolume = 0.2f;

	public float volumeRaiseSpeed = 1f;

	private bool entered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !entered)
		{
			StartCoroutine(StopAmbient());
		}
	}

	private IEnumerator StopAmbient()
	{
		Debug.Log(ambientSound.volume);
		if (ambientSound.isPlaying)
		{
			entered = true;
			for (float volume = maxVolume; volume >= 0f; volume -= Time.deltaTime * volumeRaiseSpeed)
			{
				ambientSound.volume = volume;
				Debug.Log("FOR_LOOP_RUNNING");
				yield return null;
			}
			yield return new WaitForSeconds(2f);
			entered = false;
			ambientSound.Stop();
		}
	}
}
