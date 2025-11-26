using UnityEngine;
using UnityEngine.UI;

public class OctoGallery : Interactable
{
	public enum MyState
	{
		IDLE,
		TEASE,
		TEASEVAG,
		SEX,
		DOUBLESEX,
		CUM
	}

	public AudioSource cumPumpSound;

	public AudioSource slideSound;

	public AudioSource slideSoundTwo;

	public AudioSource mouthFuckSound;

	public AudioSource vagTeaseSound;

	public AudioSource dickExtendSound;

	public AudioSource creatureAwareSound;

	public MyState state;

	public GameObject Heroine;

	public GameObject SecondOcto;

	public Transform sexPos;

	public Transform mouthPos;

	public Transform camPos;

	public Transform originPos;

	private Animator anim;

	private Animator heroineAnim;

	private Animator secondOctoAnim;

	public float maxStamina;

	private float currentStamina;

	public float maxCum;

	private float currentCum;

	public float gainCumPerThrust;

	private bool thrusted;

	private bool max;

	private bool camShaked;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public Image circle;

	public Image SexImage;

	private float fuckDelay;

	public float fuckTime;

	private float pct;

	public float teaseDmg;

	public float thrustDmg;

	private void Start()
	{
		Heroine = GameObject.Find("Heroine");
		state = MyState.IDLE;
		anim = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		secondOctoAnim = SecondOcto.GetComponent<Animator>();
		currentStamina = maxStamina;
		currentCum = 0f;
	}

	private void FixedUpdate()
	{
		StateMachine(state);
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<SphereCollider>().enabled = false;
		PlayerController.iGetFucked = true;
		PlayerController.iFalled = true;
		CameraFollow.target = camPos;
		heroineAnim.SetBool("falled", value: true);
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
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
		case MyState.TEASEVAG:
			TeaseVag();
			break;
		case MyState.SEX:
			Sex();
			break;
		case MyState.DOUBLESEX:
			DoubleSex();
			break;
		case MyState.CUM:
			Cum();
			break;
		}
	}

	private void Idle()
	{
	}

	private void Tease()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		anim.SetBool("isGrabbing", value: true);
		heroineAnim.SetBool("octoGrabbed", value: true);
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		InitiateUI();
		if (isPlaying(anim, "rig|OctoTease"))
		{
			heroineAnim.SetBool("octoTease", value: true);
			state = MyState.TEASEVAG;
		}
	}

	private void TeaseVag()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
		}
		else
		{
			InitiateUI();
			InitiateSex();
		}
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
		anim.SetBool("isVagFucking", value: true);
		heroineAnim.SetBool("octoSex", value: true);
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		if (currentCum > 30f)
		{
			state = MyState.DOUBLESEX;
		}
	}

	private void DoubleSex()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		InitiateUI();
		VignetteEffect();
		if (!camShaked)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			camShaked = true;
		}
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		heroineAnim.Play("rig|OctoDoubleSex");
		heroineAnim.SetBool("octoDoubleSex", value: true);
		secondOctoAnim.Play("rig|OctoFaceHug");
		anim.Play("rig|OctoSexFront");
		SecondOcto.transform.position = mouthPos.transform.position;
		SecondOcto.transform.rotation = mouthPos.transform.rotation;
		if (currentCum >= maxCum)
		{
			state = MyState.CUM;
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
		VignetteEffect();
		heroineAnim.SetBool("octoDoubleCum", value: true);
		secondOctoAnim.SetBool("cumFace", value: true);
		anim.SetBool("isVagCumming", value: true);
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		HeroineStats.creampied = true;
		HeroineStats.oralCreampie = true;
		if (currentCum <= 0f)
		{
			heroineAnim.SetBool("octoDoubleCum", value: false);
			secondOctoAnim.SetBool("cumFace", value: false);
			anim.SetBool("isVagCumming", value: false);
			currentCum = 0f;
			SecondOcto.transform.position = originPos.transform.position;
			SecondOcto.transform.rotation = originPos.transform.rotation;
			state = MyState.SEX;
		}
	}

	private void Release()
	{
		heroineAnim.SetBool("octoGrabbed", value: false);
		heroineAnim.SetBool("octoTease", value: false);
		heroineAnim.SetBool("octoDoubleCum", value: false);
		heroineAnim.SetBool("octoSex", value: false);
		heroineAnim.SetBool("octoDoubleSex", value: false);
		secondOctoAnim.SetBool("cumFace", value: false);
		anim.SetBool("isVagCumming", value: false);
		anim.SetBool("isGrabbing", value: false);
		anim.SetBool("isVagFucking", value: false);
		secondOctoAnim.Play("rig|Idle_Oct");
		SecondOcto.transform.position = originPos.transform.position;
		SecondOcto.transform.rotation = originPos.transform.rotation;
		GetComponent<SphereCollider>().enabled = true;
		DisableUI();
		currentCum = 0f;
		SexImage.enabled = false;
		circle.enabled = false;
		circle.fillAmount = 0f;
		pct = 0f;
		fuckDelay = 0f;
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		PlayerController.iGetFucked = false;
		state = MyState.IDLE;
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
			state = MyState.SEX;
		}
	}

	private bool isPlaying(Animator anim, string stateName)
	{
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return true;
		}
		return false;
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

	private void LoseCumInstant(float gainValue)
	{
		currentCum -= gainValue;
		EnemyUI.instance.LoseCum(gainValue);
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
	}

	public void TeaseEvent()
	{
		vagTeaseSound.Play();
	}

	public void PumpEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(10f);
		LoseCumInstant(10f);
		cumPumpSound.Play();
		slideSoundTwo.Play();
		thrusted = true;
		if (HeroineStats.MantisBuff)
		{
			GetComponent<EnemyFieldOfView>().drainHealth(2.5f);
		}
		if (currentCum <= 0f)
		{
			Release();
		}
	}

	public void ThrustEvent()
	{
		GainCumInstant(gainCumPerThrust);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(teaseDmg);
		thrusted = true;
		slideSound.Play();
	}
}
