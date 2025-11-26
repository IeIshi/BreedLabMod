using UnityEngine;

public class SpawnMayu : MonoBehaviour
{
	public GameObject Mayu;

	public AudioSource SpawnSound;

	private bool triggered;

	private void Start()
	{
		Mayu.SetActive(value: false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !triggered)
		{
			SpawnSound.Play();
			Mayu.SetActive(value: true);
			triggered = true;
		}
	}
}
