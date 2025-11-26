using UnityEngine;

public class BlobAreaEnemyDisabler : MonoBehaviour
{
	public GameObject TheEnemies;

	private void Start()
	{
		TheEnemies.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !TheEnemies.activeSelf)
		{
			TheEnemies.SetActive(value: true);
		}
	}
}
