using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaspControl : MonoBehaviour
{
	public enum MyState
	{
		FIRSTFLY,
		PATROL,
		CHASE,
		INJECTATTACK,
		TEASE,
		DICKEXTEND,
		SEX,
		CUM,
		DEAD
	}

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	public float pantiesDurDamage;

	public float gainCumPerThrust;

	public float pleasureValue;

	public float lustValue;

	private bool camShaked;

	public float maxCum;

	private float currentCum;

	public float maxHealth;

	private float currentHealth;

	private Transform staminaUI;

	private Image stamSlider;

	private GameObject stamUIPrefab;

	private Transform targetStaminaUI;

	private Transform cam;

	private Image healthSlider;

	private Transform targetHealthUI;

	private GameObject healthUIPrefab;

	private Transform healthUI;

	private Image circle;

	private Image SexImage;

	public float maxStamina;

	public float currentStamina;

	public float injectLustDmg;

	public Transform firstFlyingTarget;

	private Waypoint wp1;

	private Waypoint wp2;

	private Waypoint wp3;

	private Waypoint wp4;

	private Waypoint wp5;

	private Waypoint wp6;

	private Waypoint wp7;

	private Waypoint wp8;

	private NavMeshAgent agent;

	public MyState state;

	private GameObject Heroine;

	private Transform targetTease;

	private Transform targetSex;

	private Animator animator;

	private Animator heroineAnimator;

	public float chaseRange;

	public float attackRange;

	public float speedIncreaseRange;

	private float distance;

	private bool leader;

	private bool injecting;

	private bool fucking;

	public bool resting;

	private bool oralFucker;

	private bool analFucker;

	public float restTimer;

	private float timer;

	private AudioSource buzzSound;

	private AudioSource injectSound;

	private AudioSource vagSound;

	private AudioSource analSound;

	private AudioSource oralSound;

	private AudioSource teaseSound;

	private AudioSource cumSound;

	private float extendTimer;

	public float patrolSpeed = 2.5f;

	public bool patrolWaiting;

	public float totalWaitTime = 3f;

	public float switchProbability = 0.2f;

	private Waypoint[] patrolPoints;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	private float waitTimer;

	private int currentPatrolIndex;

	public bool isFuckWasp;

	private bool dead;

	private float deadTimer;

	public GameObject hitColliderHolder;

	private CapsuleCollider hitCollider;

	public GameObject impactEffect;

	public GameObject colliderHolder;

	private CapsuleCollider collider;

	private void Start()
	{
		Heroine = GameObject.Find("Heroine");
		collider = colliderHolder.GetComponent<CapsuleCollider>();
		Object.Destroy(Object.Instantiate(impactEffect, new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z), Quaternion.identity), 2f);
		currentCum = 0f;
		currentStamina = maxStamina;
		currentHealth = maxHealth;
		stamUIPrefab = base.transform.Find("StaminaUi").gameObject;
		healthUIPrefab = base.transform.Find("HealthUi").gameObject;
		targetStaminaUI = base.transform.Find("TargetStaminaUi");
		targetHealthUI = base.transform.Find("TargetHealthUi");
		cam = Camera.main.transform;
		hitCollider = hitColliderHolder.GetComponent<CapsuleCollider>();
		Canvas[] array = Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				healthUI = Object.Instantiate(healthUIPrefab, canvas.transform).transform;
				healthSlider = healthUI.GetChild(0).GetComponent<Image>();
				break;
			}
		}
		healthUI.gameObject.SetActive(value: false);
		wp1 = GameObject.Find("Waypoint").GetComponent<Waypoint>();
		wp2 = GameObject.Find("Waypoint2").GetComponent<Waypoint>();
		wp3 = GameObject.Find("Waypoint3").GetComponent<Waypoint>();
		wp4 = GameObject.Find("Waypoint4").GetComponent<Waypoint>();
		wp5 = GameObject.Find("Waypoint5").GetComponent<Waypoint>();
		wp6 = GameObject.Find("Waypoint6").GetComponent<Waypoint>();
		wp7 = GameObject.Find("Waypoint7").GetComponent<Waypoint>();
		wp8 = GameObject.Find("Waypoint8").GetComponent<Waypoint>();
		SexImage = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse").GetComponent<Image>();
		circle = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse/circle (1)").GetComponent<Image>();
		agent = GetComponent<NavMeshAgent>();
		patrolPoints = new Waypoint[8];
		patrolPoints[0] = wp1;
		patrolPoints[1] = wp2;
		patrolPoints[2] = wp3;
		patrolPoints[3] = wp4;
		patrolPoints[4] = wp5;
		patrolPoints[5] = wp6;
		patrolPoints[6] = wp7;
		patrolPoints[7] = wp8;
		targetTease = base.transform.Find("TargetTease");
		targetSex = base.transform.Find("TargetSex");
		buzzSound = base.transform.Find("BuzzSound").GetComponent<AudioSource>();
		injectSound = base.transform.Find("InjectSound").GetComponent<AudioSource>();
		vagSound = base.transform.Find("VagSound").GetComponent<AudioSource>();
		analSound = base.transform.Find("AnalSound").GetComponent<AudioSource>();
		oralSound = base.transform.Find("OralSound").GetComponent<AudioSource>();
		teaseSound = base.transform.Find("TeaseSound").GetComponent<AudioSource>();
		cumSound = base.transform.Find("CumSound").GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		heroineAnimator = Heroine.GetComponent<Animator>();
		animator.SetBool("fly", value: true);
		state = MyState.FIRSTFLY;
	}

	private void FixedUpdate()
	{
		ShowHealthUI();
		StateMachine(state);
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.FIRSTFLY:
			MoveFirstDestination();
			break;
		case MyState.PATROL:
			Patrol();
			break;
		case MyState.CHASE:
			Chase();
			break;
		case MyState.INJECTATTACK:
			InjectAttack();
			break;
		case MyState.TEASE:
			Tease();
			break;
		case MyState.SEX:
			Sex();
			break;
		case MyState.DICKEXTEND:
			DickExtend();
			break;
		case MyState.CUM:
			Cum();
			break;
		case MyState.DEAD:
			Dead();
			break;
		}
	}

	private void MoveFirstDestination()
	{
		float maxDistanceDelta = patrolSpeed * Time.deltaTime;
		base.transform.position = Vector3.MoveTowards(base.transform.position, firstFlyingTarget.position, maxDistanceDelta);
		if (base.transform.position == firstFlyingTarget.position)
		{
			if (isFuckWasp)
			{
				agent.enabled = true;
				state = MyState.CHASE;
			}
			else
			{
				PatrolOnStart();
				state = MyState.PATROL;
			}
		}
	}

	private void Chase()
	{
		if (Safespace.heroineSafe)
		{
			PatrolOnStart();
			state = MyState.PATROL;
			return;
		}
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		agent.SetDestination(Heroine.transform.position);
		collider.enabled = true;
		if (PlayerController.iFalled && !PlayerController.iGetFucked && !HeroineStats.masturbating)
		{
			PatrolOnStart();
			state = MyState.PATROL;
		}
		else
		{
			if (!(distance < attackRange))
			{
				return;
			}
			if (HeroineStats.currentLust < 80f)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				collider.enabled = false;
				state = MyState.INJECTATTACK;
			}
			else if (!PlayerController.iGetFucked)
			{
				if (HeroineStats.masturbating)
				{
					Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
					collider.enabled = false;
					state = MyState.TEASE;
				}
				else if (!PlayerController.iFalled)
				{
					Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
					state = MyState.TEASE;
				}
			}
			else if (Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<WaspControl>().state == MyState.SEX)
			{
				agent.isStopped = true;
				state = MyState.DICKEXTEND;
			}
			else
			{
				PatrolOnStart();
				state = MyState.PATROL;
			}
		}
	}

	private void Tease()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		agent.enabled = false;
		base.transform.position = Heroine.transform.position;
		base.transform.rotation = Heroine.transform.rotation;
		healthUI.gameObject.SetActive(value: false);
		leader = true;
		InitiateUI();
		Debug.Log("TEASING");
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		heroineAnimator.SetBool("mastStanding", value: false);
		heroineAnimator.SetBool("mastStandingHard", value: false);
		HeroineStats.masturbating = false;
		CameraFollow.target = targetTease;
		animator.SetBool("fly", value: false);
		buzzSound.Stop();
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		MovementManipulator.getsTeased = true;
		animator.Play("rig|Wasp_Standgrab");
		heroineAnimator.Play("rig|Wasp_Tease");
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(pantiesDurDamage);
		}
		else
		{
			InitiateSex();
		}
	}

	private void DickExtend()
	{
		extendTimer += Time.deltaTime;
		animator.SetBool("dickExtend", value: true);
		FaceTarget();
		if (MovementManipulator.assClaimed && MovementManipulator.mouthClaimed)
		{
			PatrolOnStart();
			animator.Play("rig|Wasp_Fly");
			state = MyState.PATROL;
			return;
		}
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			PatrolOnStart();
			animator.Play("rig|Wasp_Fly");
			state = MyState.PATROL;
		}
		if (extendTimer > 3f)
		{
			if (Heroine.GetComponent<HeroineStats>().mySexPartner != null)
			{
				state = MyState.SEX;
				return;
			}
			PatrolOnStart();
			animator.Play("rig|Wasp_Fly");
			state = MyState.PATROL;
		}
	}

	private void Cum()
	{
		base.transform.position = Heroine.transform.position;
		base.transform.rotation = Heroine.transform.rotation;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		HeroineStats.creampied = true;
		HeroineStats.oralCreampie = true;
		heroineAnimator.SetBool("isCumFilled", value: true);
		if (leader)
		{
			heroineAnimator.Play("rig|Wasp_Creampied");
			animator.Play("rig|Wasp_VagCum");
		}
		else if (oralFucker)
		{
			if (!PlayerController.iGetFucked)
			{
				ReleasePeon();
				if (currentHealth <= 0f)
				{
					agent.ResetPath();
					agent.isStopped = true;
					agent.enabled = false;
					GetComponent<Animator>().SetBool("die", value: true);
					healthUI.gameObject.SetActive(value: false);
					hitCollider.enabled = false;
					dead = true;
					state = MyState.DEAD;
				}
			}
			else
			{
				animator.Play("rig|Wasp_OralCum");
			}
		}
		else
		{
			if (!analFucker)
			{
				return;
			}
			if (!PlayerController.iGetFucked)
			{
				ReleasePeon();
				if (currentHealth <= 0f)
				{
					agent.ResetPath();
					agent.isStopped = true;
					agent.enabled = false;
					GetComponent<Animator>().SetBool("die", value: true);
					healthUI.gameObject.SetActive(value: false);
					hitCollider.enabled = false;
					dead = true;
					state = MyState.DEAD;
				}
			}
			else
			{
				animator.Play("rig|WaspAnalCum");
			}
		}
	}

	private void Sex()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			ReleasePeon();
			return;
		}
		ClaimAPlace();
		if (!analFucker && !oralFucker && !leader)
		{
			agent.enabled = true;
			PatrolOnStart();
			animator.SetBool("fly", value: true);
			state = MyState.PATROL;
		}
		if (!fucking)
		{
			agent.enabled = true;
			PatrolOnStart();
			animator.SetBool("fly", value: true);
			state = MyState.PATROL;
		}
		if (leader)
		{
			InitiateUI();
			CameraFollow.target = targetSex;
			if (currentCum > speedIncreaseRange)
			{
				animator.speed = 2f;
				heroineAnimator.speed = 2f;
			}
			if (currentCum >= 100f)
			{
				state = MyState.CUM;
			}
		}
		if (!fucking || leader)
		{
			return;
		}
		if (Heroine.GetComponent<Animator>().speed == 2f)
		{
			animator.speed = 2f;
		}
		try
		{
			if (Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<WaspControl>().state == MyState.CUM)
			{
				state = MyState.CUM;
			}
		}
		catch
		{
			Debug.Log("Whatever");
		}
	}

	private void ClaimAPlace()
	{
		if (!MovementManipulator.vaginaClaimed && !fucking)
		{
			animator.Play("rig|Wasp_VagFuck_One");
			heroineAnimator.Play("rig|Wasp_Fucked_One");
			MovementManipulator.vaginaClaimed = true;
			leader = true;
			fucking = true;
			MovementManipulator.getsTeased = false;
			agent.enabled = false;
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			animator.SetBool("fly", value: false);
			healthUI.gameObject.SetActive(value: false);
			buzzSound.Stop();
		}
		else if (!MovementManipulator.assClaimed && !fucking)
		{
			if (heroineAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.1f)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				animator.Play("rig|WaspAnalFuck");
				MovementManipulator.assClaimed = true;
				Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<WaspControl>().gainCumPerThrust += 1f;
				analFucker = true;
				fucking = true;
				agent.enabled = false;
				base.transform.position = Heroine.transform.position;
				base.transform.rotation = Heroine.transform.rotation;
				animator.SetBool("fly", value: false);
				healthUI.gameObject.SetActive(value: false);
				buzzSound.Stop();
			}
		}
		else if (!MovementManipulator.mouthClaimed && !fucking && heroineAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.1f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			animator.Play("rig|Wasp_OralFuck");
			heroineAnimator.Play("rig|Wasp_Fucked_Three");
			MovementManipulator.mouthClaimed = true;
			Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<WaspControl>().gainCumPerThrust += 1f;
			oralFucker = true;
			fucking = true;
			agent.enabled = false;
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			animator.SetBool("fly", value: false);
			healthUI.gameObject.SetActive(value: false);
			MovementManipulator.occupied = true;
			buzzSound.Stop();
		}
	}

	private void InitiateSex()
	{
		if (!EquipmentManager.heroineIsNaked)
		{
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
			PlayerManager.IsVirgin = false;
			if (!camShaked)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				camShaked = true;
			}
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			state = MyState.SEX;
		}
	}

	private void InjectAttack()
	{
		if (injecting)
		{
			CameraFollow.target = targetTease;
			if (!PlayerController.iFalled || HeroineStats.currentLust >= 100f)
			{
				Release();
				return;
			}
		}
		if (!EquipmentManager.heroineIsNaked)
		{
			heroineAnimator.SetBool("mastStanding", value: false);
			heroineAnimator.SetBool("mastStandingHard", value: false);
			heroineAnimator.SetBool("isReloading", value: false);
			HeroineStats.masturbating = false;
			heroineAnimator.SetBool("ImpregInsRub", value: true);
			animator.SetBool("oralInject", value: true);
			heroineAnimator.Play("rig|PregInsRub");
			animator.Play("rig|Wasp_OralInject");
			agent.enabled = false;
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			PlayerController.iFalled = true;
			PostProcessingManager.instance.VingetteEffectHigh();
			MovementManipulator.mouthInject = true;
		}
		if (EquipmentManager.heroineIsNaked)
		{
			heroineAnimator.SetBool("mastStanding", value: false);
			heroineAnimator.SetBool("mastStandingHard", value: false);
			heroineAnimator.SetBool("isReloading", value: false);
			HeroineStats.masturbating = false;
			heroineAnimator.SetBool("ImpregInsRub", value: true);
			animator.SetBool("assInject", value: true);
			heroineAnimator.Play("rig|PregInsRub");
			animator.Play("rig|Wasp_AssInject");
			agent.enabled = false;
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			PlayerController.iFalled = true;
			PostProcessingManager.instance.VingetteEffectHigh();
			MovementManipulator.assInject = true;
		}
		injecting = true;
		healthUI.gameObject.SetActive(value: false);
	}

	private void Release()
	{
		PlayerController.iFalled = true;
		resting = true;
		injecting = false;
		MovementManipulator.occupied = false;
		PlayerController.iGetFucked = false;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		leader = false;
		MovementManipulator.getsTeased = false;
		heroineAnimator.SetBool("isFalledBack", value: true);
		heroineAnimator.SetBool("ImpregInsRub", value: false);
		animator.SetBool("oralInject", value: false);
		animator.SetBool("assInject", value: false);
		animator.Play("rig|Wasp_Fly");
		MovementManipulator.mouthInject = false;
		MovementManipulator.assInject = false;
		MovementManipulator.vaginaClaimed = false;
		PostProcessingManager.vignette.intensity.value = 0f;
		agent.enabled = true;
		PatrolOnStart();
		currentStamina = 0f;
		currentCum = 0f;
		heroineAnimator.speed = 1f;
		animator.speed = 1f;
		CameraFollow.target = Heroine.transform;
		fuckDelay = 0f;
		MovementManipulator.chasingWaspCount = 0;
		DisableUI();
		if (currentHealth > 0f)
		{
			state = MyState.PATROL;
		}
	}

	private void ReleasePeon()
	{
		MovementManipulator.assClaimed = false;
		MovementManipulator.mouthClaimed = false;
		agent.enabled = true;
		resting = true;
		currentStamina = 0f;
		currentCum = 0f;
		analFucker = false;
		oralFucker = false;
		if (currentHealth > 0f)
		{
			PatrolOnStart();
			animator.SetBool("fly", value: true);
			animator.Play("rig|Wasp_Fly");
			state = MyState.PATROL;
		}
		else
		{
			float num = Random.Range(0.3f, 0.7f);
			base.transform.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
			animator.SetBool("die", value: true);
		}
	}

	private void RegenerateStamina()
	{
		if (staminaUI != null)
		{
			staminaUI.position = targetStaminaUI.position;
			staminaUI.forward = -cam.forward;
			stamSlider.fillAmount = currentStamina / maxStamina;
		}
		currentStamina += Time.deltaTime * 0.7f;
		if (currentStamina >= maxStamina)
		{
			resting = false;
		}
	}

	private void Patrol()
	{
		agent.enabled = true;
		animator.SetBool("dickExtend", value: false);
		collider.enabled = true;
		if (resting)
		{
			RegenerateStamina();
			if (MovementManipulator.vaginaClaimed)
			{
				currentStamina = maxStamina;
				resting = false;
			}
		}
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance < chaseRange && !resting && !MovementManipulator.getsTeased && !Safespace.heroineSafe && !MovementManipulator.occupied && (!PlayerController.iFalled || PlayerController.iGetFucked) && !LarveBirth.larveImpregnated)
		{
			state = MyState.CHASE;
		}
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

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}

	private void InjectEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainLustInstant(injectLustDmg);
		injectSound.Play();
	}

	private void OralEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		oralSound.Play();
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			ReleasePeon();
		}
	}

	private void VagEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		GainCumInstant(gainCumPerThrust);
		vagSound.Play();
	}

	private void AnalEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		analSound.Play();
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			ReleasePeon();
		}
	}

	private void TeaseEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		teaseSound.Play();
	}

	private void CumEvent()
	{
		cumSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		if (HeroineStats.MantisBuff)
		{
			TakeDamage(2f);
		}
		if (leader)
		{
			LoseCumInstant(3f);
			if (currentCum <= 0f)
			{
				LarveBirth.larveImpregnated = true;
				Release();
				if (currentHealth <= 0f)
				{
					agent.ResetPath();
					agent.isStopped = true;
					agent.enabled = false;
					float num = Random.Range(0.3f, 0.7f);
					base.transform.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
					GetComponent<Animator>().SetBool("die", value: true);
					healthUI.gameObject.SetActive(value: false);
					hitCollider.enabled = false;
					dead = true;
					state = MyState.DEAD;
				}
			}
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<WaspControl>().LoseCumInstant(3f);
		}
	}

	private void Dead()
	{
		if (dead)
		{
			buzzSound.Stop();
			deadTimer += Time.deltaTime;
			if (deadTimer > 25f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void TakeDamage(float amount)
	{
		if (agent.enabled || state == MyState.CUM)
		{
			healthUI.gameObject.SetActive(value: true);
			drainHealth(amount);
			if (state != MyState.CUM && currentHealth <= 0f)
			{
				agent.ResetPath();
				agent.isStopped = true;
				agent.enabled = false;
				GetComponent<Animator>().SetBool("die", value: true);
				healthUI.gameObject.SetActive(value: false);
				hitCollider.enabled = false;
				dead = true;
				state = MyState.DEAD;
			}
		}
	}

	private void ShowHealthUI()
	{
		if (healthUI != null)
		{
			healthUI.position = targetHealthUI.position;
			healthUI.forward = -cam.forward;
			healthSlider.fillAmount = currentHealth / maxHealth;
		}
	}

	public void drainHealth(float drainValue)
	{
		currentHealth -= drainValue;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
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

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	public void LoseCumInstant(float gainValue)
	{
		currentCum -= gainValue;
		EnemyUI.instance.LoseCum(gainValue);
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, chaseRange);
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
	}
}
