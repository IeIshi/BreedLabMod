using System;
using UnityEngine;

public class OpenSecretWall : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool open;
	}

	private Animator animator_shelf;

	private Animator animator_wall;

	private bool open;

	public GameObject animHolder_shelf;

	public GameObject animHolder_wall;

	public Dialogue dialogue;

	public Dialogue whenNoKey;

	public GameObject schalter;

	public bool haveKey;

	private Inventory inventory;

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
		inventory = Inventory.instance;
		animator_shelf = animHolder_shelf.GetComponent<Animator>();
		animator_wall = animHolder_wall.GetComponent<Animator>();
		if (open)
		{
			animator_shelf.SetBool("isOpen", value: true);
			animator_wall.SetBool("isOpen", value: true);
			schalter.SetActive(value: false);
			GetComponent<OpenSecretWall>().enabled = false;
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (PlayerManager.id1)
		{
			haveKey = true;
			PlayerManager.id1 = false;
			GameObject.Find("KeyBox").GetComponent<KeyBox>().id1.SetActive(value: false);
			if (!open)
			{
				animator_shelf.SetBool("isOpen", value: true);
				animator_wall.SetBool("isOpen", value: true);
				schalter.SetActive(value: false);
				open = true;
				GetComponent<OpenSecretWall>().enabled = false;
				base.gameObject.layer = 0;
			}
		}
		else
		{
			TriggerDialogeNoKey();
		}
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerDialogeNoKey()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(whenNoKey);
	}
}
