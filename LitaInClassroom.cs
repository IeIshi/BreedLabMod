using UnityEngine;

public class LitaInClassroom : Interactable
{
	public Dialogue dialogue;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	public Dialogue dialogue4;

	private bool d1Activated;

	private bool d2Activated;

	private bool d3Activated;

	public override void Interact()
	{
		base.Interact();
		if (d3Activated)
		{
			TriggerLastDialoge();
		}
		if (!d1Activated)
		{
			d1Activated = true;
			TriggerDialoge();
		}
		else if (!d2Activated)
		{
			d2Activated = true;
			TriggerSecondDialoge();
		}
		else if (!d3Activated)
		{
			d3Activated = true;
			TriggerThirdDialoge();
		}
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerSecondDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void TriggerThirdDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
	}

	public void TriggerLastDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue4);
	}
}
