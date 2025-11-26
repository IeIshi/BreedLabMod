using System;
using UnityEngine;

public class SchalterRiddle : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool isOn;

		public bool repaired;
	}

	public Material[] material;

	private Renderer rend;

	public AudioSource turnOn;

	public AudioSource turnOff;

	public AudioSource nene;

	public GameObject lamp;

	private Renderer rendLamp;

	[SerializeField]
	public bool isOn;

	[SerializeField]
	public bool repaired;

	private Inventory inventory;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.isOn = isOn;
		saveData.repaired = repaired;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		isOn = saveData.isOn;
		repaired = saveData.repaired;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		rendLamp = lamp.GetComponent<Renderer>();
		if (repaired)
		{
			if (isOn)
			{
				rend.sharedMaterial = material[1];
				rendLamp.sharedMaterial = material[4];
			}
			if (!isOn)
			{
				rend.sharedMaterial = material[0];
				rendLamp.sharedMaterial = material[3];
			}
		}
		else
		{
			rend.sharedMaterial = material[2];
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (repaired)
		{
			Schaltung();
			if (isOn)
			{
				rend.sharedMaterial = material[1];
				rendLamp.sharedMaterial = material[4];
			}
			else
			{
				rend.sharedMaterial = material[0];
				rendLamp.sharedMaterial = material[3];
			}
			return;
		}
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "IDCard")
			{
				repaired = true;
				inventory.items[i].RemoveFromInventory();
				isOn = true;
				rend.sharedMaterial = material[1];
				turnOn.Play();
				return;
			}
		}
		if (!repaired)
		{
			nene.Play();
		}
	}

	private void Schaltung()
	{
		if (!isOn)
		{
			isOn = true;
			turnOn.Play();
		}
	}
}
