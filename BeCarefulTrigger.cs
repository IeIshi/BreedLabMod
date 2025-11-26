using UnityEngine;

public class BeCarefulTrigger : MonoBehaviour
{
	public Dialogue dialogue;

	private void OnTriggerEnter(Collider other)
	{
		TriggerDialoge();
		Object.Destroy(base.gameObject);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
