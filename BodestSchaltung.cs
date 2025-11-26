using System;
using UnityEngine;

public class BodestSchaltung : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool activated;
	}

	public Material[] material;

	private Renderer rendLampe;

	public Dialogue dialogue;

	public Dialogue dialogue2;

	public GameObject lamp;

	private Renderer rendLamp;

	public GameObject skulptur;

	public GameObject pillar2;

	public GameObject pillar3;

	public GameObject impregSpawn;

	public static bool allActivated;

	private bool tierInDerTasche;

	private Inventory inventory;

	public bool isHund;

	public bool isVogel;

	public bool isSnek;

	private string animal;

	public bool activated;

	public AudioSource statuePlaceSound;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.activated = activated;
		return saveData;
	}

	public void RestoreState(object state)
	{
		activated = ((SaveData)state).activated;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		rendLampe = lamp.GetComponent<Renderer>();
		rendLampe.enabled = true;
		rendLampe.sharedMaterial = material[0];
		if (isHund)
		{
			animal = "Wolf";
		}
		else if (isVogel)
		{
			animal = "Crow";
		}
		else
		{
			animal = "Snake";
		}
		if (activated)
		{
			skulptur.SetActive(value: true);
			rendLampe.sharedMaterial = material[1];
			if (pillar2.GetComponent<BodestSchaltung>().activated && pillar3.GetComponent<BodestSchaltung>().activated)
			{
				allActivated = true;
				impregSpawn.gameObject.SetActive(value: true);
			}
		}
		if (!allActivated)
		{
			impregSpawn.gameObject.SetActive(value: false);
		}
	}

	public override void Interact()
	{
		base.Interact();
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == animal)
			{
				inventory.items[i].RemoveFromInventory();
				tierInDerTasche = true;
			}
		}
		if (tierInDerTasche)
		{
			skulptur.SetActive(value: true);
			statuePlaceSound.Play();
			activated = true;
			rendLampe.sharedMaterial = material[1];
			tierInDerTasche = false;
			if (pillar2.GetComponent<BodestSchaltung>().activated && pillar3.GetComponent<BodestSchaltung>().activated)
			{
				allActivated = true;
				impregSpawn.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (!tierInDerTasche)
			{
				TriggerDialoge2();
			}
			if (skulptur.activeSelf)
			{
				TriggerDialoge();
			}
		}
	}

	private void AktivateSkulptur()
	{
		skulptur.SetActive(value: true);
		rendLampe.sharedMaterial = material[1];
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerDialoge2()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}
}
