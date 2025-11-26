using UnityEngine;
using UnityEngine.UI;

public class CurScGallery : Interactable
{
	public enum SexState
	{
		IDLE,
		TEASE,
		LIFTSEX,
		COWGIRL,
		BlOWJOB,
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

	public Transform releasePos;

	public AudioSource bjSoundOne;

	public AudioSource bjSoundTwo;

	public AudioSource sexSound;

	public AudioSource bodyCollide;

	public AudioSource sexHardSound;

	public AudioSource cumSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	public GameObject povCam;

	public GameObject thirdPersoncam;

	public Transform sexCamBlowjob;

	public Transform sexCamCowgirl;

	public Transform sexCamLifted;

	public GameObject controlPanel;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private bool liftedSex;

	private bool blowJobSex;

	private bool cowGirlSex;

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
		heroineAnimator.SetBool("falled", value: true);
		GetComponent<CapsuleCollider>().enabled = false;
		PlayerController.iGetFucked = true;
		PlayerController.iFalled = true;
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		if (controlPanel.GetComponent<PosControlPanelScientist>().blowjob)
		{
			state = SexState.BlOWJOB;
		}
		else if (controlPanel.GetComponent<PosControlPanelScientist>().cowgirl)
		{
			state = SexState.COWGIRL;
			animator.Play("rig_game|HeroIsInserting");
			heroineAnimator.Play("rig|DickInsert");
		}
		else if (controlPanel.GetComponent<PosControlPanelScientist>().lifted)
		{
			state = SexState.TEASE;
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
		case SexState.LIFTSEX:
			LiftSex();
			break;
		case SexState.BlOWJOB:
			BlowJob();
			break;
		case SexState.COWGIRL:
			CowGirl();
			break;
		case SexState.CUM:
			Cum();
			break;
		}
	}

	private void Idle()
	{
		animator.Play("rig_game|Idle");
	}

	private void BlowJob()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		animator.SetBool("isIdle", value: false);
		TakeSexPos();
		InitiateUI();
		VignetteEffect();
		animator.Play("Blowjob");
		heroineAnimator.Play("rig|Blowjob");
		CameraFollow.target = sexCamBlowjob;
		blowJobSex = true;
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
		}
		if (superFuck)
		{
			animator.speed = 2f;
			heroineAnimator.speed = 2f;
			PlayerController.heIsFuckingHard = true;
		}
		if (currentCum >= 100f)
		{
			state = SexState.CUM;
		}
	}

	private void CowGirl()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		TakeSexPos();
		InitiateUI();
		VignetteEffect();
		animator.SetBool("isIdle", value: false);
		if (currentCum >= 100f)
		{
			state = SexState.CUM;
		}
		if (superFuck)
		{
			animator.speed = 2f;
			heroineAnimator.speed = 2f;
			PlayerController.heIsFuckingHard = true;
			animator.SetBool("getFuckedHard", value: true);
			heroineAnimator.SetBool("dickRideHard", value: true);
			return;
		}
		CameraFollow.target = sexCamCowgirl;
		cowGirlSex = true;
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
		}
	}

	private void Tease()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		liftedSex = true;
		animator.SetBool("isIdle", value: false);
		InitiateUI();
		TakeSexPos();
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		Heroine.GetComponent<HeroineStats>().GainLust(lustGain);
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
			state = SexState.LIFTSEX;
		}
		heroineAnimator.SetBool("isScared", value: true);
		animator.Play("rig_game|BehindGrind");
		heroineAnimator.Play("rig|BehindGrind");
	}

	private void LiftSex()
	{
		CameraFollow.target = sexCamLifted;
		TakeSexPos();
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		animator.SetBool("isIdle", value: false);
		InitiateUI();
		VignetteEffect();
		heroineAnimator.SetBool("isScared", value: false);
		HeroineStats.aroused = true;
		animator.Play("rig_game|BehindFuck");
		heroineAnimator.Play("rig|BehindLiftetSex");
		if (hardFuck && !superFuck)
		{
			animator.speed = 1.4f;
			heroineAnimator.speed = 1.4f;
		}
		if (superFuck)
		{
			animator.speed = 2f;
			heroineAnimator.speed = 2f;
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
		if (blowJobSex)
		{
			HeroineStats.oralCreampie = true;
		}
		else
		{
			HeroineStats.creampied = true;
		}
		Heroine.GetComponent<HeroineStats>().GainLust(lustGain * 5f);
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (liftedSex)
		{
			animator.Play("rig_game|BehindCUM");
			heroineAnimator.Play("rig|BehindLiftedCUM");
		}
		if (blowJobSex)
		{
			animator.SetBool("isCummingBJ", value: true);
			heroineAnimator.SetBool("ScBJCum", value: true);
		}
		if (cowGirlSex)
		{
			animator.SetBool("isCummingRide", value: true);
			heroineAnimator.SetBool("ScRideCum", value: true);
		}
		currentCum -= Time.deltaTime * 20f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 20f);
		hardFuck = false;
		superFuck = false;
		if (currentCum <= 0f)
		{
			cumSound.Stop();
			if (liftedSex)
			{
				state = SexState.LIFTSEX;
			}
			if (blowJobSex)
			{
				animator.SetBool("isCummingBJ", value: false);
				heroineAnimator.SetBool("ScBJCum", value: false);
				state = SexState.BlOWJOB;
			}
			if (cowGirlSex)
			{
				heroineAnimator.SetBool("ScRideCum", value: false);
				animator.SetBool("isCummingRide", value: false);
				state = SexState.COWGIRL;
			}
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
		Heroine.transform.rotation = releasePos.rotation;
		Heroine.transform.position = releasePos.position;
		heroineAnimator.SetBool("isScared", value: false);
		if (blowJobSex)
		{
			heroineAnimator.Play("rig|FalledFront");
			animator.SetBool("isCummingBJ", value: false);
			heroineAnimator.SetBool("ScBJCum", value: false);
		}
		if (cowGirlSex)
		{
			animator.SetBool("isCummingRide", value: false);
			heroineAnimator.SetBool("ScRideCum", value: false);
			animator.SetBool("getFuckedHard", value: false);
			heroineAnimator.SetBool("dickRideHard", value: false);
			heroineAnimator.Play("rig|FalledFront");
		}
		if (liftedSex)
		{
			heroineAnimator.Play("Locomotion");
			PlayerController.iFalled = false;
		}
		cumSound.Stop();
		liftedSex = false;
		blowJobSex = false;
		cowGirlSex = false;
		GetComponent<CapsuleCollider>().enabled = true;
		state = SexState.IDLE;
	}

	private void TakeSexPos()
	{
		if (liftedSex)
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

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void FuckSpeedController()
	{
		if (currentCum > hardFuckCumRange)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			hardFuck = true;
			if (currentCum > superFuckCumRange)
			{
				superFuck = true;
			}
		}
	}

	private void BJThrust()
	{
		thrusted = true;
		int num = Random.Range(1, 3);
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain / 3f);
		if (HeroineStats.currentLust >= 80f)
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		}
		FuckSpeedController();
		if (num == 1)
		{
			bjSoundOne.Play();
		}
		else
		{
			bjSoundTwo.Play();
		}
	}

	private void ThrustEvent()
	{
		thrusted = true;
		if (PlayerController.heIsFuckingHard)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		GainCumInstant(gainCumPerThrust);
		FuckSpeedController();
		sexSound.Play();
		bodyCollide.Play();
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
	}

	private void ThrustEventHard()
	{
		currentCum += gainCumPerThrust;
		EnemyUI.instance.GainCum(gainCumPerThrust);
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
