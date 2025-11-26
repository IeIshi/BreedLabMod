using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MiniLuciInteract : Interactable
{
	public GameObject FireSpell;

	public AudioSource FireSound;

	public AudioSource StepSound;

	public Transform impactPoint;

	public Dialogue dialogue;

	public GameObject Key;

	private Animator animator;

	private bool triggered;

	private NavMeshAgent agent;

	public Transform wayPoint;

	public GameObject LuciSaver;

	private void Start()
	{
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		Key.SetActive(value: false);
	}

	public void FixedUpdate()
	{
		if (triggered && !DialogManager.inDialogue)
		{
			Object.Instantiate(FireSpell, impactPoint);
			FireSound.Play();
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("falled", value: true);
			PlayerController.iFalled = true;
			HeroineStats.fartiged = true;
			StartCoroutine(HitAndRun());
			triggered = false;
		}
		if (Vector3.Distance(base.transform.position, wayPoint.transform.position) <= 0.1f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
		triggered = true;
		LuciSaver.GetComponent<LuciSaver>().interacted = true;
	}

	private IEnumerator HitAndRun()
	{
		yield return new WaitForSeconds(1f);
		animator.SetBool("stand", value: true);
		Key.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		animator.SetBool("run", value: true);
		agent.SetDestination(wayPoint.position);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void StepEvent()
	{
		StepSound.Play();
	}
}
