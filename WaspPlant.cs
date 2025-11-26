using UnityEngine;
using UnityEngine.AI;

public class WaspPlant : MonoBehaviour
{
	public GameObject WaspPref;

	public Transform waspSpawnPoint;

	public Transform waspGoToPoint;

	public float waspSpawnTimer;

	public float spawnRange;

	private float spawnTimer;

	public float maxWaspSpawns;

	private float currentWaspSpawns;

	private GameObject Wasp;

	private Animator animator;

	public bool isWaspPlant;

	public bool isWaspEgg;

	private float mSize;

	private bool scaledUp;

	private bool wasSpawned;

	private float distance;

	private bool trigger;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		distance = Vector3.Distance(PlayerManager.instance.player.transform.position, base.transform.position);
		if (PlayerController.iGetFucked && !MovementManipulator.occupied && MovementManipulator.chasingWaspCount < 3 && isWaspPlant)
		{
			SpawnFuckWaspsAfterSec(waspSpawnTimer);
		}
		if (distance < spawnRange)
		{
			trigger = true;
		}
		if (trigger)
		{
			if (isWaspPlant && currentWaspSpawns < maxWaspSpawns)
			{
				SpawnWaspsAfterSec(waspSpawnTimer);
			}
			if (isWaspEgg)
			{
				SpawnWaspOnce();
			}
		}
	}

	private void SpawnWaspOnce()
	{
		if (!wasSpawned && scaledUp)
		{
			Wasp = Object.Instantiate(WaspPref, new Vector3(waspSpawnPoint.position.x, waspSpawnPoint.position.y, waspSpawnPoint.position.z), Quaternion.identity);
			Wasp.GetComponent<WaspControl>().firstFlyingTarget = waspGoToPoint;
			wasSpawned = true;
		}
		if (!scaledUp)
		{
			mSize += Time.deltaTime * 50f;
			GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize);
			if (mSize >= 100f)
			{
				scaledUp = true;
			}
		}
	}

	private void SpawnWaspsAfterSec(float timer)
	{
		spawnTimer += Time.deltaTime;
		if (spawnTimer > timer)
		{
			Wasp = Object.Instantiate(WaspPref, new Vector3(waspSpawnPoint.position.x, waspSpawnPoint.position.y, waspSpawnPoint.position.z), Quaternion.identity);
			Wasp.GetComponent<WaspControl>().firstFlyingTarget = waspGoToPoint;
			Wasp.GetComponent<NavMeshAgent>().speed = 12f;
			animator.SetBool("spit", value: false);
			currentWaspSpawns += 1f;
			Debug.Log(currentWaspSpawns);
			spawnTimer = 0f;
		}
		else if (spawnTimer > timer - 1f)
		{
			animator.SetBool("spit", value: true);
		}
	}

	private void SpawnFuckWaspsAfterSec(float timer)
	{
		spawnTimer += Time.deltaTime;
		if (spawnTimer > timer)
		{
			Wasp = Object.Instantiate(WaspPref, new Vector3(waspSpawnPoint.position.x, waspSpawnPoint.position.y, waspSpawnPoint.position.z), Quaternion.identity);
			Wasp.GetComponent<WaspControl>().firstFlyingTarget = waspGoToPoint;
			Wasp.GetComponent<WaspControl>().isFuckWasp = true;
			animator.SetBool("spit", value: false);
			currentWaspSpawns += 1f;
			MovementManipulator.chasingWaspCount++;
			spawnTimer = 0f;
			trigger = false;
		}
		else if (spawnTimer > timer - 1f)
		{
			animator.SetBool("spit", value: true);
		}
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, spawnRange);
	}
}
