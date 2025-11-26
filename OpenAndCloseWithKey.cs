using System;
using UnityEngine;

public class OpenAndCloseWithKey : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool open;

		public bool keyUsed;
	}

	private Animator animator;

	public AudioSource openDoor;

	public AudioSource closeDoor;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	private bool keyUsed;

	private Inventory inventory;

	public Dialogue dialogue_open;

	public Dialogue dialogue_closed;

	public bool doorAKey;

	public bool doorBKey;

	public bool officeKey;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.open = open;
		saveData.keyUsed = keyUsed;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		open = saveData.open;
		keyUsed = saveData.keyUsed;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		if (animHolder != null)
		{
			animator = animHolder.GetComponent<Animator>();
		}
		else
		{
			animator = GetComponent<Animator>();
		}
		if (open)
		{
			animator.SetBool("isOpen", value: true);
		}
		else
		{
			animator.SetBool("isOpen", value: false);
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!keyUsed)
		{
			if (doorAKey && PlayerManager.Key)
			{
				keyUsed = true;
				TriggerDialogeOpen();
				animator.SetBool("isOpen", value: true);
				if (openDoor != null)
				{
					openDoor.Play();
				}
				open = true;
			}
			else if (doorBKey && PlayerManager.KeyB)
			{
				keyUsed = true;
				TriggerDialogeOpen();
				animator.SetBool("isOpen", value: true);
				if (openDoor != null)
				{
					openDoor.Play();
				}
				open = true;
			}
			else if (officeKey && PlayerManager.Office)
			{
				keyUsed = true;
				TriggerDialogeOpen();
				animator.SetBool("isOpen", value: true);
				if (openDoor != null)
				{
					openDoor.Play();
				}
				open = true;
			}
			else if (!keyUsed)
			{
				TriggerDialogeClosed();
			}
		}
		else if (!open)
		{
			animator.SetBool("isOpen", value: true);
			GetComponent<BoxCollider>().enabled = false;
			if (openDoor != null)
			{
				openDoor.Play();
			}
			open = true;
		}
	}

	public void TriggerDialogeOpen()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_open);
	}

	public void TriggerDialogeClosed()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue_closed);
	}
}
