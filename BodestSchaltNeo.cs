using System;
using UnityEngine;

public class BodestSchaltNeo : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool wolfOnPillar;

		public bool crowOnPillar;

		public bool snakeOnPillar;

		public bool activated;
	}

	private Inventory inventory;

	public Dialogue dialogue;

	public GameObject Wolf;

	public GameObject Snake;

	public GameObject Crow;

	public Item WolfItem;

	public Item SnakeItem;

	public Item CrowItem;

	public AudioSource statuePlaceSound;

	public AudioSource correctSound;

	public bool active;

	public bool right;

	public bool wrong;

	public bool wolfPillar;

	public bool snakePillar;

	public bool crowPillar;

	public bool activated;

	public GameObject Pillar1;

	public GameObject Pillar2;

	public GameObject TheSwarm;

	public Material[] material;

	private Renderer rendLampe1;

	private Renderer rendLampe2;

	private Renderer rendLampe3;

	public GameObject lamp1;

	public GameObject lamp2;

	public GameObject lamp3;

	private bool wolfOnPillar;

	private bool crowOnPillar;

	private bool snakeOnPillar;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.wolfOnPillar = wolfOnPillar;
		saveData.crowOnPillar = crowOnPillar;
		saveData.snakeOnPillar = snakeOnPillar;
		saveData.activated = activated;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		wolfOnPillar = saveData.wolfOnPillar;
		crowOnPillar = saveData.crowOnPillar;
		snakeOnPillar = saveData.snakeOnPillar;
		activated = saveData.activated;
	}

	private void Start()
	{
		if (TheSwarm.activeSelf)
		{
			TheSwarm.SetActive(value: false);
		}
		inventory = Inventory.instance;
		rendLampe1 = lamp1.GetComponent<Renderer>();
		rendLampe2 = lamp2.GetComponent<Renderer>();
		rendLampe3 = lamp3.GetComponent<Renderer>();
		if (wolfOnPillar)
		{
			Wolf.SetActive(value: true);
		}
		else
		{
			Wolf.SetActive(value: false);
		}
		if (snakeOnPillar)
		{
			Snake.SetActive(value: true);
		}
		else
		{
			Snake.SetActive(value: false);
		}
		if (crowOnPillar)
		{
			Crow.SetActive(value: true);
		}
		else
		{
			Crow.SetActive(value: false);
		}
		CheckStatue();
	}

	public override void Interact()
	{
		base.Interact();
		if (Wolf.activeSelf)
		{
			if (Inventory.instance.items.Count < Inventory.instance.space)
			{
				inventory.Add(WolfItem);
				right = false;
				wrong = false;
				active = false;
				Wolf.SetActive(value: false);
				wolfOnPillar = false;
				for (int i = 0; i < inventory.items.Count; i++)
				{
					if (inventory.items[i].name == "Snake")
					{
						inventory.items[i].RemoveFromInventory();
						Snake.SetActive(value: true);
						snakeOnPillar = true;
						wolfOnPillar = false;
						crowOnPillar = false;
						statuePlaceSound.Play();
						CheckStatue();
						break;
					}
					if (inventory.items[i].name == "Crow")
					{
						inventory.items[i].RemoveFromInventory();
						Crow.SetActive(value: true);
						snakeOnPillar = false;
						wolfOnPillar = false;
						crowOnPillar = true;
						statuePlaceSound.Play();
						CheckStatue();
						break;
					}
				}
			}
			else
			{
				TriggerDialoge();
			}
			return;
		}
		if (Snake.activeSelf)
		{
			if (Inventory.instance.items.Count < Inventory.instance.space)
			{
				inventory.Add(SnakeItem);
				right = false;
				wrong = false;
				active = false;
				Snake.SetActive(value: false);
				for (int j = 0; j < inventory.items.Count; j++)
				{
					if (inventory.items[j].name == "Crow")
					{
						inventory.items[j].RemoveFromInventory();
						Crow.SetActive(value: true);
						statuePlaceSound.Play();
						snakeOnPillar = false;
						wolfOnPillar = false;
						crowOnPillar = true;
						CheckStatue();
						break;
					}
					if (inventory.items[j].name == "Wolf")
					{
						inventory.items[j].RemoveFromInventory();
						Wolf.SetActive(value: true);
						statuePlaceSound.Play();
						snakeOnPillar = false;
						wolfOnPillar = true;
						crowOnPillar = false;
						CheckStatue();
						break;
					}
				}
			}
			else
			{
				TriggerDialoge();
			}
			return;
		}
		if (Crow.activeSelf)
		{
			if (Inventory.instance.items.Count < Inventory.instance.space)
			{
				inventory.Add(CrowItem);
				right = false;
				wrong = false;
				active = false;
				crowOnPillar = false;
				Crow.SetActive(value: false);
				for (int k = 0; k < inventory.items.Count; k++)
				{
					if (inventory.items[k].name == "Wolf")
					{
						inventory.items[k].RemoveFromInventory();
						Wolf.SetActive(value: true);
						statuePlaceSound.Play();
						snakeOnPillar = false;
						wolfOnPillar = true;
						crowOnPillar = false;
						CheckStatue();
						break;
					}
					if (inventory.items[k].name == "Snake")
					{
						inventory.items[k].RemoveFromInventory();
						Snake.SetActive(value: true);
						statuePlaceSound.Play();
						snakeOnPillar = true;
						wolfOnPillar = false;
						crowOnPillar = false;
						CheckStatue();
						break;
					}
				}
			}
			else
			{
				TriggerDialoge();
			}
			return;
		}
		for (int l = 0; l < inventory.items.Count; l++)
		{
			if (inventory.items[l].name == "Wolf")
			{
				inventory.items[l].RemoveFromInventory();
				Wolf.SetActive(value: true);
				statuePlaceSound.Play();
				wolfOnPillar = true;
				CheckStatue();
				break;
			}
			if (inventory.items[l].name == "Snake")
			{
				inventory.items[l].RemoveFromInventory();
				Snake.SetActive(value: true);
				statuePlaceSound.Play();
				snakeOnPillar = true;
				CheckStatue();
				break;
			}
			if (inventory.items[l].name == "Crow")
			{
				inventory.items[l].RemoveFromInventory();
				Crow.SetActive(value: true);
				statuePlaceSound.Play();
				crowOnPillar = true;
				CheckStatue();
				break;
			}
		}
	}

	private void CheckStatue()
	{
		if (wolfPillar)
		{
			if (wolfOnPillar)
			{
				right = true;
				wrong = false;
			}
			else
			{
				wrong = true;
				right = false;
			}
		}
		if (snakePillar)
		{
			if (snakeOnPillar)
			{
				right = true;
				wrong = false;
			}
			else
			{
				wrong = true;
				right = false;
			}
		}
		if (crowPillar)
		{
			if (crowOnPillar)
			{
				right = true;
				wrong = false;
			}
			else
			{
				wrong = true;
				right = false;
			}
		}
		if (!Wolf.activeSelf && !Snake.activeSelf && !Crow.activeSelf)
		{
			right = false;
			wrong = false;
			active = false;
		}
		if (Wolf.activeSelf || Snake.activeSelf || Crow.activeSelf)
		{
			active = true;
		}
		CheckOtherPillars();
	}

	private void CheckOtherPillars()
	{
		if (Pillar1.GetComponent<BodestSchaltNeo>().active && Pillar2.GetComponent<BodestSchaltNeo>().active && active)
		{
			if (Pillar1.GetComponent<BodestSchaltNeo>().wrong || Pillar1.GetComponent<BodestSchaltNeo>().wrong || wrong)
			{
				Debug.Log("FUCKED UP, ENJOY THE SEGGS!");
				TheSwarm.SetActive(value: true);
			}
			if (Pillar1.GetComponent<BodestSchaltNeo>().right && Pillar2.GetComponent<BodestSchaltNeo>().right && right)
			{
				Debug.Log("CORRECT!, DOOR OPEND");
				correctSound.Play();
				activated = true;
				Pillar1.layer = 0;
				Pillar2.layer = 0;
				base.gameObject.layer = 0;
				rendLampe1.sharedMaterial = material[1];
				rendLampe2.sharedMaterial = material[1];
				rendLampe3.sharedMaterial = material[1];
			}
			else
			{
				rendLampe1.sharedMaterial = material[0];
				rendLampe2.sharedMaterial = material[0];
				rendLampe3.sharedMaterial = material[0];
			}
		}
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
