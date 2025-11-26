using UnityEngine;
using UnityEngine.UI;

public class LurkerGallery : Interactable
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

	public Transform fallPointFront;

	public AudioSource sexSoundOne;

	public AudioSource sexSoundNormal;

	public AudioSource sexSoundFast;

	public AudioSource grabSound;

	public AudioSource teaseSound;

	public AudioSource cumSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	public GameObject povCam;

	public GameObject thirdPersoncam;

	public Transform sexCamFront;

	public Transform sexCamBehind;

	public GameObject controlPanel;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private CamShaker shake;

	private void Start()
	{
		state = SexState.IDLE;
		shake = Object.FindObjectOfType<CamShaker>();
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
		animator.Play("rig|Lurk_Idle");
	}

	private void Tease()
	{
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			CameraFollow.target = sexCamBehind;
		}
		else
		{
			CameraFollow.target = sexCamFront;
		}
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = SexState.IDLE;
			return;
		}
		if (!teaseSound.isPlaying)
		{
			teaseSound.Play();
		}
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
			state = SexState.SEX;
		}
		heroineAnimator.SetBool("isScared", value: true);
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			CameraFollow.target = sexCamBehind;
			animator.SetBool("isBehindGrab", value: true);
			heroineAnimator.SetBool("LurkBehindGrab", value: true);
		}
		else
		{
			animator.SetBool("isFrontGrab", value: true);
			heroineAnimator.SetBool("LurkFrontGrab", value: true);
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
		VignetteEffect();
		heroineAnimator.SetBool("isScared", value: false);
		HeroineStats.aroused = true;
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			animator.Play("rig|Lurk_Behindsex");
			heroineAnimator.Play("rig|Lurk_Behind_Sex");
		}
		else
		{
			animator.Play("rig|Lurk_Front_Fuck");
			heroineAnimator.Play("rig|Her_Lurk_Front_Fuck");
		}
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
		HeroineStats.creampied = true;
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			animator.Play("rig|Lurk_Behind_Cum");
			heroineAnimator.Play("rig|Lurk_Cum_Behind");
		}
		else
		{
			animator.Play("rig|Lurk_Front_Cum");
			heroineAnimator.Play("rig|Lurk_Front_Cum");
		}
		currentCum -= Time.deltaTime * 10f;
		EnemyUI.instance.LoseCum(Time.deltaTime * 20f);
		hardFuck = false;
		superFuck = false;
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
		animator.SetBool("isBehindGrab", value: false);
		heroineAnimator.SetBool("LurkBehindGrab", value: false);
		if (!controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			Heroine.transform.position = fallPointFront.position;
			Heroine.transform.rotation = fallPointFront.rotation;
		}
		animator.SetBool("isFrontGrab", value: false);
		heroineAnimator.SetBool("LurkFrontGrab", value: false);
		GetComponent<CapsuleCollider>().enabled = true;
		teaseSound.Stop();
		cumSound.Stop();
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

	public void ThrustEvent()
	{
		thrusted = true;
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
		if (currentCum > hardFuckCumRange)
		{
			if (thirdPersoncam.activeSelf)
			{
				shake.StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			}
			hardFuck = true;
			if (currentCum > superFuckCumRange)
			{
				superFuck = true;
			}
		}
		if (hardFuck)
		{
			sexSoundFast.Play();
			shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		}
		else
		{
			sexSoundNormal.Play();
		}
	}

	public void CumEvent()
	{
		sexSoundOne.Play();
		shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustGain);
	}
}
