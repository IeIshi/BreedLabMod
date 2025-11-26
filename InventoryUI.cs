using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public Transform itemsParent;

	public GameObject inventoryUI;

	public static bool inventoryIsOpen;

	private Inventory inventory;

	private InventorySlot[] slots;

	public AudioSource invopen;

	private bool openPlayed;

	public GameObject gm;

	public static bool heroineIsChased;

	private Image chasedImage;

	public GameObject lustyOb;

	public GameObject addicOb;

	public GameObject fertileOb;

	private Image lusty;

	private Image addictive;

	private Image fertile;

	public GameObject virginImage;

	private Image virginSprite;

	private void Start()
	{
		inventory = Inventory.instance;
		Inventory obj = inventory;
		obj.onItemChangedCallback = (Inventory.OnItemChanged)Delegate.Combine(obj.onItemChangedCallback, new Inventory.OnItemChanged(UpdateUI));
		chasedImage = GameObject.Find("chased").GetComponent<Image>();
		lusty = lustyOb.GetComponent<Image>();
		addictive = addicOb.GetComponent<Image>();
		fertile = fertileOb.GetComponent<Image>();
		lusty.enabled = false;
		addictive.enabled = false;
		fertile.enabled = false;
		if (virginImage != null)
		{
			virginSprite = virginImage.GetComponent<Image>();
		}
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		if (SceneManager.GetActiveScene().name == "Outdoor")
		{
			for (int i = 0; i < gm.GetComponent<Inventory>().items.Count; i++)
			{
				gm.GetComponent<Inventory>().items[i].RemoveFromInventory();
				slots[i].ClearSlot();
			}
		}
		if (gm.GetComponent<Inventory>().items.Count > 0)
		{
			Debug.Log("There are items");
			for (int j = 0; j < gm.GetComponent<Inventory>().items.Count; j++)
			{
				slots[j].AddItem(gm.GetComponent<Inventory>().items[j]);
			}
		}
		heroineIsChased = false;
		inventoryUI.SetActive(value: false);
	}

	private void LateUpdate()
	{
		if (Input.GetButtonDown("Inventory"))
		{
			if (CameraFollow.shootingMode || PlayerController.iFalled || heroineIsChased || SceneManager.GetActiveScene().name == "FinalLevel")
			{
				return;
			}
			inventoryUI.SetActive(!inventoryUI.activeSelf);
			Cursor.lockState = CursorLockMode.None;
		}
		if (inventoryUI.activeSelf)
		{
			if (PlayerController.iFalled)
			{
				inventoryIsOpen = false;
				openPlayed = false;
				TooltipSystem.Hide();
				CameraFollow.target = PlayerManager.instance.player.GetComponent<HeroineStats>().slugSexCamPos;
				inventoryUI.SetActive(value: false);
				return;
			}
			inventoryIsOpen = true;
			Cursor.visible = true;
			if (PlayerManager.IsVirgin)
			{
				if (virginSprite != null)
				{
					virginSprite.enabled = true;
				}
			}
			else if (virginSprite != null)
			{
				virginSprite.enabled = false;
			}
			if (inventoryIsOpen && !invopen.isPlaying && !openPlayed)
			{
				invopen.Play();
				openPlayed = true;
			}
			if (PlayerController.iFalled || heroineIsChased)
			{
				inventoryIsOpen = false;
				inventoryUI.SetActive(!inventoryUI.activeSelf);
			}
			if (HeroineStats.addictiveCum)
			{
				addictive.enabled = true;
			}
			else
			{
				addictive.enabled = false;
			}
			if (HeroineStats.fertileCum)
			{
				fertile.enabled = true;
			}
			else
			{
				fertile.enabled = false;
			}
			if (HeroineStats.lustyCum)
			{
				lusty.enabled = true;
			}
			else
			{
				lusty.enabled = false;
			}
		}
		else
		{
			inventoryIsOpen = false;
			openPlayed = false;
			TooltipSystem.Hide();
			if (!Pause.isPaused)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Confined;
			}
		}
		if (heroineIsChased)
		{
			chasedImage.enabled = true;
		}
		else
		{
			chasedImage.enabled = false;
		}
	}

	private void UpdateUI()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}
}
