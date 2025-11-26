using UnityEngine;

public class PhantomHeroineGallery : Interactable
{
	public GameObject Heroine;

	public AudioSource teaseSound;

	public AudioSource sexSoundOne;

	public AudioSource sexSoundTwo;

	public AudioSource cumSound;

	private Animator animator;

	private Animator heroineAnim;

	public Transform sexPos;

	public Transform camPos;

	private bool grabbed;

	private bool released;

	private bool activateDAHAIR;

	private float timer;

	private bool resetAnimSpeed;

	private bool hardFuck;

	private bool scissor;

	private void Start()
	{
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (grabbed)
		{
			if (Input.GetKey(KeyCode.Alpha1))
			{
				Release();
				grabbed = false;
			}
			else
			{
				Sex();
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		GetComponent<CapsuleCollider>().enabled = false;
		PlayerController.iGetFucked = true;
		PlayerController.iFalled = true;
		grabbed = true;
	}

	private void Sex()
	{
		timer += Time.deltaTime;
		if (timer < 30f)
		{
			Grab();
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
			PlayerController.iGetFucked = true;
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
		scissor = true;
		CameraFollow.target = camPos;
	}

	private void Release()
	{
		if (!scissor)
		{
			PlayerController.iFalled = false;
		}
		else
		{
			heroineAnim.SetBool("falled", value: true);
		}
		PlayerController.iGetFucked = false;
		animator.speed = 1f;
		heroineAnim.speed = 1f;
		GetComponent<CapsuleCollider>().enabled = true;
		animator.SetBool("phanScissor", value: false);
		animator.SetBool("isIdle", value: true);
		heroineAnim.SetBool("scissor", value: false);
		heroineAnim.SetBool("isPhanGrabbed", value: false);
		teaseSound.Stop();
		teaseSound.pitch = 1f;
		hardFuck = false;
		timer = 0f;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		PlayerController.iGetInserted = false;
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		heroineAnim.SetBool("isScared", value: false);
		grabbed = false;
		scissor = false;
	}

	private void TakeSexPos()
	{
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
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
