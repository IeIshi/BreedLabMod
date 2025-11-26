using UnityEngine;

public class ActivateGangBang : Interactable
{
	public Dialogue dialogue;

	public GameObject blobDoor;

	public bool triggerFogy;

	public GameObject bed1;

	public GameObject bed2;

	public GameObject bed3;

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
		GetComponent<TriggerFog>().enabled = true;
		RenderSettings.fog = true;
		triggerFogy = true;
		base.gameObject.layer = 0;
		bed1.GetComponent<DialogTrigger>().enabled = false;
		bed2.GetComponent<DialogTrigger>().enabled = false;
		bed3.GetComponent<DialogTrigger>().enabled = false;
		bed1.layer = 0;
		bed2.layer = 0;
		bed3.layer = 0;
		blobDoor.SetActive(value: true);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
