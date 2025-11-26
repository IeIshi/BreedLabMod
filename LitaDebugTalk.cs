using UnityEngine;

public class LitaDebugTalk : Interactable
{
	public Transform defaultTarget;

	public Dialogue dialogue;

	public override void Interact()
	{
		base.Interact();
		PlayerManager.MantisDown = true;
		TriggerDialoge();
	}

	private void FixedUpdate()
	{
		if (DialogManager.inDialogue)
		{
			FaceTarget();
		}
		else
		{
			FaceDefaultTarget();
		}
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

	private void FaceDefaultTarget()
	{
		Vector3 normalized = (defaultTarget.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}
}
