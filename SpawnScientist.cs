using UnityEngine;

public class SpawnScientist : MonoBehaviour
{
	public GameObject SaneScientist;

	private void OnTriggerEnter(Collider other)
	{
		if (!PlayerManager.ScSexAfterMath && other.tag == "Player")
		{
			SaneScientist.SetActive(value: true);
		}
	}
}
