using UnityEngine;

public class RipOfOnEnter : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player"))
		{
			return;
		}
		for (int i = 0; i < EquipmentManager.instance.currentEquipment.Length; i++)
		{
			if (EquipmentManager.instance.currentEquipment[i] != null && i > 0 && i < 8)
			{
				EquipmentManager.instance.clothRipSound.Play();
				EquipmentManager.instance.RipOff(i);
				break;
			}
		}
	}
}
