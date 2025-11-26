using UnityEngine;

public class PlaySilentMusic : MonoBehaviour
{
	private AudioSource silentAmbient;

	private bool soundIsPlaying;

	private void Start()
	{
		silentAmbient = GameObject.Find("AmbientSoundHall").GetComponent<AudioSource>();
		soundIsPlaying = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !soundIsPlaying)
		{
			silentAmbient.Play();
			soundIsPlaying = true;
		}
	}
}
