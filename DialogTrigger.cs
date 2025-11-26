using UnityEngine;

public class DialogTrigger : Interactable
{
	public Dialogue dialogue;

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
