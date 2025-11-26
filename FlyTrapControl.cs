using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FlyTrapControl : MonoBehaviour
{
	public float maxStamina;

	private float currentStamina;

	public float maxCum;

	private float currentCum;

	public float gainCumPerThrust;

	public float thrustDmg;

	public float teaseDmg;

	public GameObject Heroine;

	public Transform dragPoint;

	public Transform cumViewPoint;

	public Transform spitArea;

	public Transform sexViewPoint;

	public float awareRange;

	private float distance;

	private bool aware;

	private Animator heroineAnim;

	private Animator animator;

	public bool grabbed;

	private bool shake1;

	private bool shake2;

	private bool shake3;

	private bool sex;

	private bool cum;

	private bool spit;

	public Image circle;

	public Image SexImage;

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	private bool fartiged;

	public AudioSource[] wetSoundArray;

	public AudioSource[] slideSoundArray;

	public AudioSource[] cumSoundArray;

	public AudioSource swallowSound;

	public AudioSource stepSound;

	public AudioSource closeSound;

	public AudioSource openSound;

	private bool closeSoundPlayed;

	private bool openSoundPlayed;

	private float timer;

	public float closeTimer;

	public float fartigeTimer;

	private float fTimer;

	public GameObject Gas;

	private NavMeshAgent agent;

	public float patrolSpeed = 2.5f;

	public bool patrolWaiting;

	public float totalWaitTime = 3f;

	public float switchProbability = 0.2f;

	public Waypoint[] patrolPoints;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private int currentPatrolIndex;

	public GameObject BlockPath;

	private void Start()
	{
		currentStamina = maxStamina;
		currentCum = 0f;
		heroineAnim = Heroine.GetComponent<Animator>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		Gas.SetActive(value: false);
		PatrolOnStart();
		if (!PlayerManager.IsVirgin && !PlayerManager.FkedBySc && BlockPath != null)
		{
			BlockPath.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (spit)
		{
			float maxDistanceDelta = 10f * Time.deltaTime;
			Heroine.transform.position = Vector3.MoveTowards(Heroine.transform.position, spitArea.position, maxDistanceDelta);
			Heroine.transform.rotation = spitArea.rotation;
			Gas.SetActive(value: false);
			if (Heroine.transform.position == spitArea.position)
			{
				spit = false;
			}
			return;
		}
		if (fartiged)
		{
			fTimer += Time.deltaTime;
			if (fTimer > fartigeTimer)
			{
				fTimer = 0f;
				currentStamina = maxStamina;
				fartiged = false;
			}
			return;
		}
		if (grabbed)
		{
			if (currentStamina <= 0f)
			{
				Release();
				fartiged = true;
			}
			else if (cum)
			{
				HeroineStats.stunned = true;
				HeroineStats.currentPower = 0f;
				if (HeroineStats.currentPreg > 0.9f)
				{
					Release();
					return;
				}
				InitiateUI();
				Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
				heroineAnim.SetBool("flyTrapCum", value: true);
				heroineAnim.SetBool("isCumFilled", value: true);
				Heroine.transform.position = base.transform.position;
				Heroine.transform.rotation = base.transform.rotation;
				animator.SetBool("cum", value: true);
				CameraFollow.target = cumViewPoint;
				HeroineStats.creampied = true;
				HeroineStats.pregnant = true;
				HeroineStats.hugeAmount = true;
				PlayerController.heIsFuckingHard = true;
			}
			else if (sex)
			{
				InitiateUI();
				Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
				CameraFollow.target = sexViewPoint.transform;
				heroineAnim.Play("rig|FlyTrap_Sex2");
				animator.Play("Armature|FlyTrap_Sex2");
				Heroine.transform.position = base.transform.position;
				Heroine.transform.rotation = base.transform.rotation;
				if (HeroineStats.currentLust > 80f)
				{
					PlayerController.heIsFuckingHard = true;
				}
				if (currentCum >= maxCum)
				{
					cum = true;
				}
			}
			else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|FlyTrap_Sex1"))
			{
				InitiateUI();
				Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
				if (HeroineStats.currentLust > 80f)
				{
					PlayerController.heIsFuckingHard = true;
				}
				Heroine.GetComponent<PlayerController>().enabled = true;
				Heroine.transform.position = base.transform.position;
				Heroine.transform.rotation = base.transform.rotation;
				CameraFollow.target = sexViewPoint.transform;
				heroineAnim.Play("rig|FlyTrap_Sex1");
				heroineAnim.SetBool("flyTrapGrab", value: true);
				if (!shake2)
				{
					Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
					Gas.SetActive(value: true);
					shake2 = true;
				}
				PlayerController.iGetFucked = true;
				PlayerController.iFalled = true;
				InitiateSex();
			}
			else
			{
				float maxDistanceDelta2 = 10f * Time.deltaTime;
				Heroine.transform.position = Vector3.MoveTowards(Heroine.transform.position, dragPoint.position, maxDistanceDelta2);
				Heroine.transform.rotation = base.transform.rotation;
			}
			return;
		}
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance < awareRange)
		{
			if (!HeroineStats.pregnant && !HeroineStats.birth && Heroine.GetComponent<HeroineStats>().mySexPartner == null)
			{
				animator.SetBool("aware", value: true);
				FaceTarget();
				agent.isStopped = true;
				timer = 0f;
				aware = true;
				if (!openSoundPlayed)
				{
					openSound.Play();
					closeSoundPlayed = false;
					openSoundPlayed = true;
				}
				return;
			}
		}
		else
		{
			closeYourself();
		}
		if (!aware)
		{
			Patrol();
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}

	private void WalkOrIdle()
	{
		if (agent.velocity == Vector3.zero)
		{
			animator.SetBool("walk", value: false);
		}
		else
		{
			animator.SetBool("walk", value: true);
		}
	}

	private void closeYourself()
	{
		timer += Time.deltaTime;
		if (timer > closeTimer)
		{
			animator.SetBool("aware", value: false);
			aware = false;
			if (!closeSoundPlayed)
			{
				closeSound.Play();
				openSoundPlayed = false;
				closeSoundPlayed = true;
			}
		}
	}

	private void Release()
	{
		PlayerController.iGetFucked = false;
		shake1 = false;
		shake2 = false;
		shake3 = false;
		sex = false;
		cum = false;
		grabbed = false;
		aware = false;
		heroineAnim.SetBool("flyTrapCum", value: false);
		heroineAnim.SetBool("flyTrapGrab", value: false);
		animator.SetBool("cum", value: false);
		animator.SetBool("grab", value: false);
		animator.SetBool("aware", value: false);
		animator.SetBool("walk", value: false);
		PlayerController.iFalledBack = true;
		PlayerController.iFalled = true;
		PlayerController.heIsFuckingHard = false;
		heroineAnim.SetBool("isFalledBack", value: true);
		CameraFollow.target = Heroine.transform;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		HeroineStats.stunned = false;
		currentCum = 0f;
		fuckDelay = 0f;
		DisableUI();
		swallowSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
		spit = true;
	}

	private void InitiateSex()
	{
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
			return;
		}
		SexImage.enabled = true;
		circle.enabled = true;
		fuckDelay += Time.deltaTime;
		pct = fuckDelay / fuckTime;
		circle.fillAmount = pct;
		if (fuckDelay >= fuckTime)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			if (!shake3)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				shake3 = true;
			}
			if (PlayerManager.IsVirgin)
			{
				PlayerManager.IsVirgin = false;
				BlockPath.SetActive(value: true);
			}
			sex = true;
		}
	}

	private void Patrol()
	{
		agent.isStopped = false;
		agent.speed = patrolSpeed;
		WalkOrIdle();
		if (travelling && agent.remainingDistance <= 1f)
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
			Vector3 position = patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position;
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
			currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
		}
		else if (--currentPatrolIndex < 0)
		{
			currentPatrolIndex = patrolPoints.Length - 1;
		}
	}

	private void PatrolOnStart()
	{
		if (agent == null)
		{
			Debug.LogError("The nav mesh agent component is not attached to " + base.gameObject.name);
			return;
		}
		agent.enabled = true;
		if (patrolPoints != null && patrolPoints.Length >= 2)
		{
			currentPatrolIndex = 0;
			SetDestination();
		}
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void LoseCumInstant(float gainValue)
	{
		if (currentCum > 0f)
		{
			currentCum -= gainValue;
			EnemyUI.instance.LoseCum(gainValue);
		}
		else
		{
			currentCum = 0f;
		}
	}

	public void DrainStaminaInstant(float drainValue)
	{
		if (currentStamina < 0f)
		{
			currentStamina = 0f;
		}
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	private void TeaseEvent()
	{
		wetSoundArray[Random.Range(0, wetSoundArray.Length)].Play();
	}

	private void ThrustEvent()
	{
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		wetSoundArray[Random.Range(0, wetSoundArray.Length)].Play();
	}

	private void SlideEvent()
	{
		slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
	}

	private void CumEvent()
	{
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		HeroineStats.currentPreg += 0.025f;
		LoseCumInstant(2f);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		wetSoundArray[Random.Range(0, wetSoundArray.Length)].Play();
		cumSoundArray[Random.Range(0, cumSoundArray.Length)].Play();
	}

	private void StepEvent()
	{
		stepSound.Play();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!HeroineStats.pregnant && !fartiged && !HeroineStats.birth && Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			Heroine.GetComponent<PlayerController>().enabled = false;
			animator.SetBool("grab", value: true);
			StartCoroutine(dragHer());
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		}
	}

	private IEnumerator dragHer()
	{
		yield return new WaitForSeconds(0.5f);
		if (!shake1)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			swallowSound.Play();
			shake1 = true;
		}
		grabbed = true;
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, awareRange);
	}
}
