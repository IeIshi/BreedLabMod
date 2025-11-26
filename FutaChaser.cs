using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FutaChaser : MonoBehaviour
{
	private GameObject Heroine;

	private NavMeshAgent agent;

	private Animator anim;

	private AudioSource port;

	private float distance;

	public float attackRadius = 3f;

	public bool gotHer;

	public bool hitFront;

	public bool hitBack;

	private bool attacked;

	private void Start()
	{
		port = base.transform.GetChild(0).GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		Heroine = GameObject.Find("Heroine");
		InventoryUI.heroineIsChased = true;
		port.Play();
	}

	private void Update()
	{
		if (Safespace.heroineSafe)
		{
			StartCoroutine(Fade(0.5f));
			return;
		}
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (attacked)
		{
			StartCoroutine(Fade(2f));
		}
		if (distance <= attackRadius)
		{
			agent.speed = 2f;
			agent.ResetPath();
			FaceTarget();
			anim.SetBool("isAttacking", value: true);
			attacked = true;
			if (gotHer)
			{
				checkHitAngle();
				HeroineStats.fartiged = true;
				if (hitFront)
				{
					PlayerController.iFalled = true;
					PlayerController.gotHitFront = true;
				}
				if (hitBack)
				{
					PlayerController.iFalled = true;
					PlayerController.gotHitBack = true;
				}
				StartCoroutine(Fade(1f));
			}
		}
		else
		{
			agent.SetDestination(Heroine.transform.position);
		}
	}

	private void checkHitAngle()
	{
		Vector3 forward = Heroine.transform.forward;
		Vector3 to = Heroine.transform.position - base.transform.position;
		float f = Vector3.SignedAngle(forward, to, Vector3.up);
		if (Mathf.Abs(f) > 80f && !PlayerController.iFalled)
		{
			hitFront = true;
			hitBack = false;
		}
		if (Mathf.Abs(f) <= 80f && !PlayerController.iFalled)
		{
			hitFront = false;
			hitBack = true;
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private IEnumerator Fade(float fadingTime)
	{
		yield return new WaitForSeconds(fadingTime);
		PlayerManager.spawnedFutaChaser = false;
		if (!PlayerController.iFalled)
		{
			InventoryUI.heroineIsChased = false;
		}
		Object.Destroy(base.gameObject);
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRadius);
	}
}
