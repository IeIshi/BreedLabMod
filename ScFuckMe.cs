using UnityEngine;

public class ScFuckMe : Interactable
{
	private void Start()
	{
		GetComponent<ScJustFucking>().enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<BoxCollider>().isTrigger = true;
		GetComponent<ScJustFucking>().enabled = true;
		base.gameObject.layer = 0;
		GetComponent<ScFuckMe>().enabled = false;
	}
}
