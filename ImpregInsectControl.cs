using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ImpregInsectControl : MonoBehaviour
{
	private Transform standCam;

	public Transform anim_pos;

	public Transform mount_place;

	public Transform targetStaminaUi;

	public Transform targetHealthUi;

	public GameObject stamUiPrefab;

	public GameObject healthUiPrefab;

	public GameObject gameManager;

	public GameObject Heroine;

	public GameObject enemSaveState;

	public AudioSource sexSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public AudioSource imAware;

	public AudioSource rubSound;

	private Transform target;

	private NavMeshAgent agent;

	private Animator anim;

	private Transform staminaUi;

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

	public float expPerThrust = 1.5f;

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

	private bool iInsert;

	public bool imOnHer;

	private bool unfuck;

	public bool imSuper;

	private float pct;

	private bool cummed;

	private ImpregInsectControl thisScript;

	private Animator heroanim;

	private Transform camPosLayign;

	public List<GameObject> BodyParts = new List<GameObject>();

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private SphereCollider sphereCol;

	private void Start()
	{
		target = PlayerManager.instance.player.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		thisScript = GetComponent<ImpregInsectControl>();
		heroanim = Heroine.GetComponent<Animator>();
		cam = Camera.main.transform;
		camPosLayign = GameObject.Find("Heroine/CameraFollowOnBackFall").transform;
		standCam = GameObject.Find("CameraBase").GetComponent<CameraFollow>().cameraFollowObj.transform;
		currentStamina = maxStamina;
		currentCum = 0f;
		cummed = false;
		currentHealth = maxHealth;
		sphereCol = GetComponent<SphereCollider>();
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
		DebugStuff();
	}

	public void Update()
	{
		if (currentHealth <= 0f && !cumming)
		{
			Die();
			return;
		}
		distance = Vector3.Distance(target.position, base.transform.position);
		if (distance <= attackRadius)
		{
			sphereCol.enabled = false;
		}
		else
		{
			sphereCol.enabled = true;
		}
		if (iRest)
		{
			staminaUi.gameObject.SetActive(value: true);
			gainStamina(regenRate / 3f);
		}
		AILogic();
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
		if (currentStamina >= 1f)
		{
			noStamina = false;
		}
		if (currentStamina >= maxStamina)
		{
			iRest = false;
		}
		if (currentStamina == 0f)
		{
			noStamina = true;
		}
		if (currentCum == maxCum)
		{
			cumming = true;
			cummed = true;
		}
		if (cumming)
		{
			DrainCum(10f);
			drainStamina(0.05f);
			anim.speed = 1f;
			heroanim.speed = 1f;
			Cumming();
			HeroineStats.fartiged = true;
			Heroine.GetComponent<Animator>().SetFloat("pregSpeed", 10f);
			Heroine.GetComponent<HeroineStats>().GainPreg(0.05f);
			HeroineStats.creampied = true;
			if (!HeroineStats.pregnant)
			{
				HeroineStats.fertileCum = true;
			}
			HeroineStats.hugeAmount = true;
			heroanim.SetBool("isCumFilled", value: true);
		}
		if (currentCum <= 0f)
		{
			cumming = false;
			if (cummed)
			{
				PlayerController.iGetInserted = false;
				PlayerController.iGetFucked = false;
				PlayerController.iFalledBack = true;
				Heroine.GetComponent<Animator>().SetFloat("pregSpeed", 1f);
				heroanim.SetBool("ImpregInsRub", value: false);
				heroanim.SetBool("ImpregInsFuck", value: false);
				heroanim.SetBool("ImpregInsCum", value: false);
				heroanim.SetBool("isFalledBack", value: true);
				heroanim.SetBool("impregSex", value: false);
				heroanim.SetBool("impregCum", value: false);
				Die();
			}
		}
		if (iInsert || iFuck)
		{
			InitiateUI();
			staminaUi.gameObject.SetActive(value: false);
			healthUI.gameObject.SetActive(value: false);
		}
		if (cumming && !cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (!cumming)
		{
			cumSound.Stop();
		}
		if (iInsert)
		{
			if (!insertSound.isPlaying)
			{
				insertSound.Play();
			}
		}
		else
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
		if (AbortBed.abortBedSitting)
		{
			Patrolling();
			return;
		}
		if (Heroine.GetComponent<HeroineStats>().mySexPartner != null && Heroine.GetComponent<HeroineStats>().mySexPartner != base.gameObject)
		{
			Patrolling();
			return;
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
		if (!iRest && distance <= mountRadius && imOnHer)
		{
			if (iMount)
			{
				if (!PlayerController.iFalledFront)
				{
					Mount();
					Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
				}
				else
				{
					FaceTarget();
				}
			}
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
			iRest = true;
			PlayerController.claimed = false;
			HeroineStats.aroused = false;
			currentCum = 0f;
			DisableUI();
			GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
			iMount = false;
			Rest();
		}
		if (iRest)
		{
			Rest();
		}
		else if (Safespace.heroineSafe)
		{
			ChaseLogic();
			if (!iMount && imOnHer && !PlayerController.claimed)
			{
				AttackLogic();
			}
		}
	}

	private void ChaseLogic()
	{
		if (distance <= chaseRadius && distance > mountRadius && !iRest)
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
				agent.speed = chaseSpeed;
				anim.SetBool("isIdle", value: false);
				agent.isStopped = false;
			}
			else
			{
				agent.isStopped = true;
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
		if (!PlayerController.iFalledBack)
		{
			anim.SetBool("isCumming", value: true);
			heroanim.SetBool("ImpregInsCum", value: true);
		}
		else
		{
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
			anim.SetBool("cum", value: true);
			heroanim.SetBool("impregCum", value: true);
		}
		if (HeroineStats.GameOver)
		{
			Heroine.GetComponent<HeroineStats>().GainPreg(0.1f);
		}
	}

	private void Patrolling()
	{
		agent.isStopped = false;
		agent.speed = patrolSpeed;
		staminaUi.gameObject.SetActive(value: false);
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
			anim.SetBool("isIdle", value: true);
			if (waitTimer >= totalWaitTime)
			{
				waiting = false;
				ChangePatrolPoint();
				SetDestination();
			}
		}
	}

	private void AttackLogic()
	{
		if (distance <= attackRadius && !iRest && !HeroineStats.immune)
		{
			imOnHer = true;
			iMount = true;
		}
	}

	private void Mount()
	{
		unfuck = false;
		InitiateUI();
		PlayerController.claimed = true;
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(teaseDmg * 3f);
		}
		if (!iFuck)
		{
			iMount = true;
			iInsert = true;
			agent.enabled = false;
			PlayerController.iFalled = true;
			PlayerController.iGetInserted = true;
			if (PlayerController.iFalledBack)
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
				CameraFollow.target = camPosLayign;
				GetComponent<SphereCollider>().isTrigger = true;
				anim.SetBool("tease", value: true);
				anim.SetBool("isIdle", value: false);
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("rig|behind_tease_laying"))
				{
					Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
					rubSound.Play();
				}
				if (EquipmentManager.heroineIsNaked)
				{
					if (!iFuck)
					{
						mountDelay += Time.deltaTime;
					}
					SexImage.enabled = true;
					circle.enabled = true;
					pct = mountDelay / fuckTime;
					circle.fillAmount = pct;
					PlayerController.iGetInserted = true;
				}
			}
			else
			{
				anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
				anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
				CameraFollow.target = standCam;
				anim.SetBool("isGrabbing", value: true);
				anim.SetBool("isIdle", value: false);
				GetComponent<SphereCollider>().isTrigger = true;
				heroanim.SetBool("ImpregInsRub", value: true);
				if (!PlayerManager.SAB)
				{
					heroanim.SetBool("isScared", value: true);
				}
				else
				{
					heroanim.SetBool("isHorny", value: true);
				}
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("rig|Rub"))
				{
					Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
					rubSound.Play();
				}
				if (EquipmentManager.heroineIsNaked)
				{
					if (!iFuck)
					{
						mountDelay += Time.deltaTime;
					}
					SexImage.enabled = true;
					circle.enabled = true;
					pct = mountDelay / fuckTime;
					circle.fillAmount = pct;
					PlayerController.iGetInserted = true;
				}
			}
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
			mountDelay = 0f;
			iInsert = false;
			PlayerController.iGetInserted = false;
			heroanim.SetBool("isScared", value: false);
			heroanim.SetBool("isHorny", value: false);
			anim_pos.rotation = Quaternion.Lerp(anim_pos.rotation, mount_place.rotation, 0.2f);
			anim_pos.position = Vector3.Lerp(anim_pos.position, mount_place.position, 0.2f);
			PlayerManager.IsVirgin = false;
			HeroineStats.aroused = true;
			Heroine.GetComponent<HeroineStats>().GainLust(teaseDmg);
			VignetteEffect();
			if (currentCum > 50f)
			{
				PlayerController.heIsFuckingHard = true;
				anim.speed = 1.5f;
				heroanim.speed = 1.5f;
			}
			if (!PlayerController.iFalledBack)
			{
				anim.SetBool("isFucking", value: true);
				anim.SetBool("isGrabbing", value: false);
				anim.SetBool("isCumming", value: false);
				heroanim.SetBool("ImpregInsFuck", value: true);
				heroanim.SetBool("ImpregInsRub", value: false);
				heroanim.SetBool("ImpregInsCum", value: false);
			}
			else
			{
				anim.SetBool("sex", value: true);
				anim.SetBool("tease", value: false);
				heroanim.SetBool("impregSex", value: true);
				PlayerController.heIsFuckingHard = true;
			}
		}
	}

	private void Rest()
	{
		GetComponent<SphereCollider>().isTrigger = false;
		iFuck = false;
		iInsert = false;
		agent.enabled = true;
		anim.speed = 1f;
		heroanim.speed = 1f;
		staminaUi.gameObject.SetActive(value: true);
		rubSound.Stop();
		if (!unfuck)
		{
			PlayerController.iGetInserted = false;
			PlayerController.iGetFucked = false;
			if (!PlayerController.iFalledBack)
			{
				PlayerController.iFalled = false;
			}
			anim.speed = 1f;
			heroanim.speed = 1f;
			PlayerController.heIsFuckingHard = false;
			HeroineStats.aroused = false;
			heroanim.SetBool("ImpregInsRub", value: false);
			heroanim.SetBool("ImpregInsFuck", value: false);
			heroanim.SetBool("ImpregInsCum", value: false);
			heroanim.SetBool("impregCum", value: false);
			heroanim.SetBool("impregSex", value: false);
			heroanim.SetBool("isScared", value: false);
			heroanim.SetBool("isHorny", value: false);
			insertSound.Stop();
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			if (InventoryUI.heroineIsChased)
			{
				imOnHer = false;
				InventoryUI.heroineIsChased = false;
			}
			unfuck = true;
		}
		anim.SetBool("isFucking", value: false);
		anim.SetBool("isGrabbing", value: false);
		anim.SetBool("tease", value: false);
		anim.SetBool("sex", value: false);
		anim.SetBool("cum", value: false);
		anim.SetBool("isIdle", value: true);
		gainStamina(regenRate);
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
		EnemyUI.instance.TakeDamage(drainValue * Time.deltaTime);
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

	public void ThrustEvent()
	{
		if (PlayerController.heIsFuckingHard)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		thrusted = true;
		GainCumInstant(gainCumPerThrust);
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
	}

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
		drainHealth(amount);
		Debug.Log("TOOK DMG");
		agent.SetDestination(target.position);
		agent.speed = chaseSpeed;
		currentStamina = maxStamina;
		if (currentHealth <= 0f)
		{
			Die();
		}
	}

	private void Die()
	{
		if (cummed)
		{
			float num = Random.Range(0.3f, 0.7f);
			anim_pos.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			HeroineStats.fartiged = true;
		}
		sphereCol.enabled = false;
		healthUI.gameObject.SetActive(value: false);
		staminaUi.gameObject.SetActive(value: false);
		DisableUI();
		InventoryUI.heroineIsChased = false;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		agent.enabled = false;
		iFuck = false;
		if (!unfuck)
		{
			PlayerController.iGetInserted = false;
			PlayerController.iGetFucked = false;
			PlayerController.claimed = false;
			if (InventoryUI.heroineIsChased)
			{
				imOnHer = false;
				InventoryUI.heroineIsChased = false;
			}
			unfuck = true;
		}
		anim.speed = 1f;
		heroanim.speed = 1f;
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		heroanim.SetBool("isScared", value: false);
		heroanim.SetBool("isHorny", value: false);
		anim.SetBool("isFucking", value: false);
		anim.SetBool("isGrabbing", value: false);
		anim.SetBool("isCumming", value: false);
		anim.SetBool("cum", value: false);
		enemSaveState.GetComponent<EnemSavState>().isDead = true;
		anim.SetBool("isDed", value: true);
		iInsert = false;
		insertSound.Stop();
		rubSound.Stop();
		imOnHer = false;
		thisScript.enabled = false;
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
		BodyParts[0].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		BodyParts[1].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		BodyParts[2].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		BodyParts[3].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
		BodyParts[0].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[1].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[2].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[3].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
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
