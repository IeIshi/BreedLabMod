using UnityEngine;

public class ScDialogues : Interactable
{
	private Animator animator;

	public Dialogue firstDialogue;

	public Dialogue virginDialogue;

	public Dialogue nonVirginDialogue;

	public Dialogue afterDialogue;

	public bool interacted;

	public GameObject Heroine;

	public GameObject blockingBoxes;

	public GameObject unblockingBoxes;

	public GameObject doorBackInteractable;

	public GameObject blockingBlob;

	private bool firstDialogueTriggered;

	private bool secondDialogueTriggered;

	public bool nonVirginDialogueTriggered;

	private void Start()
	{
		animator = GetComponent<Animator>();
		interacted = false;
	}

	public override void Interact()
	{
		base.Interact();
		animator.SetBool("lookAt", value: true);
		if (!firstDialogueTriggered)
		{
			TriggerFirstDialoge();
			firstDialogueTriggered = true;
		}
		else if (firstDialogueTriggered && !secondDialogueTriggered)
		{
			if (PlayerManager.IsVirgin)
			{
				TriggerVirgingDialoge();
				PlayerManager.ScSexAfterMath = true;
				blockingBoxes.SetActive(value: false);
				unblockingBoxes.SetActive(value: true);
				blockingBlob.SetActive(value: true);
				doorBackInteractable.layer = 9;
				secondDialogueTriggered = true;
			}
			else
			{
				TriggerNonVirginDialoge();
				secondDialogueTriggered = true;
				nonVirginDialogueTriggered = true;
			}
		}
		else if (secondDialogueTriggered)
		{
			TriggerAfterDialogue();
		}
	}

	public void TriggerFirstDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(firstDialogue);
	}

	public void TriggerVirgingDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(virginDialogue);
	}

	public void TriggerNonVirginDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(nonVirginDialogue);
	}

	public void TriggerAfterDialogue()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(afterDialogue);
	}
}
