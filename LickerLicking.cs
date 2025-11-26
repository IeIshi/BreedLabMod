using UnityEngine;

public class LickerLicking : Interactable
{
	public GameObject phantomHeroine;

	private void Start()
	{
		GetComponent<LickerFuckMe>().enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<BoxCollider>().isTrigger = true;
		GetComponent<LickerFuckMe>().enabled = true;
		GetComponent<LickerFuckMe>().sex = true;
		Object.Destroy(phantomHeroine);
		base.gameObject.layer = 0;
		GetComponent<LickerLicking>().enabled = false;
	}
}
