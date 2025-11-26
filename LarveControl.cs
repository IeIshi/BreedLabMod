using UnityEngine;
using UnityEngine.AI;

public class LarveControl : MonoBehaviour
{
	private Animator animator;

	[SerializeField]
	public bool birthing;

	public bool waspPregnancy;

	private NavMeshAgent agent;

	private Transform destination;

	private float mutateTimer;

	private GameObject Wasp;

	public GameObject WaspPref;

	private bool waspSpawned;

	public float growthRate = 0.15f;

	public AudioSource slipSound;

	private void Start()
	{
		birthing = true;
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		destination = GameObject.Find("Waypoint").transform;
	}

	private void FixedUpdate()
	{
		if (birthing)
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("rig|LarvePussyCrawl"))
			{
				slipSound.Stop();
				birthing = false;
			}
			return;
		}
		agent.SetDestination(destination.transform.position);
		base.transform.localScale += new Vector3(growthRate, growthRate, growthRate) * Time.deltaTime;
		mutateTimer += Time.deltaTime;
		if (mutateTimer > 20f)
		{
			if (!waspSpawned)
			{
				Wasp = Object.Instantiate(WaspPref, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), Quaternion.identity);
				Wasp.GetComponent<WaspControl>().firstFlyingTarget = base.gameObject.transform;
				waspSpawned = true;
			}
			if (Wasp.GetComponent<WaspControl>().state == WaspControl.MyState.PATROL)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
