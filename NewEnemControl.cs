using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewEnemControl : MonoBehaviour
{
	private GameObject Heroine;

	private GameObject gameManager;

	public Image circle;

	public Image SexImage;

	public AudioSource sexSoundOne;

	public AudioSource movingSound;

	public AudioSource monsterHit;

	public AudioSource sexSoundNormal;

	public AudioSource sexSoundFast;

	public AudioSource grabSound;

	public AudioSource teaseSound;

	public AudioSource cumSound;

	public Transform targetStaminaUI;

	public Transform targetHealthUI;

	public GameObject stamUIPrefab;

	public GameObject healthUIPrefab;

	public Transform camFollowTarget;

	private Transform staminaUI;

	private Transform cam;

	private Image stamSlider;

	public float maxCum;

	private float currentCum;

	public float maxStamina;

	private float currentStamina;

	private EnemyFieldOfView controlInstance;

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

	private float distance;

	private bool followed;

	private bool attacked;

	private bool reached;

	private bool added;

	public float chaseSpeed = 4.5f;

	public float drainStamPerThrust = 10f;

	public float gainCumPerThrust = 5f;

	public float thrustDmg = 0.5f;

	public float teaseDmg = 0.5f;

	public float expPerThrust = 3f;

	public float cumDrainSpeed = 10f;

	public float staminaRegRate = 0.5f;

	public float speedIncreaseCumTime = 30f;

	private float fuckDelay;

	public float fuckTime;

	private float pct;

	public static float powerRecieved;

	private bool gotAttacked;

	public float impregChance = 30f;

	private bool cumming;

	private bool exhausted;

	public bool hitFront;

	public bool hitBack;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	private Transform anim_pos;

	public Transform mount_place;

	public bool sexyTime;

	private float mountWaitTime = 2f;

	private float remainingWaitTime;

	private CamShaker shake;

	private bool fastSex;

	private bool grabSoundPlayed;

	private bool impregRolette;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private CapsuleCollider capCollider;

	private void Start()
	{
		hitFront = false;
		hitBack = false;
		circle.enabled = false;
		SexImage.enabled = false;
		fuckDelay = 0f;
		shake = Object.FindObjectOfType<CamShaker>();
		controlInstance = GetComponent<EnemyFieldOfView>();
		capCollider = GetComponent<CapsuleCollider>();
		if (PlayerManager.SAB)
		{
			staminaRegRate = 0.5f;
		}
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
			movingSound.Stop();
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
			capCollider.enabled = false;
		}
		else
		{
			capCollider.enabled = true;
		}
		RestOrReady();
		if (GetComponent<EnemyFieldOfView>().gotHit)
		{
			currentStamina = maxStamina;
			Chase();
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
			RestState();
			return;
		}
		if (exhausted)
		{
			RestState();
			return;
		}
		ReadyState();
		HideWorldSpaceUI();
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
			if (!reached)
			{
				return;
			}
			AttackAnim();
			if (!PlayerController.iFalled)
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
				Sex();
			}
			else
			{
				MountAndTease();
			}
		}
	}

	private void RestState()
	{
		if (sexyTime)
		{
			Release();
			sexyTime = false;
		}
		else
		{
			Regenerate();
			teaseSound.Stop();
		}
	}

	private void Regenerate()
	{
		GainStamina(staminaRegRate);
		currentCum = 0f;
		ShowWorldSpaceUI();
		Patrolling();
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

	private void Release()
	{
		heroineAnim.SetBool("LurkFrontGrab", value: false);
		heroineAnim.SetBool("LurkFrontTease", value: false);
		heroineAnim.SetBool("LurkFrontFuck", value: false);
		heroineAnim.SetBool("LurkCumFront", value: false);
		heroineAnim.SetBool("LurkBehindGrab", value: false);
		heroineAnim.SetBool("LurkBehindTease", value: false);
		heroineAnim.SetBool("LurkBehindSex", value: false);
		heroineAnim.SetBool("LurkCumBehind", value: false);
		heroineAnim.SetBool("isScared", value: false);
		heroineAnim.SetBool("isHorny", value: false);
		anim.SetBool("isFrontGrab", value: false);
		anim.SetBool("isFrontTease", value: false);
		anim.SetBool("isFrontFuck", value: false);
		anim.SetBool("isFrontCum", value: false);
		anim.SetBool("isBehindGrab", value: false);
		anim.SetBool("isBehindTease", value: false);
		anim.SetBool("isBehindSex", value: false);
		anim.SetBool("isBehindCum", value: false);
		PlayerController.iGetFucked = false;
		PlayerController.iGetInserted = false;
		PlayerController.claimed = false;
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		CameraFollow.target = Heroine.transform;
		anim.speed = 1f;
		heroineAnim.speed = 1f;
		reached = false;
		fastSex = false;
		grabSoundPlayed = false;
		teaseSound.Stop();
		cumSound.Stop();
		sexSoundOne.Stop();
		RemoveMeFromList();
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
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
			agent.ResetPath();
			agent.isStopped = true;
			StartCoroutine(StartPatrol());
		}
	}

	private void ChaseOrWatch()
	{
		if (PlayerController.claimed)
		{
			Watch();
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
		if (distance < attackRange)
		{
			Vector3 vector = new Vector3(0f, 0f, 5f);
			float num = 40f;
			GetComponent<Rigidbody>().MovePosition(base.transform.position - vector * Time.deltaTime * num);
			agent.isStopped = true;
			Debug.Log("close");
		}
	}

	private void Chase()
	{
		agent.SetDestination(Heroine.transform.position);
		agent.speed = chaseSpeed;
		FaceTarget();
		if (!reached)
		{
			agent.isStopped = false;
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

	private void MountAndTease()
	{
		TakeSexPos();
		InitiateSex();
		InitiateUI();
		agent.ResetPath();
		CameraFollow.target = camFollowTarget;
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		PlayerController.iGetInserted = true;
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
		}
		else
		{
			heroineAnim.SetBool("isHorny", value: true);
		}
		if (PlayerController.iFalledFront)
		{
			if (!grabSoundPlayed)
			{
				grabSound.Play();
				grabSoundPlayed = true;
			}
			FrontTeaseAnim();
		}
		if (PlayerController.iFalledBack)
		{
			if (!grabSoundPlayed)
			{
				grabSound.Play();
				grabSoundPlayed = true;
			}
			BackTeaseAnim();
		}
	}

	private void Sex()
	{
		fuckDelay = 0f;
		heroineAnim.SetBool("isScared", value: false);
		heroineAnim.SetBool("isHorny", value: false);
		TakeSexPos();
		HeroineStats.aroused = true;
		teaseSound.Stop();
		PlayerManager.IsVirgin = false;
		if (PlayerController.iFalledFront)
		{
			if (!cumming)
			{
				FrontSexAnim();
				impregRolette = false;
				cumSound.Stop();
			}
			else
			{
				FrontCumAnim();
				DrainCum(cumDrainSpeed);
				HeroineStats.creampied = true;
				if (!HeroineStats.pregnant)
				{
					HeroineStats.fertileCum = true;
				}
				heroineAnim.SetBool("isCumFilled", value: true);
				if (!HeroineStats.GameOver)
				{
					DrainStamina(cumDrainSpeed - 9f);
				}
				Impregnation(impregChance);
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
				}
			}
		}
		if (!PlayerController.iFalledBack)
		{
			return;
		}
		if (!cumming)
		{
			BehindSexAnim();
			impregRolette = false;
			cumSound.Stop();
			return;
		}
		BackCumAnim();
		DrainCum(cumDrainSpeed);
		heroineAnim.SetBool("isCumFilled", value: true);
		Heroine.GetComponent<HeroineStats>().LargeCumDrip();
		HeroineStats.creampied = true;
		HeroineStats.fertileCum = true;
		if (!HeroineStats.GameOver)
		{
			DrainStamina(cumDrainSpeed - 9f);
		}
		Impregnation(impregChance);
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
	}

	private void InitiateSex()
	{
		if (EquipmentManager.heroineIsNaked)
		{
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
			}
		}
	}

	private void Impregnation(float impregChancePercent)
	{
		if (!impregRolette)
		{
			int num = Random.Range(1, 100);
			if (impregChancePercent >= (float)num)
			{
				HeroineStats.pregnant = true;
				heroineAnim.SetBool("isPregnant", value: true);
			}
			impregRolette = true;
		}
	}

	private void WalkAnim()
	{
		anim.SetBool("isIdle", value: false);
		anim.SetBool("isAttack", value: false);
		anim.SetBool("isWalking", value: true);
	}

	private void IdleAnim()
	{
		anim.SetBool("isIdle", value: true);
		anim.SetBool("isWalking", value: false);
	}

	private void WalkOrIdle()
	{
		if (agent.velocity.magnitude == 0f)
		{
			IdleAnim();
			movingSound.Stop();
			return;
		}
		WalkAnim();
		if (!movingSound.isPlaying)
		{
			movingSound.Play();
		}
	}

	private void AttackAnim()
	{
		FaceTarget();
		if (!PlayerController.iFalled)
		{
			if (agent.remainingDistance > attackRange)
			{
				agent.speed = chaseSpeed;
				return;
			}
			if (attacked)
			{
				StartCoroutine(AttackAgain());
				return;
			}
			anim.SetBool("isAttack", value: true);
			agent.speed = 7f;
			attacked = true;
		}
	}

	private IEnumerator AttackAgain()
	{
		yield return new WaitForSeconds(0.2f);
		anim.SetBool("isAttack", value: false);
		attacked = false;
	}

	private void FrontTeaseAnim()
	{
		anim.SetBool("isFrontGrab", value: true);
		heroineAnim.SetBool("LurkFrontGrab", value: true);
		StartCoroutine(StartTeasingFront());
	}

	private void FrontSexAnim()
	{
		anim.SetBool("isFrontFuck", value: true);
		anim.SetBool("isFrontCum", value: false);
		heroineAnim.SetBool("LurkCumFront", value: false);
		heroineAnim.SetBool("LurkFrontFuck", value: true);
	}

	private void BackTeaseAnim()
	{
		anim.SetBool("isBehindGrab", value: true);
		heroineAnim.SetBool("LurkBehindGrab", value: true);
		StartCoroutine(StartTeasingBehind());
	}

	private void BehindSexAnim()
	{
		anim.SetBool("isBehindSex", value: true);
		anim.SetBool("isBehindCum", value: false);
		heroineAnim.SetBool("LurkCumBehind", value: false);
		heroineAnim.SetBool("LurkBehindSex", value: true);
	}

	private void FrontCumAnim()
	{
		anim.SetBool("isFrontCum", value: true);
		heroineAnim.SetBool("LurkCumFront", value: true);
	}

	private void BackCumAnim()
	{
		anim.SetBool("isBehindCum", value: true);
		heroineAnim.SetBool("LurkCumBehind", value: true);
	}

	private IEnumerator StartFrontSex()
	{
		yield return new WaitForSeconds(5f);
		FrontSexAnim();
	}

	private IEnumerator StartBehindSex()
	{
		yield return new WaitForSeconds(5f);
		BehindSexAnim();
	}

	private IEnumerator StartTeasingFront()
	{
		yield return new WaitForSeconds(3f);
		if (!teaseSound.isPlaying)
		{
			teaseSound.Play();
		}
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(teaseDmg * 3f);
		}
		anim.SetBool("isFrontTease", value: true);
		heroineAnim.SetBool("LurkFrontTease", value: true);
	}

	private IEnumerator StartTeasingBehind()
	{
		yield return new WaitForSeconds(3f);
		if (!teaseSound.isPlaying)
		{
			teaseSound.Play();
		}
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(teaseDmg * 3f);
		}
		anim.SetBool("isBehindTease", value: true);
		heroineAnim.SetBool("LurkBehindTease", value: true);
	}

	private void TakeSexPos()
	{
		anim_pos.rotation = mount_place.rotation;
		anim_pos.position = mount_place.position;
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
		if (sexyTime)
		{
			if (currentCum > speedIncreaseCumTime && !cumming)
			{
				anim.speed = 1.5f;
				heroineAnim.speed = 1.5f;
				fastSex = true;
				PlayerController.heIsFuckingHard = true;
			}
			else
			{
				anim.speed = 1f;
				heroineAnim.speed = 1f;
				fastSex = false;
				PlayerController.heIsFuckingHard = false;
			}
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

	public void ThrustEvent()
	{
		thrusted = true;
		if (!HeroineStats.GameOver)
		{
			DrainStaminaInstant(drainStamPerThrust);
		}
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		if (fastSex)
		{
			sexSoundFast.Play();
			shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		}
		else
		{
			sexSoundNormal.Play();
		}
	}

	public void CumEvent()
	{
		sexSoundOne.Play();
		shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
	}

	public void AttackEvent()
	{
		monsterHit.Play();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		if (other.tag == "NavObstacle")
		{
			PlayerController.claimed = false;
			RemoveMeFromList();
			currentStamina = 20f;
			exhausted = true;
		}
		if (PlayerController.iFalled || !(other.tag == "Player"))
		{
			return;
		}
		CheckHitAngle();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
		if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1] != null)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id != 3648532)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(1);
				return;
			}
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

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
	}
}
