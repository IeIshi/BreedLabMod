using UnityEngine;

public class ActivateLewdCouple : MonoBehaviour
{
	public GameObject TheLewdCouple;

	public bool activate;

	public bool deactive;

	private void Start()
	{
		TheLewdCouple.SetActive(value: false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (activate)
			{
				TheLewdCouple.SetActive(value: true);
			}
			if (deactive)
			{
				TheLewdCouple.SetActive(value: false);
			}
		}
	}
}
