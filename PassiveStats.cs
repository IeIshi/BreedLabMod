using System;
using UnityEngine;
using UnityEngine.UI;

public class PassiveStats : MonoBehaviour
{
	public static PassiveStats instance;

	public Stat damage;

	public Stat speed;

	public Stat experience;

	public Stat def;

	public Stat lustdecrease;

	public Stat orgdecrease;

	public Stat lustRes;

	public Stat orgRes;

	public Text damageText;

	public Text lustdecreaseText;

	public Text orgdecreaseText;

	public Text lustResText;

	public Text orgResText;

	public Text speedText;

	public Text expText;

	public Text maxExpText;

	public Text defText;

	public static float MyDamage;

	public static float MyLustDecrease;

	public static float MyOrgDecrase;

	public static float MyExp;

	public static float MySpeed;

	public static float MyLustRes;

	public static float MyOrgRes;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		EquipmentManager equipmentManager = EquipmentManager.instance;
		equipmentManager.onEquipmentChanged = (EquipmentManager.OnEquipmentChanged)Delegate.Combine(equipmentManager.onEquipmentChanged, new EquipmentManager.OnEquipmentChanged(OnEquipmentChanged));
		orgRes.AddModifier(2f * (float)HeroineStats.myLevel);
		damage.AddModifier(0.05f * (float)HeroineStats.myLevel);
		for (int i = 0; i < EquipmentManager.instance.currentEquipment.Length; i++)
		{
			if (EquipmentManager.instance.currentEquipment[i] != null)
			{
				damage.AddModifier(EquipmentManager.instance.currentEquipment[i].dmgModifier);
				speed.AddModifier(EquipmentManager.instance.currentEquipment[i].speedModifier);
				lustdecrease.AddModifier(EquipmentManager.instance.currentEquipment[i].lustDecreaseModifier);
				def.AddModifier(EquipmentManager.instance.currentEquipment[i].defence);
				orgdecrease.AddModifier(EquipmentManager.instance.currentEquipment[i].orgDecreaseModifier);
				lustRes.AddModifier(EquipmentManager.instance.currentEquipment[i].lustResModifier);
				orgRes.AddModifier(EquipmentManager.instance.currentEquipment[i].orgResModifier);
			}
		}
		lustdecrease.AddModifier(GetComponent<HeroineStats>().lustDeregRate);
		orgdecrease.AddModifier(GetComponent<HeroineStats>().orgDeregRate);
		lustRes.AddModifier(GetComponent<HeroineStats>().lustResistance);
		orgRes.AddModifier(GetComponent<HeroineStats>().orgResistance);
		Debug.Log("MyOrgRes: " + orgRes.GetValue());
	}

	private void Update()
	{
		if (InventoryUI.inventoryIsOpen)
		{
			speedText.text = Math.Round(speed.GetValue(), 2).ToString();
			expText.text = Math.Round(experience.GetValue(), 2).ToString();
			orgdecreaseText.text = Math.Round(orgdecrease.GetValue(), 2).ToString();
			if (EquipmentManager.heroineIsNaked)
			{
				defText.text = 0.ToString();
			}
			else
			{
				defText.text = def.GetValue().ToString();
			}
			damageText.text = Math.Round(damage.GetValue(), 2).ToString();
			lustdecreaseText.text = Math.Round(lustdecrease.GetValue(), 2).ToString();
			lustResText.text = Math.Round(lustRes.GetValue(), 2).ToString();
			orgResText.text = Math.Round(orgRes.GetValue(), 2).ToString();
		}
	}

	private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	{
		if (newItem != null)
		{
			orgdecrease.AddModifier(newItem.orgDecreaseModifier);
			speed.AddModifier(newItem.speedModifier);
			lustdecrease.AddModifier(newItem.lustDecreaseModifier);
			damage.AddModifier(newItem.dmgModifier);
			def.AddModifier(newItem.defence);
			orgRes.AddModifier(newItem.orgResModifier);
			lustRes.AddModifier(newItem.lustResModifier);
			MySpeed = speed.GetValue();
			MyDamage = damage.GetValue();
			MyLustDecrease = lustdecrease.GetValue();
			MyExp = experience.GetValue();
			MyOrgDecrase = orgdecrease.GetValue();
			MyLustRes = lustRes.GetValue();
			MyOrgRes = orgRes.GetValue();
		}
		if (oldItem != null)
		{
			orgdecrease.RemoveModifier(oldItem.orgDecreaseModifier);
			speed.RemoveModifier(oldItem.speedModifier);
			lustdecrease.RemoveModifier(oldItem.lustDecreaseModifier);
			damage.RemoveModifier(oldItem.dmgModifier);
			def.RemoveModifier(oldItem.defence);
			orgRes.RemoveModifier(oldItem.orgResModifier);
			lustRes.RemoveModifier(oldItem.lustResModifier);
			MySpeed = speed.GetValue();
			MyDamage = damage.GetValue();
			MyLustDecrease = lustdecrease.GetValue();
			MyExp = experience.GetValue();
			MyLustRes = lustRes.GetValue();
			MyOrgRes = orgRes.GetValue();
			MyOrgDecrase = orgdecrease.GetValue();
		}
	}
}
