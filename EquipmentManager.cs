using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour, ISaveable
{
	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

	[Serializable]
	private struct SaveData
	{
		public int[] eqIdArray;

		public bool shoesOn;

		public bool skirtOn;

		public bool stockingsOn;

		public bool chestOn;

		public bool flashNeckOn;

		public bool pantiesOn;

		public float shoeDurability;

		public float skirtDurability;

		public float stockingsDurability;

		public float pantiesDurability;
	}

	public static EquipmentManager instance;

	public Equipment[] currentEquipment;

	private SkinnedMeshRenderer[] currentMeshes;

	public SkinnedMeshRenderer targetMesh;

	public OnEquipmentChanged onEquipmentChanged;

	private Inventory inventory;

	public AudioSource clothsound;

	public AudioSource clothRipSound;

	public static bool pantsuOn;

	public static bool shoesOn;

	public static bool skirtOn;

	public static bool stockingsOn;

	public static bool chestOn;

	public static bool nekomimiOn;

	public static bool flashNeckOn;

	public static bool pantiesOn;

	public static bool antiTentOn;

	public Equipment chest;

	public Equipment crotch;

	public Equipment hairRings;

	public Equipment topLongSleve;

	public Equipment shoesBasic;

	public Equipment skirtBasic;

	public Equipment tightsShoesBasic;

	public Equipment halsRingBasic;

	public Equipment neckFlash;

	public static bool heroineIsNaked;

	private bool unequipAllCalled;

	private float def = -1f;

	public static float shoeDurability;

	public static float skirtDurability;

	public static float stockingsDurability;

	public static float pantiesDurability;

	[SerializeField]
	public int[] eqIdArray;

	public static int[] eqSaveArray = new int[8];

	[SerializeField]
	public GameObject invFullText;

	[SerializeField]
	public GameObject damagedClothText;

	[SerializeField]
	public GameObject shoesDuration;

	[SerializeField]
	public GameObject skirtDuration;

	[SerializeField]
	public GameObject stockingsDuration;

	[SerializeField]
	public GameObject pantiesDuration;

	private GameObject flashlightBar;

	public GameObject DressDollPanties;

	public GameObject DressDollTop;

	public GameObject DressDollSkirt;

	public GameObject DressDollTights;

	public GameObject DressDollShoes;

	public GameObject DressDollHead;

	public GameObject DressDollNeck;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		int num = Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[num];
		currentMeshes = new SkinnedMeshRenderer[num];
		shoesDuration = GameObject.Find("shoesDuration");
		skirtDuration = GameObject.Find("skirtDuration");
		stockingsDuration = GameObject.Find("stockingsDuration");
		pantiesDuration = GameObject.Find("ManagerAndUI/UI/Canvas/Pantsu");
		if (!PlayerManager.loadedrly)
		{
			if (SceneManager.GetActiveScene().name == "Outdoor")
			{
				pantiesDurability = 1f;
				StartEquip(crotch);
				StartEquip(hairRings);
			}
			else
			{
				for (int i = 0; i < eqSaveArray.Length; i++)
				{
					if (eqSaveArray[i] == 0)
					{
						continue;
					}
					for (int j = 0; j < inventory.equipmentList.Count; j++)
					{
						if (eqSaveArray[i] == inventory.equipmentList[j].id)
						{
							StartEquip(inventory.equipmentList[j]);
						}
					}
				}
			}
			if (currentEquipment[2] == null)
			{
				skirtDurability = 1f;
			}
			if (currentEquipment[4] == null)
			{
				stockingsDurability = 1f;
			}
			if (currentEquipment[5] == null)
			{
				shoeDurability = 1f;
			}
			if (currentEquipment[3] == null)
			{
				pantiesDurability = 1f;
				heroineIsNaked = true;
			}
		}
		else
		{
			for (int k = 0; k < eqIdArray.Length; k++)
			{
				if (eqIdArray[k] == 0)
				{
					continue;
				}
				for (int l = 0; l < Inventory.instance.equipmentList.Count; l++)
				{
					if (eqIdArray[k] == Inventory.instance.equipmentList[l].id)
					{
						LoadEquip(Inventory.instance.equipmentList[l]);
					}
				}
			}
		}
		if (!shoesOn)
		{
			shoesDuration.SetActive(value: false);
		}
		else
		{
			HeroineStats.shoesDurSlider.fillAmount = shoeDurability;
		}
		if (!stockingsOn)
		{
			stockingsDuration.SetActive(value: false);
		}
		else
		{
			HeroineStats.stockingsDurSlider.fillAmount = stockingsDurability;
		}
		if (!skirtOn)
		{
			skirtDuration.SetActive(value: false);
		}
		else
		{
			HeroineStats.skirtDurSlider.fillAmount = skirtDurability;
		}
		if (!pantiesOn)
		{
			pantiesDuration.SetActive(value: false);
		}
		else
		{
			HeroineStats.PantiesCircle.fillAmount = pantiesDurability;
		}
		if (currentEquipment[3] == null)
		{
			pantiesDurability = 1f;
			heroineIsNaked = true;
		}
		DressDollCheck();
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.eqIdArray = eqIdArray;
		saveData.shoesOn = shoesOn;
		saveData.skirtOn = skirtOn;
		saveData.stockingsOn = stockingsOn;
		saveData.chestOn = chestOn;
		saveData.pantiesOn = pantiesOn;
		saveData.shoeDurability = shoeDurability;
		saveData.skirtDurability = skirtDurability;
		saveData.stockingsDurability = stockingsDurability;
		saveData.pantiesDurability = pantiesDurability;
		saveData.flashNeckOn = flashNeckOn;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		eqIdArray = saveData.eqIdArray;
		shoesOn = saveData.shoesOn;
		skirtOn = saveData.skirtOn;
		stockingsOn = saveData.stockingsOn;
		chestOn = saveData.chestOn;
		pantiesOn = saveData.pantiesOn;
		shoeDurability = saveData.shoeDurability;
		skirtDurability = saveData.skirtDurability;
		stockingsDurability = saveData.stockingsDurability;
		pantiesDurability = saveData.pantiesDurability;
		flashNeckOn = saveData.flashNeckOn;
	}

	private void DressDollCheck()
	{
		if (currentEquipment[0] != null)
		{
			DressDollHead.SetActive(value: true);
		}
		else
		{
			DressDollHead.SetActive(value: false);
		}
		if (currentEquipment[1] != null)
		{
			DressDollTop.SetActive(value: true);
		}
		else
		{
			DressDollTop.SetActive(value: false);
		}
		if (currentEquipment[2] != null)
		{
			DressDollSkirt.SetActive(value: true);
		}
		else
		{
			DressDollSkirt.SetActive(value: false);
		}
		if (currentEquipment[3] != null)
		{
			DressDollPanties.SetActive(value: true);
		}
		else
		{
			DressDollPanties.SetActive(value: false);
		}
		if (currentEquipment[4] != null)
		{
			DressDollTights.SetActive(value: true);
		}
		else
		{
			DressDollTights.SetActive(value: false);
		}
		if (currentEquipment[5] != null)
		{
			DressDollShoes.SetActive(value: true);
		}
		else
		{
			DressDollShoes.SetActive(value: false);
		}
		if (currentEquipment[6] != null)
		{
			DressDollNeck.SetActive(value: true);
		}
		else
		{
			DressDollNeck.SetActive(value: false);
		}
	}

	public void UndressPanties()
	{
		if (pantiesDurability < 1f)
		{
			StartCoroutine(showDamagedEqText());
			return;
		}
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(3);
		pantiesOn = false;
		heroineIsNaked = true;
		PlayerManager.instance.player.GetComponent<Animator>().SetBool("isNaked", value: true);
		pantiesDuration.SetActive(value: false);
		DressDollPanties.SetActive(value: false);
	}

	public void UndressStockings()
	{
		if (stockingsDurability < 1f)
		{
			StartCoroutine(showDamagedEqText());
			return;
		}
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(4);
		stockingsOn = false;
		stockingsDuration.SetActive(value: false);
		DressDollTights.SetActive(value: false);
	}

	public void UndressSkirt()
	{
		if (skirtDurability < 1f)
		{
			StartCoroutine(showDamagedEqText());
			return;
		}
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(2);
		skirtOn = false;
		skirtDuration.SetActive(value: false);
		DressDollSkirt.SetActive(value: false);
	}

	public void UndressShoes()
	{
		if (shoeDurability < 1f)
		{
			StartCoroutine(showDamagedEqText());
			return;
		}
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(5);
		shoesOn = false;
		shoesDuration.SetActive(value: false);
		DressDollShoes.SetActive(value: false);
	}

	public void UndressTop()
	{
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(1);
		chestOn = false;
		DressDollTop.SetActive(value: false);
	}

	public void UndressHead()
	{
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(0);
		DressDollHead.SetActive(value: false);
	}

	public void UndressNeck()
	{
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
			return;
		}
		UnequipCurrent(6);
		DressDollNeck.SetActive(value: false);
	}

	public void Equip(Equipment newItem)
	{
		int equipSlot = (int)newItem.equipSlot;
		Equipment oldItem = Unequip(equipSlot);
		if (onEquipmentChanged != null)
		{
			onEquipmentChanged(newItem, oldItem);
		}
		currentEquipment[equipSlot] = newItem;
		SkinnedMeshRenderer skinnedMeshRenderer = UnityEngine.Object.Instantiate(newItem.mesh);
		skinnedMeshRenderer.transform.parent = targetMesh.transform;
		skinnedMeshRenderer.bones = targetMesh.bones;
		skinnedMeshRenderer.rootBone = targetMesh.rootBone;
		currentMeshes[equipSlot] = skinnedMeshRenderer;
		eqIdArray[(int)newItem.equipSlot] = newItem.id;
		eqSaveArray[(int)newItem.equipSlot] = newItem.id;
		clothsound.Play();
		if (equipSlot == 5)
		{
			shoesOn = true;
			shoesDuration.SetActive(value: true);
			shoeDurability = 1f;
			HeroineStats.shoesDurSlider.fillAmount = shoeDurability;
		}
		if (equipSlot == 2)
		{
			skirtOn = true;
			skirtDuration.SetActive(value: true);
			skirtDurability = 1f;
			HeroineStats.skirtDurSlider.fillAmount = skirtDurability;
		}
		if (equipSlot == 4)
		{
			if (newItem.id == 451196)
			{
				return;
			}
			stockingsOn = true;
			stockingsDuration.SetActive(value: true);
			stockingsDurability = 1f;
			HeroineStats.stockingsDurSlider.fillAmount = stockingsDurability;
		}
		if (equipSlot == 1)
		{
			chestOn = true;
		}
		if (equipSlot == 3)
		{
			pantiesOn = true;
			if (newItem.id == 454874)
			{
				pantiesDuration.SetActive(value: false);
				heroineIsNaked = true;
				DressDollCheck();
				return;
			}
			pantiesDuration.SetActive(value: true);
			heroineIsNaked = false;
			pantiesDurability = 1f;
			HeroineStats.PantiesCircle.fillAmount = pantiesDurability;
		}
		if (newItem.name == "Nekomimi")
		{
			nekomimiOn = true;
		}
		if (newItem.name == "AntiTent")
		{
			antiTentOn = true;
		}
		if (newItem.name.Equals("NeckFlash"))
		{
			flashNeckOn = true;
		}
		DressDollCheck();
		if (!heroineIsNaked)
		{
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("isNaked", value: false);
		}
	}

	public void LoadEquip(Equipment newItem)
	{
		int equipSlot = (int)newItem.equipSlot;
		Equipment oldItem = Unequip(equipSlot);
		if (onEquipmentChanged != null)
		{
			onEquipmentChanged(newItem, oldItem);
		}
		currentEquipment[equipSlot] = newItem;
		SkinnedMeshRenderer skinnedMeshRenderer = UnityEngine.Object.Instantiate(newItem.mesh);
		skinnedMeshRenderer.transform.parent = targetMesh.transform;
		skinnedMeshRenderer.bones = targetMesh.bones;
		skinnedMeshRenderer.rootBone = targetMesh.rootBone;
		currentMeshes[equipSlot] = skinnedMeshRenderer;
		eqIdArray[(int)newItem.equipSlot] = newItem.id;
		eqSaveArray[(int)newItem.equipSlot] = newItem.id;
		clothsound.Play();
		if (equipSlot == 5)
		{
			shoesOn = true;
			shoesDuration.SetActive(value: true);
		}
		if (equipSlot == 2)
		{
			skirtOn = true;
			skirtDuration.SetActive(value: true);
		}
		if (equipSlot == 4)
		{
			if (newItem.id == 451196)
			{
				return;
			}
			stockingsOn = true;
			stockingsDuration.SetActive(value: true);
		}
		if (equipSlot == 1)
		{
			chestOn = true;
		}
		if (equipSlot == 3)
		{
			pantiesOn = true;
			if (newItem.id == 454874)
			{
				pantiesDuration.SetActive(value: false);
				heroineIsNaked = true;
				return;
			}
			heroineIsNaked = false;
			pantiesDuration.SetActive(value: true);
		}
		if (newItem.name == "Nekomimi")
		{
			nekomimiOn = true;
		}
		if (newItem.name == "AntiTent")
		{
			antiTentOn = true;
		}
	}

	public void StartEquip(Equipment newItem)
	{
		int equipSlot = (int)newItem.equipSlot;
		Equipment oldItem = Unequip(equipSlot);
		if (onEquipmentChanged != null)
		{
			onEquipmentChanged(newItem, oldItem);
		}
		currentEquipment[equipSlot] = newItem;
		SkinnedMeshRenderer skinnedMeshRenderer = UnityEngine.Object.Instantiate(newItem.mesh);
		skinnedMeshRenderer.transform.parent = targetMesh.transform;
		eqIdArray[(int)newItem.equipSlot] = newItem.id;
		if (SceneManager.GetActiveScene().name == "Outdoor")
		{
			eqSaveArray[(int)newItem.equipSlot] = newItem.id;
		}
		skinnedMeshRenderer.bones = targetMesh.bones;
		skinnedMeshRenderer.rootBone = targetMesh.rootBone;
		currentMeshes[equipSlot] = skinnedMeshRenderer;
		if (equipSlot == 5)
		{
			shoesOn = true;
			shoesDuration.SetActive(value: true);
		}
		if (equipSlot == 2)
		{
			skirtOn = true;
			skirtDuration.SetActive(value: true);
		}
		if (equipSlot == 4)
		{
			if (newItem.id == 451196)
			{
				return;
			}
			stockingsOn = true;
			stockingsDuration.SetActive(value: true);
		}
		if (equipSlot == 1)
		{
			chestOn = true;
		}
		if (equipSlot == 3)
		{
			pantiesOn = true;
			if (newItem.id == 454874)
			{
				pantiesDuration.SetActive(value: false);
				heroineIsNaked = true;
			}
			else
			{
				heroineIsNaked = false;
				pantiesDuration.SetActive(value: true);
			}
		}
	}

	public void RipOff(int slotIndex)
	{
		if (def == -1f)
		{
			def = currentEquipment[slotIndex].defence;
		}
		if (def > 0f)
		{
			def -= 1f;
			return;
		}
		if (def == 0f)
		{
			clothRipSound.Play();
			UnityEngine.Object.Destroy(currentMeshes[slotIndex].gameObject);
			Equipment equipment = currentEquipment[slotIndex];
			if (equipment.name == "Nekomimi")
			{
				nekomimiOn = false;
			}
			if (equipment.name == "AntiTent")
			{
				antiTentOn = false;
			}
			eqIdArray[(int)equipment.equipSlot] = 0;
			eqSaveArray[(int)equipment.equipSlot] = 0;
			onEquipmentChanged(null, currentEquipment[slotIndex]);
			currentEquipment[slotIndex] = null;
			if (slotIndex == 5)
			{
				shoesOn = false;
				shoesDuration.SetActive(value: false);
			}
			if (slotIndex == 2)
			{
				skirtOn = false;
				skirtDuration.SetActive(value: false);
			}
			if (slotIndex == 4)
			{
				stockingsOn = false;
				stockingsDuration.SetActive(value: false);
			}
			if (slotIndex == 1)
			{
				if (equipment.id == 3648532)
				{
					inventory.Add(equipment);
					return;
				}
				chestOn = false;
			}
			def = -1f;
		}
		DressDollCheck();
	}

	public void RipPantsu()
	{
		if (currentEquipment[3].id != 454874)
		{
			heroineIsNaked = true;
			clothRipSound.Play();
			UnityEngine.Object.Destroy(currentMeshes[3].gameObject);
			Equipment equipment = currentEquipment[3];
			pantiesOn = false;
			eqIdArray[(int)equipment.equipSlot] = 0;
			eqSaveArray[(int)equipment.equipSlot] = 0;
			onEquipmentChanged(null, currentEquipment[3]);
			currentEquipment[3] = null;
			pantiesDuration.SetActive(value: false);
			pantiesDurability = 0f;
			DressDollCheck();
		}
	}

	public Equipment UnequipCurrent(int slotIndex)
	{
		if (currentEquipment[slotIndex] != null)
		{
			if (currentMeshes[slotIndex] != null)
			{
				UnityEngine.Object.Destroy(currentMeshes[slotIndex].gameObject);
			}
			Equipment equipment = currentEquipment[slotIndex];
			inventory.Add(equipment);
			eqIdArray[(int)equipment.equipSlot] = 0;
			eqSaveArray[(int)equipment.equipSlot] = 0;
			if (equipment.name.Equals("Nekomimi"))
			{
				nekomimiOn = false;
			}
			if (equipment.name == "AntiTent")
			{
				antiTentOn = false;
			}
			if (equipment.name.Equals("NeckFlash"))
			{
				flashNeckOn = false;
			}
			onEquipmentChanged(null, currentEquipment[slotIndex]);
			currentEquipment[slotIndex] = null;
			if (unequipAllCalled && onEquipmentChanged != null)
			{
				onEquipmentChanged(null, equipment);
			}
			return equipment;
		}
		DressDollCheck();
		return null;
	}

	public Equipment Unequip(int slotIndex)
	{
		if (currentEquipment[slotIndex] != null)
		{
			if (currentMeshes[slotIndex] != null)
			{
				UnityEngine.Object.Destroy(currentMeshes[slotIndex].gameObject);
			}
			Equipment equipment = currentEquipment[slotIndex];
			switch (slotIndex)
			{
			case 2:
				if (skirtDurability == 1f)
				{
					inventory.Add(equipment);
				}
				else
				{
					clothRipSound.Play();
				}
				break;
			case 3:
				if (pantiesDurability == 1f)
				{
					inventory.Add(equipment);
				}
				else
				{
					clothRipSound.Play();
				}
				break;
			case 4:
				if (stockingsDurability == 1f)
				{
					inventory.Add(equipment);
				}
				else
				{
					clothRipSound.Play();
				}
				break;
			case 5:
				if (shoeDurability == 1f)
				{
					inventory.Add(equipment);
				}
				else
				{
					clothRipSound.Play();
				}
				break;
			default:
				inventory.Add(equipment);
				break;
			}
			eqIdArray[(int)equipment.equipSlot] = 0;
			eqSaveArray[(int)equipment.equipSlot] = 0;
			if (equipment.name.Equals("Nekomimi"))
			{
				nekomimiOn = false;
			}
			if (equipment.name == "AntiTent")
			{
				antiTentOn = false;
			}
			if (equipment.name.Equals("NeckFlash"))
			{
				flashNeckOn = false;
			}
			currentEquipment[slotIndex] = null;
			if (unequipAllCalled && onEquipmentChanged != null)
			{
				onEquipmentChanged(null, equipment);
			}
			return equipment;
		}
		DressDollCheck();
		return null;
	}

	public Equipment UnequipByU(int slotIndex)
	{
		if (currentEquipment[slotIndex] != null)
		{
			if (currentMeshes[slotIndex] != null)
			{
				UnityEngine.Object.Destroy(currentMeshes[slotIndex].gameObject);
			}
			Equipment equipment = currentEquipment[slotIndex];
			inventory.Add(equipment);
			eqIdArray[(int)equipment.equipSlot] = 0;
			eqSaveArray[(int)equipment.equipSlot] = 0;
			if (equipment.name.Equals("Nekomimi"))
			{
				nekomimiOn = false;
			}
			if (equipment.name == "AntiTent")
			{
				antiTentOn = false;
			}
			if (equipment.name.Equals("NeckFlash"))
			{
				flashNeckOn = false;
			}
			onEquipmentChanged(null, currentEquipment[slotIndex]);
			currentEquipment[slotIndex] = null;
			if (unequipAllCalled && onEquipmentChanged != null)
			{
				onEquipmentChanged(null, equipment);
			}
			return equipment;
		}
		DressDollCheck();
		return null;
	}

	public void UnequipAll()
	{
		for (int i = 0; i < currentEquipment.Length; i++)
		{
			Unequip(i);
		}
	}

	private IEnumerator showInventoryFullText()
	{
		invFullText = GameObject.Find("ManagerAndUI/UI/Canvas/Full");
		invFullText.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		invFullText.SetActive(value: false);
	}

	private IEnumerator showDamagedEqText()
	{
		damagedClothText = GameObject.Find("ManagerAndUI/UI/Canvas/DamagedEq");
		damagedClothText.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		damagedClothText.SetActive(value: false);
	}

	private void Undress()
	{
		if (!Input.GetKeyDown(KeyCode.U))
		{
			return;
		}
		Debug.Log("U PRESSED");
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
		}
		else
		{
			if (PlayerController.iFalled)
			{
				return;
			}
			unequipAllCalled = true;
			for (int i = 0; i < currentEquipment.Length; i++)
			{
				if (currentEquipment[i] != null && i > 0 && i < 6)
				{
					clothRipSound.Play();
					UnequipByU(i);
					return;
				}
			}
			unequipAllCalled = false;
		}
	}

	private void UndressLewdPanties()
	{
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
		}
		else
		{
			UnequipByU(3);
		}
		DressDollCheck();
	}

	private void UndressCloth()
	{
		if (!Input.GetKeyDown(KeyCode.U))
		{
			return;
		}
		if (Inventory.instance.items.Count >= Inventory.instance.space)
		{
			StartCoroutine(showInventoryFullText());
		}
		else
		{
			if (PlayerController.iFalled)
			{
				return;
			}
			if (currentEquipment[4] != null && currentEquipment[4].id == 451196)
			{
				if (Inventory.instance.items.Count >= Inventory.instance.space)
				{
					StartCoroutine(showInventoryFullText());
				}
				else
				{
					UnequipByU(4);
				}
			}
			else if (currentEquipment[1] != null && currentEquipment[1].id == 3648532)
			{
				if (Inventory.instance.items.Count >= Inventory.instance.space)
				{
					StartCoroutine(showInventoryFullText());
				}
				else
				{
					UnequipByU(1);
				}
			}
			else if (currentEquipment[3] != null)
			{
				if (currentEquipment[3].id == 454874)
				{
					if (Inventory.instance.items.Count >= Inventory.instance.space)
					{
						StartCoroutine(showInventoryFullText());
					}
					else
					{
						UndressLewdPanties();
					}
				}
				else
				{
					RipPantsu();
				}
			}
			else
			{
				DressDollCheck();
			}
		}
	}
}
