using UnityEngine;

public class ScaryRoomSoundOff : MonoBehaviour
{
	private AudioSource silentHill;

	private void Start()
	{
		silentHill = GameObject.Find("SilentHillishSound").GetComponent<AudioSource>();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			silentHill.Stop();
		}
	}
}
