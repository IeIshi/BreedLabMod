using UnityEngine;

public class MutScThrustEvent : MonoBehaviour
{
	public AudioSource sexSound1;

	public AudioSource sexSound2;

	public AudioSource sexSound3;

	private void ThrustEvent()
	{
		sexSound1.Play();
		sexSound3.Play();
	}

	private void ThrustOutEvent()
	{
		sexSound2.Play();
	}
}
