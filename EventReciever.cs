using UnityEngine;

public class EventReciever : MonoBehaviour
{
	public AudioSource ss;

	public AudioSource mf;

	private void MouthFuckEvent()
	{
		mf.Play();
	}

	private void ThrustEvent()
	{
		ss.Play();
	}
}
