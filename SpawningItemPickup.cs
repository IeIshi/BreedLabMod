using UnityEngine;

public class SpawningItemPickup : Interactable
{
	public Item item;

	public Dialogue dialogue;

	private AudioSource ammoPickup;

	public GameObject theItem;

	private void Start()
	{
		ammoPickup = GameObject.Find("AmmoPickup").GetComponent<AudioSource>();
	}

	public override void Interact()
	{
		base.Interact();
		if (Inventory.instance.items.Count < Inventory.instance.space)
		{
			PickUp();
			ammoPickup.Play();
		}
		else
		{
			TriggerDialoge();
		}
	}

	private void PickUp()
	{
		Debug.Log("Picking up " + item.name);
		Object.Destroy(theItem);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
