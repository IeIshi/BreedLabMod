using UnityEngine;

public class AppearTV : MonoBehaviour
{
	public GameObject TV;

	public AudioSource clickSound;

	private void Start()
	{
		TV.SetActive(value: false);
	}

	private void FixedUpdate()
	{
		if (HeroineStats.currentLust >= 100f)
		{
			TV.SetActive(value: false);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (HeroineStats.currentLust < 80f)
		{
			if (!TV.activeSelf)
			{
				clickSound.Play();
			}
			TV.SetActive(value: true);
		}
	}
}
