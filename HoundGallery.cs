using UnityEngine;
using UnityEngine.UI;

public class HoundGallery : Interactable
{
	public enum MyState
	{
		IDLE,
		TEASE,
		SEX,
		CUM,
		KNOT
	}

	private GameObject Heroine;

	public GameObject controlPanel;

	public Transform targetCamBehind;

	public Transform mountFrontPos;

	public Transform houndOriginPose;

	private Transform cam;

	public AudioClip[] clapSoundsArray;

	public AudioSource[] slideSoundArray;

	public AudioSource attentionGrowl;

	public AudioSource pantSound;

	public AudioSource bark;

	public AudioSource attackSound;

	public AudioSource longCumSound;

	public AudioSource cumSound;

	public AudioSource insertOneSound;

	public AudioSource insertTwoSound;

	private bool attentionSoundPlayed;

	private AudioSource audioSource;

	public Image circle;

	public Image SexImage;

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	public float sex2Range;

	public float maxCum;

	private float currentCum;

	public float gainCumPerThrust;

	public float thrustDmg;

	public float teaseDmg;

	private bool thrusted;

	private bool max;

	private bool camShaked;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public MyState state;

	private Animator animator;

	private Animator heroineAnimator;

	private float sitTimer;

	private float timer;

	private bool insert;

	private float speedModifier = 1f;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		SexImage.enabled = false;
		circle.enabled = false;
		speedModifier = 1f;
		currentCum = 0f;
		animator = GetComponent<Animator>();
		state = MyState.IDLE;
		Heroine = GameObject.Find("Heroine");
		heroineAnimator = Heroine.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		StateMachine(state);
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<CapsuleCollider>().enabled = false;
		PlayerController.iGetFucked = true;
		PlayerController.iFalled = true;
		heroineAnimator.SetBool("falled", value: true);
		state = MyState.TEASE;
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.IDLE:
			Idle();
			break;
		case MyState.TEASE:
			Tease();
			break;
		case MyState.SEX:
			Sex();
			break;
		case MyState.CUM:
			Cum();
			break;
		case MyState.KNOT:
			Knot();
			break;
		}
	}

	private void Release()
	{
		Heroine.GetComponent<PlayerController>().enabled = true;
		currentCum = 0f;
		pct = 0f;
		fuckDelay = 0f;
		timer = 0f;
		camShaked = false;
		speedModifier = 1f;
		longCumSound.Stop();
		cumSound.Stop();
		GetComponent<CapsuleCollider>().enabled = true;
		heroineAnimator.speed = 1f;
		animator.speed = 1f;
		PlayerController.iGetFucked = false;
		PlayerController.heIsFuckingHard = false;
		CameraFollow.target = Heroine.transform;
		SexImage.enabled = false;
		circle.enabled = false;
		DisableUI();
		animator.SetBool("attack", value: false);
		animator.SetBool("isSitting", value: false);
		animator.SetBool("isAlert", value: false);
		animator.SetBool("backTease", value: false);
		animator.SetBool("backSex1", value: false);
		animator.SetBool("backSex2", value: false);
		animator.SetBool("backCum", value: false);
		animator.SetBool("backKnot", value: false);
		animator.SetBool("frontTease", value: false);
		animator.SetBool("frontSex1", value: false);
		animator.SetBool("frontSex2", value: false);
		animator.SetBool("frontCum", value: false);
		heroineAnimator.SetBool("doggyFrontTease", value: false);
		heroineAnimator.SetBool("doggyFrontSex1", value: false);
		heroineAnimator.SetBool("doggyFrontSex2", value: false);
		heroineAnimator.SetBool("doggyFrontCum", value: false);
		heroineAnimator.SetBool("doggyBackTease", value: false);
		heroineAnimator.SetBool("doggyBackSex1", value: false);
		heroineAnimator.SetBool("doggyBackSex2", value: false);
		heroineAnimator.SetBool("doggyBackCum", value: false);
		base.gameObject.transform.position = houndOriginPose.position;
		base.gameObject.transform.rotation = houndOriginPose.rotation;
		state = MyState.IDLE;
	}

	private void Idle()
	{
		animator.Play("rig|Idle");
	}

	private void Sex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		InitiateUI();
		VignetteEffect();
		longCumSound.Stop();
		cumSound.Stop();
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			base.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			if (currentCum >= maxCum)
			{
				state = MyState.CUM;
			}
			if (currentCum > sex2Range)
			{
				animator.SetBool("backSex2", value: true);
				heroineAnimator.SetBool("doggyBackSex2", value: true);
				return;
			}
			animator.SetBool("backSex1", value: true);
			heroineAnimator.SetBool("doggyBackSex1", value: true);
		}
		if (!controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			base.transform.rotation = mountFrontPos.rotation;
			base.transform.position = mountFrontPos.position;
			if (currentCum >= maxCum)
			{
				state = MyState.CUM;
			}
			if (currentCum > sex2Range)
			{
				animator.SetBool("frontCum", value: false);
				heroineAnimator.SetBool("doggyFrontCum", value: false);
				animator.SetBool("frontSex2", value: true);
				heroineAnimator.SetBool("doggyFrontSex2", value: true);
			}
			else
			{
				animator.SetBool("frontSex1", value: true);
				heroineAnimator.SetBool("doggyFrontSex1", value: true);
			}
		}
	}

	private void Knot()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		cumSound.Stop();
		longCumSound.Stop();
		if (!pantSound.isPlaying)
		{
			pantSound.Play();
		}
		InitiateUI();
		base.transform.rotation = Heroine.transform.rotation;
		base.transform.position = Heroine.transform.position;
		LoseCum(4f);
		animator.SetBool("backCum", value: false);
		heroineAnimator.SetBool("doggyBackCum", value: false);
		animator.SetBool("backTease", value: false);
		heroineAnimator.SetBool("doggyBackTease", value: false);
		animator.SetBool("backSex1", value: false);
		animator.SetBool("backKnot", value: true);
		heroineAnimator.SetBool("doggyBackKnot", value: true);
		if (currentCum <= 0f)
		{
			animator.SetBool("backKnot", value: false);
			heroineAnimator.SetBool("doggyBackSex1", value: true);
			heroineAnimator.SetBool("doggyBackKnot", value: false);
			state = MyState.SEX;
		}
	}

	private void Cum()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		animator.speed = 1f;
		heroineAnimator.speed = 1f;
		speedModifier = 1f;
		HeroineStats.stunned = true;
		InitiateUI();
		VignetteEffect();
		HeroineStats.creampied = true;
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			if (currentCum <= 40f)
			{
				state = MyState.KNOT;
				return;
			}
			base.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			animator.SetBool("backKnot", value: false);
			heroineAnimator.SetBool("doggyBackKnot", value: false);
			animator.SetBool("backSex2", value: false);
			heroineAnimator.SetBool("doggyBackSex2", value: false);
			animator.SetBool("backCum", value: true);
			heroineAnimator.SetBool("doggyBackCum", value: true);
		}
		if (!controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			base.transform.rotation = mountFrontPos.rotation;
			base.transform.position = mountFrontPos.position;
			heroineAnimator.SetBool("doggyFrontTease", value: false);
			heroineAnimator.SetBool("doggyFrontSex1", value: false);
			heroineAnimator.SetBool("doggyFrontSex2", value: false);
			animator.SetBool("frontTease", value: false);
			animator.SetBool("frontSex1", value: false);
			animator.SetBool("frontSex2", value: false);
			animator.SetBool("frontCum", value: true);
			heroineAnimator.SetBool("doggyFrontCum", value: true);
		}
		if (currentCum <= 0f)
		{
			if (controlPanel.GetComponent<PosControlPanel>().doggy)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				state = MyState.KNOT;
			}
			if (!controlPanel.GetComponent<PosControlPanel>().doggy)
			{
				state = MyState.SEX;
			}
		}
	}

	private void Tease()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		if (controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			InitiateUI();
			PlayerController.iGetFucked = true;
			base.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			CameraFollow.target = targetCamBehind;
			animator.SetBool("backTease", value: true);
			heroineAnimator.SetBool("doggyBackTease", value: true);
			animator.Play("rig|DoggyBackTease");
			heroineAnimator.Play("rig|DoggyBackTease");
			InitiateSex();
		}
		if (!controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			timer += Time.deltaTime;
			InitiateUI();
			PlayerController.iGetFucked = true;
			base.transform.rotation = mountFrontPos.rotation;
			base.transform.position = mountFrontPos.position;
			CameraFollow.target = targetCamBehind;
			animator.SetBool("frontTease", value: true);
			heroineAnimator.SetBool("doggyFrontTease", value: true);
			animator.Play("rig|DoggyFrontTease");
			heroineAnimator.Play("rig|DoggyFrontTease");
			InitiateSex();
		}
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
			if (!camShaked)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				camShaked = true;
			}
			state = MyState.SEX;
		}
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxCum = maxCum;
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
	}

	private void LoseCum(float gainValue)
	{
		currentCum -= Time.deltaTime * gainValue;
		EnemyUI.instance.LoseCum(Time.deltaTime * gainValue);
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
		if (!HeroineStats.HumanoidBuff)
		{
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg / 10f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		}
		if (currentCum > sex2Range)
		{
			audioSource.clip = clapSoundsArray[Random.Range(0, clapSoundsArray.Length)];
			audioSource.PlayOneShot(audioSource.clip);
			slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		}
		else
		{
			slideSoundArray[Random.Range(0, slideSoundArray.Length)].Play();
		}
		if (currentCum > 50f)
		{
			if (speedModifier < 1.7f)
			{
				speedModifier += 0.05f;
				Debug.Log(speedModifier);
			}
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
			animator.speed = speedModifier;
			heroineAnimator.speed = speedModifier;
			if (!pantSound.isPlaying)
			{
				pantSound.Play();
			}
			if (speedModifier > 1.5f)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			}
			GainCumInstant(gainCumPerThrust / 2f);
		}
		else
		{
			GainCumInstant(gainCumPerThrust);
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		}
		thrusted = true;
	}

	private void CumEvent()
	{
		LoseCumInstant(15f);
		if (HeroineStats.HumanoidBuff)
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg * 10f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg * 10f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		}
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		longCumSound.Play();
		thrusted = true;
	}

	private void InsertEvent()
	{
		if (!insert)
		{
			insertOneSound.Play();
			insert = true;
		}
		else
		{
			insertTwoSound.Play();
			insert = false;
		}
	}
}
