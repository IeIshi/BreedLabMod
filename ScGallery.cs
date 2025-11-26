using UnityEngine;

public class ScGallery : Interactable
{
	private Animator animator;

	public GameObject heroine;

	public Transform defaultTarget;

	public Dialogue dialogue;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		if (DialogManager.inDialogue)
		{
			FaceTarget();
			animator.SetBool("lookAt", value: true);
		}
		else
		{
			animator.SetBool("lookAt", value: false);
			FaceDefaultTarget();
		}
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
		Vector3 normalized = (heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void FaceDefaultTarget()
	{
		Vector3 normalized = (defaultTarget.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}
}
