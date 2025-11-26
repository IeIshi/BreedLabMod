using UnityEngine;

public class AmbientHallOff : MonoBehaviour
{
	public AudioSource ambientHall;

	public AudioSource ambientTunnel;

	public bool isTunnel2;

	public void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player"))
		{
			return;
		}
		if (!isTunnel2)
		{
			ambientHall.Stop();
			if (!ambientTunnel.isPlaying)
			{
				ambientTunnel.Play();
			}
		}
		else
		{
			ambientHall.Stop();
			ambientTunnel.Stop();
		}
	}
}
