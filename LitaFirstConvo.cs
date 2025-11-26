using UnityEngine;
using UnityEngine.AI;

public class LitaFirstConvo : Interactable
{
	public Transform defaultTarget;

	public Dialogue dialogue;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	public GameObject tentaclePenis;

	private TentacleThrustEvent cumCheck;

	private AudioSource struggleSound;

	public Waypoint waypoint;

	private Animator litaAnim;

	private bool d1Activated;

	private bool d2Activated;

	private bool d3Activated;

	private bool soundPlayed;

	public GameObject Note;

	private NavMeshAgent agent;

	private void Start()
	{
		litaAnim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		cumCheck = tentaclePenis.GetComponent<TentacleThrustEvent>();
		struggleSound = GameObject.Find("AudioSounds/Struggle").GetComponent<AudioSource>();
		Note.SetActive(value: false);
	}

	public override void Interact()
	{
		base.Interact();
		if (!d1Activated)
		{
			d1Activated = true;
			TriggerDialoge();
		}
		else if (!d2Activated)
		{
			d2Activated = true;
			TriggerSecondDialoge();
		}
		else if (!d3Activated)
		{
			d3Activated = true;
			TriggerThirdDialoge();
		}
	}

	private void FixedUpdate()
	{
		if (cumCheck.currentCum > 20f || cumCheck.currentStamina <= 0f)
		{
			if (Vector3.Distance(waypoint.transform.position, base.transform.position) <= 1f)
			{
				Note.SetActive(value: true);
				Object.Destroy(base.gameObject);
			}
			else
			{
				agent.SetDestination(waypoint.transform.position);
				litaAnim.speed = 3f;
				litaAnim.SetBool("run", value: true);
			}
		}
		else if (d2Activated)
		{
			if (!DialogManager.inDialogue)
			{
				litaAnim.SetBool("idle", value: true);
				if (!soundPlayed)
				{
					struggleSound.Play();
					soundPlayed = true;
				}
				FaceTarget();
			}
		}
		else if (d1Activated)
		{
			litaAnim.SetBool("frightend", value: true);
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerSecondDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void TriggerThirdDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
	}
}
