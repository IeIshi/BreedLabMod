using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	public GameObject insectEnemy;

	private void Update()
	{
		if (DoorAlwaysOpen.doorOpen)
		{
			insectEnemy.gameObject.SetActive(value: true);
		}
		else
		{
			insectEnemy.gameObject.SetActive(value: false);
		}
	}
}
