using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class OctoControl : MonoBehaviour
{
	public enum MyState
	{
		PATROL,
		CHASE,
		GRAB,
		TEASEVAG,
		SEXVAG,
		SEXMOU,
		CUMVAG,
		CUMOU
	}

	public Transform targetStaminaUI;

	public Transform targetHealthUI;

	public GameObject stamUIPrefab;

	public GameObject healthUIPrefab;

	private Transform staminaUI;

	private Transform cam;

	private Image stamSlider;

	public Transform mountPos;

	public Transform camPos;

	public Image circle;

	public Image SexImage;

	public MyState state;

	private EnemyFieldOfView controlInstance;

	private GameObject Heroine;

	private NavMeshAgent agent;

	private Animator anim;

	private Animator heroineAnim;

	public AudioSource cumPumpSound;

	public AudioSource slideSound;

	public AudioSource slideSoundTwo;

	public AudioSource mouthFuckSound;

	public AudioSource vagTeaseSound;

	public AudioSource dickExtendSound;

	public AudioSource creatureAwareSound;

	private float distance;

	public float attackRange;

	public float chaseRange;

	public float maxHealth;

	private float currentHealth;

	public float maxStamina;

	private float currentStamina;

	public float maxCum;

	private float currentCum;

	public float expPerThrust;

	public float gainCumPerThrust;

	private float fuckDelay;

	public float fuckTime;

	public float teaseDmg;

	public float thrustDmg;

	private float pct;

	private bool fartiged;

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

	private bool added;

	public bool resetPath;

	private float resetTimer;

	private bool resetAgentPath;

	private bool thrusted;

	private bool max;

	private bool camShaked;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public float magnitudeCheck;

	private void Start()
	{
		currentStamina = maxStamina;
		currentHealth = maxHealth;
		currentCum = 0f;
		Heroine = GameObject.Find("Heroine");
		state = MyState.PATROL;
		controlInstance = GetComponent<EnemyFieldOfView>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
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
		if (GetComponent<EnemyFieldOfView>().isDed)
		{
			anim.SetBool("isDed", value: true);
			GetComponent<SphereCollider>().enabled = false;
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
			staminaUI.gameObject.SetActive(value: false);
			if (state == MyState.CUMVAG)
			{
				Release();
			}
			agent.ResetPath();
			agent.isStopped = true;
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<Rigidbody>().detectCollisions = false;
			base.enabled = false;
			return;
		}
		if (resetPath)
		{
			if (!resetAgentPath)
			{
				agent.ResetPath();
				RemoveMeFromList();
				PatrolOnStart();
				InventoryUI.heroineIsChased = false;
				state = MyState.PATROL;
				resetAgentPath = true;
			}
			resetTimer += Time.deltaTime;
			if (resetTimer > 5f)
			{
				resetTimer = 0f;
				resetPath = false;
				resetAgentPath = false;
			}
		}
		if (HeroineStats.birth)
		{
			Patrolling();
		}
		StateMachine(state);
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.PATROL:
			Patrolling();
			break;
		case MyState.CHASE:
			Chase();
			break;
		case MyState.GRAB:
			Grab();
			break;
		case MyState.TEASEVAG:
			TeaseVag();
			break;
		case MyState.SEXVAG:
			SexVag();
			break;
		case MyState.SEXMOU:
			SexMouth();
			break;
		case MyState.CUMVAG:
			CumVag();
			break;
		case MyState.CUMOU:
			CumMouth();
			break;
		}
	}

	private void SexMouth()
	{
		Debug.Log(base.gameObject?.ToString() + " Grabbed Her Face!");
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		if (!camShaked)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			camShaked = true;
		}
		heroineAnim.Play("rig|OctoDoubleSex");
		anim.Play("rig|OctoFaceHug");
		Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<OctoControl>().anim.Play("rig|OctoSexFront");
		HeroineStats.stunned = true;
		HeroineStats.currentPower = 0f;
		if (Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<OctoControl>().state == MyState.CUMVAG)
		{
			state = MyState.CUMOU;
		}
	}

	private void CumMouth()
	{
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			heroineAnim.SetBool("octoDoubleFinished", value: true);
			anim.SetBool("cumFace", value: false);
			heroineAnim.SetBool("octoDoubleCum", value: false);
			float num = Random.Range(0.3f, 0.7f);
			base.transform.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
			RemoveMeFromList();
			currentStamina = 0f;
			fartiged = true;
			state = MyState.PATROL;
		}
		else
		{
			Debug.Log(base.gameObject?.ToString() + " Cums in Her Face!");
			base.transform.rotation = mountPos.rotation;
			base.transform.position = mountPos.position;
			heroineAnim.SetBool("octoDoubleCum", value: true);
			anim.SetBool("cumFace", value: true);
		}
	}

	private void Chase()
	{
		Debug.Log(base.gameObject?.ToString() + " Chases Her");
		controlInstance.viewRadius = 11f;
		WalkOrIdle();
		if (!controlInstance.heroineIsVisible)
		{
			InventoryUI.heroineIsChased = false;
			state = MyState.PATROL;
			agent.isStopped = false;
			RemoveMeFromList();
		}
		if (PlayerManager.instance.enemyTurnOrder.Count > 1 && PlayerManager.instance.enemyTurnOrder[0] != base.gameObject && PlayerManager.instance.enemyTurnOrder[1] != base.gameObject)
		{
			state = MyState.PATROL;
			return;
		}
		InventoryUI.heroineIsChased = true;
		agent.SetDestination(Heroine.transform.position);
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance <= attackRange)
		{
			if (!added)
			{
				PlayerManager.instance.enemyTurnOrder.Add(base.gameObject);
				added = true;
			}
			GetComponent<SphereCollider>().enabled = false;
			if (PlayerManager.instance.enemyTurnOrder[0] == base.gameObject)
			{
				if (!PlayerController.iFalled)
				{
					PlayerController.iFalled = true;
					anim.SetBool("isAttacking", value: true);
				}
				if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
				{
					Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
					PlayerController.iGetFucked = true;
					state = MyState.GRAB;
				}
			}
			else
			{
				if (!(PlayerManager.instance.enemyTurnOrder[1] == base.gameObject))
				{
					return;
				}
				agent.isStopped = true;
				try
				{
					if (Heroine.GetComponent<HeroineStats>().mySexPartner.GetComponent<OctoControl>().state == MyState.SEXVAG)
					{
						state = MyState.SEXMOU;
					}
				}
				catch
				{
					agent.isStopped = true;
				}
			}
		}
		else
		{
			RemoveMeFromList();
		}
	}

	private void Grab()
	{
		Debug.Log(base.gameObject?.ToString() + " Grabbed Her!");
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		CameraFollow.target = camPos;
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		InitiateUI();
		heroineAnim.SetBool("octoGrab", value: true);
		anim.SetBool("isGrabbing", value: true);
		anim.SetBool("isAttacking", value: false);
		heroineAnim.SetBool("octoDoubleFinished", value: false);
		if (isPlaying(anim, "rig|OctoTease"))
		{
			heroineAnim.SetBool("octoTease", value: true);
			state = MyState.TEASEVAG;
		}
		if (!dickExtendSound.isPlaying)
		{
			dickExtendSound.Play();
		}
	}

	private void TeaseVag()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		anim.SetBool("isTeasing", value: true);
		heroineAnim.SetBool("octoTease", value: true);
		anim.speed = 2f;
		heroineAnim.speed = 2f;
		InitiateUI();
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(10f);
		}
		else
		{
			InitiateSex();
		}
	}

	private void SexVag()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		Debug.Log(base.gameObject?.ToString() + " Fucks Her Cunt!");
		PlayerManager.IsVirgin = false;
		VignetteEffect();
		anim.speed = 1f;
		heroineAnim.speed = 1f;
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		anim.SetBool("isVagFucking", value: true);
		heroineAnim.SetBool("octoSex", value: true);
		InitiateUI();
		if (currentCum >= maxCum)
		{
			state = MyState.CUMVAG;
		}
	}

	private void CumVag()
	{
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		Debug.Log(base.gameObject?.ToString() + " Cums inside Her Cunt!");
		anim.speed = 1f;
		heroineAnim.speed = 1f;
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		InitiateUI();
		anim.SetBool("isVagCumming", value: true);
		heroineAnim.SetBool("octoCumVag", value: true);
		PlayerController.heIsFuckingHard = true;
		HeroineStats.pregnant = true;
		HeroineStats.stunned = true;
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
				state = MyState.SEXVAG;
			}
		}
	}

	private void Release()
	{
		Debug.Log(base.gameObject?.ToString() + " Released Her");
		currentStamina = 0f;
		currentCum = 0f;
		fartiged = true;
		camShaked = false;
		dickExtendSound.Stop();
		state = MyState.PATROL;
		heroineAnim.SetBool("octoGrab", value: false);
		heroineAnim.SetBool("octoTease", value: false);
		heroineAnim.SetBool("octoSex", value: false);
		heroineAnim.SetBool("octoCumVag", value: false);
		heroineAnim.SetBool("falled", value: true);
		heroineAnim.SetBool("octoDoubleCum", value: false);
		anim.SetBool("isAttacking", value: false);
		anim.SetBool("isGrabbing", value: false);
		anim.SetBool("isTeasing", value: false);
		anim.SetBool("isVagFucking", value: false);
		anim.SetBool("isVagCumming", value: false);
		PlayerController.iGetFucked = false;
		PlayerController.iGetInserted = false;
		PlayerController.claimed = false;
		HeroineStats.aroused = false;
		HeroineStats.stunned = false;
		InventoryUI.heroineIsChased = false;
		CameraFollow.target = Heroine.transform;
		float num = Random.Range(0.3f, 0.7f);
		base.transform.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
		SexImage.enabled = false;
		circle.enabled = false;
		PlayerController.heIsFuckingHard = false;
		heroineAnim.speed = 1f;
		anim.speed = 1f;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		RemoveMeFromList();
		DisableUI();
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

	private bool isPlaying(Animator anim, string stateName)
	{
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return true;
		}
		return false;
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

	private void WalkOrIdle()
	{
		magnitudeCheck = agent.velocity.magnitude;
		if (agent.velocity == Vector3.zero)
		{
			anim.SetBool("isRunning", value: false);
		}
		else
		{
			anim.SetBool("isRunning", value: true);
		}
	}

	private void Patrolling()
	{
		anim.SetBool("cumFace", value: false);
		if (distance >= attackRange)
		{
			GetComponent<SphereCollider>().enabled = true;
		}
		controlInstance.viewRadius = 8f;
		agent.isStopped = false;
		agent.speed = patrolSpeed;
		WalkOrIdle();
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (travelling && agent.remainingDistance <= 1f)
		{
			travelling = false;
			anim.SetBool("isRunning", value: false);
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
		if (fartiged)
		{
			RegenerateStamina();
			return;
		}
		staminaUI.gameObject.SetActive(value: false);
		if (!HeroineStats.birth && !resetPath && controlInstance.heroineIsVisible && (PlayerManager.instance.enemyTurnOrder.Count <= 1 || !(PlayerManager.instance.enemyTurnOrder[0] != base.gameObject) || !(PlayerManager.instance.enemyTurnOrder[1] != base.gameObject)))
		{
			creatureAwareSound.Play();
			state = MyState.CHASE;
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
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void ThrustEvent()
	{
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		thrusted = true;
		slideSound.Play();
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
		currentStamina += Time.deltaTime / 2f;
		if (currentStamina >= maxStamina)
		{
			fartiged = false;
		}
	}

	public void TeaseEvent()
	{
		vagTeaseSound.Play();
	}

	public void PumpEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(10f);
		LoseCumInstant(10f);
		cumPumpSound.Play();
		slideSoundTwo.Play();
		thrusted = true;
		if (HeroineStats.MantisBuff)
		{
			GetComponent<EnemyFieldOfView>().drainHealth(2.5f);
		}
		if (currentCum <= 0f)
		{
			Release();
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

	public void MouthFuckEvent()
	{
		mouthFuckSound.Play();
		Heroine.GetComponent<HeroineStats>().GainLustInstant(5f);
	}

	public void FacePumpEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainLustInstant(15f);
		cumPumpSound.Play();
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, attackRange);
	}
}
