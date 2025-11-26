using UnityEngine;
using UnityEngine.AI;

public class GoToPoint : MonoBehaviour
{
	public Transform target;

	public Waypoint patrolPoint;

	private NavMeshAgent agent;

	private Animator animator;

	private float distance;

	public GameObject blobDoor;

	public GameObject progControl;

	public bool imPhantomThree;

	public bool imPhantomFour;

	public GameObject moveOutCollider;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (imPhantomFour)
		{
			if (!GetComponent<PhantomAfterSexDialogue>().run)
			{
				return;
			}
			if (!DialogManager.inDialogue)
			{
				goToPoint();
			}
			else
			{
				FaceTarget();
			}
			distance = Vector3.Distance(patrolPoint.transform.position, base.transform.position);
			if (distance <= 1f)
			{
				blobDoor.SetActive(value: false);
				progControl.GetComponent<ChallageRoomProgControl>().phantomFourOpenedDoor = true;
				if (moveOutCollider != null)
				{
					moveOutCollider.SetActive(value: true);
				}
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			if (!GetComponent<PhantomControl>().interacted)
			{
				return;
			}
			if (!DialogManager.inDialogue)
			{
				goToPoint();
			}
			else
			{
				FaceTarget();
			}
			distance = Vector3.Distance(patrolPoint.transform.position, base.transform.position);
			if (!(distance <= 1f))
			{
				return;
			}
			blobDoor.SetActive(value: false);
			progControl.GetComponent<ChallageRoomProgControl>().phantomTwoOpenedDoor = true;
			if (imPhantomThree)
			{
				progControl.GetComponent<ChallageRoomProgControl>().phantomThreeOpenedDoor = true;
			}
			if (imPhantomFour)
			{
				progControl.GetComponent<ChallageRoomProgControl>().phantomFourOpenedDoor = true;
				if (moveOutCollider != null)
				{
					moveOutCollider.SetActive(value: true);
				}
			}
			Object.Destroy(base.gameObject);
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (target.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void goToPoint()
	{
		if (patrolPoint != null)
		{
			Vector3 position = patrolPoint.transform.position;
			GetComponent<BoxCollider>().isTrigger = true;
			agent.SetDestination(position);
			animator.SetBool("isIdle", value: false);
			animator.SetBool("isWalking", value: true);
		}
	}
}
