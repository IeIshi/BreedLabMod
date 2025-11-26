using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HumanoidController : MonoBehaviour
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

	public GameObject enemSaveState;

	public GameObject Heroine;

	public GameObject gameManager;

	public AudioSource sexSound;

	public AudioSource sexHardSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public AudioSource stepSound;

	public AudioSource attackSound;

	public AudioSource growlSound;

	public AudioSource firstInsertSound;

	public Image circle;

	public Image SexImage;

	private Image humImage;

	private Transform target;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	private Transform staminaUi;

	private Transform cumUI;

	private Transform healthUI;

	private Transform cam;

	private Image stamSlider;

	private Image cumSlider;

	private Image healthSlider;

	public float maxStamina;

	public float maxCum;

	public float maxHealth = 50f;

	public float startSexLust = 10f;

	public float teaseDmg = 2.5f;

	public float thrustDmg = 0.05f;

	public int impregnateChance = 10;

	public float gainCumPerThrust = 4f;

	public float drainStamPerThrust = 5f;

	public float expPerThrust = 1.5f;

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

	private float fuckDelay;

	private float mountTime = 1f;

	private float fuckTime = 5f;

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

	private bool unfuck;

	public bool imSuper;

	private float myCum;

	private float pct;

	public float midSpeed = 15f;

	public float highSpeed = 25f;

	private bool soundPlayed;

	private bool tookDmg;

	private bool firstInsert;

	private float fartigeCd;

	public float fartigeTime;

	public float fartigeSpeed = 1.5f;

	private bool attackFartige;

	private bool thrusted;

	private bool max;

	public GameObject camPosFront;

	public GameObject camPosBehind;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private CapsuleCollider capCollider;

	private bool pathReseted;

	private void Start()
	{
		if (enemSaveState.GetComponent<EnemSavState>().isDead)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		imOnHer = false;
		InventoryUI.heroineIsChased = false;
		capCollider = GetComponent<CapsuleCollider>();
		target = PlayerManager.instance.player.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		cam = Camera.main.transform;
		myCum = maxCum;
		currentStamina = maxStamina;
		currentCum = 0f;
		currentHealth = maxHealth;
		SexImage.enabled = false;
		circle.enabled = false;
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
		humImage = GameObject.Find("ManagerAndUI/UI/Canvas/Inventory/HumSprite").GetComponent<Image>();
	}

	public void Update()
	{
		distance = Vector3.Distance(target.position, base.transform.position);
		if (distance <= attackRadius)
		{
			capCollider.enabled = false;
		}
		else
		{
			capCollider.enabled = true;
		}
		if (!Safespace.heroineSafe)
		{
			if (pathReseted)
			{
				pathReseted = false;
			}
			AILogic();
		}
		else if (!tookDmg)
		{
			if (imOnHer)
			{
				anim.SetBool("isIdle", value: true);
				anim.SetBool("isWalking", value: false);
				imOnHer = false;
			}
			if (!pathReseted)
			{
				agent.ResetPath();
				pathReseted = true;
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
		if (!iRest && !InventoryUI.heroineIsChased && distance <= chaseRadius && !Safespace.heroineSafe)
		{
			imOnHer = true;
		}
		if (currentStamina <= 0f)
		{
			noStamina = true;
		}
		if (currentCum >= maxCum)
		{
			cumming = true;
			HeroineStats.HumanoidBuff = true;
			HeroineStats.creampied = true;
			HeroineStats.hugeAmount = true;
			HeroineStats.fertileCum = true;
			humImage.enabled = true;
		}
		if (cumming)
		{
			maxCum = 100f;
			DrainCum(10f);
			heroineAnim.SetBool("isCumFilled", value: true);
			if (!HeroineStats.GameOver)
			{
				drainStamina(0.05f);
			}
			Cumming();
			if (!PlayerManager.SAB)
			{
				heroineAnim.SetBool("isScared", value: true);
			}
			else
			{
				heroineAnim.SetBool("isHorny", value: true);
			}
		}
		if (imOnHer && currentCum <= 0f)
		{
			maxCum = myCum;
			cumming = false;
			PlayerController.heIsFuckingHard = false;
		}
		if (iRest)
		{
			currentCum = 0f;
			cumSlider.fillAmount = CalculateCum();
		}
		if (iInsert || iFuck)
		{
			healthUI.gameObject.SetActive(value: false);
		}
		else
		{
			cumUI.gameObject.SetActive(value: false);
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

	private void AILogic()
	{
		if (HeroineStats.birth)
		{
			Patrolling();
			return;
		}
		if (distance <= chaseRadius && distance > attackRadius && !iRest && !iAttacked)
		{
			Chase();
		}
		else
		{
			iChase = false;
		}
		if (distance <= attackRadius && !PlayerController.iFalled && !iRest)
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
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			GetComponent<CapsuleCollider>().isTrigger = true;
			InitiateUI();
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
			CameraFollow.target = Heroine.gameObject.transform;
			iAttacked = false;
			iInsert = false;
			iRest = true;
			GetComponent<CapsuleCollider>().isTrigger = false;
			DisableUI();
			firstInsert = false;
			Rest();
		}
		if (iRest)
		{
			PatrolRest();
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

	private void Chase()
	{
		if (!InventoryUI.heroineIsChased)
		{
			imOnHer = true;
			if (!growlSound.isPlaying)
			{
				growlSound.Play();
			}
			InventoryUI.heroineIsChased = true;
		}
		if (imOnHer && !iAttacked)
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

	private void Cumming()
	{
		if (PlayerController.iFalledFront)
		{
			if (!PlayerController.heIsFuckingHard)
			{
				anim.SetBool("isCummingFront1", value: true);
				anim.SetBool("isSexingFront1", value: false);
				heroineAnim.SetBool("HumCumFront1", value: true);
				heroineAnim.SetBool("HumSexFront1", value: false);
			}
			else
			{
				anim.SetBool("isCummingFront2", value: true);
				anim.SetBool("isSexingFront2", value: false);
				heroineAnim.SetBool("HumCumFront2", value: true);
				heroineAnim.SetBool("HumSexFront2", value: false);
			}
		}
		if (PlayerController.iFalledBack)
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
			if (!PlayerController.heIsFuckingHard)
			{
				anim.SetBool("isSexingBehind1", value: false);
				anim.SetBool("isCummingBehind1", value: true);
				heroineAnim.SetBool("HumCumBehind1", value: true);
				heroineAnim.SetBool("HumSexBehind1", value: false);
			}
			else
			{
				anim.SetBool("isCummingBehind2", value: true);
				anim.SetBool("isSexingBehind2", value: false);
				heroineAnim.SetBool("HumCumBehind2", value: true);
				heroineAnim.SetBool("HumSexBehind2", value: false);
			}
		}
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
		StartCoroutine(Attack());
		IEnumerator Attack()
		{
			FaceTarget();
			if (!iAttacked)
			{
				anim.SetBool("isWalking", value: false);
				anim.SetBool("isAttacking", value: true);
				iAttacked = true;
				fartigeCd = 0f;
				attackFartige = true;
			}
			agent.isStopped = true;
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
		PlayerController.claimed = true;
		unfuck = false;
		if (!(mountDelay >= mountTime) || iFuck)
		{
			return;
		}
		iMount = true;
		agent.enabled = false;
		if (PlayerController.iFalledFront)
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
			CameraFollow.target = camPosFront.transform;
			anim.SetBool("isInsertFront", value: true);
			anim.SetBool("isWalking", value: false);
			heroineAnim.SetBool("HumTeaseFront", value: true);
			if (!PlayerManager.SAB)
			{
				heroineAnim.SetBool("isScared", value: true);
			}
			else
			{
				heroineAnim.SetBool("isHorny", value: true);
			}
		}
		if (PlayerController.iFalledBack)
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
			CameraFollow.target = camPosBehind.transform;
			anim.SetBool("isInsertBehind", value: true);
			anim.SetBool("isWalking", value: false);
			heroineAnim.SetBool("HumTeaseBehind", value: true);
			if (!PlayerManager.SAB)
			{
				heroineAnim.SetBool("isScared", value: true);
			}
			else
			{
				heroineAnim.SetBool("isHorny", value: true);
			}
		}
		anim.SetBool("isIdle", value: false);
		iInsert = true;
		if (EquipmentManager.heroineIsNaked)
		{
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
		PlayerController.iGetInserted = true;
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
		}
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
	}

	private void Fuck()
	{
		if (!iFuck)
		{
			return;
		}
		mountDelay = 0f;
		fuckDelay = 0f;
		VignetteEffect();
		iInsert = false;
		PlayerController.iGetInserted = false;
		HeroineStats.aroused = true;
		PlayerManager.IsVirgin = false;
		if (PlayerController.iFalledFront)
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
			if (!cumming && currentCum < 50f)
			{
				anim.SetBool("isSexingFront1", value: true);
				anim.SetBool("isInsertFront", value: false);
				anim.SetBool("isCummingFront1", value: false);
				anim.SetBool("isCummingFront2", value: false);
				anim.SetBool("isWalking", value: false);
				heroineAnim.SetBool("HumSexFront1", value: true);
				heroineAnim.SetBool("HumCumFront2", value: false);
				heroineAnim.SetBool("HumCumFront1", value: false);
				if (currentCum > midSpeed && currentCum < highSpeed)
				{
					PlayerController.animator.speed = 1.5f;
					anim.speed = 1.5f;
				}
				if (currentCum >= highSpeed)
				{
					PlayerController.animator.speed = 2f;
					PlayerController.heIsFuckingHard = true;
					anim.speed = 2f;
				}
			}
			if (currentCum >= startPoundingFront)
			{
				PlayerController.animator.speed = 1f;
				anim.speed = 1f;
				IFuckHard();
			}
		}
		if (!PlayerController.iFalledBack)
		{
			return;
		}
		anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place_behind.rotation, 0.2f);
		anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place_behind.position, 0.2f);
		if (!cumming && currentCum <= 50f)
		{
			anim.SetBool("isSexingBehind1", value: true);
			anim.SetBool("isInsertBehind", value: false);
			anim.SetBool("isCummingBehind1", value: false);
			anim.SetBool("isCummingBehind2", value: false);
			anim.SetBool("isWalking", value: false);
			heroineAnim.SetBool("HumSexBehind1", value: true);
			heroineAnim.SetBool("HumCumBehind2", value: false);
			if (currentCum > midSpeed && currentCum < highSpeed)
			{
				PlayerController.animator.speed = 1.5f;
				anim.speed = 1.5f;
			}
			if (currentCum >= highSpeed)
			{
				PlayerController.animator.speed = 2f;
				anim.speed = 2f;
				PlayerController.heIsFuckingHard = true;
			}
		}
		if (currentCum >= startPoundingBehind)
		{
			PlayerController.animator.speed = 1f;
			anim.speed = 1f;
			IFuckHard();
		}
	}

	private void IFuckHard()
	{
		PlayerController.heIsFuckingHard = true;
		if (PlayerController.iFalledFront)
		{
			anim.SetBool("isSexingFront1", value: false);
			anim.SetBool("isSexingFront2", value: true);
			heroineAnim.SetBool("HumSexFront2", value: true);
		}
		if (PlayerController.iFalledBack)
		{
			anim.SetBool("isSexingBehind1", value: false);
			anim.SetBool("isSexingBehind2", value: true);
			heroineAnim.SetBool("HumSexBehind2", value: true);
		}
	}

	private void Rest()
	{
		iFuck = false;
		agent.enabled = true;
		SexImage.enabled = false;
		circle.enabled = false;
		anim.speed = 1f;
		if (!unfuck)
		{
			PlayerController.iGetInserted = false;
			PlayerController.iGetFucked = false;
			PlayerController.animator.speed = 1f;
			PlayerController.claimed = false;
			PlayerController.heIsFuckingHard = false;
			HeroineStats.aroused = false;
			heroineAnim.SetBool("HumTeaseFront", value: false);
			heroineAnim.SetBool("HumTeaseBehind", value: false);
			heroineAnim.SetBool("HumSexFront1", value: false);
			heroineAnim.SetBool("HumSexFront2", value: false);
			heroineAnim.SetBool("HumCumFront1", value: false);
			heroineAnim.SetBool("HumCumFront2", value: false);
			heroineAnim.SetBool("HumSexBehind1", value: false);
			heroineAnim.SetBool("HumSexBehind2", value: false);
			heroineAnim.SetBool("HumCumBehind1", value: false);
			heroineAnim.SetBool("HumCumBehind2", value: false);
			heroineAnim.SetBool("isScared", value: false);
			heroineAnim.SetBool("isHorny", value: false);
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			if (InventoryUI.heroineIsChased)
			{
				mountDelay = 0f;
				imOnHer = false;
				InventoryUI.heroineIsChased = false;
				SexImage.enabled = false;
				circle.enabled = false;
			}
			unfuck = true;
		}
		anim.SetBool("isSexingFront1", value: false);
		anim.SetBool("isSexingFront2", value: false);
		anim.SetBool("isInsertFront", value: false);
		anim.SetBool("isInsertBehind", value: false);
		anim.SetBool("isSexingBehind1", value: false);
		anim.SetBool("isSexingBehind2", value: false);
		anim.SetBool("isResting", value: true);
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

	public void ThrustEvent()
	{
		if (!HeroineStats.GameOver)
		{
			drainStamina(drainStamPerThrust);
		}
		if (!firstInsert)
		{
			firstInsertSound.Play();
			firstInsert = true;
		}
		GainCumInstant(gainCumPerThrust);
		thrusted = true;
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		if (EquipmentManager.nekomimiOn)
		{
			GainCumInstant(gainCumPerThrust);
		}
	}

	public void ThrustEventHard()
	{
		if (!HeroineStats.GameOver)
		{
			drainStamina(drainStamPerThrust);
		}
		thrusted = true;
		GainCumInstant(gainCumPerThrust);
		if (EquipmentManager.nekomimiOn)
		{
			GainCumInstant(gainCumPerThrust);
		}
		sexHardSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 2f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg * 2f);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
	}

	public void StepEvent()
	{
		stepSound.Play();
	}

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
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
		cumUI.gameObject.SetActive(value: false);
		InventoryUI.heroineIsChased = false;
		anim.SetBool("dead", value: true);
		enemSaveState.GetComponent<EnemSavState>().isDead = true;
		base.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		base.gameObject.GetComponent<HumanoidController>().enabled = false;
		agent.ResetPath();
		agent.isStopped = true;
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitWolf.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitWolf.enabled = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (iRest || enemSaveState.GetComponent<EnemSavState>().isDead || !(other.tag == "Player"))
		{
			return;
		}
		checkHitAngle();
		if (!iFuck)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
		}
		if (!EquipmentManager.heroineIsNaked && gameManager.GetComponent<EquipmentManager>().currentEquipment[1] != null)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id == 3648532)
			{
				if (!PlayerController.iFalled && !iRest)
				{
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
				}
			}
			else if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id != 3648532)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(1);
			}
		}
		else if (!PlayerController.iFalled && !iRest)
		{
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
