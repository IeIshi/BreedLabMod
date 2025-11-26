using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HoundControl : MonoBehaviour
{
	public enum MyState
	{
		PATROL,
		CHASE,
		ATTENTION,
		ATTACK,
		TEASE,
		SEX,
		CUM,
		KNOT
	}

	private GameObject Heroine;

	public Transform targetStaminaUI;

	public Transform targetHealthUI;

	public Transform targetCamBehind;

	public Transform mountFrontPos;

	public GameObject stamUIPrefab;

	public GameObject healthUIPrefab;

	private Transform staminaUI;

	private Transform cam;

	private Image stamSlider;

	public AudioClip[] clapSoundsArray;

	public AudioSource[] slideSoundArray;

	public AudioSource attentionGrowl;

	public AudioSource stepSound1;

	public AudioSource stepSound2;

	public AudioSource pantSound;

	public AudioSource bark;

	public AudioSource attackSound;

	public AudioSource longCumSound;

	public AudioSource cumSound;

	public AudioSource insertOneSound;

	public AudioSource insertTwoSound;

	private bool attentionSoundPlayed;

	private AudioSource audioSource;

	private NavMeshAgent agent;

	private EnemyFieldOfView controlInstance;

	private Rigidbody body;

	public Image circle;

	public Image SexImage;

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	public float sex2Range;

	public float maxStamina;

	private float currentStamina;

	public float maxCum;

	private float currentCum;

	public float gainCumPerThrust;

	public float thrustDmg;

	public float teaseDmg;

	public float expPerThrust;

	private bool thrusted;

	private bool max;

	private bool camShaked;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public MyState state;

	private float distance;

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

	private Animator animator;

	private Animator heroineAnimator;

	public float sitOnTimer;

	public float alertTimer;

	public float attentionTimer;

	private float sitTimer;

	private float timer;

	private bool insert;

	public float attackRange;

	private bool ignoreHer;

	private bool hitFront;

	private bool hitBack;

	private bool attackedAgain;

	private float speedModifier = 1f;

	private bool knottedOnce;

	private bool fartiged;

	public float attentionRange;

	private EnemyFieldOfView checker;

	private bool deadReset;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		deadReset = false;
		SexImage.enabled = false;
		circle.enabled = false;
		checker = GetComponent<EnemyFieldOfView>();
		speedModifier = 1f;
		currentStamina = maxStamina;
		currentCum = 0f;
		controlInstance = GetComponent<EnemyFieldOfView>();
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
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
		state = MyState.PATROL;
		Heroine = GameObject.Find("Heroine");
		heroineAnimator = Heroine.GetComponent<Animator>();
		PatrolOnStart();
	}

	private void FixedUpdate()
	{
		if (checker.isDed)
		{
			Dead();
			return;
		}
		if (checker.gotHit)
		{
			currentStamina = maxStamina;
			if (state != MyState.ATTENTION)
			{
				animator.SetBool("hit", value: true);
				state = MyState.CHASE;
			}
		}
		else
		{
			animator.SetBool("hit", value: false);
		}
		StateMachine(state);
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.PATROL:
			Patroll();
			break;
		case MyState.CHASE:
			Chase();
			break;
		case MyState.ATTENTION:
			Attention();
			break;
		case MyState.ATTACK:
			Attack();
			break;
		case MyState.TEASE:
			Tease();
			break;
		case MyState.SEX:
			Sex();
			break;
		case MyState.CUM:
			Cum();
			break;
		case MyState.KNOT:
			Knot();
			break;
		}
	}

	private void Dead()
	{
		if (!deadReset)
		{
			animator.SetBool("dead", value: true);
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			Heroine.GetComponent<PlayerController>().enabled = true;
			agent.ResetPath();
			agent.isStopped = true;
			agent.enabled = false;
			GetComponent<CapsuleCollider>().enabled = false;
			if (PlayerController.iGetFucked)
			{
				longCumSound.Stop();
				cumSound.Stop();
				PlayerController.iGetFucked = false;
				PlayerController.heIsFuckingHard = false;
				CameraFollow.target = Heroine.transform;
				HeroineStats.stunned = false;
				heroineAnimator.SetBool("doggyFrontTease", value: false);
				heroineAnimator.SetBool("doggyFrontSex1", value: false);
				heroineAnimator.SetBool("doggyFrontSex2", value: false);
				heroineAnimator.SetBool("doggyFrontCum", value: false);
				heroineAnimator.SetBool("doggyBackTease", value: false);
				heroineAnimator.SetBool("doggyBackSex1", value: false);
				heroineAnimator.SetBool("doggyBackSex2", value: false);
				heroineAnimator.SetBool("doggyBackCum", value: false);
				DisableUI();
			}
			deadReset = true;
		}
	}

	private void Release()
	{
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		Heroine.GetComponent<PlayerController>().enabled = true;
		currentCum = 0f;
		currentStamina = 0f;
		pct = 0f;
		fuckDelay = 0f;
		timer = 0f;
		camShaked = false;
		speedModifier = 1f;
		knottedOnce = false;
		agent.enabled = true;
		agent.isStopped = false;
		fartiged = true;
		attackedAgain = false;
		longCumSound.Stop();
		cumSound.Stop();
		agent.ResetPath();
		GetComponent<CapsuleCollider>().enabled = true;
		heroineAnimator.speed = 1f;
		animator.speed = 1f;
		PlayerController.iGetFucked = false;
		PlayerController.heIsFuckingHard = false;
		CameraFollow.target = Heroine.transform;
		HeroineStats.stunned = false;
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
		animator.SetBool("attack", value: false);
		animator.SetBool("isSitting", value: false);
		animator.SetBool("isAlert", value: false);
		animator.SetBool("backTease", value: false);
		animator.SetBool("backSex1", value: false);
		animator.SetBool("backSex2", value: false);
		animator.SetBool("backCum", value: false);
		animator.SetBool("backKnot", value: false);
		animator.SetBool("frontTease", value: false);
		animator.SetBool("frontSex1", value: false);
		animator.SetBool("frontSex2", value: false);
		animator.SetBool("frontCum", value: false);
		if (PlayerController.iFalledFront)
		{
			heroineAnimator.SetBool("doggyFrontTease", value: false);
			heroineAnimator.SetBool("doggyFrontSex1", value: false);
			heroineAnimator.SetBool("doggyFrontSex2", value: false);
			heroineAnimator.SetBool("doggyFrontCum", value: false);
		}
		if (PlayerController.iFalledBack)
		{
			heroineAnimator.SetBool("doggyBackTease", value: false);
			heroineAnimator.SetBool("doggyBackSex1", value: false);
			heroineAnimator.SetBool("doggyBackSex2", value: false);
			heroineAnimator.SetBool("doggyBackCum", value: false);
		}
		state = MyState.PATROL;
	}

	private void Chase()
	{
		WalkOrIdle();
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		agent.SetDestination(Heroine.transform.position);
		agent.speed = 5f;
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		if (distance < attackRange && !PlayerController.iFalled)
		{
			GetComponent<CapsuleCollider>().enabled = false;
			agent.isStopped = true;
			state = MyState.ATTACK;
		}
	}

	private void Sex()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		InitiateUI();
		VignetteEffect();
		longCumSound.Stop();
		cumSound.Stop();
		PlayerManager.IsVirgin = false;
		if (PlayerController.iFalledBack)
		{
			base.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			if (currentCum >= maxCum)
			{
				state = MyState.CUM;
			}
			if (currentCum > sex2Range)
			{
				animator.SetBool("backSex2", value: true);
				heroineAnimator.SetBool("doggyBackSex2", value: true);
				return;
			}
			animator.SetBool("backSex1", value: true);
			heroineAnimator.SetBool("doggyBackSex1", value: true);
		}
		if (PlayerController.iFalledFront)
		{
			base.transform.rotation = mountFrontPos.rotation;
			base.transform.position = mountFrontPos.position;
			if (currentCum >= maxCum)
			{
				state = MyState.CUM;
			}
			if (currentCum > sex2Range)
			{
				animator.SetBool("frontCum", value: false);
				heroineAnimator.SetBool("doggyFrontCum", value: false);
				animator.SetBool("frontSex2", value: true);
				heroineAnimator.SetBool("doggyFrontSex2", value: true);
			}
			else
			{
				animator.SetBool("frontSex1", value: true);
				heroineAnimator.SetBool("doggyFrontSex1", value: true);
			}
		}
	}

	private void Knot()
	{
		if (!HeroineStats.GameOver && knottedOnce)
		{
			Release();
			return;
		}
		cumSound.Stop();
		longCumSound.Stop();
		if (!pantSound.isPlaying)
		{
			pantSound.Play();
		}
		HeroineStats.stunned = false;
		InitiateUI();
		base.transform.rotation = Heroine.transform.rotation;
		base.transform.position = Heroine.transform.position;
		currentStamina += 5f * Time.deltaTime;
		EnemyUI.instance.RestoreHealth(5f * Time.deltaTime);
		if (currentCum <= 20f)
		{
			currentCum += Time.deltaTime;
		}
		animator.SetBool("backCum", value: false);
		heroineAnimator.SetBool("doggyBackCum", value: false);
		animator.SetBool("backTease", value: false);
		heroineAnimator.SetBool("doggyBackTease", value: false);
		animator.SetBool("backSex1", value: false);
		heroineAnimator.SetBool("doggyBackSex1", value: false);
		animator.SetBool("backKnot", value: true);
		heroineAnimator.SetBool("doggyBackKnot", value: true);
		if (currentStamina >= maxStamina)
		{
			currentCum = 20f;
			if (!HeroineStats.GameOver)
			{
				knottedOnce = true;
			}
			else
			{
				knottedOnce = false;
			}
			state = MyState.SEX;
		}
	}

	private void Cum()
	{
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		speedModifier = 1f;
		HeroineStats.stunned = true;
		InitiateUI();
		VignetteEffect();
		HeroineStats.creampied = true;
		HeroineStats.fertileCum = true;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (PlayerController.iFalledBack)
		{
			base.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			animator.SetBool("backKnot", value: false);
			heroineAnimator.SetBool("doggyBackKnot", value: false);
			animator.SetBool("backSex2", value: false);
			heroineAnimator.SetBool("doggyBackSex2", value: false);
			animator.SetBool("backCum", value: true);
			heroineAnimator.SetBool("doggyBackCum", value: true);
		}
		if (PlayerController.iFalledFront)
		{
			base.transform.rotation = mountFrontPos.rotation;
			base.transform.position = mountFrontPos.position;
			heroineAnimator.SetBool("doggyFrontTease", value: false);
			heroineAnimator.SetBool("doggyFrontSex1", value: false);
			heroineAnimator.SetBool("doggyFrontSex2", value: false);
			animator.SetBool("frontTease", value: false);
			animator.SetBool("frontSex1", value: false);
			animator.SetBool("frontSex2", value: false);
			animator.SetBool("frontCum", value: true);
			heroineAnimator.SetBool("doggyFrontCum", value: true);
		}
		if (!(currentCum <= 0f))
		{
			return;
		}
		HeroineStats.HumanoidBuff = true;
		if (PlayerController.iFalledBack)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			state = MyState.KNOT;
		}
		if (PlayerController.iFalledFront)
		{
			if (HeroineStats.GameOver)
			{
				currentCum = 0f;
				currentStamina = maxStamina;
				state = MyState.SEX;
			}
			else
			{
				Release();
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
		if (PlayerController.iFalledBack)
		{
			if (attackedAgain)
			{
				if (!PlayerController.iFalled)
				{
					return;
				}
				if (HeroineStats.birth)
				{
					Heroine.GetComponent<HeroineStats>().mySexPartner = null;
					timer = 0f;
					agent.enabled = true;
					agent.isStopped = false;
					state = MyState.PATROL;
					return;
				}
				timer += Time.deltaTime;
				if (timer > 0.5f)
				{
					InitiateUI();
					PlayerController.iGetFucked = true;
					base.transform.rotation = Heroine.transform.rotation;
					base.transform.position = Heroine.transform.position;
					CameraFollow.target = targetCamBehind;
					animator.SetBool("backTease", value: true);
					heroineAnimator.SetBool("doggyBackTease", value: true);
					if (!EquipmentManager.heroineIsNaked)
					{
						Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
					}
					else
					{
						InitiateSex();
					}
				}
				return;
			}
			if (HeroineStats.birth)
			{
				Heroine.GetComponent<HeroineStats>().mySexPartner = null;
				agent.enabled = true;
				agent.isStopped = false;
				state = MyState.PATROL;
				return;
			}
			animator.SetBool("isSitting", value: true);
			GetComponent<CapsuleCollider>().enabled = false;
			FaceTarget();
		}
		if (PlayerController.iFalledFront)
		{
			animator.SetBool("isRunning", value: false);
			timer += Time.deltaTime;
			if (HeroineStats.birth)
			{
				Heroine.GetComponent<HeroineStats>().mySexPartner = null;
				timer = 0f;
				agent.enabled = true;
				agent.isStopped = false;
				state = MyState.PATROL;
				return;
			}
			if (timer > 0.5f)
			{
				InitiateUI();
				PlayerController.iGetFucked = true;
				base.transform.rotation = mountFrontPos.rotation;
				base.transform.position = mountFrontPos.position;
				CameraFollow.target = targetCamBehind;
				animator.SetBool("frontTease", value: true);
				heroineAnimator.SetBool("doggyFrontTease", value: true);
				animator.Play("rig|DoggyFrontTease");
				heroineAnimator.Play("rig|DoggyFrontTease");
				if (!EquipmentManager.heroineIsNaked)
				{
					Heroine.GetComponent<HeroineStats>().LosePantiesDurability(10f);
				}
				else
				{
					InitiateSex();
				}
			}
			else
			{
				animator.SetBool("isSitting", value: true);
			}
		}
		if (!PlayerController.iFalled)
		{
			Heroine.GetComponent<PlayerController>().enabled = false;
			animator.SetBool("attack", value: true);
			base.transform.position = Vector3.MoveTowards(base.transform.position, Heroine.transform.position, Time.deltaTime * 100f);
			attackedAgain = true;
		}
		else
		{
			animator.SetBool("attack", value: false);
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
			if (!camShaked)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				camShaked = true;
			}
			state = MyState.SEX;
		}
	}

	private void Attack()
	{
		if (HeroineStats.birth)
		{
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			agent.enabled = true;
			agent.isStopped = false;
			state = MyState.PATROL;
			return;
		}
		if (PlayerController.iFalled)
		{
			animator.SetBool("attack", value: false);
			timer = 0f;
			state = MyState.TEASE;
			return;
		}
		timer += Time.deltaTime;
		animator.SetBool("attack", value: true);
		FaceTarget();
		checkHitAngle();
		if (timer > 0.25f)
		{
			agent.isStopped = false;
			agent.SetDestination(Heroine.transform.position);
			body.velocity = base.transform.forward * Time.deltaTime * 50f;
			if (timer > 2f)
			{
				animator.SetBool("attack", value: false);
				timer = 0f;
			}
		}
	}

	private void Patroll()
	{
		controlInstance.viewRadius = 8f;
		agent.isStopped = false;
		agent.speed = patrolSpeed;
		WalkOrIdle();
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (ignoreHer)
		{
			timer += Time.deltaTime;
			if (Heroine.GetComponent<HeroineStats>().mySexPartner == base.gameObject)
			{
				Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			}
			if (timer > 2f)
			{
				ignoreHer = false;
				timer = 0f;
			}
		}
		if (travelling && agent.remainingDistance <= 1f)
		{
			travelling = false;
			animator.SetBool("isRunning", value: false);
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
		staminaUI.gameObject.SetActive(value: false);
		if (HeroineStats.birth)
		{
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			return;
		}
		if (HeroineStats.GameOver)
		{
			currentStamina = maxStamina;
		}
		if (!fartiged)
		{
			if (!(distance < attentionRange) || PlayerController.walking)
			{
				return;
			}
			if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
			{
				Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			}
			if (!(Heroine.GetComponent<HeroineStats>().mySexPartner != base.gameObject) && controlInstance.heroineIsVisible && !ignoreHer)
			{
				if (HeroineStats.GameOver)
				{
					state = MyState.ATTACK;
				}
				else
				{
					state = MyState.ATTENTION;
				}
			}
		}
		else
		{
			RegenerateStamina();
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

	private void Attention()
	{
		if (HeroineStats.birth)
		{
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			agent.enabled = true;
			agent.isStopped = false;
			state = MyState.PATROL;
			return;
		}
		if (!controlInstance.heroineIsVisible || Heroine.GetComponent<HeroineStats>().mySexPartner != base.gameObject)
		{
			agent.isStopped = false;
			animator.SetBool("isAlert", value: false);
			attentionSoundPlayed = false;
			state = MyState.PATROL;
			return;
		}
		timer += Time.deltaTime;
		if (attentionTimer > timer)
		{
			FaceTarget();
			if (!attentionSoundPlayed)
			{
				attentionGrowl.Play();
				attentionSoundPlayed = true;
			}
			agent.isStopped = true;
			animator.SetBool("isAlert", value: true);
			return;
		}
		if (PlayerController.standing)
		{
			timer += Time.deltaTime;
			FaceTarget();
			if (timer > alertTimer)
			{
				ignoreHer = true;
				animator.SetBool("isAlert", value: false);
				attentionSoundPlayed = false;
				Heroine.GetComponent<HeroineStats>().mySexPartner = null;
				state = MyState.PATROL;
				timer = 0f;
			}
		}
		else
		{
			animator.SetBool("isAlert", value: false);
			agent.isStopped = false;
			timer = 0f;
			bark.Play();
			attentionSoundPlayed = false;
			state = MyState.CHASE;
		}
		if (checker.gotHit)
		{
			animator.SetBool("isAlert", value: false);
			agent.isStopped = false;
			timer = 0f;
			bark.Play();
			attentionSoundPlayed = false;
			state = MyState.CHASE;
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

	private void WalkOrIdle()
	{
		if (agent.velocity == Vector3.zero)
		{
			animator.SetBool("isRunning", value: false);
			sitTimer += Time.deltaTime;
			if (sitTimer > sitOnTimer)
			{
				animator.SetBool("isSitting", value: true);
			}
		}
		else
		{
			animator.SetBool("isRunning", value: true);
			animator.SetBool("isSitting", value: false);
			sitTimer = 0f;
		}
	}

	private void RegenerateStamina()
	{
		staminaUI.gameObject.SetActive(value: true);
		if (staminaUI != null)
		{
			staminaUI.position = targetStaminaUI.position;
			staminaUI.forward = -cam.forward;
			stamSlider.fillAmount = currentStamina / maxStamina;
		}
		currentStamina += Time.deltaTime * 0.7f;
		if (currentStamina >= maxStamina)
		{
			fartiged = false;
			staminaUI.gameObject.SetActive(value: false);
		}
	}

	private void checkHitAngle()
	{
		Vector3 forward = Heroine.transform.forward;
		Vector3 to = Heroine.transform.position - base.transform.position;
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

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void LoseCumInstant(float gainValue)
	{
		currentCum -= gainValue;
		EnemyUI.instance.LoseCum(gainValue);
	}

	public void DrainStaminaInstant(float drainValue)
	{
		if (currentStamina < 0f)
		{
			currentStamina = 0f;
		}
		if (currentCum < 50f)
		{
			currentStamina -= drainValue;
			EnemyUI.instance.TakeDamage(drainValue);
		}
		else
		{
			currentStamina -= drainValue / 2f;
			EnemyUI.instance.TakeDamage(drainValue / 2f);
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

	private void ThrustEvent()
	{
		if (!HeroineStats.HumanoidBuff)
		{
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg / 10f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		}
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		if (currentCum > sex2Range)
		{
			audioSource.clip = clapSoundsArray[Random.Range(0, clapSoundsArray.Length)];
			audioSource.PlayOneShot(audioSource.clip);
			slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		}
		else
		{
			slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		}
		if (currentCum > 50f)
		{
			if (speedModifier < 2f)
			{
				speedModifier += 0.05f;
				Debug.Log(speedModifier);
			}
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
			animator.speed = speedModifier;
			heroineAnimator.speed = speedModifier;
			if (!pantSound.isPlaying)
			{
				pantSound.Play();
			}
			if (speedModifier > 1.5f)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			}
			GainCumInstant(gainCumPerThrust / 2f);
		}
		else
		{
			GainCumInstant(gainCumPerThrust);
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		}
		thrusted = true;
	}

	private void CumEvent()
	{
		if (HeroineStats.MantisBuff)
		{
			checker.drainHealth(5f);
		}
		DrainStaminaInstant(20f);
		LoseCumInstant(15f);
		if (HeroineStats.HumanoidBuff)
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 10f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg * 10f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		}
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		longCumSound.Play();
		thrusted = true;
	}

	private void InsertEvent()
	{
		if (!insert)
		{
			insertOneSound.Play();
			insert = true;
		}
		else
		{
			insertTwoSound.Play();
			insert = false;
		}
	}

	private void AttackEvent()
	{
		checkHitAngle();
		attackSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
		if (hitFront)
		{
			PlayerController.iFalled = true;
			PlayerController.iFalledFront = true;
			heroineAnimator.SetBool("falled", value: true);
		}
		if (hitBack)
		{
			PlayerController.iFalled = true;
			PlayerController.iFalledBack = true;
			heroineAnimator.SetBool("isFalledBack", value: true);
		}
	}

	private void StepEvent()
	{
		stepSound1.Play();
	}

	private void Step2Event()
	{
		stepSound2.Play();
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, attentionRange);
	}
}
