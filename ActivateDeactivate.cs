using UnityEngine;

public class ActivateDeactivate : MonoBehaviour
{
	public AudioSource sound;

	public GameObject activateAndDeactivate;

	private void OnTriggerEnter(Collider other)
	{
		if (sound.isPlaying)
		{
			if (activateAndDeactivate.GetComponent<RunAmbient>().enabled)
			{
				activateAndDeactivate.GetComponent<RunAmbient>().t = 0f;
				activateAndDeactivate.GetComponent<RunAmbient>().soundPlaying = false;
				activateAndDeactivate.GetComponent<RunAmbient>().enabled = false;
			}
			activateAndDeactivate.GetComponent<DeactivateAmbient>().enabled = true;
		}
	}
}
