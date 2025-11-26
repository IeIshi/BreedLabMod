using UnityEngine;

public class ScJustFucking : MonoBehaviour
{
	public GameObject heroine;

	public Transform mountPos;

	public Transform camPos;

	private Animator animator;

	public GameObject dialogTrigger;

	private bool sex;

	public AudioSource sexSound;

	public AudioSource sexHardSound;

	public AudioSource cumSound;

	private bool cumming;

	private bool pantieCheck;

	private float sexTimer;

	private bool p1;

	private void Start()
	{
		animator = GetComponent<Animator>();
		sex = true;
	}

	private void Update()
	{
		if (!sex)
		{
			return;
		}
		heroine.GetComponent<PlayerController>().enabled = false;
		heroine.GetComponent<HeroineStats>().GainLust(2f);
		heroine.transform.rotation = Quaternion.Lerp(heroine.transform.rotation, mountPos.transform.rotation, 0.2f);
		heroine.transform.position = Vector3.Lerp(heroine.transform.position, mountPos.transform.position, 0.2f);
		CameraFollow.target = camPos;
		PlayerManager.IsVirgin = false;
		if (!pantieCheck)
		{
			if (EquipmentManager.instance.currentEquipment[3] != null)
			{
				EquipmentManager.instance.RipPantsu();
			}
			pantieCheck = true;
		}
		SexRoutine();
	}

	private void SexRoutine()
	{
		sexTimer += Time.deltaTime;
		if (sexTimer < 10f)
		{
			p1 = true;
			PlayerController.iFalled = true;
			animator.SetBool("HerIsInserting", value: true);
			heroine.GetComponent<Animator>().SetBool("ScDickInsert", value: true);
			HeroineStats.aroused = true;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
		}
		if (sexTimer >= 10f && sexTimer < 15f)
		{
			p1 = false;
			animator.SetBool("getFuckedHard", value: true);
			animator.SetBool("HerIsInserting", value: false);
			heroine.GetComponent<Animator>().SetBool("dickRideHard", value: true);
			heroine.GetComponent<Animator>().SetBool("ScDickInsert", value: false);
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
		}
		if (sexTimer >= 15f && sexTimer < 40f)
		{
			animator.speed = 2f;
			heroine.GetComponent<Animator>().speed = 2f;
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
		}
		if (sexTimer >= 30f && sexTimer <= 40f)
		{
			animator.speed = 1f;
			heroine.GetComponent<Animator>().speed = 1f;
			animator.SetBool("isCummingRide", value: true);
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
			if (!cumming)
			{
				cumSound.Play();
				cumming = true;
			}
			heroine.GetComponent<Animator>().SetBool("ScRideCum", value: true);
			heroine.GetComponent<Animator>().SetBool("dickRideHard", value: false);
			animator.SetBool("getFuckedHard", value: false);
			HeroineStats.pregnant = false;
			HeroineStats.creampied = true;
			HeroineStats.lustyCum = true;
			HeroineStats.fertileCum = false;
		}
		if (sexTimer >= 40f)
		{
			animator.SetBool("isCummingRide", value: false);
			animator.SetBool("HerIsInserting", value: false);
			animator.SetBool("getFuckedHard", value: false);
			cumSound.Stop();
			heroine.GetComponent<Animator>().SetBool("ScDickInsert", value: false);
			heroine.GetComponent<Animator>().SetBool("ScRideCum", value: false);
			heroine.GetComponent<Animator>().SetBool("dickRideHard", value: false);
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: false);
			heroine.GetComponent<PlayerController>().enabled = true;
			PlayerController.iFalled = false;
			HeroineStats.aroused = false;
			PlayerController.iGetFucked = false;
			HeroineStats.stunned = false;
			GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
			GetComponent<ScJustFucking>().enabled = false;
			CameraFollow.target = heroine.transform;
			if (sex)
			{
				dialogTrigger.SetActive(value: true);
			}
			sex = false;
			GetComponent<ScJustFucking>().enabled = false;
		}
	}

	private void ThrustEvent()
	{
		if (p1)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(2f);
			heroine.GetComponent<HeroineStats>().GainExp(5f);
			sexSound.Play();
		}
		else
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(3f);
			heroine.GetComponent<HeroineStats>().GainExp(5f);
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			sexHardSound.Play();
		}
	}
}
