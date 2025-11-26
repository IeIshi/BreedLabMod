using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
	public Transform anim_pos;

	public Transform mount_place;

	public Transform mount_place_behind;

	public Transform targetStaminaUi;

	public Transform targetCumUi;

	public Transform targetHealthUi;

	public GameObject stamUiPrefab;

	public GameObject cumUiPrefab;

	public GameObject healthUiPrefab;

	public GameObject gameManager;

	public GameObject Heroine;

	public AudioSource sexSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public AudioSource imAware;

	public AudioSource attackSound;

	private Transform target;

	private NavMeshAgent agent;

	private Animator anim;

	private Transform staminaUi;

	private Transform cumUI;

	private Transform healthUI;

	private Transform cam;

	private Image stamSlider;

	private Image cumSlider;

	private Image healthSlider;

	public Image circle;

	public Image SexImage;

	public float maxStamina;

	public float maxCum;

	public float maxHealth = 50f;

	public float teaseDmg = 2.5f;

	public float thrustDmg = 0.05f;

	public int impregnateChance = 10;

	public float gainCumPerThrust = 4f;

	public float drainStamPerThrust = 5f;

	public float attackDelay = 0.5f;

	public float insertLust = 5f;

	public float chaseRadius = 10f;

	public float attackRadius = 2f;

	public float chaseSpeed = 4f;

	public float patrolSpeed = 2.5f;

	public float waitForSex = 3f;

	public float insertTime = 3f;

	public float mountRadius = 3f;

	public float regenRate = 10f;

	public bool patrolWaiting;

	public float totalWaitTime = 3f;

	public float switchProbability = 0.2f;

	public List<Waypoint> patrolPoints;

	private int currentPatrolIndex;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private float distance;

	private float mountDelay;

	private float fuckDelay;

	private float mountTime = 1f;

	public float fuckTime = 4f;

	private float currentStamina;

	private float currentCum;

	private float currentHealth;

	public bool iRest;

	public bool iPatrol;

	public bool iChase;

	public bool iMount;

	public bool iFuck;

	private bool noStamina;

	private bool cumming;

	private bool gotAttacked;

	public static float powerRecieved;

	public bool hitFront;

	public bool hitBack;

	private bool iAttacked;

	private bool iInsert;

	public bool imOnHer;

	private bool impregRolette;

	private bool unfuck;

	public bool imSuper;

	private float pct;

	private float fartigeCd;

	public float fartigeTime;

	public float fartigeSpeed = 1.5f;

	private bool attackFartige;

	private bool attack;

	public Transform camFront;

	public Transform camBehind;

	public GameObject enemSaveState;

	private void Start()
	{
		target = PlayerManager.instance.player.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		cam = Camera.main.transform;
		currentStamina = maxStamina;
		currentCum = 0f;
		attackFartige = false;
		currentHealth = maxHealth;
		Canvas[] array = Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				staminaUi = Object.Instantiate(stamUiPrefab, canvas.transform).transform;
				cumUI = Object.Instantiate(cumUiPrefab, canvas.transform).transform;
				healthUI = Object.Instantiate(healthUiPrefab, canvas.transform).transform;
				stamSlider = staminaUi.GetChild(0).GetComponent<Image>();
				cumSlider = cumUI.GetChild(0).GetComponent<Image>();
				healthSlider = healthUI.GetChild(0).GetComponent<Image>();
				break;
			}
		}
		healthUI.gameObject.SetActive(value: false);
		DebugStuff();
	}

	public void Update()
	{
		distance = Vector3.Distance(target.position, base.transform.position);
		checkHitAngle();
		AILogic();
		if (staminaUi != null)
		{
			staminaUi.position = targetStaminaUi.position;
			staminaUi.forward = -cam.forward;
		}
		if (cumUI != null)
		{
			cumUI.position = targetCumUi.position;
			cumUI.forward = -cam.forward;
		}
		if (healthUI != null)
		{
			healthUI.position = targetHealthUi.position;
			healthUI.forward = -cam.forward;
		}
		if (iRest)
		{
			staminaUi.gameObject.SetActive(value: true);
		}
		if (currentStamina >= 1f)
		{
			noStamina = false;
		}
		if (currentStamina >= maxStamina)
		{
			iRest = false;
		}
		if (!iRest && !InventoryUI.heroineIsChased && distance <= chaseRadius)
		{
			imOnHer = true;
		}
		if (currentStamina == 0f)
		{
			noStamina = true;
		}
		if (currentCum == 100f)
		{
			cumming = true;
		}
		if (cumming)
		{
			DrainCum(30f);
			drainStamina(0.05f);
			Cumming();
			HeroineStats.creampied = true;
			if (!impregRolette)
			{
				int num = Random.Range(1, 100);
				if (impregnateChance >= num)
				{
					HeroineStats.pregnant = true;
				}
				impregRolette = true;
			}
		}
		if (currentCum <= 0f)
		{
			cumming = false;
			impregRolette = false;
		}
		if (iRest)
		{
			currentCum = 0f;
			cumSlider.fillAmount = CalculateCum();
		}
		if (iInsert || iFuck)
		{
			RecieveDamage();
			staminaUi.gameObject.SetActive(value: true);
			cumUI.gameObject.SetActive(value: true);
			healthUI.gameObject.SetActive(value: false);
		}
		else
		{
			cumUI.gameObject.SetActive(value: false);
		}
		if (iInsert)
		{
			Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
		}
		if (cumming && !cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (!cumming)
		{
			cumSound.Stop();
		}
		if (iInsert && !insertSound.isPlaying)
		{
			insertSound.Play();
		}
		if (!iInsert)
		{
			insertSound.Stop();
		}
		ClampStats();
	}

	private void AILogic()
	{
		ChaseLogic();
		if (distance <= attackRadius && !PlayerController.iFalled && !iRest)
		{
			attack = true;
		}
		if (attack)
		{
			Attack();
		}
		if (distance > chaseRadius && !iRest)
		{
			if (imOnHer)
			{
				InventoryUI.heroineIsChased = false;
				imOnHer = false;
			}
			agent.isStopped = false;
			Patrolling();
			iPatrol = true;
		}
		else
		{
			iPatrol = false;
		}
		if (PlayerController.iFalled && !iRest && distance <= mountRadius && imOnHer)
		{
			Mount();
			InventoryUI.heroineIsChased = true;
			if (iFuck && !cumming)
			{
				mountDelay = 0f;
				Fuck();
			}
		}
		if (noStamina && !cumming)
		{
			float num = Random.Range(0.3f, 0.7f);
			anim_pos.position = new Vector3(target.transform.position.x - num, target.transform.position.y, target.transform.position.z);
			iAttacked = false;
			iRest = true;
			Rest();
		}
		if (iRest)
		{
			Rest();
		}
	}

	private void checkHitAngle()
	{
		Vector3 forward = target.transform.forward;
		Vector3 to = target.transform.position - base.transform.position;
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

	private void ChaseLogic()
	{
		if (distance <= chaseRadius && distance > attackRadius && !iRest)
		{
			if (!InventoryUI.heroineIsChased)
			{
				imAware.Play();
				imOnHer = true;
				InventoryUI.heroineIsChased = true;
			}
			if (imOnHer)
			{
				iChase = true;
				agent.SetDestination(target.position);
				if (!attackFartige)
				{
					agent.speed = chaseSpeed;
				}
				else
				{
					fartigeCd += Time.deltaTime;
					agent.speed = fartigeSpeed;
					if (fartigeCd >= fartigeTime)
					{
						attackFartige = false;
					}
				}
				anim.SetBool("isWalking", value: true);
				anim.SetBool("isIdle", value: false);
				iAttacked = false;
				agent.isStopped = false;
			}
			else
			{
				agent.isStopped = true;
				anim.SetBool("isWalking", value: false);
				anim.SetBool("isIdle", value: true);
				FaceTarget();
			}
		}
		else
		{
			iChase = false;
		}
	}

	private void Cumming()
	{
		if (PlayerController.iFalledFront)
		{
			anim.SetBool("isFucking", value: false);
			anim.SetBool("isCumming", value: true);
		}
		if (PlayerController.iFalledBack)
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
			anim.SetBool("isFuckingBehind", value: false);
			anim.SetBool("isCummingBehind", value: true);
		}
	}

	private void Patrolling()
	{
		agent.speed = patrolSpeed;
		staminaUi.gameObject.SetActive(value: false);
		if (travelling && agent.remainingDistance <= 1f)
		{
			travelling = false;
			anim.SetBool("isWalking", value: false);
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
			anim.SetBool("isIdle", value: true);
			if (waitTimer >= totalWaitTime)
			{
				waiting = false;
				ChangePatrolPoint();
				SetDestination();
			}
		}
	}

	private void Attack()
	{
		StartCoroutine(Attack());
		IEnumerator Attack()
		{
			FaceTarget();
			if (!iAttacked)
			{
				anim.SetBool("isIdle", value: true);
				anim.SetBool("isWalking", value: false);
				anim.SetBool("isAttacking", value: true);
				iAttacked = true;
				fartigeCd = 0f;
				attackFartige = true;
			}
			agent.isStopped = true;
			yield return new WaitForSeconds(attackDelay);
			anim.SetBool("isAttacking", value: false);
			iAttacked = false;
			attack = false;
		}
	}

	private void Mount()
	{
		mountDelay += Time.deltaTime;
		unfuck = false;
		if (mountDelay >= mountTime && !iFuck)
		{
			iMount = true;
			agent.enabled = false;
			if (PlayerController.iFalledFront)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
				CameraFollow.target = camFront;
				anim.SetBool("isInserting", value: true);
				anim.SetBool("isWalking", value: false);
			}
			if (PlayerController.iFalledBack)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
				CameraFollow.target = camBehind;
				anim.SetBool("isInsertingBehind", value: true);
				anim.SetBool("isWalking", value: false);
			}
			anim.SetBool("isIdle", value: false);
			iInsert = true;
			PlayerController.iGetInserted = true;
			fuckDelay += Time.deltaTime;
			SexImage.enabled = true;
			circle.enabled = true;
			pct = fuckDelay / fuckTime;
			circle.fillAmount = pct;
		}
		if (fuckDelay >= fuckTime)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			iMount = false;
			iFuck = true;
			PlayerController.iGetFucked = true;
		}
	}

	private void Fuck()
	{
		if (iFuck)
		{
			mountDelay = 0f;
			fuckDelay = 0f;
			iInsert = false;
			PlayerController.iGetInserted = false;
			if (PlayerController.iFalledFront)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
				anim.SetBool("isFucking", value: true);
				anim.SetBool("isInserting", value: false);
				anim.SetBool("isCumming", value: false);
				anim.SetBool("isWalking", value: false);
			}
			if (PlayerController.iFalledBack)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
				anim.SetBool("isFuckingBehind", value: true);
				anim.SetBool("isInsertingBehind", value: false);
				anim.SetBool("isCummingBehind", value: false);
				anim.SetBool("isWalking", value: false);
			}
		}
	}

	private void Rest()
	{
		iFuck = false;
		iInsert = false;
		agent.enabled = true;
		if (!unfuck)
		{
			PlayerController.iGetInserted = false;
			PlayerController.iGetFucked = false;
			mountDelay = 0f;
			if (InventoryUI.heroineIsChased)
			{
				imOnHer = false;
				InventoryUI.heroineIsChased = false;
				SexImage.enabled = false;
				circle.enabled = false;
			}
			unfuck = true;
		}
		anim.SetBool("isFucking", value: false);
		anim.SetBool("isInserting", value: false);
		anim.SetBool("isInsertingBehind", value: false);
		anim.SetBool("isFuckingBehind", value: false);
		anim.SetBool("isIdle", value: true);
		gainStamina(regenRate);
	}

	private void SetDestination()
	{
		if (patrolPoints != null)
		{
			Vector3 position = patrolPoints[currentPatrolIndex].transform.position;
			agent.SetDestination(position);
			anim.SetBool("isWalking", value: true);
			anim.SetBool("isIdle", value: false);
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

	private void FaceTarget()
	{
		Vector3 normalized = (target.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void DebugStuff()
	{
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

	public void drainStamina(float drainValue)
	{
		currentStamina -= drainValue;
		float fillAmount = currentStamina / maxStamina;
		stamSlider.fillAmount = fillAmount;
	}

	public void drainHealth(float drainValue)
	{
		currentHealth -= drainValue;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
	}

	public void gainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
		float fillAmount = currentStamina / maxStamina;
		stamSlider.fillAmount = fillAmount;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		float fillAmount = currentCum / maxCum;
		cumSlider.fillAmount = fillAmount;
	}

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		float fillAmount = currentCum / maxCum;
		cumSlider.fillAmount = fillAmount;
	}

	private float CalculateCum()
	{
		return currentCum / maxCum;
	}

	private float CalculateStamina()
	{
		return currentStamina / maxStamina;
	}

	private float CalculateHealth()
	{
		return currentHealth / maxHealth;
	}

	private void RecieveDamage()
	{
		if (powerRecieved > 0f)
		{
			drainStamina(powerRecieved);
			gotAttacked = true;
		}
		if (gotAttacked)
		{
			powerRecieved = 0f;
			gotAttacked = false;
		}
	}

	public void ThrustEvent()
	{
		drainStamina(drainStamPerThrust);
		GainCumInstant(gainCumPerThrust);
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
	}

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
		drainHealth(amount);
		if (!iRest)
		{
			agent.SetDestination(target.position);
			agent.speed = chaseSpeed;
			anim.SetBool("isWalking", value: true);
		}
		if (currentHealth <= 0f)
		{
			Die();
		}
	}

	private void Die()
	{
		healthUI.gameObject.SetActive(value: false);
		staminaUi.gameObject.SetActive(value: false);
		cumUI.gameObject.SetActive(value: false);
		InventoryUI.heroineIsChased = false;
		enemSaveState.GetComponent<EnemSavState>().isDead = true;
		Object.Destroy(base.gameObject);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player"))
		{
			return;
		}
		if (gameManager.GetComponent<EquipmentManager>().currentEquipment[3] != null)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1] != null)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(1);
			}
			else
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(3);
			}
		}
		else
		{
			if (PlayerController.iFalled || iRest)
			{
				return;
			}
			PlayerController.iFalled = true;
			if (hitFront)
			{
				PlayerController.gotHitFront = true;
				PlayerController.gotHitBack = false;
			}
			if (hitBack)
			{
				PlayerController.gotHitFront = false;
				PlayerController.gotHitBack = true;
			}
			for (int i = 0; i < gameManager.GetComponent<EquipmentManager>().currentEquipment.Length; i++)
			{
				if (gameManager.GetComponent<EquipmentManager>().currentEquipment[i] != null && i > 0 && i < 6)
				{
					gameManager.GetComponent<EquipmentManager>().RipOff(i);
					break;
				}
			}
		}
	}

	private void ClampStats()
	{
		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
		currentCum = Mathf.Clamp(currentCum, 0f, maxCum);
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, chaseRadius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, attackRadius);
	}
}
