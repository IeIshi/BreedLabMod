using UnityEngine;

public class PhanTalkThenDissa : Interactable
{
	private Animator animator;

	public Dialogue dialogue;

	private bool interacted;

	private void Start()
	{
		animator = GetComponent<Animator>();
		interacted = false;
	}

	private void FixedUpdate()
	{
		if (interacted)
		{
			FaceTarget();
		}
	}

	public override void Interact()
	{
		base.Interact();
		HeroineStats.debuffedStam = 0f;
		HeroineStats.currentStamina = HeroineStats.maxStamina;
		if (interacted)
		{
			Object.Destroy(base.gameObject);
		}
		if (!interacted)
		{
			TriggerDialoge();
			animator.SetBool("isIdle", value: true);
		}
		interacted = true;
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
