using System;
using System.Collections;
using UnityEngine;

public class GasExtruder : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool open;

		public bool conInside;
	}

	private Animator animator;

	private Inventory inventory;

	private bool open;

	private bool conInside;

	public Dialogue dialogue_open;

	public GameObject GasConDummyFull;

	public GameObject Smoke;

	public GameObject Wolf;

	public AudioSource gasSound;

	public AudioSource openDoor;

	public GameObject glasGate;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.open = open;
		saveData.conInside = conInside;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		open = saveData.open;
		conInside = saveData.conInside;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		inventory = Inventory.instance;
		GasConDummyFull.SetActive(value: false);
		Smoke.SetActive(value: false);
		if (open)
		{
			animator.SetBool("isOpen", value: true);
		}
		if (conInside)
		{
			Wolf.GetComponent<SupHum>().sleeping = true;
			Wolf.GetComponent<Animator>().SetBool("sleep", value: true);
			base.gameObject.layer = 0;
			GetComponent<GasExtruder>().enabled = true;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			openDoor.Play();
			open = true;
			return;
		}
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "GasContainerFull")
			{
				inventory.items[i].RemoveFromInventory();
				openDoor.Play();
				StartCoroutine(PutInContainer());
				conInside = true;
				base.gameObject.layer = 0;
				return;
			}
		}
		if (open && !conInside)
		{
			TriggerDialogeOpen();
		}
	}

	public void TriggerDialogeOpen()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_open);
	}

	private IEnumerator PutInContainer()
	{
		GasConDummyFull.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		Debug.Log("VerarbeitungsSounds");
		animator.SetBool("isOpen", value: false);
		open = false;
		yield return new WaitForSeconds(3f);
		GasConDummyFull.SetActive(value: false);
		Smoke.SetActive(value: true);
		gasSound.Play();
		yield return new WaitForSeconds(4f);
		if (!glasGate.GetComponent<SecuritySchalterDoor>().open)
		{
			Wolf.GetComponent<Animator>().SetBool("sleep", value: true);
			Wolf.GetComponent<SupHum>().enabled = false;
			Wolf.GetComponent<SupHum>().sleeping = true;
		}
		yield return new WaitForSeconds(3f);
		gasSound.Stop();
	}
}
