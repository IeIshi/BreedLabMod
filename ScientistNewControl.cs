using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ScientistNewControl : MonoBehaviour
{
	private GameObject Heroine;

	private GameObject gameManager;

	public Image circle;

	public Image SexImage;

	public Transform targetStaminaUI;

	public Transform targetHealthUI;

	public GameObject stamUIPrefab;

	public GameObject healthUIPrefab;

	public Transform camBJ;

	public Transform camBehind;

	public Transform camRide;

	public AudioSource bjSoundOne;

	public AudioSource bjSoundTwo;

	public AudioSource sexSoundOne;

	public AudioSource bodyCollide;

	public AudioSource sexHardSound;

	public AudioSource cumSound;

	public AudioSource stepSound;

	private Transform staminaUI;

	private Transform cam;

	private Image stamSlider;

	public float maxCum;

	private float currentCum;

	public float maxStamina;

	public float currentStamina;

	public float patrolSpeed = 2.5f;

	public bool patrolWaiting;

	public float totalWaitTime = 3f;

	public float switchProbability = 0.2f;

	public List<Waypoint> patrolPoints;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private int currentPatrolIndex;

	public float attackRange;

	public float chargeRange;

	public float chargeSpeed;

	private float distance;

	private bool followed;

	private bool attacked;

	private bool reached;

	private bool added;

	public float drainStamPerThrust = 10f;

	public float gainCumPerThrust = 5f;

	public float thrustDmg = 0.5f;

	public float teaseDmg = 0.5f;

	public float cumDrainSpeed = 10f;

	public float staminaRegRate = 0.5f;

	public float speedIncreaseCumTime = 50f;

	private float fuckDelay;

	public float fuckTime;

	private float pct;

	public static float powerRecieved;

	private bool gotAttacked;

	public float expPerThrust = 2f;

	private bool cumming;

	private bool exhausted;

	private bool masturbating;

	private bool alreadyUsed;

	public bool hitFront;

	public bool hitBack;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	private EnemyFieldOfView controlInstance;

	private Transform anim_pos;

	public Transform mount_place;

	public Transform mount_place_behind;

	private bool sexyTime;

	private float mountWaitTime = 2f;

	private float remainingWaitTime;

	public bool getCowGirlAction;

	private bool camShakeCalled;

	private bool blowJob;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private CapsuleCollider capCollider;

	private void Start()
	{
		hitFront = false;
		hitBack = false;
		controlInstance = GetComponent<EnemyFieldOfView>();
		capCollider = GetComponent<CapsuleCollider>();
		circle.enabled = false;
		SexImage.enabled = false;
		fuckDelay = 0f;
		Canvas[] array = Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				staminaUI = Object.Instantiate(stamUIPrefab, canvas.transform).transform;
				stamSlider = staminaUI.GetChild(0).GetComponent<Image>();
				break;
			}
		}
		cam = Camera.main.transform;
		staminaUI.gameObject.SetActive(value: false);
		currentStamina = maxStamina;
		currentCum = 0f;
		remainingWaitTime = 0f;
		Heroine = GameObject.Find("Heroine");
		gameManager = GameObject.Find("Game Manager");
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		anim_pos = base.gameObject.transform;
		if (GetComponent<EnemyFieldOfView>().isDed)
		{
			anim.SetBool("isDed", value: true);
			GetComponent<Rigidbody>().detectCollisions = false;
			base.enabled = false;
		}
		else
		{
			PatrolOnStart();
		}
	}

	private void Update()
	{
		if (GetComponent<EnemyFieldOfView>().isDed)
		{
			InventoryUI.heroineIsChased = false;
			agent.ResetPath();
			agent.isStopped = true;
			GetComponent<Rigidbody>().detectCollisions = false;
			base.enabled = false;
			return;
		}
		CheckState(currentCum, currentStamina);
		WalkOrIdle();
		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
		currentCum = Mathf.Clamp(currentCum, 0f, maxCum);
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance <= attackRange)
		{
			capCollider.isTrigger = true;
		}
		else
		{
			capCollider.isTrigger = false;
		}
		RestOrReady();
		if (GetComponent<EnemyFieldOfView>().gotHit)
		{
			currentStamina = maxStamina;
			Chase();
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

	private void Patrolling()
	{
		agent.isStopped = false;
		agent.speed = patrolSpeed;
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

	private void PatrolOnStart()
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

	private void RestOrReady()
	{
		if (HeroineStats.birth)
		{
			Masturbate();
		}
		else if (AbortBed.abortBedSitting && distance <= attackRange)
		{
			Masturbate();
		}
		else if (exhausted)
		{
			if (!getCowGirlAction)
			{
				if (distance <= attackRange && HeroineStats.horny && !PlayerController.iFalled && !alreadyUsed && PlayerManager.instance.lastScientistWhoFuckedMe == base.gameObject)
				{
					getCowGirlAction = true;
				}
				Masturbate();
			}
			else
			{
				masturbating = false;
				sexyTime = true;
				PlayerController.claimed = true;
				RideSex();
				HideWorldSpaceUI();
			}
		}
		else if (!getCowGirlAction)
		{
			ReadyState();
			HideWorldSpaceUI();
		}
		else
		{
			RideSex();
		}
	}

	private void ShowWorldSpaceUI()
	{
		staminaUI.gameObject.SetActive(value: true);
		if (staminaUI != null)
		{
			staminaUI.position = targetStaminaUI.position;
			staminaUI.forward = -cam.forward;
			stamSlider.fillAmount = currentStamina / maxStamina;
		}
	}

	private void HideWorldSpaceUI()
	{
		staminaUI.gameObject.SetActive(value: false);
	}

	private void ReadyState()
	{
		if (!sexyTime)
		{
			PatrolOrChase(controlInstance.heroineIsVisible, followed);
			remainingWaitTime = 0f;
			masturbating = false;
			alreadyUsed = false;
			if (!reached || !PlayerController.iFalled)
			{
				return;
			}
			if (GetComponent<EnemyFieldOfView>().claimedHer && PlayerManager.instance.enemyTurnOrder[0] != base.gameObject)
			{
				if (PlayerManager.instance.enemyTurnOrder[0].gameObject.GetComponent<EnemyFieldOfView>().claimedHer)
				{
					GetComponent<EnemyFieldOfView>().claimedHer = false;
				}
				else
				{
					sexyTime = true;
					PlayerController.claimed = true;
				}
			}
			if (!PlayerController.claimed)
			{
				PlayerController.claimed = true;
				GetComponent<EnemyFieldOfView>().claimedHer = true;
				sexyTime = true;
			}
			return;
		}
		remainingWaitTime += Time.deltaTime * 1f;
		if (remainingWaitTime >= mountWaitTime)
		{
			if (PlayerController.iGetFucked)
			{
				VignetteEffect();
				BehindSex();
			}
			else
			{
				BlowjobOrBehindTease();
				PlayerManager.instance.lastScientistWhoFuckedMe = base.gameObject;
			}
		}
	}

	private void Masturbate()
	{
		if (sexyTime)
		{
			Release();
			sexyTime = false;
		}
		masturbating = true;
		currentCum = 0f;
		ShowWorldSpaceUI();
		MasturbateAnim();
		agent.ResetPath();
		agent.isStopped = true;
		if (!(distance <= attackRange) || !HeroineStats.horny)
		{
			if (!HeroineStats.GameOver)
			{
				GainStamina(staminaRegRate / 2f);
			}
			else
			{
				GainStamina(staminaRegRate * 2f);
			}
		}
	}

	private void Release()
	{
		DisableUI();
		heroineAnim.SetBool("ScBJCum", value: false);
		heroineAnim.SetBool("ScRideCum", value: false);
		heroineAnim.SetBool("ScDickInsert", value: false);
		heroineAnim.SetBool("dickRideHard", value: false);
		heroineAnim.SetBool("ScBehindCum", value: false);
		heroineAnim.SetBool("ScBehindFuck", value: false);
		heroineAnim.SetBool("behindGrinded", value: false);
		heroineAnim.SetBool("ScBJ", value: false);
		anim.SetBool("isBjCum", value: false);
		anim.SetBool("isFrontCum", value: false);
		anim.SetBool("isFucked", value: false);
		anim.SetBool("isPumping", value: false);
		anim.SetBool("isBehindSex", value: false);
		anim.SetBool("isBehindGrind", value: false);
		anim.SetBool("isBehindCum", value: false);
		PlayerController.iGetFucked = false;
		PlayerController.iGetInserted = false;
		PlayerController.claimed = false;
		followed = false;
		blowJob = false;
		CameraFollow.target = Heroine.transform;
		if (getCowGirlAction)
		{
			if (HeroineStats.GameOver)
			{
				heroineAnim.SetBool("isFalledBack", value: true);
				heroineAnim.SetBool("falled", value: false);
			}
			else
			{
				PlayerController.iFalled = false;
				alreadyUsed = true;
			}
			getCowGirlAction = false;
		}
		camShakeCalled = false;
		reached = false;
		RemoveMeFromList();
		anim.speed = 1f;
		heroineAnim.speed = 1f;
		HeroineStats.aroused = false;
		PlayerController.heIsFuckingHard = false;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		float num = Random.Range(0.3f, 0.7f);
		anim_pos.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
		SexImage.enabled = false;
		circle.enabled = false;
	}

	private void PatrolOrChase(bool visible, bool followed)
	{
		if (visible)
		{
			if (!followed)
			{
				WatchThenChase();
				return;
			}
			ChaseOrWatch();
			AddMeOnList();
		}
		else if (followed)
		{
			LastSeenWalkThenPatrol();
		}
		else
		{
			Patrolling();
			RemoveMeFromList();
		}
	}

	private void WatchThenChase()
	{
		FaceTarget();
		agent.ResetPath();
		agent.isStopped = true;
		StartCoroutine(StartFollowDelay());
	}

	private void LastSeenWalkThenPatrol()
	{
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			if (agent.velocity.magnitude == 0f)
			{
				IdleAnim();
			}
			PatrolOnStart();
			agent.ResetPath();
			agent.isStopped = true;
			StartCoroutine(StartPatrol());
		}
	}

	private void ChaseOrWatch()
	{
		if (PlayerController.claimed && !sexyTime)
		{
			Watch();
		}
		else if (HeroineStats.horny && PlayerController.iFalled)
		{
			if (!sexyTime)
			{
				Watch();
			}
		}
		else
		{
			Chase();
		}
	}

	private void Watch()
	{
		FaceTarget();
		if (distance < 5f)
		{
			agent.isStopped = true;
		}
		else
		{
			agent.SetDestination(Heroine.transform.position);
		}
		if (HeroineStats.horny)
		{
			Masturbate();
		}
	}

	private void Chase()
	{
		agent.SetDestination(Heroine.transform.position);
		FaceTarget();
		if (!reached)
		{
			agent.isStopped = false;
		}
		if (agent.remainingDistance < chargeRange)
		{
			agent.speed = chargeSpeed;
			Debug.Log("charging");
		}
		else
		{
			agent.speed = 3.5f;
		}
		if (agent.remainingDistance > 0f && agent.remainingDistance <= attackRange)
		{
			reached = true;
			agent.speed = 1.5f;
			if (agent.remainingDistance < 0.4f)
			{
				agent.isStopped = true;
			}
		}
		if (agent.remainingDistance > attackRange && reached)
		{
			StartCoroutine(StartFollowDelay());
			agent.speed = 3.5f;
		}
	}

	private IEnumerator StartFollowDelay()
	{
		yield return new WaitForSeconds(2f);
		reached = false;
		followed = true;
		agent.isStopped = false;
	}

	private IEnumerator StartPatrol()
	{
		yield return new WaitForSeconds(2f);
		followed = false;
		SetDestination();
	}

	private void AddMeOnList()
	{
		if (!added)
		{
			PlayerManager.instance.enemyTurnOrder.Add(base.gameObject);
			InventoryUI.heroineIsChased = true;
			added = true;
		}
	}

	private void RemoveMeFromList()
	{
		if (!added)
		{
			return;
		}
		for (int i = 0; i < PlayerManager.instance.enemyTurnOrder.Count; i++)
		{
			if (PlayerManager.instance.enemyTurnOrder[i] == base.gameObject)
			{
				PlayerManager.instance.enemyTurnOrder.RemoveAt(i);
				InventoryUI.heroineIsChased = false;
				added = false;
			}
		}
	}

	private void BlowjobOrBehindTease()
	{
		agent.ResetPath();
		agent.isStopped = true;
		PlayerController.iGetInserted = true;
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		if (PlayerController.iFalledFront)
		{
			TakeSexPos();
			ShakeCam();
			CameraFollow.target = camBJ;
			InitiateUI();
			if (!cumming)
			{
				blowJob = true;
				BlowjobAnim();
				VignetteEffect();
			}
			else
			{
				BJCumAnim();
				PlayerController.heIsFuckingHard = true;
				HeroineStats.oralCreampie = true;
				DrainCum(cumDrainSpeed);
			}
		}
		if (PlayerController.iFalledBack)
		{
			TakeBackSexPos();
			ShakeCam();
			InitiateUI();
			InitiateSex();
			BackTeaseAnim();
			if (!PlayerManager.SAB)
			{
				heroineAnim.SetBool("isScared", value: true);
			}
			else
			{
				heroineAnim.SetBool("isHorny", value: true);
			}
		}
	}

	private void BehindSex()
	{
		fuckDelay = 0f;
		TakeBackSexPos();
		CameraFollow.target = camBehind;
		InitiateUI();
		alreadyUsed = true;
		PlayerManager.IsVirgin = false;
		if (!PlayerController.iFalledBack)
		{
			return;
		}
		if (!cumming)
		{
			heroineAnim.SetBool("isScared", value: false);
			heroineAnim.SetBool("isHorny", value: false);
			HeroineStats.aroused = true;
			BehindSexAnim();
			return;
		}
		BackCumAnim();
		CumSoundPlay();
		DrainCum(cumDrainSpeed);
		if (!PlayerManager.SAB)
		{
			HeroineStats.pregnant = false;
			heroineAnim.SetBool("isPregnant", value: false);
		}
		HeroineStats.fertileCum = false;
		HeroineStats.lustyCum = true;
		HeroineStats.creampied = true;
		if (!HeroineStats.GameOver)
		{
			DrainStamina(cumDrainSpeed - 9f);
		}
	}

	private void RideSex()
	{
		TakeSexPos();
		CameraFollow.target = camRide;
		InitiateUI();
		PlayerController.iFalled = true;
		PlayerManager.IsVirgin = false;
		if (!PlayerController.iGetFucked)
		{
			currentStamina = 0f;
			PlayerController.iGetFucked = true;
		}
		if (!cumming)
		{
			if (exhausted)
			{
				RideSexAnim();
				GainStamina(staminaRegRate);
			}
			else
			{
				HardRideAnim();
			}
		}
		else
		{
			RideCumAnim();
			DrainCum(cumDrainSpeed);
			HeroineStats.pregnant = false;
			heroineAnim.SetBool("isPregnant", value: false);
			HeroineStats.fertileCum = false;
			HeroineStats.lustyCum = true;
			HeroineStats.creampied = true;
			if (!HeroineStats.GameOver)
			{
				DrainStamina(cumDrainSpeed);
			}
			CumSoundPlay();
			HeroineStats.pregnant = false;
			if (currentCum <= 0f)
			{
				Release();
				cumSound.Stop();
			}
		}
		if (gameManager.GetComponent<EquipmentManager>().currentEquipment[3] != null)
		{
			gameManager.GetComponent<EquipmentManager>().RipPantsu();
		}
	}

	private void ShakeCam()
	{
		if (!camShakeCalled)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.1f, 0.1f, 1.5f, 0.5f, 0.1f, 0.1f, 0.1f, 0.2f));
			camShakeCalled = true;
		}
	}

	private void InitiateSex()
	{
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg * 5f);
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(teaseDmg * 4f);
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
			PlayerController.iGetFucked = true;
			PlayerController.iGetInserted = false;
			PlayerManager.IsVirgin = false;
		}
	}

	private void WalkAnim()
	{
		anim.SetBool("isIdle", value: false);
		anim.SetBool("isWalking", value: true);
		anim.SetBool("isMasturbating", value: false);
	}

	private void IdleAnim()
	{
		anim.SetBool("isIdle", value: true);
		anim.SetBool("isWalking", value: false);
		anim.SetBool("isMasturbating", value: false);
	}

	private void WalkOrIdle()
	{
		if (agent.velocity.magnitude < 0.8f)
		{
			IdleAnim();
		}
		else
		{
			WalkAnim();
		}
	}

	private void MasturbateAnim()
	{
		agent.isStopped = true;
		anim.SetBool("isMasturbating", value: true);
	}

	private void RideSexAnim()
	{
		anim.SetBool("isFucked", value: true);
		heroineAnim.SetBool("ScDickInsert", value: true);
	}

	private void HardRideAnim()
	{
		anim.SetBool("isPumping", value: true);
		heroineAnim.SetBool("dickRideHard", value: true);
	}

	private void BlowjobAnim()
	{
		anim.Play("Blowjob");
		heroineAnim.Play("rig|Blowjob");
		heroineAnim.SetBool("ScBJ", value: true);
	}

	private void BJCumAnim()
	{
		CumSoundPlay();
		anim.SetBool("isBjCum", value: true);
		heroineAnim.SetBool("ScBJCum", value: true);
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg * 10f);
		DrainStamina(15f);
	}

	private void BackTeaseAnim()
	{
		anim.SetBool("isBehindGrind", value: true);
		heroineAnim.SetBool("behindGrinded", value: true);
	}

	private void BehindSexAnim()
	{
		anim.SetBool("isBehindSex", value: true);
		anim.SetBool("isBehindCum", value: false);
		heroineAnim.SetBool("ScBehindCum", value: false);
		heroineAnim.SetBool("ScBehindFuck", value: true);
	}

	private void BackCumAnim()
	{
		anim.SetBool("isBehindCum", value: true);
		heroineAnim.SetBool("ScBehindCum", value: true);
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg * 10f);
	}

	private void RideCumAnim()
	{
		anim.SetBool("isFrontCum", value: true);
		heroineAnim.SetBool("ScRideCum", value: true);
	}

	private void TakeSexPos()
	{
		anim_pos.rotation = mount_place.rotation;
		anim_pos.position = mount_place.position;
	}

	private void TakeBackSexPos()
	{
		anim_pos.rotation = mount_place_behind.rotation;
		anim_pos.position = mount_place_behind.position;
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 4f);
		if (normalized.x == 0f)
		{
			GetComponent<Rigidbody>().AddForce(-base.transform.forward * 1000f);
		}
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

	private void CheckState(float cum, float stamina)
	{
		if (cum == maxCum)
		{
			cumming = true;
		}
		else if (cum <= 0f)
		{
			cumming = false;
			cumSound.Stop();
		}
		if (stamina <= 0f)
		{
			if (!cumming)
			{
				exhausted = true;
			}
		}
		else if (stamina >= maxStamina)
		{
			exhausted = false;
		}
		if (!PlayerController.iFalled)
		{
			GetComponent<EnemyFieldOfView>().claimedHer = false;
			sexyTime = false;
		}
		if (!sexyTime)
		{
			return;
		}
		if (currentCum > speedIncreaseCumTime && !cumming)
		{
			anim.speed = 1.5f;
			heroineAnim.speed = 1.5f;
			if (!blowJob)
			{
				PlayerController.heIsFuckingHard = true;
			}
			HeroineStats.aroused = true;
		}
		else
		{
			anim.speed = 1f;
			heroineAnim.speed = 1f;
			PlayerController.heIsFuckingHard = false;
		}
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void DrainStamina(float drainValue)
	{
		currentStamina -= drainValue * Time.deltaTime;
		EnemyUI.instance.TakeDamage(drainValue * Time.deltaTime);
	}

	public void GainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
	}

	private void CumSoundPlay()
	{
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
	}

	public void ThrustEvent()
	{
		thrusted = true;
		if (PlayerController.heIsFuckingHard)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		if (getCowGirlAction)
		{
			if (exhausted)
			{
				GainCumInstant(gainCumPerThrust / 10f);
				sexSoundOne.Play();
			}
			else
			{
				GainCumInstant(gainCumPerThrust);
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				sexHardSound.Play();
			}
		}
		else
		{
			GainCumInstant(gainCumPerThrust);
			if (!HeroineStats.GameOver)
			{
				DrainStaminaInstant(drainStamPerThrust);
			}
			sexSoundOne.Play();
		}
		bodyCollide.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
	}

	public void CumEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
	}

	public void StepEvent()
	{
		stepSound.Play();
	}

	public void BJThrust()
	{
		thrusted = true;
		int num = Random.Range(1, 3);
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
		Heroine.GetComponent<HeroineStats>().GainExp(5f);
		if (num == 1)
		{
			bjSoundOne.Play();
		}
		else
		{
			bjSoundTwo.Play();
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (base.enabled && !masturbating && !PlayerController.iFalled && other.tag == "Player")
		{
			CheckHitAngle();
			PlayerController.iFalled = true;
			GetComponent<EnemyFieldOfView>().claimedHer = true;
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

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, chargeRange);
	}
}
