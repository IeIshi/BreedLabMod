using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LickerController : MonoBehaviour
{
	public Transform anim_pos;

	public Transform mount_place;

	public Transform targetStaminaUi;

	public Transform targetHealthUi;

	public GameObject stamUiPrefab;

	public GameObject healthUiPrefab;

	public GameObject camPos;

	public GameObject Heroine;

	public GameObject gameManager;

	public AudioSource sexSound;

	public AudioSource cumSound;

	public AudioSource stepSound;

	public AudioSource attackSound;

	public AudioSource lickSound;

	public AudioSource lickSexSound;

	public Image circle;

	public Image SexImage;

	private Transform target;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	private Transform staminaUi;

	private Transform healthUI;

	private Transform cam;

	private Image stamSlider;

	private Image cumSlider;

	private Image healthSlider;

	public float maxStamina;

	public float maxCum;

	public float maxHealth = 50f;

	public float teaseDmg = 2.5f;

	public float thrustDmg = 0.05f;

	public int impregnateChance = 10;

	public float gainCumPerThrust = 4f;

	public float drainStamPerThrust = 5f;

	public float expPerThrust = 2f;

	public float startPoundingFront = 25f;

	public float startPoundingBehind = 25f;

	public float chaseRadius = 10f;

	public float attackRadius = 2f;

	public float chaseSpeed = 4f;

	public float patrolSpeed = 2.5f;

	public float waitForSex = 3f;

	public float mountRadius = 3f;

	public float regenRate = 10f;

	public float attackDelay = 0.5f;

	public bool patrolWaiting;

	public float totalWaitTime = 3f;

	public float totalWaitTimeOnrest = 3f;

	public float switchProbability = 0.2f;

	public List<Waypoint> patrolPoints;

	private int currentPatrolIndex;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private float distance;

	private float mountDelay;

	private float mountTime = 1f;

	private float fuckTime = 4f;

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

	private float myCum;

	private float pct;

	public float midSpeed = 15f;

	public float highSpeed = 25f;

	private bool soundPlayed;

	private bool tookDmg;

	private bool gotHit;

	private float time;

	public static bool imLickFucking;

	private LickerController thiscontroller;

	private SphereCollider thiscollider;

	private bool dead;

	private bool ignoreSilent;

	private bool lickSoundPlayed;

	private float myPowerRecieved;

	public GameObject enemSaveState;

	private void Start()
	{
		imOnHer = false;
		InventoryUI.heroineIsChased = false;
		thiscontroller = GetComponent<LickerController>();
		target = PlayerManager.instance.player.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		thiscollider = GetComponent<SphereCollider>();
		lickSoundPlayed = false;
		cam = Camera.main.transform;
		myCum = maxCum;
		currentStamina = maxStamina;
		currentCum = 0f;
		currentHealth = maxHealth;
		SexImage.enabled = false;
		circle.enabled = false;
		dead = false;
		Canvas[] array = Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				staminaUi = Object.Instantiate(stamUiPrefab, canvas.transform).transform;
				healthUI = Object.Instantiate(healthUiPrefab, canvas.transform).transform;
				stamSlider = staminaUi.GetChild(0).GetComponent<Image>();
				healthSlider = healthUI.GetChild(0).GetComponent<Image>();
				break;
			}
		}
		healthUI.gameObject.SetActive(value: false);
		imLickFucking = false;
		gotHit = false;
		DebugStuff();
	}

	public void Update()
	{
		distance = Vector3.Distance(target.position, base.transform.position);
		if (distance <= chaseRadius)
		{
			if (!PlayerController.isSilent)
			{
				ignoreSilent = true;
			}
		}
		else
		{
			ignoreSilent = false;
		}
		if (!Safespace.heroineSafe)
		{
			if (ignoreSilent)
			{
				AILogic();
			}
			else if (distance <= attackRadius)
			{
				if (!PlayerController.iFalled && !iRest)
				{
					FaceTarget();
					Attack();
					ignoreSilent = true;
				}
			}
			else
			{
				Patrolling();
			}
		}
		else if (!tookDmg)
		{
			if (imOnHer)
			{
				anim.SetBool("isIdle", value: true);
				anim.SetBool("isWalking", value: false);
				imOnHer = false;
			}
			Patrolling();
		}
		else
		{
			AILogic();
		}
		if (staminaUi != null)
		{
			staminaUi.position = targetStaminaUi.position;
			staminaUi.forward = -cam.forward;
		}
		if (healthUI != null)
		{
			healthUI.position = targetHealthUi.position;
			healthUI.forward = -cam.forward;
		}
		if (iRest)
		{
			tookDmg = false;
		}
		if (currentStamina >= 0.1f)
		{
			noStamina = false;
		}
		if (currentStamina >= maxStamina)
		{
			iRest = false;
			anim.SetBool("isResting", value: false);
		}
		if (currentStamina <= 0f)
		{
			noStamina = true;
		}
		if (currentCum >= maxCum)
		{
			cumming = true;
		}
		if (cumming)
		{
			maxCum = 100f;
			DrainCum(15f);
			drainStamina(10f);
			Cumming();
			HeroineStats.creampied = true;
			heroineAnim.SetBool("LickLickerCum", value: true);
			if (!impregRolette)
			{
				int num = Random.Range(1, 100);
				if (impregnateChance >= num)
				{
					HeroineStats.pregnant = true;
					heroineAnim.SetBool("isPregnant", value: true);
				}
				impregRolette = true;
			}
		}
		if (imOnHer && currentCum <= 0f)
		{
			maxCum = myCum;
			cumming = false;
			PlayerController.heIsFuckingHard = false;
			impregRolette = false;
		}
		if (iRest)
		{
			currentCum = 0f;
		}
		if (iInsert || iFuck)
		{
			RecieveDamage();
			InitiateUI();
			healthUI.gameObject.SetActive(value: false);
		}
		if (gotHit)
		{
			anim.SetBool("gotHit", value: true);
			agent.isStopped = true;
			time += Time.deltaTime;
			if (time >= 0.3f)
			{
				anim.SetBool("gotHit", value: false);
				time = 0f;
				agent.isStopped = false;
				gotHit = false;
			}
		}
		if (cumming && !cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (!cumming)
		{
			cumSound.Stop();
		}
		ClampStats();
	}

	private void AILogic()
	{
		if (HeroineStats.birth)
		{
			Patrolling();
			return;
		}
		if (distance <= chaseRadius && distance > attackRadius)
		{
			if (!iRest && !iAttacked)
			{
				Chase();
			}
		}
		else
		{
			iChase = false;
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
		if (distance <= mountRadius && PlayerController.iFalled && !iRest && imOnHer)
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
			DisableUI();
			mountDelay = 0f;
			anim.SetBool("HLickFuckOrg", value: false);
			anim.SetBool("isLickFucking", value: false);
			anim.SetBool("isLicking", value: false);
			anim.SetBool("isCumming", value: false);
			anim.SetBool("isBeforeLicking", value: false);
			heroineAnim.SetBool("LickBeforeLicking", value: false);
			heroineAnim.SetBool("LickLicking", value: false);
			heroineAnim.SetBool("LickLickFuckOrg", value: false);
			heroineAnim.SetBool("LickLickFucking", value: false);
			CameraFollow.target = Heroine.gameObject.transform;
			iAttacked = false;
			iInsert = false;
			iRest = true;
			Rest();
		}
		if (iRest)
		{
			PatrolRest();
		}
	}

	private void Chase()
	{
		if (!InventoryUI.heroineIsChased)
		{
			if (!PlayerManager.heroineIsMounted)
			{
				imOnHer = true;
			}
			InventoryUI.heroineIsChased = true;
		}
		if (imOnHer && !iAttacked)
		{
			iChase = true;
			agent.SetDestination(target.position);
			agent.speed = chaseSpeed;
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

	private void Cumming()
	{
		HeroineStats.fartiged = false;
		anim.SetBool("isCumming", value: true);
	}

	private void Patrolling()
	{
		agent.speed = patrolSpeed;
		staminaUi.gameObject.SetActive(value: false);
		iChase = false;
		iPatrol = true;
		if (travelling && agent.remainingDistance <= 0.1f)
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

	private void PatrolRest()
	{
		agent.speed = patrolSpeed;
		staminaUi.gameObject.SetActive(value: false);
		if (travelling && agent.remainingDistance <= 0.1f)
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
			if (waitTimer >= totalWaitTimeOnrest)
			{
				waiting = false;
				ChangePatrolPoint();
				SetDestination();
			}
		}
		gainStamina(regenRate);
	}

	private void Attack()
	{
		FaceTarget();
		StartCoroutine(Attack());
		IEnumerator Attack()
		{
			if (!iAttacked)
			{
				anim.SetBool("isIdle", value: true);
				anim.SetBool("isWalking", value: false);
				anim.SetBool("isAttacking", value: true);
				iAttacked = true;
			}
			agent.velocity = Vector3.zero;
			if (!soundPlayed)
			{
				attackSound.Play();
				soundPlayed = true;
			}
			yield return new WaitForSeconds(attackDelay);
			anim.SetBool("isAttacking", value: false);
			soundPlayed = false;
			iAttacked = false;
		}
	}

	private void Mount()
	{
		mountDelay += Time.deltaTime;
		anim.SetBool("isAttacking", value: false);
		iAttacked = false;
		unfuck = false;
		if (mountDelay >= mountTime && !iFuck)
		{
			iMount = true;
			agent.enabled = false;
			GetComponent<CapsuleCollider>().isTrigger = true;
			PlayerManager.heroineIsMounted = true;
			if (PlayerController.iFalledFront)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
				anim.SetBool("isBeforeLicking", value: true);
				if (iMount)
				{
					heroineAnim.SetBool("LickBeforeLicking", value: true);
				}
				anim.SetBool("isWalking", value: false);
			}
			anim.SetBool("isIdle", value: false);
			iInsert = true;
			SexImage.enabled = true;
			circle.enabled = true;
			pct = mountDelay / fuckTime;
			circle.fillAmount = pct;
			PlayerController.iGetInserted = true;
		}
		if (mountDelay >= fuckTime)
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
			CameraFollow.target = camPos.transform;
			mountDelay = 0f;
			Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
			iInsert = false;
			heroineAnim.SetBool("LickLickerCum", value: false);
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
			if (!lickSoundPlayed)
			{
				lickSound.Play();
				lickSoundPlayed = true;
			}
			anim.SetBool("isLicking", value: true);
			anim.SetBool("isBeforeLicking", value: false);
			heroineAnim.SetBool("LickBeforeLicking", value: false);
			heroineAnim.SetBool("LickLicking", value: true);
			anim.SetBool("isWalking", value: false);
			StartCoroutine(StartLickFucking());
		}
	}

	private IEnumerator StartLickFucking()
	{
		yield return new WaitForSeconds(15f);
		if (iFuck)
		{
			lickSound.Stop();
			anim.SetBool("isLicking", value: false);
			heroineAnim.SetBool("LickBeforeLicking", value: false);
			imLickFucking = true;
			anim.SetBool("isLickFucking", value: true);
			heroineAnim.SetBool("LickLickFucking", value: true);
			if (HeroineStats.orgasm)
			{
				anim.SetBool("HLickFuckOrg", value: true);
				heroineAnim.SetBool("LickLickFuckOrg", value: true);
				HeroineStats.stunned = true;
				HeroineStats.currentPower = 0f;
				Heroine.GetComponent<HeroineStats>().UpdateStats();
			}
			else
			{
				anim.SetBool("HLickFuckOrg", value: false);
				heroineAnim.SetBool("LickLickFuckOrg", value: false);
			}
		}
	}

	private void Rest()
	{
		iFuck = false;
		imLickFucking = false;
		agent.enabled = true;
		SexImage.enabled = false;
		circle.enabled = false;
		GetComponent<CapsuleCollider>().isTrigger = false;
		anim.speed = 1f;
		PlayerManager.heroineIsMounted = false;
		if (!unfuck)
		{
			GetComponent<SphereCollider>().isTrigger = false;
			PlayerController.iGetInserted = false;
			PlayerController.iGetFucked = false;
			PlayerController.animator.speed = 1f;
			HeroineStats.stunned = false;
			lickSoundPlayed = false;
			if (InventoryUI.heroineIsChased)
			{
				imOnHer = false;
				InventoryUI.heroineIsChased = false;
			}
			unfuck = true;
		}
		gainStamina(regenRate);
	}

	private void SetDestination()
	{
		if (patrolPoints != null)
		{
			agent.isStopped = false;
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
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 50f);
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
		EnemyUI.instance.TakeDamage(drainValue * Time.deltaTime);
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
		_ = currentCum / maxCum;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		_ = currentCum / maxCum;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
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
			if (iFuck)
			{
				myPowerRecieved = powerRecieved;
				drainStamina(myPowerRecieved);
				if (currentStamina <= 0f)
				{
					lickSound.Stop();
				}
				gotAttacked = true;
			}
			else if (iInsert)
			{
				myPowerRecieved = powerRecieved;
				drainStamina(myPowerRecieved);
				gotAttacked = true;
			}
			else
			{
				myPowerRecieved = 0f;
			}
		}
		if (gotAttacked)
		{
			powerRecieved = 0f;
			myPowerRecieved = 0f;
			gotAttacked = false;
		}
	}

	public void ThrustEvent()
	{
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		if (!HeroineStats.GameOver)
		{
			GainCumInstant(gainCumPerThrust);
		}
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
	}

	public void LickEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 4f);
		lickSexSound.Play();
	}

	public void StepEvent()
	{
		stepSound.Play();
	}

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
		gotHit = true;
		tookDmg = true;
		drainHealth(amount);
		currentStamina = maxStamina;
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
		InventoryUI.heroineIsChased = false;
		anim.SetBool("dead", value: true);
		agent.enabled = false;
		dead = true;
		thiscollider.enabled = false;
		thiscontroller.enabled = false;
		enemSaveState.GetComponent<EnemSavState>().isDead = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player") || dead)
		{
			return;
		}
		Debug.Log("ENEMY HIT!");
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
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
			PlayerController.gotHitFront = true;
			if (!InventoryUI.heroineIsChased)
			{
				if (!PlayerManager.heroineIsMounted)
				{
					imOnHer = true;
				}
				InventoryUI.heroineIsChased = true;
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
