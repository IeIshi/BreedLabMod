using UnityEngine;
using UnityEngine.UI;

public class FutaGallery : Interactable
{
	public enum SexState
	{
		IDLE,
		TEASE,
		SEX,
		CUM
	}

	public float maxStamina;

	public float maxCum;

	public float fuckDelay;

	public float gainCumPerThrust;

	public float thrustDmg;

	public float lustGain;

	public float hardFuckCumRange;

	public float superFuckCumRange;

	private float currentStamina;

	private float currentCum;

	private float fuckTime;

	private float pct;

	public SexState state;

	public GameObject FutaBlowjober;

	private GameObject FutaSpawn;

	private GameObject Heroine;

	private Animator animator;

	private Animator heroineAnimator;

	public Transform sexPos;

	public Transform sexPosBehind;

	public AudioSource port;

	public AudioSource slideInsideSound;

	public AudioSource sexSound;

	public AudioSource sexHardSound;

	public AudioSource cumSound;

	public AudioSource insertSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	private bool blowjoberSpawned;

	public GameObject povCam;

	public GameObject thirdPersoncam;

	public Transform sexCamFront;

	public Transform sexCamBehind;

	public GameObject controlPanel;

	private bool thrusted;

	private bool max;

	private bool setPos;

	private bool shake2;

	private bool shake3;

	private bool shake4;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private void Start()
	{
		state = SexState.IDLE;
		Heroine = GameObject.Find("Heroine");
		animator = GetComponent<Animator>();
		heroineAnimator = Heroine.GetComponent<Animator>();
		currentCum = 0f;
		currentStamina = maxStamina;
	}

	public override void Interact()
	{
		base.Interact();
		heroineAnimator.SetBool("falled", value: true);
		GetComponent<CapsuleCollider>().enabled = false;
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		if (state == SexState.IDLE)
		{
			state = SexState.TEASE;
		}
		else
		{
			state = SexState.IDLE;
		}
	}

	private void FixedUpdate()
	{
		SexRoutine(state);
	}

	private void SexRoutine(SexState state)
	{
		switch (state)
		{
		case SexState.IDLE:
			Idle();
			break;
		case SexState.TEASE:
			Tease();
			break;
		case SexState.SEX:
			Sex();
			break;
		case SexState.CUM:
			Cum();
			break;
		}
	}

	private void Idle()
	{
		animator.Play("rig|Idle 0");
	}

	private void Tease()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			return;
		}
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
		}
		InitiateUI();
		Heroine.GetComponent<HeroineStats>().GainLust(lustGain / 2f);
		if (!setPos)
		{
			port.Play();
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			PlayerController.iGetFucked = true;
			heroineAnimator.SetBool("isScared", value: true);
			heroineAnimator.SetBool("phantomFucked", value: true);
			TakeSexPos();
			if (controlPanel.GetComponent<PosControlPanel>().doggy)
			{
				CameraFollow.target = sexCamBehind;
				PlayerController.iFalledBack = true;
				animator.Play("rig|Futa_Behind_Grind");
				heroineAnimator.Play("rig|futaBehindGrind");
			}
			else
			{
				CameraFollow.target = sexCamFront;
				PlayerController.iFalledFront = true;
				animator.Play("rig|Futa_FrontTease");
				heroineAnimator.Play("rig|futaFrontTease");
			}
			setPos = true;
		}
		SexImage.enabled = true;
		circle.enabled = true;
		fuckTime += Time.deltaTime;
		pct = fuckTime / fuckDelay;
		circle.fillAmount = pct;
		if (fuckTime >= fuckDelay)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			PlayerController.iGetFucked = true;
			state = SexState.SEX;
		}
	}

	private void Sex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			return;
		}
		if (!shake2)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
			slideInsideSound.Play();
			cumSound.Stop();
			insertSound.Stop();
			shake2 = true;
		}
		VignetteEffect();
		TakeSexPos();
		if (currentCum > 100f)
		{
			state = SexState.CUM;
		}
		if (currentCum > 15f && !blowjoberSpawned)
		{
			FutaSpawn = Object.Instantiate(FutaBlowjober, new Vector3(Heroine.transform.position.x - 1f, Heroine.transform.position.y, Heroine.transform.position.z), Quaternion.identity);
			blowjoberSpawned = true;
		}
		heroineAnimator.SetBool("isScared", value: false);
		HeroineStats.aroused = true;
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			if (currentCum > 50f)
			{
				animator.Play("rig|Futa_BehindSex2");
				heroineAnimator.Play("rig|futaBehindSex2");
				FutaSpawn.GetComponent<FutaBlowjober>().startFucking = true;
				if (!shake3)
				{
					Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
					shake3 = true;
				}
			}
			else
			{
				animator.Play("rig|Futa_Behind_Sex");
				heroineAnimator.Play("rig|futaBehindSex1");
			}
		}
		else if (currentCum > 50f)
		{
			animator.Play("rig|Futa_FrontSex2");
			heroineAnimator.Play("rig|futaFrontSex2");
			FutaSpawn.GetComponent<FutaBlowjober>().startFucking = true;
			if (!shake3)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
				shake3 = true;
			}
		}
		else
		{
			animator.Play("rig|Futa_FrontSex1");
			heroineAnimator.Play("rig|futaFrontSex1");
		}
	}

	private void Cum()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			return;
		}
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (!shake4)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
			shake4 = true;
		}
		Heroine.GetComponent<HeroineStats>().DrainStamina(3f);
		currentCum -= Time.deltaTime * 20f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 20f);
		HeroineStats.addictiveCum = true;
		HeroineStats.creampied = true;
		HeroineStats.oralCreampie = true;
		if (PlayerController.iFalledFront)
		{
			animator.Play("rig|Futa_FrontSexCum");
			heroineAnimator.Play("rig|futaFrontCum");
			FutaSpawn.GetComponent<Animator>().Play("rig|Futa_FrontBlowjobCum");
		}
		if (PlayerController.iFalledBack)
		{
			animator.Play("rig|Futa_BehindCum");
			heroineAnimator.Play("rig|futaBehindCum");
			FutaSpawn.GetComponent<Animator>().Play("rig|Futa_BehindBlowjobCum");
		}
		if (currentCum <= 0f)
		{
			Object.Destroy(FutaSpawn);
			port.Play();
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
			shake2 = false;
			shake3 = false;
			shake4 = false;
			blowjoberSpawned = false;
			state = SexState.SEX;
		}
	}

	private void Release()
	{
		blowjoberSpawned = false;
		GetComponent<CapsuleCollider>().enabled = true;
		shake2 = false;
		shake3 = false;
		shake4 = false;
		cumSound.Stop();
		insertSound.Stop();
		currentCum = 0f;
		pct = 0f;
		fuckTime = 0f;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
		setPos = false;
		PlayerController.heIsFuckingHard = false;
		PlayerController.iGetFucked = false;
		PlayerController.iFalled = true;
		HeroineStats.aroused = false;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		Object.Destroy(FutaSpawn);
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			heroineAnimator.Play("rig|futaBehindAfterCum");
		}
		else
		{
			heroineAnimator.Play("rig|futaFrontAfterCum");
		}
		state = SexState.IDLE;
	}

	private void TakeSexPos()
	{
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			Heroine.transform.rotation = sexPosBehind.rotation;
			Heroine.transform.position = sexPosBehind.position;
		}
		else
		{
			Heroine.transform.rotation = sexPos.rotation;
			Heroine.transform.position = sexPos.position;
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
		currentCum += gainCumPerThrust;
		EnemyUI.instance.GainCum(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
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
