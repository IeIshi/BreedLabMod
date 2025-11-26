using UnityEngine;

public class PWalkerGallery : Interactable
{
	public enum MyState
	{
		IDLE,
		SIT,
		LURING,
		MISSSEX,
		BLOWJOB,
		REVERSERAPED
	}

	public bool sitter;

	public bool stander;

	private Animator animator;

	private Animator heroineAnim;

	public MyState state;

	public float thrustDmg;

	public float lustDmg;

	public float gainCumPerThrust;

	private float maxCum = 100f;

	private float currentCum;

	private float timer;

	private bool thrusted;

	private bool max;

	private bool cumming;

	private bool passion;

	private bool adjustPos;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public ParticleSystem cumParticle;

	public GameObject Heroine;

	public Transform HeroineMount;

	public Transform BjPos;

	public Transform MastPos;

	public Transform LyingPos;

	public Transform CamBj;

	public Transform CamCow;

	public Transform CamMission;

	public AudioSource blowJobSound1;

	public AudioSource blowJobSound2;

	public AudioSource cumSound;

	public AudioSource sexSound1;

	public AudioSource sexSound2;

	public AudioSource sexSound3;

	public AudioSource collideSound;

	public AudioSource wetSound;

	private CamShaker shake;

	private void Start()
	{
		maxCum = 100f;
		currentCum = 0f;
		shake = Object.FindObjectOfType<CamShaker>();
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		if (stander)
		{
			state = MyState.IDLE;
		}
		if (sitter)
		{
			state = MyState.SIT;
		}
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
		heroineAnim.SetBool("falled", value: true);
		if (state == MyState.IDLE)
		{
			state = MyState.BLOWJOB;
		}
		if (state == MyState.SIT)
		{
			state = MyState.LURING;
		}
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.IDLE:
			Idle();
			break;
		case MyState.SIT:
			Sit();
			break;
		case MyState.LURING:
			Luring();
			break;
		case MyState.MISSSEX:
			MissiSex();
			break;
		case MyState.BLOWJOB:
			Blowjob();
			break;
		case MyState.REVERSERAPED:
			ReverseRape();
			break;
		}
	}

	private void Luring()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			if (stander)
			{
				state = MyState.IDLE;
			}
			if (sitter)
			{
				state = MyState.SIT;
			}
			return;
		}
		heroineAnim.SetBool("luring", value: true);
		Heroine.transform.position = BjPos.transform.position;
		Heroine.transform.rotation = BjPos.transform.rotation;
		timer += Time.deltaTime;
		if (timer > 3f)
		{
			timer = 0f;
			state = MyState.MISSSEX;
		}
	}

	private void Idle()
	{
		animator.Play("rig|Idle");
	}

	private void Sit()
	{
		animator.Play("rig|Sit");
	}

	private void MissiSex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			if (stander)
			{
				state = MyState.IDLE;
			}
			if (sitter)
			{
				state = MyState.SIT;
			}
			return;
		}
		InitiateUI();
		VignetteEffect();
		base.gameObject.transform.position = HeroineMount.position;
		base.gameObject.transform.rotation = HeroineMount.rotation;
		CameraFollow.target = CamMission;
		if (currentCum >= 100f)
		{
			cumming = true;
		}
		if (cumming)
		{
			HeroineStats.creampied = true;
			animator.speed = 1f;
			heroineAnim.speed = 1f;
			if (currentCum <= 0f)
			{
				animator.SetBool("missCum", value: false);
				heroineAnim.SetBool("missCum", value: false);
				timer = 0f;
				cumSound.Stop();
				cumming = false;
			}
			else
			{
				animator.SetBool("missPass", value: false);
				heroineAnim.SetBool("missPass", value: false);
				animator.SetBool("missCum", value: true);
				heroineAnim.SetBool("missCum", value: true);
				DrainCum(1f);
			}
			return;
		}
		if (currentCum > 60f)
		{
			animator.speed = 1.5f;
			heroineAnim.speed = 1.5f;
		}
		if (timer > 15f)
		{
			animator.SetBool("missPass", value: true);
			heroineAnim.SetBool("missPass", value: true);
			passion = true;
			if (!wetSound.isPlaying)
			{
				wetSound.Play();
			}
		}
		animator.SetBool("missInsert", value: true);
		heroineAnim.SetBool("missInsert", value: true);
		timer += Time.deltaTime;
	}

	private void Blowjob()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			if (stander)
			{
				state = MyState.IDLE;
			}
			if (sitter)
			{
				state = MyState.SIT;
			}
			return;
		}
		InitiateUI();
		VignetteEffect();
		Heroine.transform.position = BjPos.transform.position;
		Heroine.transform.rotation = BjPos.transform.rotation;
		CameraFollow.target = CamBj;
		if (cumming && currentCum <= 0f)
		{
			if (timer > 3f)
			{
				animator.SetBool("lyingBack", value: true);
				animator.SetBool("addictOralCum", value: false);
				heroineAnim.SetBool("addictOralCum", value: false);
				Heroine.transform.position = MastPos.position;
				Heroine.transform.rotation = MastPos.rotation;
				state = MyState.REVERSERAPED;
				animator.speed = 1f;
				heroineAnim.speed = 1f;
				cumParticle.Stop();
				timer = 0f;
				cumming = false;
			}
			else
			{
				timer += Time.deltaTime;
				animator.speed = 0.1f;
				heroineAnim.speed = 0.1f;
				cumParticle.Play();
			}
		}
		else
		{
			if (cumming)
			{
				animator.SetBool("addictOralCum", value: true);
				heroineAnim.SetBool("addictOralCum", value: true);
				HeroineStats.oralCreampie = true;
				animator.speed = 2f;
				heroineAnim.speed = 2f;
				DrainCum(1f);
			}
			else if (currentCum > 40f)
			{
				animator.speed = 1.5f;
				heroineAnim.speed = 1.5f;
			}
			animator.SetBool("addictOral", value: true);
			heroineAnim.SetBool("addictOral", value: true);
		}
	}

	private void ReverseRape()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			if (stander)
			{
				state = MyState.IDLE;
			}
			if (sitter)
			{
				state = MyState.SIT;
			}
			return;
		}
		InitiateUI();
		VignetteEffect();
		CameraFollow.target = CamCow;
		if (!cumming)
		{
			cumSound.Stop();
		}
		if (timer > 5f)
		{
			if (!adjustPos)
			{
				Heroine.transform.position = BjPos.position;
				Heroine.transform.rotation = BjPos.rotation;
				adjustPos = true;
			}
			base.gameObject.transform.position = HeroineMount.position;
			base.gameObject.transform.rotation = HeroineMount.rotation;
			animator.SetBool("addictCowgirl", value: true);
			heroineAnim.SetBool("addictCowgirl", value: true);
			if (cumming)
			{
				if (currentCum <= 0f)
				{
					if (!cumSound.isPlaying)
					{
						cumSound.Play();
					}
					animator.speed = 0.3f;
					heroineAnim.speed = 0.3f;
					cumParticle.Play();
					timer += Time.deltaTime;
					if (timer > 9f)
					{
						Release();
					}
					return;
				}
			}
			else if (currentCum > 40f)
			{
				animator.speed = 1.5f;
				heroineAnim.speed = 1.5f;
			}
			if (currentCum >= 100f)
			{
				animator.SetBool("addictCowCum", value: true);
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
				}
				HeroineStats.creampied = true;
				animator.speed = 2f;
				heroineAnim.speed = 2f;
				cumming = true;
			}
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	private void Release()
	{
		cumming = false;
		if (state == MyState.REVERSERAPED || state == MyState.BLOWJOB)
		{
			heroineAnim.SetBool("falled", value: false);
			PlayerController.iFalled = false;
			Heroine.transform.position = BjPos.position;
			Heroine.transform.rotation = BjPos.rotation;
		}
		heroineAnim.SetBool("luring", value: false);
		heroineAnim.SetBool("addictCowgirl", value: false);
		heroineAnim.SetBool("addictOralCum", value: false);
		heroineAnim.SetBool("addictOral", value: false);
		heroineAnim.SetBool("missInsert", value: false);
		heroineAnim.SetBool("missPass", value: false);
		animator.SetBool("addictCowgirl", value: false);
		animator.SetBool("addictCowCum", value: false);
		animator.SetBool("addictOralCum", value: false);
		animator.SetBool("addictOral", value: false);
		animator.SetBool("missInsert", value: false);
		animator.SetBool("missPass", value: false);
		animator.SetBool("missCum", value: false);
		heroineAnim.SetBool("missCum", value: false);
		GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
		cumSound.Stop();
		cumParticle.Stop();
		wetSound.Stop();
		DisableUI();
		PlayerController.iGetFucked = false;
		GetComponent<CapsuleCollider>().enabled = true;
		CameraFollow.target = Heroine.transform;
		animator.speed = 1f;
		heroineAnim.speed = 1f;
		currentCum = 0f;
		adjustPos = false;
		timer = 0f;
		if (passion)
		{
			Heroine.transform.position = LyingPos.position;
			Heroine.transform.rotation = LyingPos.rotation;
			passion = false;
		}
		if (stander)
		{
			state = MyState.IDLE;
		}
		if (sitter)
		{
			state = MyState.SIT;
		}
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
	}

	private void DrainCumInstant(float drainValue)
	{
		currentCum -= drainValue;
		EnemyUI.instance.LoseCum(drainValue);
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
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

	public void ThrustEvent()
	{
		thrusted = true;
		if (state == MyState.BLOWJOB)
		{
			if ((float)Random.Range(0, 2) == 1f)
			{
				blowJobSound1.Play();
			}
			else
			{
				blowJobSound2.Play();
			}
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 5f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
			GainCumInstant(gainCumPerThrust);
		}
		if (state == MyState.MISSSEX)
		{
			if (passion)
			{
				sexSound3.Play();
				Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
				Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
				GainCumInstant(gainCumPerThrust);
			}
			else
			{
				sexSound2.Play();
				Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 3f);
				Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
				GainCumInstant(gainCumPerThrust / 3f);
			}
		}
		if (state == MyState.REVERSERAPED)
		{
			sexSound1.Play();
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
			GainCumInstant(gainCumPerThrust);
		}
		if (currentCum >= maxCum)
		{
			cumming = true;
		}
	}

	public void CumEvent()
	{
		shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
		if (state == MyState.BLOWJOB)
		{
			if ((float)Random.Range(0, 2) == 1f)
			{
				blowJobSound1.Play();
			}
			else
			{
				blowJobSound2.Play();
			}
		}
		if (state == MyState.MISSSEX)
		{
			sexSound2.Play();
		}
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		DrainCumInstant(10f);
	}
}
