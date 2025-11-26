using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
	public EquipmentSlot equipSlot;

	public SkinnedMeshRenderer mesh;

	public float dmgModifier;

	public float lustDecreaseModifier;

	public float orgDecreaseModifier;

	public float lustResModifier;

	public float orgResModifier;

	public float speedModifier;

	public float defence;

	public override void Use()
	{
		base.Use();
		if (equipSlot == EquipmentSlot.Shoes && EquipmentManager.shoesOn)
		{
			Debug.Log("Shoes already on");
		}
		else if (equipSlot == EquipmentSlot.Crotch && EquipmentManager.skirtOn)
		{
			Debug.Log("Skirt already on");
		}
		else if (equipSlot == EquipmentSlot.Legs && EquipmentManager.stockingsOn)
		{
			if (EquipmentManager.instance.currentEquipment[4].id == 451196)
			{
				EquipmentManager.instance.Equip(this);
				RemoveFromInventory();
			}
			else
			{
				Debug.Log("Stockings already on");
			}
		}
		else
		{
			EquipmentManager.instance.Equip(this);
			RemoveFromInventory();
		}
	}
}
