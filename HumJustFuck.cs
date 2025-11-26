using UnityEngine;

public class HumJustFuck : Interactable
{
	public GameObject phantomHeroine;

	private void Start()
	{
		GetComponent<WolfTakeMe>().enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<BoxCollider>().isTrigger = true;
		GetComponent<WolfTakeMe>().enabled = true;
		GetComponent<WolfTakeMe>().sex = true;
		Object.Destroy(phantomHeroine);
		base.gameObject.layer = 0;
		GetComponent<HumJustFuck>().enabled = false;
	}

	private void ThrustEventHard()
	{
	}
}
