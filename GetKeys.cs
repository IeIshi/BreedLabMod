using UnityEngine;

public class GetKeys : Interactable
{
	public override void Interact()
	{
		base.Interact();
		PlayerManager.smallKey = true;
		GameObject.Find("KeyBox").GetComponent<KeyBox>().smallKey.SetActive(value: true);
		PlayerManager.id2 = true;
		GameObject.Find("KeyBox").GetComponent<KeyBox>().id2.SetActive(value: true);
	}
}
