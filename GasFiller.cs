using System;
using System.Collections;
using UnityEngine;

public class GasFiller : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool readyToPickUp;

		public bool pickedUpGas;
	}

	private Animator animator;

	private Inventory inventory;

	private bool open;

	public Dialogue dialogue_open;

	public Dialogue dialogue_invfull;

	public GameObject GasConDummyEmpty;

	public GameObject GasConDummyFull;

	public Item item;

	public bool readyToPickUp;

	public bool pickedUpGas;

	public AudioSource gasSound;

	public AudioSource doorSound;

	public AudioSource pickUpSound;

	public AudioSource putInSound;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.readyToPickUp = readyToPickUp;
		saveData.pickedUpGas = pickedUpGas;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		readyToPickUp = saveData.readyToPickUp;
		pickedUpGas = saveData.pickedUpGas;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		inventory = Inventory.instance;
		if (readyToPickUp)
		{
			animator.SetBool("isOpen", value: true);
			GasConDummyFull.SetActive(value: true);
		}
		else if (pickedUpGas)
		{
			animator.SetBool("isOpen", value: true);
			GasConDummyEmpty.SetActive(value: false);
			GasConDummyFull.SetActive(value: false);
			base.gameObject.layer = 0;
		}
		else
		{
			GasConDummyEmpty.SetActive(value: false);
			GasConDummyFull.SetActive(value: false);
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (readyToPickUp)
		{
			if (Inventory.instance.items.Count < Inventory.instance.space)
			{
				if (!pickedUpGas)
				{
					Inventory.instance.Add(item);
					pickUpSound.Play();
					GasConDummyFull.SetActive(value: false);
					pickedUpGas = true;
					readyToPickUp = false;
					base.gameObject.layer = 0;
				}
			}
			else
			{
				TriggerDialogeInvFull();
			}
			return;
		}
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			doorSound.Play();
			open = true;
			return;
		}
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "GasContainer")
			{
				inventory.items[i].RemoveFromInventory();
				putInSound.Play();
				StartCoroutine(PutInContainer());
				return;
			}
		}
		TriggerDialogeOpen();
	}

	public void TriggerDialogeOpen()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_open);
	}

	public void TriggerDialogeInvFull()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_invfull);
	}

	private IEnumerator PutInContainer()
	{
		GasConDummyEmpty.SetActive(value: true);
		base.gameObject.layer = 0;
		yield return new WaitForSeconds(1f);
		gasSound.Play();
		doorSound.Play();
		animator.SetBool("isOpen", value: false);
		open = false;
		GasConDummyEmpty.SetActive(value: false);
		yield return new WaitForSeconds(3f);
		animator.SetBool("isOpen", value: true);
		doorSound.Play();
		gasSound.Stop();
		GasConDummyFull.SetActive(value: true);
		base.gameObject.layer = 9;
		readyToPickUp = true;
	}
}
