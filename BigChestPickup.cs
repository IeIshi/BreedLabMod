using System;
using System.Collections.Generic;
using UnityEngine;

public class BigChestPickup : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool opened;
	}

	public List<Item> items = new List<Item>();

	private Inventory inventory;

	public Dialogue dialogue;

	public Dialogue dialogueTwo;

	public AudioSource openSound;

	private Animator anim;

	private bool opened;

	public bool lewdChest;

	private void Start()
	{
		anim = GetComponent<Animator>();
		inventory = Inventory.instance;
		if (opened)
		{
			anim.SetBool("isOpen", value: true);
			base.gameObject.layer = 0;
		}
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.opened = opened;
		return saveData;
	}

	public void RestoreState(object state)
	{
		opened = ((SaveData)state).opened;
	}

	public override void Interact()
	{
		base.Interact();
		if (opened)
		{
			return;
		}
		int count = Inventory.instance.items.Count;
		int space = Inventory.instance.space;
		int count2 = items.Count;
		int num = count + count2;
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "Lewd_Tights")
			{
				inventory.items[i].RemoveFromInventory();
			}
			if (inventory.items[i].name == "LewdBra")
			{
				inventory.items[i].RemoveFromInventory();
			}
			if (inventory.items[i].name == "CrothlessPanties")
			{
				inventory.items[i].RemoveFromInventory();
			}
		}
		if (num > space)
		{
			TriggerDialoge();
			return;
		}
		PickUpItemList();
		openSound.Play();
	}

	private void PickUpItemList()
	{
		if (lewdChest)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if ((i == 0 && EquipmentManager.instance.currentEquipment[4] != null && EquipmentManager.instance.currentEquipment[4].id == 451196) || (i == 1 && EquipmentManager.instance.currentEquipment[1] != null && EquipmentManager.instance.currentEquipment[1].id == 3648532) || (i == 2 && EquipmentManager.instance.currentEquipment[3] != null && EquipmentManager.instance.currentEquipment[3].id == 454874))
				{
					return;
				}
				Inventory.instance.Add(items[i]);
			}
			base.gameObject.layer = 0;
			if (true)
			{
				anim.SetBool("isOpen", value: true);
				opened = true;
			}
		}
		else
		{
			for (int j = 0; j < items.Count; j++)
			{
				Inventory.instance.Add(items[j]);
			}
			base.gameObject.layer = 0;
			if (true)
			{
				anim.SetBool("isOpen", value: true);
				opened = true;
			}
		}
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerDialogeTwo()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueTwo);
	}
}
