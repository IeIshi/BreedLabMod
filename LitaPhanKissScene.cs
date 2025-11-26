using UnityEngine;

public class LitaPhanKissScene : Interactable
{
	public GameObject Heroine;

	public Transform mountPos;

	public Transform heroinePos;

	public Transform camPlace;

	private Animator anim;

	private Animator heroAnim;

	private bool kissed;

	private float timer;

	private float startKissTimer;

	public AudioSource AmbientMusic;

	public AudioSource KissSound1;

	public AudioSource KissSound2;

	public AudioSource KissSound3;

	private bool positionHer;

	private void Start()
	{
		anim = GetComponent<Animator>();
		heroAnim = Heroine.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (base.gameObject.activeSelf)
		{
			startKissTimer += Time.deltaTime;
			if (startKissTimer > 10f && !kissed)
			{
				CameraFollow.shootingMode = false;
				HeroineStats.masturbating = true;
				HeroineStats.stunned = true;
				base.gameObject.layer = 0;
				Heroine.transform.position = base.transform.position;
				Heroine.transform.rotation = heroinePos.rotation;
				Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
				base.transform.rotation = mountPos.rotation;
				base.transform.position = mountPos.position;
				PlayerController.iFalled = true;
				PlayerController.iGetFucked = true;
				heroAnim.Play("rig|Lita_Kiss");
				anim.Play("rig|Kiss");
				kissed = true;
			}
		}
		if (kissed)
		{
			if (!AmbientMusic.isPlaying)
			{
				AmbientMusic.Play();
			}
			if (!positionHer)
			{
				Heroine.transform.position = base.transform.position;
				Heroine.transform.rotation = heroinePos.rotation;
				base.transform.rotation = mountPos.rotation;
				base.transform.position = mountPos.position;
				positionHer = true;
			}
			Heroine.GetComponent<HeroineStats>().GainLust(3.5f);
			if (timer > 5f)
			{
				CameraFollow.target = camPlace;
				heroAnim.SetBool("kissLying", value: true);
				anim.SetBool("kissLying", value: true);
				Heroine.GetComponent<HeroineStats>().GainLust(3.5f);
				Heroine.GetComponent<HeroineStats>().GainOrg(2.5f);
			}
			else if (HeroineStats.currentLust >= 75f)
			{
				heroAnim.SetBool("kissGiveIn", value: true);
				PlayerController.heIsFuckingHard = true;
				timer += Time.deltaTime;
				heroAnim.speed = 2f;
				anim.speed = 2f;
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		base.gameObject.layer = 0;
		Heroine.transform.position = base.transform.position;
		Heroine.transform.rotation = heroinePos.rotation;
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		base.transform.rotation = mountPos.rotation;
		base.transform.position = mountPos.position;
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		HeroineStats.masturbating = true;
		HeroineStats.stunned = true;
		kissed = true;
		heroAnim.Play("rig|Lita_Kiss");
		anim.Play("rig|Kiss");
	}

	public void KissEvent1()
	{
		float num = Random.Range(1, 4);
		Debug.Log(num);
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
