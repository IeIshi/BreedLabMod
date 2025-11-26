using System;
using UnityEngine;

public class SecuritySchalter : Interactable, ISaveable
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
		if (repaired)
		{
			if (isOn)
			{
				rend.sharedMaterial = material[1];
			}
			if (!isOn)
			{
				rend.sharedMaterial = material[0];
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
			}
			else
			{
				rend.sharedMaterial = material[0];
			}
		}
		else if (PlayerManager.id2)
		{
			repaired = true;
			PlayerManager.id2 = false;
			GameObject.Find("KeyBox").GetComponent<KeyBox>().id2.SetActive(value: false);
			isOn = true;
			rend.sharedMaterial = material[1];
			turnOn.Play();
		}
		else if (!repaired)
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
