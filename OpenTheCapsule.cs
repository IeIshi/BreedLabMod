using UnityEngine;

public class OpenTheCapsule : MonoBehaviour
{
	public AudioSource openSound;

	public GameObject MindFleyer;

	private void OpenSound()
	{
		openSound.Play();
	}

	private void StopSound()
	{
		openSound.Stop();
		MindFleyer.GetComponent<MindFleyerControl>().enabled = true;
	}
}
