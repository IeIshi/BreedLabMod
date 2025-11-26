using UnityEngine;

public class AmbientHallOn : MonoBehaviour
{
	public AudioSource ambientHall;

	public AudioSource ambientTunnel;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!ambientHall.isPlaying)
			{
				ambientHall.Play();
			}
			ambientTunnel.Stop();
		}
	}
}
