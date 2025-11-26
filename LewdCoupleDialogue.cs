using UnityEngine;

public class LewdCoupleDialogue : Interactable
{
	public Dialogue dialogue1;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	private bool d1Triggered;

	private bool d2Triggered;

	public override void Interact()
	{
		base.Interact();
		if (d2Triggered)
		{
			TriggerDialoge3();
		}
		else if (d1Triggered)
		{
			TriggerDialoge2();
			d2Triggered = true;
		}
		else
		{
			TriggerDialoge1();
			d1Triggered = true;
		}
	}

	public void TriggerDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue1);
	}

	public void TriggerDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void TriggerDialoge3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
	}
}
