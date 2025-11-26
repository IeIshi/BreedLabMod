using System;
using UnityEngine;

public class Schalter : Interactable, ISaveable
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

	public GameObject Lamp;

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
			base.gameObject.layer = 0;
			if (isOn)
			{
				if (Lamp != null)
				{
					Lamp.GetComponent<LampChanger>().rend.sharedMaterial = Lamp.GetComponent<LampChanger>().material[1];
				}
				rend.sharedMaterial = material[1];
			}
			if (!isOn)
			{
				if (Lamp != null)
				{
					Lamp.GetComponent<LampChanger>().rend.sharedMaterial = Lamp.GetComponent<LampChanger>().material[0];
				}
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
		else if (PlayerManager.id1)
		{
			repaired = true;
			base.gameObject.layer = 0;
			PlayerManager.id1 = false;
			GameObject.Find("KeyBox").GetComponent<KeyBox>().id1.SetActive(value: false);
			if (Lamp != null)
			{
				Lamp.GetComponent<LampChanger>().rend.sharedMaterial = Lamp.GetComponent<LampChanger>().material[1];
			}
			isOn = true;
			rend.sharedMaterial = material[1];
			turnOn.Play();
		}
		else if (PlayerManager.id11)
		{
			repaired = true;
			PlayerManager.id11 = false;
			base.gameObject.layer = 0;
			GameObject.Find("KeyBox").GetComponent<KeyBox>().id11.SetActive(value: false);
			if (Lamp != null)
			{
				Lamp.GetComponent<LampChanger>().rend.sharedMaterial = Lamp.GetComponent<LampChanger>().material[1];
			}
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
