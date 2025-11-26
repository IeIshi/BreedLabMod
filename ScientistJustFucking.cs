using UnityEngine;

public class ScientistJustFucking : Interactable
{
	public GameObject phantomHeroine;

	private void Start()
	{
		GetComponent<ScientistTakeMe>().enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<BoxCollider>().isTrigger = true;
		GetComponent<ScientistTakeMe>().enabled = true;
		GetComponent<ScientistTakeMe>().sex = true;
		Object.Destroy(phantomHeroine);
		base.gameObject.layer = 0;
		GetComponent<ScientistJustFucking>().enabled = false;
	}

	private void ThrustEvent()
	{
	}
}
