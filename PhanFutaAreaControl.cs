using UnityEngine;
using UnityEngine.AI;

public class PhanFutaAreaControl : Interactable
{
	private Animator animator;

	public Dialogue dialogue;

	public bool interacted;

	public Waypoint patrolPoint;

	private NavMeshAgent agent;

	private float distance;

	public bool originphan;

	public bool phan1;

	public bool phan2;

	public bool phan3;

	public GameObject phanOb1;

	public GameObject phanOb2;

	public GameObject phanOb3;

	public GameObject TVBlocker;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (interacted)
		{
			if (!DialogManager.inDialogue)
			{
				GoToPoint();
			}
			else
			{
				FaceTarget();
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!interacted)
		{
			TriggerDialoge();
			animator.SetBool("isIdle", value: true);
			if (originphan)
			{
				phanOb1.SetActive(value: true);
			}
			if (phan1)
			{
				phanOb2.SetActive(value: true);
			}
			if (phan2)
			{
				phanOb3.SetActive(value: true);
			}
		}
		interacted = true;
	}

	private void GoToPoint()
	{
		if (!(patrolPoint != null))
		{
			return;
		}
		Vector3 position = patrolPoint.transform.position;
		GetComponent<CapsuleCollider>().isTrigger = true;
		agent.SetDestination(position);
		distance = Vector3.Distance(patrolPoint.transform.position, base.transform.position);
		if (distance < 1f)
		{
			if (phan3)
			{
				Object.Destroy(TVBlocker);
			}
			Object.Destroy(base.gameObject);
		}
		animator.SetBool("isIdle", value: false);
		animator.SetBool("isWalking", value: true);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void Step()
	{
	}
}
