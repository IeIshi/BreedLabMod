using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HuggerControl : MonoBehaviour
{
	private GameObject Heroine;

	public Image circle;

	public Image SexImage;

	public AudioSource awareSound;

	public AudioSource sexSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public AudioSource attackSound;

	public Transform targetStaminaUI;

	public Transform targetHealthUI;

	public GameObject stamUIPrefab;

	public GameObject healthUIPrefab;

	public Transform camPosFront;

	public Transform camPosBehind;

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

	public bool followed;

	private bool attacked;

	private bool reached;

	private bool added;

	public float chaseSpeed = 4.5f;

	private bool aware;

	public float drainStamPerThrust = 10f;

	public float gainCumPerThrust = 5f;

	public float thrustDmg = 0.5f;

	public float teaseDmg = 0.5f;

	public float cumDrainSpeed = 10f;

	public float staminaRegRate = 0.5f;

	public float expPerThrust = 1f;

	private float fuckDelay;

	public float fuckTime;

	private float pct;

	public float impregChance = 30f;

	private bool cumming;

	private bool exhausted;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	private Transform anim_pos;

	public Transform mount_place;

	public Transform mount_place_behind;

	private bool sexyTime;

	private float mountWaitTime = 2f;

	private float remainingWaitTime;

	private float attackDelay;

	public float attackDelayTime = 2.5f;

	private bool hardFuck;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private CapsuleCollider capCollider;

	private bool triggerPatrol;

	public bool reverseSafeSpaceHug;

	public GameObject mySpine;

	private SphereCollider myAttackCollider;

	private void Start()
	{
		myAttackCollider = mySpine.GetComponent<SphereCollider>();
		myAttackCollider.enabled = false;
		capCollider = GetComponent<CapsuleCollider>();
		circle.enabled = false;
		SexImage.enabled = false;
		if (PlayerManager.SAB)
		{
			staminaRegRate = 0.5f;
		}
		fuckDelay = 0f;
		controlInstance = GetComponent<EnemyFieldOfView>();
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
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		anim_pos = base.gameObject.transform;
		if (GetComponent<EnemyFieldOfView>().isDed)
		{
			anim.SetBool("isDed", value: true);
			GetComponent<CapsuleCollider>().enabled = false;
			agent.isStopped = true;
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
			RemoveMeFromList();
			agent.ResetPath();
			agent.isStopped = true;
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<Rigidbody>().detectCollisions = false;
			staminaUI.gameObject.SetActive(value: false);
			base.enabled = false;
			return;
		}
		WalkOrIdle();
		CheckState(currentCum, currentStamina);
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
		if (!reverseSafeSpaceHug)
		{
			if (Safespace.heroineSafe && !triggerPatrol)
			{
				agent.SetDestination(patrolPoints[0].transform.position);
				if (agent.remainingDistance < 0.1f)
				{
					triggerPatrol = true;
				}
			}
		}
		else if (!Safespace.heroineSafe && !triggerPatrol)
		{
			agent.SetDestination(patrolPoints[0].transform.position);
			if (agent.remainingDistance < 0.1f)
			{
				triggerPatrol = true;
			}
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
		triggerPatrol = false;
		if (aware)
		{
			aware = false;
		}
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
			GetComponent<CapsuleCollider>().isTrigger = false;
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
			if (Heroine.GetComponent<HeroineStats>().mySexPartner != null)
			{
				if (Heroine.GetComponent<HeroineStats>().mySexPartner != base.gameObject)
				{
					Watch();
				}
			}
			else if (PlayerManager.instance.enemyTurnOrder[0] == base.gameObject)
			{
				sexyTime = true;
			}
			return;
		}
		remainingWaitTime += Time.deltaTime * 1f;
		GetComponent<CapsuleCollider>().isTrigger = true;
		if (remainingWaitTime >= mountWaitTime)
		{
			if (PlayerController.iGetFucked)
			{
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
		}
	}

	private void Regenerate()
	{
		GainStamina(staminaRegRate);
		currentCum = 0f;
		ShowWorldSpaceUI();
		Patrolling();
	}

	private void Release()
	{
		heroineAnim.SetBool("isInserting", value: false);
		heroineAnim.SetBool("isFucking", value: false);
		heroineAnim.SetBool("isFuckingBehind", value: false);
		anim.SetBool("isInserting", value: false);
		anim.SetBool("isFucking", value: false);
		anim.SetBool("isCumming", value: false);
		anim.SetBool("isInsertingBehind", value: false);
		anim.SetBool("isFuckingBehind", value: false);
		anim.SetBool("isCummingBehind", value: false);
		PlayerController.iGetFucked = false;
		PlayerController.iGetInserted = false;
		PlayerController.claimed = false;
		HeroineStats.aroused = false;
		HeroineStats.horny = false;
		reached = false;
		followed = false;
		CameraFollow.target = Heroine.transform;
		GetComponent<CapsuleCollider>().isTrigger = false;
		insertSound.Stop();
		cumSound.Stop();
		float num = Random.Range(0.3f, 0.7f);
		anim_pos.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
		RemoveMeFromList();
		SexImage.enabled = false;
		circle.enabled = false;
		hardFuck = false;
		PlayerController.heIsFuckingHard = false;
		heroineAnim.speed = 1f;
		anim.speed = 1f;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
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
		agent.ResetPath();
		agent.isStopped = true;
		StartCoroutine(StartPatrol());
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
		if (distance < attackRange)
		{
			agent.isStopped = true;
			FaceTarget();
		}
		else
		{
			agent.SetDestination(Heroine.transform.position);
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
		if (distance <= attackRange)
		{
			reached = true;
			agent.speed = 2.5f;
			if (agent.remainingDistance < 0.2f)
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
		yield return new WaitForSeconds(1f);
		reached = false;
		followed = true;
		agent.isStopped = false;
		if (!aware)
		{
			if (!awareSound.isPlaying)
			{
				awareSound.Play();
			}
			aware = true;
		}
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
		InitiateSex();
		InitiateUI();
		agent.ResetPath();
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
		}
		PlayerController.iGetInserted = true;
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg * 2f);
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
		}
		if (PlayerController.iFalledFront)
		{
			TakeSexPos();
			FrontTeaseAnim();
			CameraFollow.target = camPosFront;
		}
		if (PlayerController.iFalledBack)
		{
			TakeSexPosBehind();
			BackTeaseAnim();
			CameraFollow.target = camPosBehind;
		}
	}

	private void Sex()
	{
		VignetteEffect();
		fuckDelay = 0f;
		HeroineStats.aroused = true;
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
		if (PlayerController.iFalledFront)
		{
			TakeSexPos();
			if (!cumming)
			{
				FrontSexAnim();
				heroineAnim.SetBool("isScared", value: false);
				heroineAnim.SetBool("isHorny", value: false);
				cumSound.Stop();
			}
			else
			{
				FrontCumAnim();
				Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
				anim.speed = 1f;
				heroineAnim.speed = 1f;
				hardFuck = false;
				PlayerController.heIsFuckingHard = false;
				DrainCum(cumDrainSpeed);
				Heroine.GetComponent<HeroineStats>().GainOrg(3f);
				if (!PlayerManager.SAB)
				{
					heroineAnim.SetBool("isScared", value: true);
				}
				else
				{
					heroineAnim.SetBool("isHorny", value: true);
					HeroineStats.horny = true;
				}
				HeroineStats.creampied = true;
				if (!HeroineStats.pregnant)
				{
					HeroineStats.fertileCum = true;
				}
				if (!HeroineStats.GameOver)
				{
					DrainStamina(cumDrainSpeed / 3f);
				}
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
				}
				insertSound.Stop();
			}
		}
		if (!PlayerController.iFalledBack)
		{
			return;
		}
		TakeSexPosBehind();
		if (!cumming)
		{
			BehindSexAnim();
			heroineAnim.SetBool("isScared", value: false);
			cumSound.Stop();
			return;
		}
		BackCumAnim();
		anim.speed = 1f;
		heroineAnim.speed = 1f;
		hardFuck = false;
		DrainCum(cumDrainSpeed);
		HeroineStats.creampied = true;
		HeroineStats.fertileCum = true;
		if (!HeroineStats.GameOver)
		{
			DrainStamina(cumDrainSpeed / 3f);
		}
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
		}
		else
		{
			heroineAnim.SetBool("isHorny", value: true);
			HeroineStats.horny = true;
		}
		Heroine.GetComponent<HeroineStats>().GainOrg(3f);
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
				PlayerManager.IsVirgin = false;
				PlayerController.iGetFucked = true;
				PlayerController.iGetInserted = false;
			}
		}
	}

	private void WalkAnim()
	{
		anim.SetBool("isIdle", value: false);
		anim.SetBool("isAttacking", value: false);
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
		}
		else
		{
			WalkAnim();
		}
	}

	private void AttackAnim()
	{
		FaceTarget();
		if (PlayerController.iFalled)
		{
			attacked = false;
			anim.SetBool("isAttacking", value: false);
			return;
		}
		if (distance <= attackRange)
		{
			if (!attacked)
			{
				anim.SetBool("isAttacking", value: true);
			}
			attacked = true;
		}
		if (attacked)
		{
			attackDelay += Time.deltaTime * 1f;
			agent.speed = 1f;
			if (attackDelay >= attackDelayTime)
			{
				attackDelay = 0f;
				attacked = false;
			}
		}
	}

	private void FrontTeaseAnim()
	{
		anim.SetBool("isInserting", value: true);
		heroineAnim.SetBool("isInserting", value: true);
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
			return;
		}
		heroineAnim.SetBool("isHorny", value: true);
		HeroineStats.horny = true;
	}

	private void FrontSexAnim()
	{
		anim.SetBool("isFucking", value: true);
		anim.SetBool("isCumming", value: false);
		heroineAnim.SetBool("isInserting", value: false);
		heroineAnim.SetBool("isFucking", value: true);
		heroineAnim.SetBool("isScared", value: false);
		heroineAnim.SetBool("isHorny", value: false);
		if (currentCum > 50f)
		{
			anim.speed = 1.5f;
			heroineAnim.speed = 1.5f;
			hardFuck = true;
			PlayerController.heIsFuckingHard = true;
		}
	}

	private void BackTeaseAnim()
	{
		anim.SetBool("isInsertingBehind", value: true);
		heroineAnim.SetBool("isFalledBack", value: true);
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
			return;
		}
		heroineAnim.SetBool("isHorny", value: true);
		HeroineStats.horny = true;
	}

	private void BehindSexAnim()
	{
		anim.SetBool("isFuckingBehind", value: true);
		anim.SetBool("isCummingBehind", value: false);
		heroineAnim.SetBool("isFalledBack", value: false);
		heroineAnim.SetBool("isFuckingBehind", value: true);
		heroineAnim.SetBool("isScared", value: false);
		heroineAnim.SetBool("isHorny", value: false);
		if (currentCum > 50f)
		{
			anim.speed = 1.5f;
			heroineAnim.speed = 1.5f;
			hardFuck = true;
			PlayerController.heIsFuckingHard = true;
		}
	}

	private void FrontCumAnim()
	{
		anim.SetBool("isCumming", value: true);
		heroineAnim.SetBool("isInserting", value: true);
		heroineAnim.SetBool("isFucking", value: false);
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
			return;
		}
		heroineAnim.SetBool("isHorny", value: true);
		HeroineStats.horny = true;
	}

	private void BackCumAnim()
	{
		anim.SetBool("isCummingBehind", value: true);
		heroineAnim.SetBool("isFalledBack", value: true);
		heroineAnim.SetBool("isFuckingBehind", value: false);
		if (!PlayerManager.SAB)
		{
			heroineAnim.SetBool("isScared", value: true);
			return;
		}
		heroineAnim.SetBool("isHorny", value: true);
		HeroineStats.horny = true;
	}

	private void TakeSexPos()
	{
		anim_pos.rotation = mount_place.rotation;
		anim_pos.position = mount_place.position;
	}

	private void TakeSexPosBehind()
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
			GetComponent<Rigidbody>().AddForce(-base.transform.forward * 100f);
		}
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.gameObject.SetActive(value: true);
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.gameObject.SetActive(value: false);
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
		if (!HeroineStats.GameOver)
		{
			DrainStaminaInstant(drainStamPerThrust);
		}
		if (hardFuck)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		thrusted = true;
		GainCumInstant(gainCumPerThrust);
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
	}

	public void OnAttack()
	{
		myAttackCollider.enabled = true;
		attackSound.Play();
	}

	public void OnAttFinish()
	{
		myAttackCollider.enabled = false;
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
	}
}
