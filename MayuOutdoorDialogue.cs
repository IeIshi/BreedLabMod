using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayuOutdoorDialogue : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool d1Triggered;

		public bool d2Triggered;

		public bool mayuTalkedOutdoor;
	}

	public Dialogue Dialogue1;

	public Dialogue Dialogue2;

	public Dialogue Dialogue3;

	public Dialogue AltDialogue1;

	private bool d1Triggered;

	private bool d2Triggered;

	public static bool mayuTalkedOutdoor;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name != "Outdoor")
		{
			if (PlayerManager.KeyDoorChillArea && !EnergyEngine.engineIsOn)
			{
				base.gameObject.SetActive(value: true);
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.d1Triggered = d1Triggered;
		saveData.d2Triggered = d2Triggered;
		saveData.mayuTalkedOutdoor = mayuTalkedOutdoor;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		d1Triggered = saveData.d1Triggered;
		d2Triggered = saveData.d2Triggered;
		mayuTalkedOutdoor = saveData.mayuTalkedOutdoor;
	}

	public override void Interact()
	{
		base.Interact();
		if (SceneManager.GetActiveScene().name != "Outdoor")
		{
			if (!d1Triggered)
			{
				if (mayuTalkedOutdoor)
				{
					TriggerDialogue1();
					d1Triggered = true;
				}
				else
				{
					TriggerAltDialogue1();
					d1Triggered = true;
				}
			}
			else if (d1Triggered && !d2Triggered)
			{
				TriggerDialogue2();
				d2Triggered = true;
			}
			else if (d2Triggered)
			{
				TriggerDialogue3();
			}
		}
		else if (!d1Triggered)
		{
			TriggerDialogue1();
			d1Triggered = true;
			mayuTalkedOutdoor = true;
		}
		else if (d1Triggered && !d2Triggered)
		{
			TriggerDialogue2();
			d2Triggered = true;
		}
		else if (d2Triggered)
		{
			TriggerDialogue3();
		}
	}

	public void TriggerDialogue1()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(Dialogue1);
	}

	public void TriggerDialogue2()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(Dialogue2);
	}

	public void TriggerDialogue3()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(Dialogue3);
	}

	public void TriggerAltDialogue1()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(AltDialogue1);
	}
}
