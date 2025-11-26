using UnityEngine;
using UnityEngine.UI;

public class WaspGallery : Interactable
{
	public enum MyState
	{
		IDLE,
		TEASE,
		DICKEXTEND,
		SEX,
		CUM
	}

	private float pct;

	private float fuckDelay;

	public float fuckTime;

	public float gainCumPerThrust;

	public float pleasureValue;

	public float lustValue;

	private bool camShaked;

	public GameObject mouthWasp;

	public GameObject analWasp;

	public float maxCum;

	private float currentCum;

	private Transform cam;

	private Image circle;

	private Image SexImage;

	public MyState state;

	private GameObject Heroine;

	private Transform targetTease;

	private Transform targetSex;

	private Animator animator;

	private Animator analAnim;

	private Animator oralAnim;

	private Animator heroineAnimator;

	private bool leader;

	private bool oralFucker;

	private bool analFucker;

	private float timer;

	public float speedIncreaseRange;

	private AudioSource buzzSound;

	private AudioSource vagSound;

	private AudioSource teaseSound;

	private AudioSource cumSound;

	private float extendTimer;

	public float joinAnalTimer;

	public float joinOralTimer;

	private bool vagWaspJoined;

	private bool analWaspJoined;

	private bool oralWaspJoined;

	private bool fullSquad;

	public Transform originPosLead;

	public Transform originPosMouth;

	public Transform originPosAnal;

	private void Start()
	{
		Heroine = GameObject.Find("Heroine");
		currentCum = 0f;
		cam = Camera.main.transform;
		SexImage = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse").GetComponent<Image>();
		circle = GameObject.Find("ManagerAndUI/UI/Canvas/Intercourse/circle (1)").GetComponent<Image>();
		targetTease = base.transform.Find("TargetTease");
		targetSex = base.transform.Find("TargetSex");
		buzzSound = base.transform.Find("BuzzSound").GetComponent<AudioSource>();
		vagSound = base.transform.Find("VagSound").GetComponent<AudioSource>();
		teaseSound = base.transform.Find("TeaseSound").GetComponent<AudioSource>();
		cumSound = base.transform.Find("CumSound").GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		heroineAnimator = Heroine.GetComponent<Animator>();
		analAnim = analWasp.GetComponent<Animator>();
		oralAnim = mouthWasp.GetComponent<Animator>();
		animator.SetBool("fly", value: true);
		state = MyState.IDLE;
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
		heroineAnimator.SetBool("waspSex", value: true);
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
		case MyState.DICKEXTEND:
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
		base.transform.position = Heroine.transform.position;
		base.transform.rotation = Heroine.transform.rotation;
		InitiateUI();
		CameraFollow.target = targetTease;
		animator.SetBool("fly", value: false);
		buzzSound.Stop();
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		animator.Play("rig|Wasp_Standgrab");
		heroineAnimator.Play("rig|Wasp_Tease");
		InitiateSex();
	}

	private void Cum()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
		}
		else if (currentCum <= 0f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			heroineAnimator.SetBool("isCumFilled", value: false);
			fullSquad = true;
			oralAnim.SetBool("oralCum", value: false);
			analAnim.SetBool("analCum", value: false);
			analWasp.transform.position = Heroine.transform.position;
			analWasp.transform.rotation = Heroine.transform.rotation;
			mouthWasp.transform.position = Heroine.transform.position;
			mouthWasp.transform.rotation = Heroine.transform.rotation;
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			state = MyState.SEX;
		}
		else
		{
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			animator.speed = 1f;
			heroineAnimator.speed = 1f;
			analAnim.speed = 1f;
			oralAnim.speed = 1f;
			oralAnim.SetBool("oralCum", value: true);
			analAnim.SetBool("analCum", value: true);
			HeroineStats.creampied = true;
			HeroineStats.oralCreampie = true;
			heroineAnimator.SetBool("isCumFilled", value: true);
			heroineAnimator.Play("rig|Wasp_Creampied");
			animator.Play("rig|Wasp_VagCum");
			oralAnim.Play("rig|Wasp_OralCum");
			analAnim.Play("rig|WaspAnalCum");
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
		if (fullSquad)
		{
			animator.Play("rig|Wasp_VagFuck_One");
			heroineAnimator.Play("rig|Wasp_Fucked_Three");
			if (currentCum > speedIncreaseRange)
			{
				animator.speed = 2f;
				analAnim.speed = 2f;
				oralAnim.speed = 2f;
				heroineAnimator.speed = 2f;
			}
			if (currentCum >= 100f)
			{
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				state = MyState.CUM;
			}
			return;
		}
		timer += Time.deltaTime;
		if (timer > joinAnalTimer - 3f)
		{
			analAnim.SetBool("dickExtend", value: true);
		}
		if (timer > joinAnalTimer && !analWaspJoined && heroineAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.1f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			analAnim.Play("rig|WaspAnalFuck");
			analWasp.transform.position = Heroine.transform.position;
			analWasp.transform.rotation = Heroine.transform.rotation;
			animator.SetBool("fly", value: false);
			analWaspJoined = true;
		}
		if (timer > joinOralTimer - 3f)
		{
			oralAnim.SetBool("dickExtend", value: true);
		}
		if (timer > joinOralTimer && !oralWaspJoined && heroineAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.1f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			oralAnim.Play("rig|Wasp_OralFuck");
			heroineAnimator.Play("rig|Wasp_Fucked_Three");
			mouthWasp.transform.position = Heroine.transform.position;
			mouthWasp.transform.rotation = Heroine.transform.rotation;
			oralAnim.SetBool("fly", value: false);
			oralWaspJoined = true;
		}
		if (!vagWaspJoined)
		{
			animator.SetBool("fly", value: false);
			animator.Play("rig|Wasp_VagFuck_One");
			heroineAnimator.Play("rig|Wasp_Fucked_One");
			base.transform.position = Heroine.transform.position;
			base.transform.rotation = Heroine.transform.rotation;
			buzzSound.Stop();
			CameraFollow.target = targetSex;
			vagWaspJoined = true;
		}
		if (currentCum > speedIncreaseRange)
		{
			animator.speed = 2f;
			analAnim.speed = 2f;
			oralAnim.speed = 2f;
			heroineAnimator.speed = 2f;
		}
		if (currentCum >= 100f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
			state = MyState.CUM;
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
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			state = MyState.SEX;
		}
	}

	private void Release()
	{
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = false;
		heroineAnimator.SetBool("isFalledBack", value: true);
		heroineAnimator.SetBool("ImpregInsRub", value: false);
		heroineAnimator.SetBool("waspSex", value: false);
		heroineAnimator.SetBool("isCumFilled", value: false);
		animator.SetBool("oralInject", value: false);
		animator.SetBool("assInject", value: false);
		oralAnim.SetBool("dickExtend", value: false);
		analAnim.SetBool("dickExtend", value: false);
		animator.Play("rig|Wasp_Fly");
		oralAnim.Play("rig|Wasp_Fly");
		analAnim.Play("rig|Wasp_Fly");
		PostProcessingManager.vignette.intensity.value = 0f;
		currentCum = 0f;
		heroineAnimator.speed = 1f;
		animator.speed = 1f;
		oralAnim.speed = 1f;
		analAnim.speed = 1f;
		CameraFollow.target = Heroine.transform;
		fuckDelay = 0f;
		analWaspJoined = false;
		oralWaspJoined = false;
		vagWaspJoined = false;
		fullSquad = false;
		oralAnim.SetBool("oralCum", value: false);
		analAnim.SetBool("analCum", value: false);
		timer = 0f;
		SexImage.enabled = false;
		circle.enabled = false;
		circle.fillAmount = 0f;
		pct = 0f;
		GetComponent<CapsuleCollider>().enabled = true;
		DisableUI();
		base.transform.position = originPosLead.position;
		base.transform.rotation = originPosLead.rotation;
		mouthWasp.transform.position = originPosMouth.position;
		mouthWasp.transform.rotation = originPosMouth.rotation;
		analWasp.transform.position = originPosAnal.position;
		analWasp.transform.rotation = originPosAnal.rotation;
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

	private void VagEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(1f);
		GainCumInstant(gainCumPerThrust);
		vagSound.Play();
	}

	private void TeaseEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(2f);
		teaseSound.Play();
	}

	private void CumEvent()
	{
		cumSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(2f);
		LoseCumInstant(3f);
		if (currentCum <= 0f)
		{
			Release();
		}
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	public void LoseCumInstant(float gainValue)
	{
		currentCum -= gainValue;
		EnemyUI.instance.LoseCum(gainValue);
	}
}
