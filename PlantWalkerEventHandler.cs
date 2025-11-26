using UnityEngine;

public class PlantWalkerEventHandler : MonoBehaviour
{
	public AudioSource sexSound2;

	public AudioSource[] wetSoundArray;

	private void ThrustEvent()
	{
		if (EquipmentManager.instance.currentEquipment[3] != null)
		{
			if (EquipmentManager.instance.currentEquipment[3].id == 454874)
			{
				return;
			}
			EquipmentManager.instance.RipPantsu();
		}
		wetSoundArray[Random.Range(0, wetSoundArray.Length)].Play();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
	}
}
