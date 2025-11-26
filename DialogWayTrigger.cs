using UnityEngine;

public class DialogWayTrigger : MonoBehaviour
{
	public Dialogue dialogue;

	private bool triggered;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !triggered)
		{
			TriggerDialoge();
			triggered = true;
		}
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
