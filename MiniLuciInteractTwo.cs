using UnityEngine;

public class MiniLuciInteractTwo : Interactable
{
	public Dialogue dialogue;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		_ = DialogManager.inDialogue;
	}

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}
}
