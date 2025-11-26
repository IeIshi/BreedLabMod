using UnityEngine;
using UnityEngine.UI;

public class LickerGallery : Interactable
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

	public float lickTime;

	public float fuckTime;

	public float gainCumPerThrust;

	public float thrustDmg;

	public float lustGain;

	public float hardFuckCumRange;

	public float superFuckCumRange;

	private float currentStamina;

	private float currentCum;

	private float fuckDelay;

	private float pct;

	public SexState state;

	private GameObject Heroine;

	private Animator animator;

	private Animator heroineAnimator;

	public Transform sexPos;

	public Transform sexPosCum;

	public AudioSource insertSound;

	public AudioSource sexSound;

	public AudioSource creatureAware;

	public AudioSource cumSound;

	public AudioSource ripSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	public Transform sexCamFront;

	public GameObject gallaryEqManager;

	private bool sheOrgasmed;

	private bool thrusted;

	private bool max;

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

	private void FixedUpdate()
	{
		SexRoutine(state);
	}

	public override void Interact()
	{
		base.Interact();
		creatureAware.Play();
		heroineAnimator.SetBool("falled", value: true);
		if (state == SexState.IDLE)
		{
			state = SexState.TEASE;
		}
		else
		{
			state = SexState.IDLE;
		}
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
		animator.Play("rig|Idle");
	}

	private void Tease()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		CameraFollow.target = sexCamFront;
		InitiateUI();
		TakeSexPos();
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		SexImage.enabled = true;
		circle.enabled = true;
		fuckDelay += Time.deltaTime;
		pct = fuckDelay / fuckTime;
		circle.fillAmount = pct;
		if (HeroineStats.orgasm)
		{
			animator.Play("rig|LickOrgasm");
			heroineAnimator.Play("rig|LickLickFuckOrg");
			sheOrgasmed = true;
			return;
		}
		if (sheOrgasmed)
		{
			state = SexState.SEX;
		}
		if (fuckDelay < lickTime)
		{
			heroineAnimator.SetBool("isScared", value: true);
			animator.Play("rig|BeforeLicking");
			heroineAnimator.Play("rig|HBefore_Licking");
		}
		if (fuckDelay >= lickTime && fuckDelay < fuckTime)
		{
			if (!insertSound.isPlaying)
			{
				insertSound.Play();
			}
			animator.Play("rig|Licking");
			heroineAnimator.Play("rig|LickLicking");
			Heroine.GetComponent<HeroineStats>().GainLust(lustGain);
		}
		if (fuckDelay >= fuckTime)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			heroineAnimator.SetBool("isScared", value: false);
			animator.Play("rig|LickFucking");
			heroineAnimator.Play("rig|LickLickFuck");
			VignetteEffect();
		}
	}

	private void Sex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		animator.Play("rig|LickerSex");
		heroineAnimator.Play("rig|LickSex");
		VignetteEffect();
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
			HeroineStats.aroused = true;
		}
		if (superFuck)
		{
			animator.speed = 2f;
			heroineAnimator.speed = 2f;
		}
		if (currentCum >= 100f)
		{
			state = SexState.CUM;
		}
	}

	private void Cum()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		Heroine.transform.rotation = sexPosCum.rotation;
		Heroine.transform.position = sexPosCum.position;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		animator.Play("rig|LickerCum");
		heroineAnimator.Play("rig|LickLickerCum");
		currentCum -= Time.deltaTime * 20f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 20f);
		hardFuck = false;
		superFuck = false;
		HeroineStats.creampied = true;
		if (currentCum <= 0f)
		{
			cumSound.Stop();
			state = SexState.SEX;
		}
	}

	private void Release()
	{
		hardFuck = false;
		superFuck = false;
		sheOrgasmed = false;
		currentCum = 0f;
		pct = 0f;
		fuckDelay = 0f;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
		PlayerController.heIsFuckingHard = false;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		heroineAnimator.Play("rig|FalledBack");
		insertSound.Stop();
		cumSound.Stop();
	}

	private void TakeSexPos()
	{
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
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
		thrusted = true;
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
		if (currentCum > hardFuckCumRange)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			hardFuck = true;
			if (currentCum > superFuckCumRange)
			{
				superFuck = true;
			}
		}
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
	}

	private void LickEvent()
	{
		sexSound.Play();
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
		thrusted = true;
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 5f);
	}
}
