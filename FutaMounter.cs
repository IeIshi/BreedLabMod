using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FutaMounter : MonoBehaviour
{
	private GameObject Heroine;

	public GameObject FutaBlowjober;

	private GameObject FutaSpawn;

	private Transform camPosFront;

	private Transform camPosBehind;

	private float fuckTimer;

	private float mountTimer;

	private float pct;

	private float destroyTimer;

	public float mountDelay;

	public float fuckDelay;

	public float pantiesDurDamage;

	public float gainCumPerThrust;

	public float pleasureValue;

	public float lustValue;

	private bool startTeasing;

	private bool startFucking;

	private bool blowjoberSpawned;

	private bool stopCumming;

	private bool lastPortPlayed;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private Image circle;

	private Image SexImage;

	private Animator myAnim;

	private Animator heroineAnim;

	public float maxCum;

	private float currentCum;

	public float maxStamina;

	private float currentStamina;

	private bool setPos;

	private bool shake1;

	private bool shake2;

	private bool shake3;

	private bool shake4;

	private bool cumSoundStarted;

	private bool cummed;

	private bool max;

	private bool thrusted;

	private SkinnedMeshRenderer Body;

	private SkinnedMeshRenderer Brows;

	private SkinnedMeshRenderer Eyes;

	private SkinnedMeshRenderer Hair;

	private AudioSource port;

	private AudioSource slideInsideSound;

	private AudioSource sexSound;

	private AudioSource sexHardSound;

	private AudioSource cumSound;

	private AudioSource insertSound;

	private GameObject FutaArea;

	private void Start()
	{
		port = base.transform.GetChild(0).GetComponent<AudioSource>();
		slideInsideSound = base.transform.GetChild(5).GetComponent<AudioSource>();
		sexSound = base.transform.GetChild(6).GetComponent<AudioSource>();
		sexHardSound = base.transform.GetChild(7).GetComponent<AudioSource>();
		cumSound = base.transform.GetChild(8).GetComponent<AudioSource>();
		insertSound = base.transform.GetChild(10).GetComponent<AudioSource>();
		Body = base.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
		Brows = base.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
		Eyes = base.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();
		Hair = base.transform.GetChild(4).GetComponent<SkinnedMeshRenderer>();
		port.Play();
		Heroine = GameObject.Find("Heroine");
		SexImage = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse").GetComponent<Image>();
		circle = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse/circle (1)").GetComponent<Image>();
		camPosBehind = GameObject.Find("Heroine/FutaCamBehind").GetComponent<Transform>();
		camPosFront = GameObject.Find("Heroine/CameraFollowWhenFalled").GetComponent<Transform>();
		FutaArea = GameObject.Find("FutaArea");
		myAnim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		currentStamina = maxStamina;
		mountTimer = 0f;
	}

	private void Update()
	{
		if (!PlayerController.iFalled || currentStamina <= 0f || HeroineStats.birth)
		{
			Release();
			Fade();
			if (FutaSpawn != null)
			{
				Object.Destroy(FutaSpawn);
			}
			Destroy();
			return;
		}
		StartMountAndSexTimer();
		if (startFucking)
		{
			Sex();
			if (!shake2)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
				slideInsideSound.Play();
				shake2 = true;
			}
		}
		else if (startTeasing)
		{
			InitiateUI();
			Tease();
			if (!shake1)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
				shake1 = true;
			}
		}
		else
		{
			FaceTarget();
		}
	}

	private void StartMountAndSexTimer()
	{
		mountTimer += Time.deltaTime;
		if (mountTimer > mountDelay)
		{
			startTeasing = true;
		}
	}

	private void Tease()
	{
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
		}
		Heroine.GetComponent<HeroineStats>().GainLust(lustValue / 2f);
		if (!setPos)
		{
			port.Play();
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			PlayerController.iGetFucked = true;
			heroineAnim.SetBool("isScared", value: true);
			heroineAnim.SetBool("phantomFucked", value: true);
			if (PlayerController.iFalledFront)
			{
				base.transform.position = Heroine.transform.position;
				base.transform.eulerAngles = new Vector3(Heroine.transform.eulerAngles.x, Heroine.transform.eulerAngles.y + 180f, 0f - Heroine.transform.eulerAngles.z);
				CameraFollow.target = camPosFront;
				myAnim.Play("rig|Futa_FrontTease");
				heroineAnim.Play("rig|futaFrontTease");
			}
			if (PlayerController.iFalledBack)
			{
				base.transform.position = Heroine.transform.position;
				base.transform.localRotation = Heroine.transform.localRotation;
				CameraFollow.target = camPosBehind;
				myAnim.Play("rig|Futa_Behind_Grind");
				heroineAnim.Play("rig|futaBehindGrind");
			}
			setPos = true;
		}
		if (EquipmentManager.heroineIsNaked)
		{
			SexImage.enabled = true;
			circle.enabled = true;
			fuckTimer += Time.deltaTime;
			pct = fuckTimer / fuckDelay;
			circle.fillAmount = pct;
			if (fuckTimer >= fuckDelay)
			{
				SexImage.enabled = false;
				circle.enabled = false;
				circle.fillAmount = 0f;
				pct = 0f;
				startFucking = true;
				PlayerController.iGetFucked = true;
			}
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(pantiesDurDamage);
		}
	}

	private void Sex()
	{
		if (!stopCumming)
		{
			InitiateUI();
		}
		VignetteEffect();
		PlayerManager.IsVirgin = false;
		if (currentCum > 100f)
		{
			StartCoroutine(StartCumming());
			PlayerController.heIsFuckingHard = true;
			HeroineStats.fartiged = true;
			if (!cummed)
			{
				HeroineStats.debuffedStam += 10f;
				cummed = true;
			}
			if (!cumSoundStarted)
			{
				cumSound.Play();
				cumSoundStarted = true;
			}
			return;
		}
		if (currentCum > 15f && !blowjoberSpawned)
		{
			FutaSpawn = Object.Instantiate(FutaBlowjober, new Vector3(Heroine.transform.position.x - 1f, Heroine.transform.position.y, Heroine.transform.position.z), Quaternion.identity);
			blowjoberSpawned = true;
		}
		heroineAnim.SetBool("isScared", value: false);
		HeroineStats.aroused = true;
		if (PlayerController.iFalledFront)
		{
			base.transform.position = Heroine.transform.position;
			base.transform.eulerAngles = new Vector3(Heroine.transform.eulerAngles.x, Heroine.transform.eulerAngles.y + 180f, 0f - Heroine.transform.eulerAngles.z);
			if (currentCum > 50f)
			{
				myAnim.Play("rig|Futa_FrontSex2");
				heroineAnim.Play("rig|futaFrontSex2");
				FutaSpawn.GetComponent<FutaBlowjober>().startFucking = true;
				if (!shake3)
				{
					Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
					shake3 = true;
				}
				return;
			}
			myAnim.Play("rig|Futa_FrontSex1");
			heroineAnim.Play("rig|futaFrontSex1");
		}
		if (!PlayerController.iFalledBack)
		{
			return;
		}
		base.transform.position = Heroine.transform.position;
		base.transform.localRotation = Heroine.transform.localRotation;
		if (currentCum > 50f)
		{
			myAnim.Play("rig|Futa_BehindSex2");
			heroineAnim.Play("rig|futaBehindSex2");
			FutaSpawn.GetComponent<FutaBlowjober>().startFucking = true;
			if (!shake3)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
				shake3 = true;
			}
		}
		else
		{
			myAnim.Play("rig|Futa_Behind_Sex");
			heroineAnim.Play("rig|futaBehindSex1");
		}
	}

	private IEnumerator StartCumming()
	{
		if (!stopCumming)
		{
			if (!shake4)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
				shake4 = true;
			}
			Heroine.GetComponent<HeroineStats>().DrainStamina(3f);
			HeroineStats.pregnant = false;
			heroineAnim.SetBool("isPregnant", value: false);
			HeroineStats.addictiveCum = true;
			HeroineStats.creampied = true;
			HeroineStats.oralCreampie = true;
			if (PlayerController.iFalledFront)
			{
				myAnim.Play("rig|Futa_FrontSexCum");
				heroineAnim.Play("rig|futaFrontCum");
				FutaSpawn.GetComponent<Animator>().Play("rig|Futa_FrontBlowjobCum");
			}
			if (PlayerController.iFalledBack)
			{
				myAnim.Play("rig|Futa_BehindCum");
				heroineAnim.Play("rig|futaBehindCum");
				FutaSpawn.GetComponent<Animator>().Play("rig|Futa_BehindBlowjobCum");
			}
		}
		yield return new WaitForSeconds(5f);
		stopCumming = true;
		if (PlayerController.iFalledFront)
		{
			heroineAnim.Play("rig|futaFrontAfterCum");
		}
		if (PlayerController.iFalledBack)
		{
			heroineAnim.Play("rig|futaBehindAfterCum");
		}
		Release();
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
		Debug.Log("Released");
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
		InventoryUI.heroineIsChased = false;
		PlayerController.iGetFucked = false;
		heroineAnim.SetBool("phantomFucked", value: false);
		if (insertSound.isPlaying)
		{
			insertSound.Stop();
		}
		currentCum = 0f;
		currentStamina = maxStamina;
		fuckTimer = 0f;
		mountTimer = 0f;
		pct = 0f;
		startTeasing = false;
		startFucking = false;
		blowjoberSpawned = false;
		stopCumming = false;
		setPos = false;
		shake1 = false;
		shake2 = false;
		shake3 = false;
		shake4 = false;
		HeroineStats.fartiged = false;
		cumSound.Stop();
		Fade();
		Object.Destroy(FutaSpawn);
		Destroy();
	}

	private void Fade()
	{
		Body.enabled = false;
		Brows.enabled = false;
		Eyes.enabled = false;
		Hair.enabled = false;
	}

	private void Destroy()
	{
		if (!lastPortPlayed)
		{
			port.Play();
			lastPortPlayed = true;
		}
		PlayerManager.spawnedFutaMounter = false;
		Object.Destroy(base.gameObject);
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
		EnemyUI.instance.portraitWolf.enabled = false;
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		_ = currentCum / maxCum;
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	private void ThrustEvent()
	{
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustValue);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(pleasureValue);
		if (shake3)
		{
			sexHardSound.Play();
			thrusted = true;
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		else
		{
			sexSound.Play();
		}
	}
}
