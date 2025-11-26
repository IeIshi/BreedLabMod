using UnityEngine;

public class DeactivateAmbientTunnel : MonoBehaviour
{
	public AudioSource ambientSound1;

	public AudioSource ambientSound2;

	public bool tunnel1;

	public bool tunnel2;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (tunnel1 && ActivateAmbientTunnel.ambientTunnel1)
			{
				ambientSound1.Stop();
				ActivateAmbientTunnel.ambientTunnel1 = false;
			}
			if (tunnel2 && ActivateAmbientTunnel.ambientTunnel2)
			{
				ambientSound2.Stop();
				ActivateAmbientTunnel.ambientTunnel2 = false;
			}
		}
	}
}
