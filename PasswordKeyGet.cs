using System;
using UnityEngine;

public class PasswordKeyGet : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool opened;
	}

	public Dialogue dialogueOnKey;

	public Dialogue dialogueOnLock;

	public Dialogue dialogueOnOpenedLock;

	public GameObject Mayu;

	public bool isALock;

	public bool isAKey;

	public bool chillArea;

	public bool opened;

	public bool hasALamp;

	public GameObject lamp;

	private bool interactedKey;

	private bool interactedLock;

	private GameObject lp;

	public AudioSource openSound;

	public bool SABPad;

	public GameObject weapon;

	public bool terminal;

	private void Start()
	{
		if (PlayerManager.SAB)
		{
			if (!SABPad && !terminal)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!terminal)
			{
				UnityEngine.Object.Destroy(weapon);
			}
		}
		else if (SABPad && !terminal)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (opened)
		{
			lamp.GetComponent<LampChanger>().rend.sharedMaterial = lamp.GetComponent<LampChanger>().material[1];
			base.gameObject.layer = 0;
		}
	}

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

	public override void Interact()
	{
		base.Interact();
		if (isAKey && !interactedKey)
		{
			getKey();
			TriggerDialoge();
			base.gameObject.layer = 0;
			interactedKey = true;
		}
		if (isALock)
		{
			checkLock();
			TriggerDialoge();
			if (hasALamp && opened)
			{
				openSound.Play();
				base.gameObject.layer = 0;
				lamp.GetComponent<LampChanger>().rend.sharedMaterial = lamp.GetComponent<LampChanger>().material[1];
			}
		}
	}

	private void getKey()
	{
		if (chillArea)
		{
			PlayerManager.KeyDoorChillArea = true;
			Mayu.SetActive(value: true);
			opened = true;
		}
	}

	private void checkLock()
	{
		if (chillArea && PlayerManager.KeyDoorChillArea)
		{
			opened = true;
		}
	}

	public void TriggerDialoge()
	{
		if (isAKey)
		{
			UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueOnKey);
		}
		if (isALock && !opened)
		{
			UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueOnLock);
		}
		if (isALock && opened)
		{
			UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueOnOpenedLock);
		}
	}
}
