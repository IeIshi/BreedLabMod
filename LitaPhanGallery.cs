using UnityEngine;

public class LitaPhanGallery : Interactable
{
	public enum MyState
	{
		IDLE,
		GRAB,
		KISS
	}

	public MyState state;

	public GameObject Heroine;

	public Transform sexPos;

	public Transform camPlace;

	public Transform camPlace1;

	public Transform backPos;

	private Animator animator;

	private Animator heroineAnim;

	private float timer;

	public float giveInTimer;

	public float lyingKissTimer;

	public AudioSource KissSound1;

	public AudioSource KissSound2;

	public AudioSource KissSound3;

	private void Start()
	{
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
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
		heroineAnim.SetBool("kissed", value: true);
		Heroine.transform.rotation = sexPos.rotation;
		Heroine.transform.position = sexPos.position;
		state = MyState.GRAB;
	}

	private void StateMachine(MyState state)
	{
		switch (state)
		{
		case MyState.IDLE:
			Idle();
			break;
		case MyState.GRAB:
			Grab();
			break;
		case MyState.KISS:
			Kiss();
			break;
		}
	}

	private void Idle()
	{
		animator.Play("rig|Idle");
	}

	private void Grab()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		Heroine.GetComponent<HeroineStats>().GainLust(3.5f);
		timer += Time.deltaTime;
		CameraFollow.target = camPlace1;
		if (timer > giveInTimer)
		{
			PlayerController.heIsFuckingHard = true;
			HeroineStats.aroused = true;
			heroineAnim.speed = 2f;
			animator.speed = 2f;
			heroineAnim.SetBool("giveIn", value: true);
			if (timer > lyingKissTimer)
			{
				state = MyState.KISS;
			}
		}
		else
		{
			animator.Play("rig|Kiss");
			heroineAnim.Play("rig|Lita_Kiss");
		}
	}

	private void Kiss()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			state = MyState.IDLE;
			return;
		}
		heroineAnim.SetBool("kissLying", value: true);
		heroineAnim.SetBool("falled", value: true);
		animator.SetBool("kissLying", value: true);
		CameraFollow.target = camPlace;
	}

	private void Release()
	{
		if (state == MyState.GRAB)
		{
			PlayerController.iFalled = false;
		}
		heroineAnim.SetBool("kissed", value: false);
		heroineAnim.SetBool("kissLying", value: false);
		heroineAnim.SetBool("giveIn", value: false);
		animator.SetBool("kissLying", value: false);
		GetComponent<CapsuleCollider>().enabled = true;
		heroineAnim.speed = 1f;
		animator.speed = 1f;
		Heroine.transform.position = backPos.transform.position;
		timer = 0f;
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		PlayerController.iGetFucked = false;
		state = MyState.IDLE;
	}

	public void KissEvent1()
	{
		float num = Random.Range(1, 4);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(2f);
		if (num == 1f)
		{
			KissSound1.Play();
		}
		if (num == 2f)
		{
			KissSound2.Play();
		}
		if (num == 3f)
		{
			KissSound3.Play();
		}
	}
}
