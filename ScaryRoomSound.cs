using UnityEngine;

public class ScaryRoomSound : MonoBehaviour
{
	private AudioSource silentHill;

	private void Start()
	{
		silentHill = GameObject.Find("SilentHillishSound").GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			silentHill.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			silentHill.Stop();
		}
	}
}
