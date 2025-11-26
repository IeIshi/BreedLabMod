using UnityEngine;

public class FirstHoul : MonoBehaviour
{
	public AudioSource HoulSound;

	private void Start()
	{
		if (!PlayerManager.firstHoul)
		{
			HoulSound.Play();
			PlayerManager.firstHoul = true;
		}
	}
}
