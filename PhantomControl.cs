using UnityEngine;

public class PhantomControl : Interactable
{
	private Animator animator;

	public Dialogue dialogue;

	public bool interacted;

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
			TriggerDialoge();
			animator.SetBool("isIdle", value: true);
		}
		interacted = true;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void Step()
	{
	}
}
