using UnityEngine;

public class ActivateAmbientTunnel : MonoBehaviour
{
	public AudioSource ambientSound1;

	public AudioSource ambientSound2;

	public static bool ambientTunnel1;

	public static bool ambientTunnel2;

	public bool tunnel1;

	public bool tunnel2;

	private void Start()
	{
		ambientTunnel1 = false;
		ambientTunnel2 = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (tunnel1 && !ambientTunnel1)
			{
				ambientSound1.Play();
				ambientTunnel1 = true;
			}
			if (tunnel2 && !ambientTunnel2)
			{
				ambientSound2.Play();
				ambientTunnel2 = true;
			}
		}
	}
}
