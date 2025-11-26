using System;
using System.Collections;
using UnityEngine;

public class FreezerLock : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool open;
	}

	private Animator animator;

	public GameObject animHolder;

	private Inventory inventory;

	public bool open;

	public GameObject GasContainer;

	public Dialogue dialogue_open;

	public AudioSource doorOpen;

	public GameObject outSideItemSafer;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.open = open;
		return saveData;
	}

	public void RestoreState(object state)
	{
		open = ((SaveData)state).open;
	}

	private void Start()
	{
		animator = animHolder.GetComponent<Animator>();
		inventory = Inventory.instance;
		GasContainer.layer = 0;
		if (open)
		{
			animator.SetBool("isOpen", value: true);
			GasContainer.layer = 9;
			base.gameObject.layer = 0;
		}
		else if (outSideItemSafer != null && OutsideItemSafer.lockOpen)
		{
			animator.SetBool("isOpen", value: true);
			GasContainer.layer = 9;
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (PlayerManager.smallKey)
		{
			PlayerManager.smallKey = false;
			GameObject.Find("KeyBox").GetComponent<KeyBox>().smallKey.SetActive(value: false);
			animator.SetBool("isOpen", value: true);
			doorOpen.Play();
			animHolder.GetComponent<BoxCollider>().isTrigger = true;
			StartCoroutine(makeTriggerAway());
			GetComponent<BoxCollider>().isTrigger = true;
			GasContainer.layer = 9;
			base.gameObject.layer = 0;
			open = true;
			if (outSideItemSafer != null)
			{
				OutsideItemSafer.lockOpen = true;
			}
		}
		else if (!open)
		{
			TriggerDialogeOpen();
		}
	}

	private IEnumerator makeTriggerAway()
	{
		yield return new WaitForSeconds(1f);
		animHolder.GetComponent<BoxCollider>().isTrigger = false;
	}

	public void TriggerDialogeOpen()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_open);
	}
}
