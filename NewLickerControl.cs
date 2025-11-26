using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewLickerControl : MonoBehaviour
{
	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	[SerializeField]
	public Transform healthUI;

	[SerializeField]
	public Transform staminaUi;

	[SerializeField]
	public Transform cam;

	private Image healthSlider;

	private GameObject Heroine;

	private float distance;

	private float patrolStoppingDistance = 0.3f;

	public GameObject healthUiPrefab;

	public GameObject staminaUiPrefab;

	public AudioSource stepSound;

	public AudioSource attackSound;

	public AudioSource lickSexSound;

	public AudioSource sexSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public Transform targetHealthUi;

	public Transform targetStaminaUi;

	public GameObject mySaveStateHolder;

	public GameObject myTongue;

	public Transform camPosLick;

	public Transform camPosFuck;

	public List<Waypoint> patrolPoints;

	public float patrolSpeed = 2.5f;

	public float totalWaitTime = 3f;

	public float switchProbability = 0.2f;

	private bool travelling;

	private bool waiting;

	private bool patrolForward;

	public bool patrolWaiting;

	private float waitTimer;

	private int currentPatrolIndex;

	private float currentHealth;

	private float currentStamina;

	private float currentCum;

	[Header("Stats")]
	public float maxHealth;

	[Header("Stats")]
	public float maxStamina;

	[Header("Stats")]
	public float maxCum;

	[Header("Stats")]
	public float chaseRadius;

	[Header("Stats")]
	public float attackRadius;

	[Header("Stats")]
	public float silentAttackRadius;

	[Header("Stats")]
	public float thrustDmg;

	[Header("Stats")]
	public float teaseDmg;

	[Header("Stats")]
	public float gainCumPerThrust;

	[Header("Stats")]
	public float staminaRegAmount;

	[Header("Stats")]
	public float drainStamAmount;

	[Header("Stats")]
	public float drainCumAmount;

	[Header("Stats")]
	public float expPerThrust;

	[Header("Stats")]
	public int impregnateChance = 10;

	private bool gotHit;

	private bool impregRolette;

	private bool gotAttacked;

	public bool sheMine;

	public float reactionTime;

	public float chaseSpeed;

	public Transform mount_place;

	private Image circle;

	private Image SexImage;

	private Image stamSlider;

	private float mountDelay;

	private float mountTime = 1f;

	private float fuckTime = 4f;

	private float pct;

	private bool iLick;

	private bool iLickFuck;

	private bool iFuck;

	private bool iRest;

	private bool sheOrgasmed;

	private bool fireRelease;

	private bool cumming;

	private bool detected;

	public float lickFuckTime = 10f;

	private int cumCount;

	public bool superLicker;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		DebugStuff();
		anim = GetComponent<Animator>();
		Heroine = GameObject.Find("Heroine");
		heroineAnim = Heroine.GetComponent<Animator>();
		cam = Camera.main.transform;
		SexImage = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse").GetComponent<Image>();
		circle = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse/circle (1)").GetComponent<Image>();
		currentHealth = maxHealth;
		currentStamina = maxStamina;
		currentCum = 0f;
		Canvas[] array = Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				healthUI = Object.Instantiate(healthUiPrefab, canvas.transform).transform;
				staminaUi = Object.Instantiate(staminaUiPrefab, canvas.transform).transform;
				healthSlider = healthUI.GetChild(0).GetComponent<Image>();
				stamSlider = staminaUi.GetChild(0).GetComponent<Image>();
				break;
			}
		}
		healthUI.gameObject.SetActive(value: false);
		staminaUi.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		WalkOrIdle();
		if (healthUI != null)
		{
			healthUI.position = targetHealthUi.position;
			healthUI.forward = -cam.forward;
		}
		if (staminaUi != null)
		{
			staminaUi.position = targetStaminaUi.position;
			staminaUi.forward = -cam.forward;
		}
		if (Safespace.heroineSafe)
		{
			if (detected)
			{
				agent.ResetPath();
				ChangePatrolPoint();
				detected = false;
			}
			Patrolling();
			return;
		}
		if (HeroineStats.birth)
		{
			Patrolling();
			return;
		}
		if (iRest)
		{
			if (!fireRelease)
			{
				Release();
			}
			Patrolling();
			staminaUi.gameObject.SetActive(value: true);
			GetComponent<CapsuleCollider>().enabled = true;
			GetComponent<CapsuleCollider>().isTrigger = false;
			GainStamina(staminaRegAmount);
			if (gotHit)
			{
				currentStamina = maxStamina;
			}
			if (currentStamina >= maxStamina)
			{
				gotHit = false;
				fireRelease = false;
				iRest = false;
			}
			return;
		}
		if (PlayerController.iFalled)
		{
			if (sheMine)
			{
				ClampStats();
				staminaUi.gameObject.SetActive(value: false);
				healthUI.gameObject.SetActive(value: false);
				mountDelay += Time.deltaTime;
				InitiateUI();
				if (currentStamina <= 0f)
				{
					insertSound.Stop();
					if (!cumming)
					{
						iRest = true;
					}
				}
				Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
				if (cumming)
				{
					Cum();
					return;
				}
				cumSound.Stop();
				if (iFuck)
				{
					Fuck();
					CameraFollow.target = camPosFuck;
					PlayerController.heIsFuckingHard = true;
				}
				else if (iLickFuck)
				{
					if (HeroineStats.orgasm)
					{
						anim.SetBool("HLickFuckOrg", value: true);
						heroineAnim.SetBool("LickLickFuckOrg", value: true);
						HeroineStats.stunned = true;
						HeroineStats.currentPower = 0f;
						Heroine.GetComponent<HeroineStats>().UpdateStats();
						sheOrgasmed = true;
					}
					else
					{
						if (sheOrgasmed)
						{
							HeroineStats.stunned = false;
							iFuck = true;
						}
						LickFuck();
						HeroineStats.aroused = true;
					}
				}
				else if (iLick)
				{
					Lick();
				}
				else
				{
					Mount();
					CameraFollow.target = camPosLick;
				}
			}
			else
			{
				Patrolling();
			}
			return;
		}
		sheMine = false;
		if (PlayerController.isSilent)
		{
			if (distance <= silentAttackRadius)
			{
				detected = true;
			}
		}
		else if (distance <= chaseRadius)
		{
			detected = true;
		}
		if (!sheMine && !detected)
		{
			Patrolling();
		}
		else if (detected)
		{
			if (!superLicker)
			{
				agent.stoppingDistance = attackRadius;
			}
			if (distance <= attackRadius + 0.5f)
			{
				FaceTarget();
				if (!PlayerController.iFalled)
				{
					anim.SetBool("isAttacking", value: true);
				}
			}
			else
			{
				anim.SetBool("isAttacking", value: false);
				StartCoroutine(Chase());
			}
		}
		else
		{
			detected = false;
			Patrolling();
		}
	}

	private void Release()
	{
		DisableUI();
		mountDelay = 0f;
		GetComponent<CapsuleCollider>().enabled = false;
		anim.SetBool("HLickFuckOrg", value: false);
		anim.SetBool("isLickFucking", value: false);
		anim.SetBool("isLicking", value: false);
		anim.SetBool("isCumming", value: false);
		anim.SetBool("isBeforeLicking", value: false);
		anim.SetBool("isLickerSex", value: false);
		heroineAnim.SetBool("LickBeforeLicking", value: false);
		heroineAnim.SetBool("LickLicking", value: false);
		heroineAnim.SetBool("LickLickFuckOrg", value: false);
		heroineAnim.SetBool("LickLickFucking", value: false);
		PlayerController.heIsFuckingHard = false;
		iLick = false;
		iLickFuck = false;
		iFuck = false;
		sheOrgasmed = false;
		cumming = false;
		iRest = true;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		insertSound.Stop();
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		CameraFollow.target = Heroine.gameObject.transform;
		agent.enabled = true;
		agent.isStopped = false;
		currentCum = 0f;
		cumCount = 0;
		currentStamina = 0f;
		sheMine = false;
		fireRelease = true;
	}

	private void Mount()
	{
		anim.SetBool("isAttacking", value: false);
		GetComponent<CapsuleCollider>().isTrigger = true;
		if (mountDelay >= mountTime)
		{
			PlayerController.iGetFucked = true;
			agent.enabled = false;
			base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, mount_place.rotation, 0.2f);
			base.gameObject.transform.position = Vector3.Lerp(base.gameObject.transform.position, mount_place.position, 0.2f);
			anim.SetBool("isBeforeLicking", value: true);
			heroineAnim.SetBool("LickBeforeLicking", value: true);
			SexImage.enabled = true;
			circle.enabled = true;
			pct = mountDelay / fuckTime;
			circle.fillAmount = pct;
		}
		if (mountDelay >= fuckTime)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			iLick = true;
		}
	}

	private void Lick()
	{
		anim.SetBool("isLicking", value: true);
		anim.SetBool("isBeforeLicking", value: false);
		heroineAnim.SetBool("LickBeforeLicking", value: false);
		heroineAnim.SetBool("LickLicking", value: true);
		heroineAnim.SetBool("isScared", value: true);
		HeroineStats.creampied = false;
		HeroineStats.lustyCum = false;
		HeroineStats.addictiveCum = false;
		HeroineStats.fertileCum = false;
		Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMesh);
		Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
		gotHit = false;
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
		}
		if (EquipmentManager.heroineIsNaked)
		{
			iLickFuck = true;
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(teaseDmg);
		}
	}

	private void LickFuck()
	{
		anim.SetBool("isLickFucking", value: true);
		heroineAnim.SetBool("LickLickFucking", value: true);
		heroineAnim.SetBool("isScared", value: false);
		PlayerManager.IsVirgin = false;
	}

	private void Fuck()
	{
		anim.SetBool("HLickFuckOrg", value: false);
		anim.SetBool("isLickerSex", value: true);
		heroineAnim.SetBool("LickLickFuckOrg", value: false);
		PlayerManager.IsVirgin = false;
	}

	private void Cum()
	{
		if (currentCum <= 0f)
		{
			anim.SetBool("isCumming", value: false);
			heroineAnim.SetBool("LickLickerCum", value: false);
			cumming = false;
			currentCum = 0f;
			impregRolette = false;
			return;
		}
		if (!impregRolette)
		{
			cumCount++;
			Debug.Log("CumCount: " + cumCount);
			impregRolette = true;
		}
		if (!HeroineStats.pregnant)
		{
			HeroineStats.fertileCum = true;
		}
		HeroineStats.lustyCum = true;
		if (cumCount > 2)
		{
			HeroineStats.hugeAmount = true;
			heroineAnim.SetBool("isCumFilled", value: true);
		}
		anim.SetBool("isCumming", value: true);
		heroineAnim.SetBool("LickLickerCum", value: true);
		HeroineStats.creampied = true;
		HeroineStats.lustyCum = true;
		HeroineStats.fertileCum = true;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		DrainCum(drainCumAmount);
		if (!HeroineStats.GameOver)
		{
			DrainStamina(drainStamAmount);
		}
	}

	private IEnumerator Chase()
	{
		yield return new WaitForSeconds(reactionTime);
		agent.speed = chaseSpeed;
		agent.SetDestination(Heroine.transform.position);
	}

	private void Patrolling()
	{
		agent.stoppingDistance = patrolStoppingDistance;
		anim.SetBool("isAttacking", value: false);
		sheMine = false;
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

	private void WalkOrIdle()
	{
		if (agent.velocity.magnitude != 0f)
		{
			anim.SetBool("isWalking", value: true);
		}
		else
		{
			anim.SetBool("isWalking", value: false);
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
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

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		_ = currentCum / maxCum;
		EnemyUI.instance.LoseCum(drainValue);
	}

	public void DrainStamina(float drainValue)
	{
		currentStamina -= drainValue * Time.deltaTime;
		_ = currentStamina / maxStamina;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		_ = currentStamina / maxStamina;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void GainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
		float fillAmount = currentStamina / maxStamina;
		stamSlider.fillAmount = fillAmount;
	}

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
		gotHit = true;
		drainHealth(amount);
		currentStamina = maxStamina;
		agent.SetDestination(Heroine.transform.position);
		if (currentHealth <= 0f)
		{
			Die();
		}
	}

	public void Die()
	{
		healthUI.gameObject.SetActive(value: false);
		staminaUi.gameObject.SetActive(value: false);
		anim.SetBool("dead", value: true);
		agent.enabled = false;
		mySaveStateHolder.GetComponent<EnemSavState>().isDead = true;
		GetComponent<CapsuleCollider>().enabled = false;
		GetComponent<NewLickerControl>().enabled = false;
	}

	public void drainHealth(float drainValue)
	{
		currentHealth -= drainValue;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		_ = currentCum / maxCum;
		EnemyUI.instance.GainCum(gainValue);
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
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, silentAttackRadius);
	}

	private void StepEvent()
	{
		stepSound.Play();
	}

	private void AttackEvent()
	{
		attackSound.Play();
	}

	private void LickEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 4f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg * 4f);
		lickSexSound.Play();
	}

	private void ThrustEvent()
	{
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		GainCumInstant(gainCumPerThrust);
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		if (currentCum >= maxCum)
		{
			cumming = true;
		}
		if (currentStamina <= 0f && !cumming)
		{
			iRest = true;
		}
	}
}
