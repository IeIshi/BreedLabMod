using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestPickup : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool opened;
	}

	public Item item;

	public AudioSource openSound;

	private Animator anim;

	public Dialogue dialogue;

	public bool opened;

	private GameObject showItemUI;

	private Image itemImage;

	private TextMeshProUGUI itemText;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.opened = opened;
		return saveData;
	}

	public void RestoreState(object state)
	{
		opened = ((SaveData)state).opened;
	}

	private void Start()
	{
		anim = GetComponent<Animator>();
		itemImage = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Image").GetComponent<Image>();
		itemText = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Text").GetComponent<TextMeshProUGUI>();
		showItemUI = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem");
		if (opened)
		{
			anim.SetBool("isOpen", value: true);
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (Inventory.instance.items.Count < Inventory.instance.space)
		{
			if (!opened)
			{
				PickUp();
				openSound.Play();
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
		if (Inventory.instance.Add(item))
		{
			anim.SetBool("isOpen", value: true);
			StartCoroutine(ShowPickedItem());
			opened = true;
			base.gameObject.layer = 0;
		}
	}

	private IEnumerator ShowPickedItem()
	{
		showItemUI.SetActive(value: true);
		itemImage.sprite = item.icon;
		itemText.text = item.header.ToString();
		showItemUI.GetComponent<Animator>().SetBool("startAnim", value: true);
		yield return new WaitForSeconds(1f);
		showItemUI.GetComponent<Animator>().SetBool("startAnim", value: false);
		showItemUI.SetActive(value: false);
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
