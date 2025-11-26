using UnityEngine;

public class SpawnTheLickers : MonoBehaviour
{
	public GameObject progControl;

	public GameObject theLickers;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !progControl.GetComponent<ChallageRoomProgControl>().doubleDoorOpened)
		{
			theLickers.SetActive(value: true);
			progControl.GetComponent<ChallageRoomProgControl>().lickersSpawned = true;
		}
	}
}
