using UnityEngine;

public class LewdPhantom : MonoBehaviour
{
	public float maxStamina = 10f;

	private float currentStamina;

	public static float powerRecieved;

	private bool gotAttacked;

	public GameObject Heroine;

	public GameObject Hair;

	public GameObject Hugger;

	public AudioSource teaseSound;

	public AudioSource sexSoundOne;

	public AudioSource sexSoundTwo;

	public AudioSource cumSound;

	private Animator animator;

	private Animator heroineAnim;

	private Animator huggerAnim;

	private Transform anim_pos;

	public Transform mount_place;

	public Transform camPos;

	public Transform hugger_mount_pos;

	private bool grabbed;

	private bool released;

	private bool activateDAHAIR;

	private float timer;

	private float timerTwo;

	private bool resetAnimSpeed;

	private bool finishAction;

	private HeroineStats herStats;

	private bool hardFuck;

	private void Start()
	{
		currentStamina = maxStamina;
		herStats = Heroine.GetComponent<HeroineStats>();
		Hugger.SetActive(value: false);
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		huggerAnim = Hugger.GetComponent<Animator>();
		anim_pos = base.gameObject.transform;
	}

	private void Update()
	{
		if (!GetComponent<PhantomAfterSexDialogue>().interacted)
		{
			return;
		}
		if (DialogManager.inDialogue)
		{
			FaceTarget();
			return;
		}
		if (!activateDAHAIR)
		{
			Hair.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
			activateDAHAIR = true;
		}
		if (GetComponent<PhantomAfterSexDialogue>().run)
		{
			return;
		}
		FaceTarget();
		if (currentStamina <= 0f)
		{
			Release();
			DisableUI();
			GetComponent<PhantomAfterSexDialogue>().struggled = true;
			GetComponent<PhantomAfterSexDialogue>().secondInteract = true;
			return;
		}
		if (timer < 75f)
		{
			InitiateUI();
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			Sex();
			return;
		}
		SwitchPartner();
		DisableUI();
		timerTwo += Time.deltaTime;
		if (!finishAction)
		{
			if (timerTwo > 5f)
			{
				animator.SetBool("masturbate", value: true);
			}
			if (timerTwo > 8f && timerTwo < 15f)
			{
				huggerAnim.SetBool("sex", value: true);
				heroineAnim.SetBool("isFucking", value: true);
			}
			if (timerTwo > 15f)
			{
				heroineAnim.speed = 1f;
				huggerAnim.SetBool("isCumming", value: true);
				heroineAnim.SetBool("isInserting", value: true);
				heroineAnim.SetBool("isFucking", value: false);
				HeroineStats.creampied = true;
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
				}
			}
			if (timerTwo > 20f)
			{
				Object.Destroy(Hugger);
				cumSound.Stop();
				heroineAnim.SetBool("isInserting", value: false);
				heroineAnim.SetBool("isFucking", value: false);
				heroineAnim.SetBool("falled", value: true);
				animator.SetBool("masturbate", value: false);
				PlayerController.iGetInserted = false;
				PlayerController.iGetFucked = false;
				heroineAnim.speed = 1f;
				GetComponent<PhantomAfterSexDialogue>().secondInteract = true;
				GetComponent<PhantomAfterSexDialogue>().struggled = false;
				finishAction = true;
			}
		}
		else
		{
			animator.SetBool("masturbate", value: false);
			heroineAnim.speed = 1f;
			heroineAnim.SetBool("isScared", value: false);
			PlayerController.heIsFuckingHard = false;
			HeroineStats.aroused = false;
			CameraFollow.target = Heroine.transform;
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		}
	}

	private void Sex()
	{
		timer += Time.deltaTime;
		if (!EquipmentManager.heroineIsNaked)
		{
			Heroine.GetComponent<HeroineStats>().LosePantiesDurability(20f);
		}
		if (timer < 30f)
		{
			Grab();
			PlayerController.iGetFucked = true;
			if (timer > 15f)
			{
				animator.speed = 1.5f;
				heroineAnim.speed = 1.5f;
				heroineAnim.SetBool("isScared", value: false);
				HeroineStats.aroused = true;
			}
			else
			{
				heroineAnim.SetBool("isScared", value: true);
			}
			return;
		}
		if (!resetAnimSpeed)
		{
			animator.speed = 1f;
			heroineAnim.speed = 1f;
			resetAnimSpeed = true;
		}
		if (timer > 45f && timer < 60f)
		{
			animator.speed = 1.5f;
			heroineAnim.speed = 1.5f;
			teaseSound.pitch = 1.5f;
			PlayerController.heIsFuckingHard = true;
		}
		if (timer > 60f)
		{
			teaseSound.pitch = 2f;
			animator.speed = 2f;
			heroineAnim.speed = 2f;
			hardFuck = true;
		}
		Scissor();
	}

	private void Grab()
	{
		TakeSexPos();
		animator.Play("rig|PhanGrabbing");
		animator.SetBool("isIdle", value: false);
		heroineAnim.Play("rig|GrabbedByPhan");
		heroineAnim.SetBool("isPhanGrabbed", value: true);
		if (!grabbed)
		{
			PlayerController.iFalled = true;
			PlayerController.iGetInserted = true;
			grabbed = true;
		}
		Heroine.GetComponent<HeroineStats>().GainOrg(1.5f);
		Heroine.GetComponent<HeroineStats>().GainLust(1f);
		if (!teaseSound.isPlaying)
		{
			teaseSound.Play();
		}
	}

	private void Scissor()
	{
		TakeSexPos();
		animator.SetBool("phanScissor", value: true);
		heroineAnim.SetBool("scissor", value: true);
		Heroine.GetComponent<HeroineStats>().GainOrg(2f);
		Heroine.GetComponent<HeroineStats>().GainLust(1f);
		CameraFollow.target = camPos;
	}

	private void Release()
	{
		if (!released)
		{
			PlayerController.iGetFucked = false;
			animator.speed = 1f;
			heroineAnim.speed = 1f;
			GetComponent<BoxCollider>().isTrigger = false;
			animator.SetBool("phanScissor", value: false);
			animator.SetBool("isIdle", value: true);
			heroineAnim.SetBool("scissor", value: false);
			heroineAnim.SetBool("isPhanGrabbed", value: false);
			teaseSound.Stop();
			teaseSound.pitch = 1f;
			PlayerController.iFalled = false;
			PlayerController.iGetInserted = false;
			PlayerController.heIsFuckingHard = false;
			HeroineStats.aroused = false;
			heroineAnim.SetBool("isScared", value: false);
			GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
			Heroine.GetComponent<HeroineStats>().mySexPartner = null;
			float num = Random.Range(0.3f, 0.7f);
			anim_pos.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
			released = true;
		}
	}

	private void SwitchPartner()
	{
		if (!released)
		{
			GetComponent<BoxCollider>().isTrigger = false;
			animator.speed = 1f;
			heroineAnim.speed = 1.5f;
			animator.SetBool("phanScissor", value: false);
			animator.SetBool("isIdle", value: true);
			Hugger.SetActive(value: true);
			heroineAnim.SetBool("isInserting", value: true);
			heroineAnim.SetBool("scissor", value: false);
			heroineAnim.SetBool("isPhanGrabbed", value: false);
			teaseSound.Stop();
			teaseSound.pitch = 1f;
			float num = Random.Range(0.3f, 0.7f);
			anim_pos.position = new Vector3(Heroine.transform.position.x - num, Heroine.transform.position.y, Heroine.transform.position.z);
			CameraFollow.target = hugger_mount_pos;
			released = true;
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void TakeSexPos()
	{
		anim_pos.rotation = mount_place.rotation;
		anim_pos.position = mount_place.position;
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.health = currentStamina;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void VagThrust()
	{
		if (hardFuck)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		}
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		if (Random.Range(1, 3) == 1)
		{
			sexSoundOne.Play();
		}
		else
		{
			sexSoundTwo.Play();
		}
	}
}
