using UnityEngine;
using UnityEngine.UI;

public class MantisGallery : Interactable
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

	public GameObject MantisCum;

	public GameObject povCam;

	public GameObject thirdPersoncam;

	public Transform sexCamFront;

	public Transform sexCamCum;

	public GameObject controlPanel;

	public GameObject gallaryEqManager;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private float mSize;

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

	private void LateUpdate()
	{
		if (state != 0)
		{
			CamControl();
		}
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
		Heroine.GetComponent<HeroineStats>().GainLust(lustGain);
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
		if (fuckDelay >= fuckTime)
		{
			SexImage.enabled = false;
			circle.enabled = false;
			circle.fillAmount = 0f;
			pct = 0f;
			state = SexState.SEX;
		}
		heroineAnimator.SetBool("isScared", value: true);
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			animator.Play("rig|MantisMateBehindStart");
			heroineAnimator.Play("rig|MantisBehindMateStart");
		}
		else
		{
			animator.Play("rig|MantisMateFrontStart");
			heroineAnimator.Play("rig|MantisFrontMateStart");
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
		cumSound.Stop();
		TakeSexPos();
		VignetteEffect();
		MantisCum.SetActive(value: false);
		heroineAnimator.SetBool("isScared", value: false);
		CameraFollow.target = sexCamFront;
		if (currentCum >= 100f)
		{
			state = SexState.CUM;
			return;
		}
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
			HeroineStats.aroused = true;
		}
		if (superFuck)
		{
			if (currentCum <= 80f)
			{
				animator.speed = 1f;
				heroineAnimator.speed = 1f;
			}
			else
			{
				animator.speed = 1.5f;
				heroineAnimator.speed = 1.5f;
			}
			if (controlPanel.GetComponent<PosControlPanel>().doggy)
			{
				animator.Play("rig|MantisMateBehindFast");
				heroineAnimator.Play("rig|MantisBehindMateFast");
			}
			else
			{
				animator.Play("rig|MateFrontFast");
				heroineAnimator.Play("rig|MantisFrontFastMate");
			}
		}
		else if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			animator.Play("rig|MantisMateBehind");
			heroineAnimator.Play("rig|MantisBehindMate");
		}
		else
		{
			animator.Play("rig|MateFront");
			heroineAnimator.Play("rig|MantisFrontMate");
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
		PlayerController.heIsFuckingHard = true;
		Heroine.transform.rotation = sexPosBehind.rotation;
		Heroine.transform.position = sexPosBehind.position;
		CameraFollow.target = sexCamCum;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		MantisCum.SetActive(value: true);
		if (mSize < 300f)
		{
			MantisCum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize += 1f);
		}
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		if (!cumSound.isPlaying)
		{
			Heroine.GetComponent<HeroineStats>().LargeCumDrip();
			cumSound.Play();
		}
		animator.SetBool("isCumming", value: true);
		heroineAnimator.SetBool("mantisCumming", value: true);
		heroineAnimator.SetBool("isCumFilled", value: true);
		currentCum -= Time.deltaTime * 10f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 10f);
		hardFuck = false;
		superFuck = false;
		HeroineStats.creampied = true;
		if (currentCum <= 0f)
		{
			cumSound.Stop();
			animator.SetBool("isCumming", value: false);
			heroineAnimator.SetBool("mantisCumming", value: false);
			state = SexState.SEX;
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
		DisablePOV();
		PlayerController.heIsFuckingHard = false;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		MantisCum.SetActive(value: false);
		cumSound.Stop();
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		mSize = 0f;
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			heroineAnimator.Play("rig|futaBehindAfterCum");
		}
		else
		{
			heroineAnimator.Play("rig|futaFrontAfterCum");
		}
		cumSound.Stop();
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

	private void CamControl()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (thirdPersoncam.activeSelf)
			{
				EnablePOV();
			}
			else
			{
				DisablePOV();
			}
		}
	}

	private void EnablePOV()
	{
		thirdPersoncam.SetActive(value: false);
		povCam.GetComponent<Camera>().enabled = true;
		povCam.GetComponent<AudioListener>().enabled = true;
	}

	private void DisablePOV()
	{
		thirdPersoncam.SetActive(value: true);
		povCam.GetComponent<Camera>().enabled = false;
		povCam.GetComponent<AudioListener>().enabled = false;
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
		if (superFuck)
		{
			if (currentCum <= 80f)
			{
				currentCum += gainCumPerThrust;
				EnemyUI.instance.GainCum(gainCumPerThrust);
			}
			else
			{
				currentCum += 0.5f;
				EnemyUI.instance.GainCum(0.5f);
			}
		}
		else
		{
			currentCum += gainCumPerThrust;
			EnemyUI.instance.GainCum(gainCumPerThrust);
		}
		thrusted = true;
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
		if (currentCum > hardFuckCumRange)
		{
			if (thirdPersoncam.activeSelf)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			}
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
