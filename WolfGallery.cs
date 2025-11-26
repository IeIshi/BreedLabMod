using UnityEngine;
using UnityEngine.UI;

public class WolfGallery : Interactable
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

	public Transform sexPosFrontWolf;

	public AudioSource insertSound;

	public AudioSource sexSound;

	public AudioSource sexSoundClap;

	public AudioSource creatureAware;

	public AudioSource cumSound;

	public AudioSource ripSound;

	public Image circle;

	public Image SexImage;

	private bool hardFuck;

	private bool superFuck;

	public GameObject povCam;

	public GameObject thirdPersoncam;

	public Transform sexCamFront;

	public Transform sexCamBehind;

	public Transform sexCamBehind2;

	public GameObject controlPanel;

	public GameObject gallaryEqManager;

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
		if (!insertSound.isPlaying)
		{
			insertSound.Play();
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
			animator.Play("rig|InsertBehind");
			heroineAnimator.Play("rig|HumInsertionBehind");
		}
		else
		{
			animator.Play("rig|InsertFront");
			heroineAnimator.Play("rig|HumInsertion");
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
			if (!hardFuck)
			{
				animator.Play("rig|HumSexBehind1");
				heroineAnimator.Play("rig|HumSexBehind1");
				CameraFollow.target = sexCamBehind;
			}
			else
			{
				animator.Play("rig|HumSexBehind2");
				heroineAnimator.Play("rig|HumSexBehind2");
				CameraFollow.target = sexCamBehind2;
			}
		}
		else if (!hardFuck)
		{
			animator.Play("rig|HumSexFront1");
			heroineAnimator.Play("rig|HumSexFront1");
		}
		else
		{
			animator.Play("rig|HumSexFront2");
			heroineAnimator.Play("rig|HumSexFront2");
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
			animator.Play("rig|HumCumBehind2");
			heroineAnimator.Play("rig|HumCumBehind2");
		}
		else
		{
			animator.Play("rig|HumCumFront2");
			heroineAnimator.Play("rig|HumCumFront2");
		}
		currentCum -= Time.deltaTime * 20f;
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
		DisablePOV();
		PlayerController.heIsFuckingHard = false;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		base.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			heroineAnimator.Play("rig|FalledBack");
		}
		else
		{
			heroineAnimator.Play("rig|FalledFront");
		}
		insertSound.Stop();
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
			base.transform.rotation = sexPosFrontWolf.rotation;
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
		if (!controlPanel.GetComponent<PosControlPanel>().doggy && Input.GetKeyDown(KeyCode.C))
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

	private void ThrustEventHard()
	{
		currentCum += gainCumPerThrust;
		EnemyUI.instance.GainCum(gainCumPerThrust);
		sexSoundClap.Play();
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
