using UnityEngine;
using UnityEngine.UI;

public class ImpregnatorGallary : Interactable
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

	public Transform sexPosBehind;

	public AudioSource insertSound;

	public AudioSource sexSound;

	public AudioSource creatureAware;

	public AudioSource cumSound;

	public AudioSource ripSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	public Transform sexCam;

	public Transform sexCamDoggy;

	public GameObject gallaryEqManager;

	private bool doggy;

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
		if (doggy)
		{
			CameraFollow.target = sexCamDoggy;
		}
		else
		{
			CameraFollow.target = sexCam;
		}
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
		}
		InitiateUI();
		TakeSexPos();
		Heroine.GetComponent<HeroineStats>().GainLust(lustGain);
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
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
			state = SexState.SEX;
		}
		heroineAnimator.SetBool("isScared", value: true);
		if (doggy)
		{
			animator.Play("rig|behind_tease_laying");
			return;
		}
		animator.SetBool("isGrabbing", value: true);
		heroineAnimator.Play("rig|PregInsRub");
	}

	private void Sex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		heroineAnimator.SetBool("isScared", value: false);
		HeroineStats.aroused = true;
		if (doggy)
		{
			animator.Play("rig|behind_sex_laying");
			heroineAnimator.Play("rig|Impreg_LaySex");
			CameraFollow.target = sexCamDoggy;
		}
		else
		{
			animator.Play("rig|Fuck");
			heroineAnimator.Play("rig|PregInsFuck");
		}
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
			VignetteEffect();
		}
		if (superFuck)
		{
			animator.speed = 2f;
			heroineAnimator.speed = 2f;
			VignetteEffect();
			PlayerController.heIsFuckingHard = true;
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
		HeroineStats.creampied = true;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (doggy)
		{
			animator.Play("rig|behind_sex_lay_cum");
			heroineAnimator.Play("rig|Impreg_sex_lay_cum");
		}
		else
		{
			animator.Play("rig|Cum");
			heroineAnimator.Play("rig|PregInsCum");
		}
		currentCum -= Time.deltaTime * 20f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 20f);
		hardFuck = false;
		superFuck = false;
		if (currentCum <= 0f)
		{
			cumSound.Stop();
			state = SexState.SEX;
			doggy = true;
		}
	}

	private void Release()
	{
		hardFuck = false;
		superFuck = false;
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
		base.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		heroineAnimator.Play("rig|FalledBack");
		animator.SetBool("isGrabbing", value: false);
		PlayerController.iFalledBack = true;
		doggy = false;
		insertSound.Stop();
		cumSound.Stop();
	}

	private void TakeSexPos()
	{
		if (!doggy)
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
}
