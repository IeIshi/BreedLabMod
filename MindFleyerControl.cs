using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MindFleyerControl : MonoBehaviour
{
	public enum MyState
	{
		FIRSTFLY,
		CHANGELOCATION,
		RAYATTACK,
		TEASE,
		SEX,
		SEX2,
		CUM,
		DEAD
	}

	private float fadeSpeed = 0.1f;

	private float gainSpeed = 0.3f;

	public GameObject ImageHolder;

	private Image imageComponent;

	private Color targetColor;

	public GameObject GangBangRoom;

	public Image circle;

	public Image SexImage;

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	public GameObject CameraObj;

	public GameObject GangBangScene;

	public GameObject GangProtag;

	public GameObject GangMayu;

	public GameObject GangLita;

	public GameObject ObedientHounds;

	public float maxIllusionStamina;

	public float maxStamina;

	public float maxHealth;

	private float currentStamina;

	private float currentHealth;

	private float currentIllusionStamina;

	public float maxCum;

	private float currentCum;

	public float backToIllusionTimer;

	private bool mindbreak;

	private Animator animator;

	private Animator heroineAnim;

	private Animator gangBangProtagAnim;

	private Animator gangBangMayuAnim;

	private Animator gangBangLitaAnim;

	public Transform moveToPoint;

	public Transform camTransform;

	public Transform yuriCam;

	public Transform sexCam;

	public MyState state;

	public List<Waypoint> wayPoint = new List<Waypoint>();

	private Transform moveLocation;

	public float patrolSpeed = 10f;

	private float rotationSpeed = 0.5f;

	public float rayCastLength = 5f;

	public float gainCumPerThrust;

	public GameObject SphereRay;

	public GameObject Ray;

	public GameObject Heroine;

	private RayControl rayControl;

	private float rayTimer;

	public float rayActivationTime;

	private bool sheStruggled;

	private float illusionTimer;

	private bool thrusted;

	private bool max;

	private bool cum;

	[SerializeField]
	public bool dead;

	private float vignetteIntensity = 0.2f;

	private float pulsingSpeed = 1.5f;

	public AudioSource[] slideSoundArray;

	public AudioSource[] gulpSoundArray;

	public AudioSource[] cumSoundArray;

	private Transform cam;

	private Image healthSlider;

	private Transform targetHealthUI;

	private GameObject healthUIPrefab;

	private Transform healthUI;

	public AudioSource ghostAh;

	public AudioSource RaySound;

	public AudioSource alienNoises;

	private bool yuriScene;

	private void Start()
	{
		if (PlayerManager.IsVirgin)
		{
			maxHealth = 30f;
		}
		if (PlayerManager.AfterMindFleyer)
		{
			GangBangRoom.SetActive(value: false);
			ObedientHounds.SetActive(value: true);
			base.gameObject.SetActive(value: false);
			return;
		}
		currentStamina = maxStamina;
		currentIllusionStamina = maxIllusionStamina;
		currentCum = 0f;
		currentHealth = maxHealth;
		cam = Camera.main.transform;
		healthUIPrefab = base.transform.Find("HealthUi").gameObject;
		targetHealthUI = base.transform.Find("TargetHealthUi");
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		gangBangProtagAnim = GangProtag.GetComponent<Animator>();
		gangBangMayuAnim = GangMayu.GetComponent<Animator>();
		gangBangLitaAnim = GangLita.GetComponent<Animator>();
		animator.SetBool("fly", value: true);
		InventoryUI.heroineIsChased = true;
		rayControl = Ray.GetComponent<RayControl>();
		imageComponent = ImageHolder.GetComponent<Image>();
		targetColor = imageComponent.color;
		ImageHolder.SetActive(value: false);
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
		state = MyState.FIRSTFLY;
	}

	private void Update()
	{
		if (currentStamina <= 0f)
		{
			state = MyState.DEAD;
		}
		if (!alienNoises.isPlaying)
		{
			alienNoises.Play();
		}
		ShowHealthUI();
		RayChecker();
		StateMachine(state);
		if (yuriScene && HeroineStats.orgasm)
		{
			HeroineStats.debuffedStam -= Time.deltaTime * 2.5f;
		}
		if (SphereRay.activeSelf)
		{
			rotationSpeed = 1f;
		}
		else
		{
			rotationSpeed = 5f;
		}
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.FIRSTFLY:
			MoveFirstDestination();
			break;
		case MyState.CHANGELOCATION:
			ChangeLocation();
			break;
		case MyState.RAYATTACK:
			RayAttack();
			break;
		case MyState.TEASE:
			Tease();
			break;
		case MyState.SEX:
			Sex();
			break;
		case MyState.SEX2:
			Sex2();
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
		float maxDistanceDelta = 1f * Time.deltaTime;
		base.transform.position = Vector3.MoveTowards(base.transform.position, moveToPoint.position, maxDistanceDelta);
		if (base.transform.position == moveToPoint.position)
		{
			state = MyState.RAYATTACK;
		}
	}

	private void RayAttack()
	{
		FaceTarget();
		moveLocation = null;
		if (SphereRay.activeSelf)
		{
			rotationSpeed = 0.5f;
		}
		else
		{
			rotationSpeed = 2f;
		}
		SphereRay.SetActive(value: true);
		rayTimer += Time.deltaTime;
		if (rayTimer > rayCastLength)
		{
			DeactivateRay();
			state = MyState.CHANGELOCATION;
			rayTimer = 0f;
		}
		else if (rayTimer > rayActivationTime)
		{
			Ray.SetActive(value: true);
			if (!RaySound.isPlaying)
			{
				RaySound.Play();
			}
		}
	}

	private void RayChecker()
	{
		if (targetColor.a < 0f)
		{
			ImageHolder.SetActive(value: false);
		}
		if (!mindbreak && rayControl.inRay)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a += Time.deltaTime * gainSpeed;
			imageComponent.color = targetColor;
			Debug.Log("IN RAY");
			if (targetColor.a > 1f)
			{
				mindbreak = true;
				state = MyState.TEASE;
			}
		}
		if (!rayControl.inRay)
		{
			targetColor.a -= Time.deltaTime * fadeSpeed;
			imageComponent.color = targetColor;
		}
		if (mindbreak)
		{
			targetColor.a -= Time.deltaTime * fadeSpeed;
			imageComponent.color = targetColor;
		}
	}

	private void ChangeLocation()
	{
		RaySound.Stop();
		if (moveLocation == null)
		{
			moveLocation = wayPoint[Random.Range(0, wayPoint.Count)].transform;
			Debug.Log(moveLocation.ToString());
		}
		rayControl.inRay = false;
		float maxDistanceDelta = patrolSpeed * Time.deltaTime;
		FaceTargetLoc(moveLocation);
		base.transform.position = Vector3.MoveTowards(base.transform.position, moveLocation.position, maxDistanceDelta);
		if (base.transform.position == moveLocation.position)
		{
			moveLocation = null;
			state = MyState.RAYATTACK;
		}
	}

	private void Tease()
	{
		RaySound.Stop();
		Heroine.transform.position = base.transform.position;
		Heroine.transform.rotation = base.transform.rotation;
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(10f);
		}
		Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		DeactivateRay();
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		heroineAnim.Play("rig|MindTease");
		animator.Play("Armature|MindFleyer_Tease");
		InitiateSex();
		if (!sheStruggled)
		{
			GangBangRoom.SetActive(value: true);
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			InitiateIllusionUI();
			yuriScene = true;
			Heroine.GetComponent<HeroineStats>().StaminaGainRate = 2f;
			if (currentIllusionStamina < 0f)
			{
				sheStruggled = true;
				currentIllusionStamina = maxIllusionStamina;
			}
			return;
		}
		GangBangRoom.SetActive(value: false);
		InitiateUI();
		yuriScene = false;
		CameraFollow.target = sexCam;
		CameraObj.transform.position = sexCam.position;
		illusionTimer += Time.deltaTime;
		Heroine.GetComponent<HeroineStats>().StaminaGainRate = 1f;
		if (illusionTimer > backToIllusionTimer - 10f)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 0.1f;
		}
		if (illusionTimer > backToIllusionTimer - (float)Random.Range(0, 10))
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			illusionTimer = 0f;
			sheStruggled = false;
		}
	}

	private void Sex()
	{
		animator.Play("Armature|MindFleyer_Sex1_001");
		heroineAnim.Play("rig|MindSex1");
		PlayerManager.IsVirgin = false;
		if (GangBangRoom.activeSelf)
		{
			gangBangLitaAnim.Play("rig_002|LitaSex");
			gangBangMayuAnim.Play("rig_001|MayuSex");
			gangBangProtagAnim.Play("rig|GangSex");
		}
		HeroineStats.aroused = true;
		PlayerController.heIsFuckingHard = true;
		if (!sheStruggled)
		{
			GangBangRoom.SetActive(value: true);
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			InitiateIllusionUI();
			yuriScene = true;
			Heroine.GetComponent<HeroineStats>().StaminaGainRate = 2f;
			if (currentIllusionStamina < 0f)
			{
				sheStruggled = true;
				currentIllusionStamina = maxIllusionStamina;
			}
			return;
		}
		GangBangRoom.SetActive(value: false);
		InitiateUI();
		yuriScene = false;
		CameraFollow.target = sexCam;
		CameraObj.transform.position = sexCam.position;
		illusionTimer += Time.deltaTime;
		Heroine.GetComponent<HeroineStats>().StaminaGainRate = 1f;
		if (illusionTimer > backToIllusionTimer - 10f)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 0.2f;
		}
		if (illusionTimer > backToIllusionTimer - (float)Random.Range(0, 10))
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			Heroine.GetComponent<HeroineStats>().StaminaGainRate = 2f;
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			illusionTimer = 0f;
			sheStruggled = false;
		}
	}

	private void Sex2()
	{
		heroineAnim.Play("rig|MindSex2");
		animator.Play("Armature|MindFleyer_Sex2");
		if (GangBangRoom.activeSelf)
		{
			gangBangLitaAnim.Play("rig_002|LitaSex");
			gangBangMayuAnim.Play("rig_001|MayuSex");
			gangBangProtagAnim.Play("rig|GangSex");
		}
		gangBangLitaAnim.speed = 1.5f;
		gangBangMayuAnim.speed = 1.5f;
		gangBangProtagAnim.speed = 1.5f;
		if (!sheStruggled)
		{
			GangBangRoom.SetActive(value: true);
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			InitiateIllusionUI();
			yuriScene = true;
			Heroine.GetComponent<HeroineStats>().StaminaGainRate = 2f;
			if (currentIllusionStamina < 0f)
			{
				sheStruggled = true;
				currentIllusionStamina = maxIllusionStamina;
			}
			return;
		}
		GangBangRoom.SetActive(value: false);
		InitiateUI();
		yuriScene = false;
		CameraFollow.target = sexCam;
		CameraObj.transform.position = sexCam.position;
		illusionTimer += Time.deltaTime;
		Heroine.GetComponent<HeroineStats>().StaminaGainRate = 1f;
		if (illusionTimer > backToIllusionTimer - 10f)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 0.2f;
		}
		if (illusionTimer > backToIllusionTimer - (float)Random.Range(0, 10))
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			Heroine.GetComponent<HeroineStats>().StaminaGainRate = 2f;
			CameraFollow.target = yuriCam;
			CameraObj.transform.position = yuriCam.position;
			illusionTimer = 0f;
			sheStruggled = false;
		}
	}

	private void Cum()
	{
		GangBangRoom.SetActive(value: false);
		InitiateUI();
		yuriScene = false;
		ObedientHounds.SetActive(value: true);
		CameraFollow.target = sexCam;
		CameraObj.transform.position = sexCam.position;
		Heroine.GetComponent<HeroineStats>().StaminaGainRate = 1f;
		if (HeroineStats.GameOver)
		{
			HeroineStats.GameOver = false;
			Heroine.GetComponent<HeroineStats>().gameOverUI.SetActive(value: false);
		}
		if (!HeroineStats.corrupted)
		{
			Heroine.GetComponent<HeroineStats>().rend = Heroine.GetComponent<HeroineStats>().Eyes.GetComponent<Renderer>();
			Heroine.GetComponent<HeroineStats>().Eyes.GetComponent<SkinnedMeshRenderer>().sharedMesh = Heroine.GetComponent<HeroineStats>().monsterEyes;
			Heroine.GetComponent<HeroineStats>().rend.sharedMaterial = Heroine.GetComponent<HeroineStats>().material[1];
			HeroineStats.debuffedStam = 0f;
			HeroineStats.maxStamina = 999f;
			HeroineStats.currentStamina = HeroineStats.maxStamina;
			HeroineStats.corrupted = true;
		}
		if (!cum)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			cum = true;
		}
		heroineAnim.Play("rig|MindCum");
		animator.Play("Armature|MindFleyer_Cum");
	}

	private void Dead()
	{
		if (currentHealth > 0f)
		{
			heroineAnim.Play("rig|futaBehindAfterCum");
			heroineAnim.SetBool("isFalledBack", value: true);
		}
		if (!dead)
		{
			Release();
		}
	}

	private void Release()
	{
		animator.Play("Armature|MindFleyer_Idle");
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == base.gameObject)
		{
			if (currentHealth <= 0.1f)
			{
				heroineAnim.Play("rig|Idle");
			}
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		}
		SexImage.enabled = false;
		circle.enabled = false;
		ImageHolder.SetActive(value: true);
		Heroine.GetComponent<HeroineStats>().StaminaGainRate = 1f;
		RaySound.Stop();
		alienNoises.Stop();
		InventoryUI.heroineIsChased = false;
		PlayerController.iGetFucked = false;
		healthUI.gameObject.SetActive(value: false);
		PlayerManager.AfterMindFleyer = true;
		ghostAh.Play();
		ImageHolder.SetActive(value: true);
		targetColor.a = 1f;
		imageComponent.color = targetColor;
		DisableUI();
		dead = true;
		GetComponent<MindFleyerControl>().enabled = false;
	}

	private void InitiateSex()
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
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			state = MyState.SEX;
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * rotationSpeed);
	}

	private void FaceTargetLoc(Transform targetLoc)
	{
		Vector3 normalized = (targetLoc.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * rotationSpeed);
	}

	private void DeactivateRay()
	{
		SphereRay.SetActive(value: false);
		Ray.SetActive(value: false);
		rayTimer = 0f;
	}

	public void DrainStaminaInstant(float drainValue)
	{
		if (currentStamina < 0f)
		{
			currentStamina = 0f;
		}
		if (currentIllusionStamina < 0f)
		{
			currentIllusionStamina = 0f;
			sheStruggled = true;
			currentIllusionStamina = maxIllusionStamina;
		}
		if (sheStruggled)
		{
			currentStamina -= drainValue;
		}
		else
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			currentIllusionStamina -= drainValue;
		}
		EnemyUI.instance.TakeDamage(drainValue);
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

	private void InitiateIllusionUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxIllusionStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentIllusionStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
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
		if (currentCum < 0f)
		{
			state = MyState.DEAD;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && GetComponent<MindFleyerControl>().enabled)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			mindbreak = true;
			state = MyState.TEASE;
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
		if (state == MyState.TEASE)
		{
			slideSoundArray[0].Play();
			PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(1f);
			PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(2f);
			return;
		}
		GainCumInstant(gainCumPerThrust);
		thrusted = true;
		slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		if (state != MyState.SEX2 && currentCum > 50f)
		{
			ImageHolder.SetActive(value: true);
			targetColor.a = 1f;
			imageComponent.color = targetColor;
			state = MyState.SEX2;
		}
		if (currentCum >= maxCum)
		{
			state = MyState.CUM;
		}
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(3f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(1f);
	}

	private void GulpEvent()
	{
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(3f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(1f);
		gulpSoundArray[Random.Range(0, gulpSoundArray.Length)].Play();
	}

	private void CumEvent()
	{
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(10f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(1f);
		cumSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		LoseCumInstant(10f);
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

	public void TakeDamage(float amount)
	{
		healthUI.gameObject.SetActive(value: true);
		drainHealth(amount);
		if (currentHealth <= 0f)
		{
			state = MyState.DEAD;
		}
	}
}
