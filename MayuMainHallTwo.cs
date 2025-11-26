using System;
using UnityEngine;

public class MayuMainHallTwo : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool firstDialogueTriggered;

		public bool secondDialogueTriggered;
	}

	public Dialogue firstDialogue;

	public Dialogue secondDialogue;

	public Dialogue afterDialogue;

	private bool firstDialogueTriggered;

	private bool secondDialogueTriggered;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.firstDialogueTriggered = firstDialogueTriggered;
		saveData.secondDialogueTriggered = secondDialogueTriggered;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		firstDialogueTriggered = saveData.firstDialogueTriggered;
		secondDialogueTriggered = saveData.secondDialogueTriggered;
	}

	private void Start()
	{
	}

	public override void Interact()
	{
		base.Interact();
		if (!firstDialogueTriggered)
		{
			TriggerFirstDialoge();
			firstDialogueTriggered = true;
		}
		else if (!secondDialogueTriggered)
		{
			TriggerSecondDialoge();
			secondDialogueTriggered = true;
		}
		else
		{
			TriggerAfterDialogue();
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	public void TriggerFirstDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(firstDialogue);
	}

	public void TriggerSecondDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(secondDialogue);
	}

	public void TriggerAfterDialogue()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(afterDialogue);
	}
}
