using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool itemPickedUp;
	}

	public Item item;

	public Dialogue dialogue;

	public Dialogue noGunDialogue;

	private AudioSource bookFlip;

	private AudioSource bottleSound;

	private AudioSource ammoPickup;

	private AudioSource weaponPickup;

	[SerializeField]
	public bool itemPickedUp;

	public GameObject theItem;

	public bool ignoreFullInvenotry;

	public GameObject outSideItemSafer;

	private GameObject showItemUI;

	private Image itemImage;

	private TextMeshProUGUI itemText;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.itemPickedUp = itemPickedUp;
		return saveData;
	}

	public void RestoreState(object state)
	{
		itemPickedUp = ((SaveData)state).itemPickedUp;
	}

	private void Start()
	{
		bookFlip = GameObject.Find("BookFlip").GetComponent<AudioSource>();
		bottleSound = GameObject.Find("BottleSound").GetComponent<AudioSource>();
		ammoPickup = GameObject.Find("AmmoPickup").GetComponent<AudioSource>();
		weaponPickup = GameObject.Find("WeaponPickup").GetComponent<AudioSource>();
		itemImage = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Image").GetComponent<Image>();
		itemText = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Text").GetComponent<TextMeshProUGUI>();
		showItemUI = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem");
		if (itemPickedUp)
		{
			UnityEngine.Object.Destroy(theItem);
			base.gameObject.layer = 0;
			if (GetComponent<BoxCollider>() != null)
			{
				GetComponent<BoxCollider>().enabled = false;
			}
			GetComponent<ItemPickup>().enabled = true;
		}
		else
		{
			if (!(outSideItemSafer != null))
			{
				return;
			}
			if (item.name == "IDCardTwo" && OutsideItemSafer.idCardPicked)
			{
				UnityEngine.Object.Destroy(theItem);
				base.gameObject.layer = 0;
				if (GetComponent<BoxCollider>() != null)
				{
					GetComponent<BoxCollider>().enabled = false;
				}
				GetComponent<ItemPickup>().enabled = true;
			}
			if (item.name == "Ammo" && OutsideItemSafer.ammoPicked)
			{
				UnityEngine.Object.Destroy(theItem);
				base.gameObject.layer = 0;
				if (GetComponent<BoxCollider>() != null)
				{
					GetComponent<BoxCollider>().enabled = false;
				}
				GetComponent<ItemPickup>().enabled = true;
			}
			if (item.name == "StaminaPotion" && OutsideItemSafer.willpowerPicked)
			{
				UnityEngine.Object.Destroy(theItem);
				base.gameObject.layer = 0;
				if (GetComponent<BoxCollider>() != null)
				{
					GetComponent<BoxCollider>().enabled = false;
				}
				GetComponent<ItemPickup>().enabled = true;
			}
			if (item.name == "LovePotion" && OutsideItemSafer.lovePotionPicked)
			{
				UnityEngine.Object.Destroy(theItem);
				base.gameObject.layer = 0;
				if (GetComponent<BoxCollider>() != null)
				{
					GetComponent<BoxCollider>().enabled = false;
				}
				GetComponent<ItemPickup>().enabled = true;
			}
			if (item.name == "RedOrbContainer" && OutsideItemSafer.redConPicked)
			{
				UnityEngine.Object.Destroy(theItem);
				base.gameObject.layer = 0;
				if (GetComponent<BoxCollider>() != null)
				{
					GetComponent<BoxCollider>().enabled = false;
				}
				GetComponent<ItemPickup>().enabled = true;
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (item.name == "IDCard" || item.name == "IDCardTwo")
		{
			PickUpKey();
			bookFlip.Play();
			if (item.name == "IDCard")
			{
				if (!PlayerManager.id1)
				{
					PlayerManager.id1 = true;
					GameObject.Find("KeyBox").GetComponent<KeyBox>().id1.SetActive(value: true);
				}
				else
				{
					PlayerManager.id11 = true;
					GameObject.Find("KeyBox").GetComponent<KeyBox>().id11.SetActive(value: true);
				}
			}
			if (item.name == "IDCardTwo")
			{
				PlayerManager.id2 = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().id2.SetActive(value: true);
				if (outSideItemSafer != null)
				{
					OutsideItemSafer.idCardPicked = true;
				}
			}
		}
		else if (item.name == "KeyA" || item.name == "KeyB" || item.name == "SmallKey" || item.name == "OfficeKey")
		{
			weaponPickup.Play();
			if (item.name == "KeyA")
			{
				PickUpKey();
				PlayerManager.Key = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().Key.SetActive(value: true);
			}
			if (item.name == "KeyB")
			{
				PickUpKey();
				PlayerManager.KeyB = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().KeyB.SetActive(value: true);
			}
			if (item.name == "SmallKey")
			{
				PickUpKey();
				PlayerManager.smallKey = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().smallKey.SetActive(value: true);
			}
			if (item.name == "OfficeKey")
			{
				PickUpKey();
				PlayerManager.Office = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().Office.SetActive(value: true);
			}
		}
		else if (ignoreFullInvenotry)
		{
			PickUp();
			if (item.name == "StaminaPotion")
			{
				bottleSound.Play();
				if (outSideItemSafer != null)
				{
					OutsideItemSafer.willpowerPicked = true;
				}
			}
			else if (item.name == "LovePotion")
			{
				bottleSound.Play();
				if (outSideItemSafer != null)
				{
					OutsideItemSafer.lovePotionPicked = true;
				}
			}
		}
		else if (Inventory.instance.items.Count < Inventory.instance.space)
		{
			if (item.name == "Ammo" && outSideItemSafer != null)
			{
				OutsideItemSafer.ammoPicked = true;
			}
			if (item.name == "Ammo" || item.name == "GasContainer")
			{
				PickUp();
				ammoPickup.Play();
			}
			else if (item.name == "RedOrbContainer")
			{
				weaponPickup.Play();
				if (outSideItemSafer != null)
				{
					OutsideItemSafer.redConPicked = true;
				}
				PickUp();
			}
			else if (item.name == "Weapon")
			{
				weaponPickup.Play();
				PickUp();
			}
			else
			{
				if (!(item.name == "Wolf") && !(item.name == "Snake"))
				{
					_ = item.name == "Crow";
				}
				PickUp();
				weaponPickup.Play();
			}
		}
		else
		{
			TriggerDialoge();
		}
	}

	private void PickUp()
	{
		Debug.Log("Picking up " + item.name);
		if (item.id == 5565)
		{
			Inventory.energyDrinkCount++;
		}
		else if (item.id == 6969)
		{
			Inventory.lovePotionCount++;
		}
		else if (item.id == 101)
		{
			if (!(EquipmentManager.instance.currentEquipment[7] != null))
			{
				TriggerNoGunDialogue();
				return;
			}
			ammoPickup.Play();
			Gun.additionalAmmo++;
			PlayerManager.instance.player.GetComponent<Gun>().maxAmmo++;
			if (outSideItemSafer != null)
			{
				OutsideItemSafer.ammoPicked = true;
			}
		}
		else
		{
			Inventory.instance.Add(item);
		}
		itemPickedUp = true;
		StartCoroutine(ShowPickedItem());
		UnityEngine.Object.Destroy(theItem);
		base.gameObject.layer = 0;
		if (GetComponent<BoxCollider>() != null)
		{
			GetComponent<BoxCollider>().enabled = false;
		}
	}

	private void PickUpKey()
	{
		Debug.Log("Picking up " + item.name);
		itemPickedUp = true;
		StartCoroutine(ShowPickedItem());
		UnityEngine.Object.Destroy(theItem);
		base.gameObject.layer = 0;
		if (GetComponent<BoxCollider>() != null)
		{
			GetComponent<BoxCollider>().enabled = false;
		}
	}

	private IEnumerator ShowPickedItem()
	{
		showItemUI.SetActive(value: true);
		itemImage.sprite = item.icon;
		itemText.text = item.name.ToString();
		showItemUI.GetComponent<Animator>().SetBool("startAnim", value: true);
		yield return new WaitForSeconds(1f);
		showItemUI.GetComponent<Animator>().SetBool("startAnim", value: false);
		showItemUI.SetActive(value: false);
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerNoGunDialogue()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(noGunDialogue);
	}
}
