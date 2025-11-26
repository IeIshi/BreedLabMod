using UnityEngine;

public class Safespace : MonoBehaviour
{
	public static bool heroineSafe;

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			heroineSafe = true;
			InventoryUI.heroineIsChased = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			heroineSafe = false;
			MasturbationArea.mastArea = false;
		}
	}
}
