using System;
using UnityEngine;

public class ContHolders : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool redConInserted;

		public bool blueConInserted;

		public bool greenConInserted;
	}

	public GameObject container;

	private Inventory inventory;

	public Dialogue dialogue_noCon;

	public Dialogue dialogue_con;

	public AudioSource putInSound;

	public static bool redConInserted;

	public static bool blueConInserted;

	public static bool greenConInserted;

	public bool redConHolder;

	public bool greenConHolder;

	public bool blueConHolder;

	public GameObject TheCapsule;

	private Animator capsuleAnimator;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.redConInserted = redConInserted;
		saveData.blueConInserted = blueConInserted;
		saveData.greenConInserted = greenConInserted;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData obj = (SaveData)state;
		redConInserted = obj.redConInserted;
		blueConInserted = obj.blueConInserted;
		greenConInserted = obj.greenConInserted;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		if (TheCapsule != null)
		{
			capsuleAnimator = TheCapsule.GetComponent<Animator>();
		}
		if (redConHolder && container != null)
		{
			if (redConInserted)
			{
				container.SetActive(value: true);
			}
			else
			{
				container.SetActive(value: false);
			}
		}
		if (blueConHolder && container != null)
		{
			if (blueConInserted)
			{
				container.SetActive(value: true);
			}
			else
			{
				container.SetActive(value: false);
			}
		}
		if (greenConHolder && container != null)
		{
			if (greenConInserted)
			{
				container.SetActive(value: true);
			}
			else
			{
				container.SetActive(value: false);
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (redConHolder)
		{
			if (!redConInserted)
			{
				for (int i = 0; i < inventory.items.Count; i++)
				{
					if (inventory.items[i].name == "RedOrbContainer")
					{
						inventory.items[i].RemoveFromInventory();
						putInSound.Play();
						container.SetActive(value: true);
						redConInserted = true;
						if (redConInserted && blueConInserted && greenConInserted)
						{
							InventoryUI.heroineIsChased = true;
							capsuleAnimator.SetBool("isOpen", value: true);
						}
						return;
					}
				}
				TriggerDialogeNoCon();
			}
			else
			{
				TriggerDialogeCon();
			}
		}
		if (blueConHolder)
		{
			if (!blueConInserted)
			{
				for (int j = 0; j < inventory.items.Count; j++)
				{
					if (inventory.items[j].name == "BlueOrbContainer")
					{
						inventory.items[j].RemoveFromInventory();
						putInSound.Play();
						container.SetActive(value: true);
						blueConInserted = true;
						if (redConInserted && blueConInserted && greenConInserted)
						{
							InventoryUI.heroineIsChased = true;
							capsuleAnimator.SetBool("isOpen", value: true);
						}
						return;
					}
				}
				TriggerDialogeNoCon();
			}
			else
			{
				TriggerDialogeCon();
			}
		}
		if (!greenConHolder)
		{
			return;
		}
		if (!greenConInserted)
		{
			for (int k = 0; k < inventory.items.Count; k++)
			{
				if (inventory.items[k].name == "GreenOrbContainer")
				{
					inventory.items[k].RemoveFromInventory();
					putInSound.Play();
					container.SetActive(value: true);
					greenConInserted = true;
					if (redConInserted && blueConInserted && greenConInserted)
					{
						InventoryUI.heroineIsChased = true;
						capsuleAnimator.SetBool("isOpen", value: true);
					}
					return;
				}
			}
			TriggerDialogeNoCon();
		}
		else
		{
			TriggerDialogeCon();
		}
	}

	public void TriggerDialogeNoCon()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_noCon);
	}

	public void TriggerDialogeCon()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_con);
	}
}
