using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SupHum : MonoBehaviour
{
	private Animator anim;

	private Animator heroAnim;

	private GameObject Heroine;

	private NavMeshAgent agent;

	public Transform mount_place;

	public Transform mount_place_behind;

	public Transform camPos;

	public Transform camPosBehind;

	private float distance;

	public float sexRadius = 3f;

	private float timer = 4f;

	private float time;

	private bool hardFuck;

	private bool cumming;

	private bool pantsuOff;

	public AudioSource thrust1;

	public AudioSource thrust2;

	public AudioSource cumSound;

	public AudioSource stepSound;

	public AudioSource growlSound;

	public bool sleeping;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public bool patrollingWolf;

	private bool travelling;

	private bool patrolWaiting;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private float totalWaitTime;

	private int currentPatrolIndex;

	public float switchProbability;

	public List<Waypoint> patrolPoints;

	private bool iSeeYou;

	private bool growlSoundPlayed;

	private Image skullImage;

	private bool hitFront;

	private bool hitBack;

	public GameObject FireSpell;

	public Transform SpellImpact;

	public AudioSource SpellSound;

	private bool lastImpact;

	public bool chasingWolf;

	private void Start()
	{
		Heroine = GameObject.Find("Heroine");
		skullImage = GameObject.Find("skull").GetComponent<Image>();
		anim = GetComponent<Animator>();
		heroAnim = Heroine.GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		if (agent == null)
		{
			Debug.LogError("The nav mesh agent component is not attached to " + base.gameObject.name);
		}
		else if (patrolPoints != null && patrolPoints.Count >= 2)
		{
			currentPatrolIndex = 0;
			SetDestination();
		}
	}

	private void Update()
	{
		if (lastImpact)
		{
			return;
		}
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance < sexRadius)
		{
			if (patrollingWolf && !iSeeYou)
			{
				return;
			}
			if (!growlSoundPlayed)
			{
				CheckHitAngle();
				growlSound.Play();
				if (chasingWolf)
				{
					StartCoroutine(GetGrilled());
				}
				growlSoundPlayed = true;
			}
			agent.isStopped = true;
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
			Heroine.GetComponent<PlayerController>().enabled = false;
			if (hitFront)
			{
				base.gameObject.transform.rotation = mount_place.rotation;
				base.gameObject.transform.position = mount_place.position;
				CameraFollow.target = camPos;
			}
			if (hitBack)
			{
				base.gameObject.transform.rotation = mount_place_behind.rotation;
				base.gameObject.transform.position = mount_place_behind.position;
				CameraFollow.target = camPosBehind;
			}
			time += Time.deltaTime;
			if (time > timer)
			{
				PlayerManager.IsVirgin = false;
				VignetteEffect();
				if (hardFuck)
				{
					anim.speed = 1f;
					heroAnim.speed = 1f;
					if (hitFront)
					{
						anim.SetBool("sexHard", value: true);
						heroAnim.SetBool("matingPressSexSpread", value: true);
						heroAnim.SetBool("isPregnant", value: true);
					}
					if (hitBack)
					{
						anim.SetBool("sexBehindHard", value: true);
						heroAnim.SetBool("supHumSexHard", value: true);
						heroAnim.SetBool("isPregnant", value: true);
					}
					heroAnim.SetBool("isAhegao", value: true);
					if (time > 50f && !cumSound.isPlaying)
					{
						cumSound.Play();
					}
					if (time > 55f)
					{
						cumming = true;
					}
					return;
				}
				if (hitFront)
				{
					anim.SetBool("sex", value: true);
					heroAnim.SetBool("matingPressSex", value: true);
				}
				if (hitBack)
				{
					anim.SetBool("sexBehind", value: true);
					heroAnim.SetBool("supHumSex", value: true);
				}
				heroAnim.SetBool("isScared", value: false);
				HeroineStats.aroused = true;
				if (!pantsuOff)
				{
					if (EquipmentManager.instance.currentEquipment[3] != null)
					{
						if (EquipmentManager.instance.currentEquipment[3].id == 454874)
						{
							pantsuOff = true;
							return;
						}
						EquipmentManager.instance.RipPantsu();
					}
					pantsuOff = true;
				}
				if (time > 20f)
				{
					heroAnim.SetBool("isScared", value: false);
					anim.speed = 2f;
					heroAnim.speed = 2f;
				}
				if (time > 40f)
				{
					hardFuck = true;
				}
			}
			else
			{
				skullImage.enabled = true;
				if (hitFront)
				{
					anim.SetBool("tease", value: true);
					heroAnim.SetBool("matingPressTease", value: true);
				}
				if (hitBack)
				{
					anim.SetBool("teaseBehind", value: true);
					heroAnim.SetBool("supHumTease", value: true);
				}
				heroAnim.SetBool("isScared", value: true);
			}
			return;
		}
		WalkOrIdle();
		if (patrollingWolf)
		{
			if (GetComponent<EnemyFieldOfView>().heroineIsVisible)
			{
				Debug.Log("I SEE YOU");
				iSeeYou = true;
				agent.SetDestination(Heroine.transform.position);
			}
			Patrolling();
		}
		if (!patrollingWolf)
		{
			agent.SetDestination(Heroine.transform.position);
		}
	}

	private IEnumerator GetGrilled()
	{
		yield return new WaitForSeconds(25f);
		Object.Instantiate(FireSpell, SpellImpact);
		SpellSound.Play();
		yield return new WaitForSeconds(13f);
		Object.Instantiate(FireSpell, SpellImpact);
		SpellSound.Play();
		yield return new WaitForSeconds(20f);
		Object.Instantiate(FireSpell, SpellImpact);
		SpellSound.Play();
		yield return new WaitForSeconds(15f);
		Object.Instantiate(FireSpell, SpellImpact);
		SpellSound.Play();
		if (!HeroineStats.GameOver)
		{
			lastImpact = true;
			growlSound.Play();
			cumSound.Stop();
			PlayerController.iGetFucked = false;
			HeroineStats.stunned = false;
			Heroine.GetComponent<PlayerController>().enabled = true;
			anim.SetBool("teaseBehind", value: false);
			heroAnim.SetBool("supHumTease", value: false);
			anim.SetBool("tease", value: false);
			heroAnim.SetBool("matingPressTease", value: false);
			anim.SetBool("sexBehind", value: false);
			heroAnim.SetBool("supHumSex", value: false);
			anim.SetBool("sex", value: false);
			heroAnim.SetBool("matingPressSex", value: false);
			anim.SetBool("sexBehindHard", value: false);
			heroAnim.SetBool("supHumSexHard", value: false);
			heroAnim.SetBool("isPregnant", value: false);
			anim.SetBool("sexHard", value: false);
			heroAnim.SetBool("matingPressSexSpread", value: false);
			heroAnim.SetBool("falled", value: true);
			anim.SetBool("run", value: false);
			anim.SetBool("sleep", value: true);
			skullImage.enabled = false;
		}
	}

	private void Patrolling()
	{
		if (travelling && agent.remainingDistance <= 0.1f)
		{
			travelling = false;
			if (patrolWaiting)
			{
				waiting = true;
				waitTimer = 0f;
			}
			else
			{
				ChangePatrolPoint();
				SetDestination();
			}
		}
		if (waiting)
		{
			waitTimer += Time.deltaTime;
			if (waitTimer >= totalWaitTime)
			{
				waiting = false;
				ChangePatrolPoint();
				SetDestination();
			}
		}
	}

	private void SetDestination()
	{
		if (patrolPoints != null)
		{
			agent.isStopped = false;
			Vector3 position = patrolPoints[currentPatrolIndex].transform.position;
			agent.SetDestination(position);
			travelling = true;
		}
	}

	private void ChangePatrolPoint()
	{
		if (Random.Range(0f, 1f) <= switchProbability)
		{
			patrolForward = !patrolForward;
		}
		if (patrolForward)
		{
			currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
		}
		else if (--currentPatrolIndex < 0)
		{
			currentPatrolIndex = patrolPoints.Count - 1;
		}
	}

	private void WalkOrIdle()
	{
		if (agent.velocity.magnitude == 0f)
		{
			anim.SetBool("run", value: false);
		}
		else
		{
			anim.SetBool("run", value: true);
		}
	}

	private void VignetteEffect()
	{
		if (!thrusted)
		{
			return;
		}
		if (max)
		{
			PostProcessingManager.vignette.intensity.value -= Time.deltaTime * pulsingSpeed;
			if (PostProcessingManager.vignette.intensity.value <= 0f)
			{
				thrusted = false;
				max = false;
			}
		}
		else if (PostProcessingManager.vignette.intensity.value <= vignetteIntensity)
		{
			PostProcessingManager.vignette.intensity.value += Time.deltaTime * pulsingSpeed;
		}
		else
		{
			max = true;
		}
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, sexRadius);
	}

	private void CheckHitAngle()
	{
		Vector3 forward = Heroine.transform.forward;
		Vector3 to = Heroine.transform.position - base.transform.position;
		float f = Vector3.SignedAngle(forward, to, Vector3.up);
		if (Mathf.Abs(f) > 80f)
		{
			hitFront = true;
			hitBack = false;
			Debug.Log("Hit Front");
		}
		if (Mathf.Abs(f) <= 80f)
		{
			hitFront = false;
			hitBack = true;
			Debug.Log("Hit Back");
		}
	}

	private void ThrustEvent()
	{
		thrusted = true;
		if (!cumming)
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(2.5f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(0.3f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(8f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(8f);
		}
		if (hardFuck)
		{
			thrust2.Play();
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		else
		{
			thrust1.Play();
		}
	}

	private void StepEvent()
	{
		stepSound.Play();
	}
}
