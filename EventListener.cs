using UnityEngine;

public class EventListener : MonoBehaviour
{
	public AudioSource sound;

	public AudioSource sound2;

	private void SoundEvent()
	{
		sound.Play();
	}

	private void SoundEvent2()
	{
		sound2.Play();
	}
}
