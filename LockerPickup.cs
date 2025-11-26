using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerPickup : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool opened;

		public bool empty;
	}

	public Item item;

	public AudioSource openSound;

	private Animator anim;

	public Dialogue dialogue;

	public Dialogue dialogueEmpty;

	private GameObject showItemUI;

	private Image itemImage;

	private TextMeshProUGUI itemText;

	[SerializeField]
	private bool opened;

	[SerializeField]
	private bool empty;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.opened = opened;
		saveData.empty = empty;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		opened = saveData.opened;
		empty = saveData.empty;
	}

	private void Start()
	{
		anim = GetComponent<Animator>();
		itemImage = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Image").GetComponent<Image>();
		itemText = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem/Text").GetComponent<TextMeshProUGUI>();
		showItemUI = GameObject.Find("ManagerAndUI/UI/Canvas/ShowItem");
		if (empty)
		{
			item = null;
		}
		if (opened)
		{
			anim.SetBool("isOpen", value: true);
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (item == null)
		{
			if (!opened)
			{
				anim.SetBool("isOpen", value: true);
				TriggerDialogeEmpty();
				empty = true;
				base.gameObject.layer = 0;
				opened = true;
			}
		}
		else if (Inventory.instance.items.Count < Inventory.instance.space)
		{
			if (!opened)
			{
				PickUp();
				openSound.Play();
				base.gameObject.layer = 0;
				empty = true;
			}
		}
		else
		{
			TriggerDialoge();
		}
	}

	private void PickUp()
	{
		Inventory.instance.Add(item);
		if (true)
		{
			anim.SetBool("isOpen", value: true);
			StartCoroutine(ShowPickedItem());
			opened = true;
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

	public void TriggerDialogeEmpty()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueEmpty);
	}
}
