using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MantisAi : MonoBehaviour
{
	private NavMeshAgent agent;

	public CamShaker.Properties mountShake;

	public GameObject HeroinesCam;

	public Transform camPosMount;

	public Transform camPosCum;

	public GameObject Hebel;

	public GameObject Heroine;

	public GameObject MantisCum;

	private float mSize;

	private float cumFlowSpeed = 1f;

	private Animator animator;

	private Animator heroineAnim;

	public Transform pp1;

	private Transform cam;

	private Transform anim_pos;

	public Transform mount_place;

	public Transform mount_place_front;

	private bool ready;

	private bool activeOnceWhenDead;

	private float delay;

	private float rechargeDelay;

	private float adjustDirectionTime;

	public float chargeDelaytime = 0.5f;

	public float rechargingTime = 3f;

	public float endOfAdjustDirektionTime = 1.5f;

	private bool chargeDestinationSet;

	private bool finishedCharging;

	private bool hitFront;

	private bool hitBack;

	public float maxHealth = 300f;

	public float maxStamina = 30f;

	public float maxCum = 100f;

	public float cumGainRate = 2f;

	public float stamDrainRate = 1f;

	public float restoreStaminaRate = 5f;

	public float cumDrainValue = 5f;

	public float thrustDmg = 5f;

	public float teaseDmg = 10f;

	public float expPerThrust = 10f;

	private float currentStamina;

	private float currentHealth;

	private float currentCum;

	private Image healthSlider;

	public Transform targetHealthUI;

	public GameObject healthUIPrefab;

	private Transform healthUI;

	private bool gotHit;

	private float time;

	public float mountTime = 2f;

	public float sexTime = 2f;

	private float mountT;

	private bool shakeCam;

	public static float powerRecieved;

	private bool gotAttacked;

	private bool resting;

	private bool cumming;

	private bool rotateHer;

	private bool fastFuck;

	private float startCummingTime;

	public AudioSource bossBattle;

	public AudioSource sexSound1;

	public AudioSource cumSound;

	public AudioSource step;

	private CamShaker shake;

	private float startValue;

	private float endValue = 0.7f;

	private float t;

	private float transitionTime = 5f;

	private bool activateBossMusic;

	public GameObject goodJob;

	public GameObject mastArea;

	public static bool isDed;

	public GameObject equipManager;

	private float distance;

	private float attackRadius;

	private int cumCounter;

	private Image mantisImage;

	private void Start()
	{
		attackRadius = 0.5f;
		mantisImage = GameObject.Find("ManagerAndUI/UI/Canvas/Inventory/MantisSprite").GetComponent<Image>();
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
		shake = Object.FindObjectOfType<CamShaker>();
		cam = Camera.main.transform;
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		currentHealth = maxHealth;
		currentCum = 0f;
		currentStamina = maxStamina;
		anim_pos = base.gameObject.transform;
		StartCoroutine(Activate());
	}

	private void Update()
	{
		ShowHealthUI();
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
		currentCum = Mathf.Clamp(currentCum, 0f, maxCum);
		if (currentHealth <= 0f && !cumming)
		{
			Debug.Log("ACTIVATED THE DEAD BLOG");
			isDed = true;
			Hebel.GetComponent<Hebel>().mantisIsDead = true;
			PlayerManager.MantisDown = true;
			goodJob.SetActive(value: true);
			mastArea.SetActive(value: true);
			bossBattle.Stop();
			animator.SetBool("isDed", value: true);
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<MantisAi>().enabled = false;
			DisableUI();
			return;
		}
		ReactToShots();
		if (resting)
		{
			bossBattle.volume = 0.7f;
			InventoryUI.heroineIsChased = false;
			GainStamina(restoreStaminaRate);
			animator.SetBool("isIdle", value: true);
			if (currentStamina >= maxStamina && !HeroineStats.birth)
			{
				resting = false;
			}
			return;
		}
		if (HeroineStats.birth)
		{
			resting = true;
			return;
		}
		if (PlayerController.iFalled)
		{
			Mount();
			return;
		}
		WalkOrIdle();
		if (!ready)
		{
			return;
		}
		if (!activateBossMusic)
		{
			t += Time.deltaTime / transitionTime;
			bossBattle.volume = Mathf.SmoothStep(startValue, endValue, t);
			if (startValue == endValue)
			{
				activateBossMusic = true;
			}
		}
		if (finishedCharging)
		{
			animator.speed = 1f;
			rechargeDelay += Time.deltaTime;
			if (rechargeDelay > rechargingTime)
			{
				delay = 0f;
				adjustDirectionTime = 0f;
				finishedCharging = false;
				chargeDestinationSet = false;
			}
		}
		else
		{
			Charge();
		}
	}

	private IEnumerator Activate()
	{
		bossBattle.Play();
		yield return new WaitForSeconds(5f);
		agent.SetDestination(pp1.position);
		yield return new WaitForSeconds(8f);
		ready = true;
		StopAllCoroutines();
	}

	private void Charge()
	{
		animator.SetBool("isCharging", value: true);
		delay += Time.deltaTime;
		if (distance <= attackRadius)
		{
			CheckHitAngle();
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
			HeroineStats.fartiged = true;
			PlayerController.iFalled = true;
		}
		if (delay > chargeDelaytime)
		{
			if (!chargeDestinationSet)
			{
				adjustDirectionTime += Time.deltaTime;
				agent.SetDestination(Heroine.transform.position);
				if (adjustDirectionTime > endOfAdjustDirektionTime)
				{
					chargeDestinationSet = true;
				}
			}
			agent.speed = 10f;
			animator.speed = 4f;
			if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
			{
				animator.SetBool("isCharging", value: false);
				animator.SetBool("isIdle", value: true);
				rechargeDelay = 0f;
				finishedCharging = true;
			}
		}
		else
		{
			FaceTarget();
		}
	}

	private void Mount()
	{
		HeroinesCam.GetComponent<CameraCollision>().maxCamDistance = 3.2f;
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		InventoryUI.heroineIsChased = true;
		if (!EquipmentManager.heroineIsNaked)
		{
			equipManager.GetComponent<EquipmentManager>().RipPantsu();
		}
		bossBattle.volume = 0.4f;
		if (currentStamina <= 0f)
		{
			Release();
			return;
		}
		mountT += Time.deltaTime;
		healthUI.gameObject.SetActive(value: false);
		InitiateUI();
		PlayerController.iGetInserted = true;
		animator.SetBool("isIdle", value: false);
		animator.SetBool("isWalking", value: false);
		animator.SetBool("isCharging", value: false);
		if (currentCum >= 100f)
		{
			cumming = true;
		}
		if (cumming)
		{
			CameraFollow.target = camPosCum;
			animator.SetBool("isCumming", value: true);
			heroineAnim.SetBool("isCumFilled", value: true);
			HeroineStats.stunned = true;
			HeroineStats.creampied = true;
			HeroineStats.hugeAmount = true;
			Heroine.GetComponent<HeroineStats>().LargeCumDrip();
			if (!cumSound.isPlaying)
			{
				cumSound.Play();
			}
			if (PlayerController.iFalledFront && !rotateHer)
			{
				Heroine.transform.Rotate(0f, 180f, 0f);
				rotateHer = true;
			}
			heroineAnim.SetBool("mantisCumming", value: true);
			startCummingTime += Time.deltaTime;
			if (!(startCummingTime > 2f))
			{
				return;
			}
			MantisCum.SetActive(value: true);
			if (mSize < 300f)
			{
				MantisCum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize += cumFlowSpeed);
			}
			animator.speed = 1.3f;
			heroineAnim.speed = 1.3f;
			DrainCum(cumDrainValue);
			if (currentCum <= 0f)
			{
				cumSound.Stop();
				if (HeroineStats.GameOver)
				{
					SceneManager.LoadScene("ThankYou");
				}
				Release();
				currentStamina = 0f;
			}
			return;
		}
		CameraFollow.target = camPosMount;
		if (mountT > sexTime + mountTime)
		{
			PlayerController.iGetFucked = true;
			HeroineStats.aroused = true;
			PlayerController.heIsFuckingHard = true;
			if (currentCum > 30f)
			{
				fastFuck = true;
				if (PlayerController.iFalledBack)
				{
					animator.SetBool("isFastFuckBehind", value: true);
					heroineAnim.SetBool("mantisFastFuckBehind", value: true);
				}
				if (PlayerController.iFalledFront)
				{
					animator.SetBool("isFastFuckFront", value: true);
					heroineAnim.SetBool("mantisFastFuckFront", value: true);
				}
			}
			else
			{
				if (PlayerController.iFalledBack)
				{
					animator.Play("rig|MantisMateBehind");
					heroineAnim.Play("rig|MantisBehindMate");
				}
				if (PlayerController.iFalledFront)
				{
					animator.Play("rig|MateFront");
					heroineAnim.Play("rig|MantisFrontMate");
				}
			}
		}
		else if (mountT > mountTime)
		{
			TakeSexPos();
			if (!shakeCam)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(mountShake);
				shakeCam = true;
			}
			if (PlayerController.iFalledBack)
			{
				animator.Play("rig|MantisMateBehindStart");
				heroineAnim.Play("rig|MantisBehindMateStart");
				heroineAnim.SetBool("mantisFuckBehind", value: true);
			}
			if (PlayerController.iFalledFront)
			{
				animator.Play("rig|MantisMateFrontStart");
				heroineAnim.Play("rig|MantisFrontMateStart");
				heroineAnim.SetBool("mantisFuckFront", value: true);
			}
		}
		else
		{
			FaceTarget();
			agent.enabled = false;
			animator.speed = 1f;
		}
	}

	private void Release()
	{
		mountT = 0f;
		startCummingTime = 0f;
		mSize = 0f;
		currentCum = 0f;
		fastFuck = false;
		shakeCam = false;
		resting = true;
		agent.enabled = true;
		cumming = false;
		rotateHer = false;
		CameraFollow.target = Heroine.transform;
		HeroineStats.stunned = false;
		HeroinesCam.GetComponent<CameraCollision>().maxCamDistance = 2.3f;
		animator.speed = 1f;
		heroineAnim.speed = 1f;
		if (MantisCum.activeSelf)
		{
			cumCounter++;
			if (cumCounter >= 2)
			{
				HeroineStats.MantisBuff = true;
				mantisImage.enabled = true;
			}
			MantisCum.SetActive(value: false);
		}
		HeroineStats.aroused = false;
		PlayerController.heIsFuckingHard = false;
		animator.SetBool("isFastFuckBehind", value: false);
		animator.SetBool("isFastFuckFront", value: false);
		animator.SetBool("isCumming", value: false);
		heroineAnim.SetBool("mantisFuckBehind", value: false);
		heroineAnim.SetBool("mantisFuckFront", value: false);
		heroineAnim.SetBool("mantisFastFuckFront", value: false);
		heroineAnim.SetBool("mantisFastFuckBehind", value: false);
		heroineAnim.SetBool("mantisCumming", value: false);
		PlayerController.iGetFucked = false;
		PlayerController.iGetInserted = false;
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		InventoryUI.heroineIsChased = false;
		healthUI.gameObject.SetActive(value: true);
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
	}

	private void WalkOrIdle()
	{
		if (agent.velocity.magnitude == 0f)
		{
			animator.SetBool("isWalking", value: false);
		}
		else
		{
			animator.SetBool("isWalking", value: true);
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
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

	private void TakeSexPos()
	{
		if (PlayerController.iFalledBack)
		{
			anim_pos.rotation = mount_place.rotation;
			anim_pos.position = mount_place.position;
		}
		if (PlayerController.iFalledFront)
		{
			anim_pos.rotation = mount_place_front.rotation;
			anim_pos.position = mount_place_front.position;
		}
	}

	public void TakeDamage(float amount)
	{
		gotHit = true;
		drainHealth(amount);
	}

	public void drainHealth(float drainValue)
	{
		currentHealth -= drainValue;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
	}

	public void drainHealthSlow(float drainValue)
	{
		currentHealth -= drainValue * Time.deltaTime;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
	}

	private void ShowHealthUI()
	{
		if (healthUI != null)
		{
			healthUI.position = targetHealthUI.position;
			healthUI.forward = -cam.forward;
			healthSlider.fillAmount = currentHealth / maxHealth;
			if (currentHealth <= 0f && !cumming && !activeOnceWhenDead)
			{
				GetComponent<Animator>().SetBool("isDed", value: true);
				GetComponent<CapsuleCollider>().enabled = false;
				healthUI.gameObject.SetActive(value: false);
				InventoryUI.heroineIsChased = false;
				GetComponent<MantisAi>().enabled = false;
				activeOnceWhenDead = true;
			}
		}
	}

	private void ReactToShots()
	{
		if (gotHit)
		{
			GetComponent<Animator>().SetBool("isHit", value: true);
			time += Time.deltaTime;
			if (time >= 0.3f)
			{
				GetComponent<Animator>().SetBool("isHit", value: false);
				time = 0f;
				gotHit = false;
			}
		}
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	private void GainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
		EnemyUI.instance.RestoreHealth(gainValue * Time.deltaTime);
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void DrainCum(float drainValue)
	{
		if (HeroineStats.MantisBuff)
		{
			drainHealthSlow(drainValue);
			healthUI.gameObject.SetActive(value: true);
		}
		currentCum -= drainValue * Time.deltaTime;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
	}

	private void ThrustEvent()
	{
		GainCumInstant(cumGainRate);
		PlayerManager.IsVirgin = false;
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		sexSound1.Play();
		if (fastFuck)
		{
			shake.StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
	}

	private void StepEvent()
	{
		step.Play();
		shake.StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
	}

	public void OnTriggerEnter(Collider other)
	{
		if (base.enabled && !PlayerController.iFalled && other.tag == "Player")
		{
			CheckHitAngle();
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
			HeroineStats.fartiged = true;
			PlayerController.iFalled = true;
		}
	}
}
