using UnityEngine;

public class SupHumGallery : Interactable
{
	private Animator anim;

	private Animator heroAnim;

	private GameObject Heroine;

	public GameObject controlPanel;

	public Transform mount_place;

	public Transform camPos;

	public Transform camPosBehind;

	public Transform sexPos;

	public Transform wolfPos;

	private float timer = 4f;

	private float time;

	private bool hardFuck;

	private bool cumming;

	public AudioSource thrust1;

	public AudioSource thrust2;

	public AudioSource cumSound;

	public AudioSource growlSound;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private bool grabbed;

	private bool grawlPlayed;

	private void Start()
	{
		Heroine = GameObject.Find("Heroine");
		anim = GetComponent<Animator>();
		heroAnim = Heroine.GetComponent<Animator>();
	}

	public override void Interact()
	{
		base.Interact();
		grabbed = true;
		heroAnim.SetBool("falled", value: true);
	}

	private void FixedUpdate()
	{
		if (!grabbed)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Alpha1))
		{
			Release();
			grabbed = false;
			return;
		}
		if (!grawlPlayed)
		{
			growlSound.Play();
			grawlPlayed = true;
		}
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		HeroineStats.stunned = true;
		Heroine.GetComponent<PlayerController>().enabled = false;
		growlSound.Play();
		if (!controlPanel.GetComponent<PosControlPanel>().doggy)
		{
			Heroine.transform.rotation = sexPos.rotation;
			Heroine.transform.position = sexPos.position;
			base.transform.rotation = wolfPos.rotation;
			CameraFollow.target = camPos;
		}
		else
		{
			Heroine.transform.rotation = wolfPos.rotation;
			Heroine.transform.position = wolfPos.position;
			base.transform.rotation = wolfPos.rotation;
			CameraFollow.target = camPosBehind;
		}
		time += Time.deltaTime;
		if (time > timer)
		{
			VignetteEffect();
			if (hardFuck)
			{
				anim.speed = 1f;
				heroAnim.speed = 1f;
				if (!controlPanel.GetComponent<PosControlPanel>().doggy)
				{
					anim.SetBool("sexHard", value: true);
					heroAnim.SetBool("matingPressSexSpread", value: true);
				}
				else
				{
					anim.SetBool("sexBehindHard", value: true);
					heroAnim.SetBool("supHumSexHard", value: true);
				}
				heroAnim.SetBool("isAhegao", value: true);
				if (time > 50f)
				{
					HeroineStats.creampied = true;
					heroAnim.SetBool("isCumFilled", value: true);
					if (!cumSound.isPlaying)
					{
						cumSound.Play();
					}
				}
				if (time > 55f)
				{
					cumming = true;
				}
			}
			else
			{
				if (!controlPanel.GetComponent<PosControlPanel>().doggy)
				{
					anim.SetBool("sex", value: true);
					heroAnim.SetBool("matingPressSex", value: true);
				}
				else
				{
					anim.SetBool("sexBehind", value: true);
					heroAnim.SetBool("supHumSex", value: true);
				}
				heroAnim.SetBool("isScared", value: false);
				HeroineStats.aroused = true;
				if (time > 20f)
				{
					heroAnim.SetBool("isScared", value: false);
					anim.speed = 2f;
					heroAnim.speed = 2f;
				}
				if (time > 40f)
				{
					hardFuck = true;
				}
			}
		}
		else
		{
			if (!controlPanel.GetComponent<PosControlPanel>().doggy)
			{
				anim.SetBool("tease", value: true);
				heroAnim.SetBool("matingPressTease", value: true);
			}
			else
			{
				anim.SetBool("teaseBehind", value: true);
				heroAnim.SetBool("supHumTease", value: true);
			}
			heroAnim.SetBool("isScared", value: true);
		}
	}

	private void Release()
	{
		hardFuck = false;
		PlayerController.heIsFuckingHard = false;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentOrg = 0f;
		Heroine.GetComponent<PlayerController>().enabled = true;
		heroAnim.SetBool("matingPressTease", value: false);
		heroAnim.SetBool("matingPressSex", value: false);
		heroAnim.SetBool("matingPressSexSpread", value: false);
		heroAnim.SetBool("isScared", value: false);
		heroAnim.SetBool("isCumFilled", value: false);
		heroAnim.SetBool("supHumTease", value: false);
		heroAnim.SetBool("supHumSex", value: false);
		heroAnim.SetBool("supHumSexHard", value: false);
		anim.SetBool("tease", value: false);
		anim.SetBool("sex", value: false);
		anim.SetBool("sexHard", value: false);
		anim.SetBool("teaseBehind", value: false);
		anim.SetBool("sexBehind", value: false);
		anim.SetBool("sexBehindHard", value: false);
		grawlPlayed = false;
		time = 0f;
		anim.speed = 1f;
		heroAnim.speed = 1f;
		base.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		cumSound.Stop();
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
		thrusted = true;
		if (!cumming)
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(2.5f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(0.3f);
		}
		else
		{
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(8f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(8f);
		}
		if (hardFuck)
		{
			thrust2.Play();
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		else
		{
			thrust1.Play();
		}
	}
}
