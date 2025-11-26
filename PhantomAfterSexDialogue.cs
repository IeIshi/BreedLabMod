using UnityEngine;

public class PhantomAfterSexDialogue : Interactable
{
	private Animator animator;

	public Dialogue dialogueWhenStruggled;

	public Dialogue dialogueWhenLetHappen;

	public Dialogue firstDialogue;

	public bool interacted;

	public bool secondInteract;

	public bool struggled;

	public bool run;

	private void Start()
	{
		animator = GetComponent<Animator>();
		interacted = false;
	}

	public override void Interact()
	{
		base.Interact();
		if (!interacted)
		{
			TriggerFirstDialogue();
			GetComponent<BoxCollider>().isTrigger = true;
			animator.SetBool("isIdle", value: true);
			interacted = true;
		}
		if (secondInteract)
		{
			if (struggled)
			{
				TriggerDialogeStruggle();
				run = true;
			}
			else
			{
				TriggerDialogeSex();
				run = true;
			}
			GetComponent<GoToPoint>().enabled = true;
			animator.SetBool("isIdle", value: true);
			secondInteract = false;
		}
	}

	public void TriggerFirstDialogue()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(firstDialogue);
	}

	public void TriggerDialogeStruggle()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueWhenStruggled);
	}

	public void TriggerDialogeSex()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueWhenLetHappen);
	}

	private void Step()
	{
	}
}
