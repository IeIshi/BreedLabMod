using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public int id;

	public new string name = "New Item";

	public Sprite icon;

	public bool isDefaultItem;

	public bool isImportant;

	public string content;

	public string header;

	private AudioSource drinkSound;

	public virtual void Use()
	{
		if (name == "Tissues")
		{
			if (!HeroineStats.creampied)
			{
				return;
			}
			HeroineStats.creampied = false;
			HeroineStats.oralCreampie = false;
			HeroineStats.fertileCum = false;
			HeroineStats.lustyCum = false;
			HeroineStats.hugeAmount = false;
			HeroineStats.addictiveCum = false;
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("isCumFilled", value: false);
			PlayerManager.instance.player.GetComponent<HeroineStats>().cumStrain.Stop();
			Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMesh);
			PlayerManager.instance.player.GetComponent<HeroineStats>().showCumSpot = false;
			Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMouth);
			PlayerManager.instance.player.GetComponent<HeroineStats>().showCumMouth = false;
			RemoveFromInventory();
		}
		if (name == "RepairKit")
		{
			if (EquipmentManager.pantiesDurability == 1f && EquipmentManager.stockingsDurability == 1f && EquipmentManager.shoeDurability == 1f && EquipmentManager.skirtDurability == 1f)
			{
				return;
			}
			if (EquipmentManager.pantiesDurability < 1f)
			{
				EquipmentManager.pantiesDurability = 1f;
				HeroineStats.PantiesCircle.fillAmount = 1f;
			}
			if (EquipmentManager.stockingsDurability < 1f)
			{
				EquipmentManager.stockingsDurability = 1f;
				HeroineStats.stockingsDurSlider.fillAmount = 1f;
			}
			if (EquipmentManager.shoeDurability < 1f)
			{
				EquipmentManager.shoeDurability = 1f;
				HeroineStats.shoesDurSlider.fillAmount = 1f;
			}
			if (EquipmentManager.skirtDurability < 1f)
			{
				EquipmentManager.skirtDurability = 1f;
				HeroineStats.skirtDurSlider.fillAmount = 1f;
			}
			RemoveFromInventory();
		}
		Debug.Log("Using" + name);
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);
	}
}
