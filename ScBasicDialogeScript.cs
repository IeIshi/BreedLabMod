using UnityEngine;

public class ScBasicDialogeScript : Interactable
{
	private Animator animator;

	public GameObject heroine;

	public Transform defaultTarget;

	public Dialogue dialogue1;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	private bool d1Triggered;

	private bool d2Triggered;

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
		if (!d1Triggered)
		{
			TriggerDialoge1();
			d1Triggered = true;
		}
		else if (d1Triggered && !d2Triggered)
		{
			TriggerDialoge2();
			d2Triggered = true;
		}
		else if (d2Triggered)
		{
			TriggerDialoge3();
		}
	}

	public void TriggerDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue1);
	}

	public void TriggerDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void TriggerDialoge3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
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
