using System.Collections;
using UnityEngine;

public class LickerFuckMe : MonoBehaviour
{
	public GameObject heroine;

	public GameObject hospDoor;

	public GameObject oldTv;

	public GameObject progControl;

	public GameObject wolf;

	public GameObject scientist;

	public Transform mountPos;

	public Transform camPos;

	private Animator animator;

	public AudioSource sexSound;

	private float waitForLick;

	public bool sex;

	private bool licksex;

	private bool grabbing;

	private bool lick;

	private float startValue;

	private float endValue = 0.3f;

	private float transitionTime = 10f;

	private float t;

	private bool pantieCheck;

	private bool pantiesAreOn;

	private bool pantiesLickSex;

	private void Start()
	{
		animator = GetComponent<Animator>();
		pantieCheck = false;
	}

	private void Update()
	{
		if (sex)
		{
			CameraFollow.target = camPos;
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
			HeroineStats.aroused = true;
			if (!pantieCheck)
			{
				if (EquipmentManager.instance.currentEquipment[3] != null)
				{
					pantiesAreOn = true;
				}
				pantieCheck = true;
			}
			heroine.GetComponent<HeroineStats>().GainLust(1f);
			heroine.transform.rotation = Quaternion.Lerp(heroine.transform.rotation, mountPos.transform.rotation, 0.2f);
			heroine.transform.position = Vector3.Lerp(heroine.transform.position, mountPos.transform.position, 0.2f);
			oldTv.GetComponent<TriggerFog>().enabled = false;
			Object.Destroy(scientist);
			Object.Destroy(wolf);
			t += Time.deltaTime / transitionTime;
			RenderSettings.fogDensity = Mathf.SmoothStep(endValue, startValue, t);
			if (pantiesLickSex && !HeroineStats.orgasm)
			{
				animator.speed = 2f;
				heroine.GetComponent<Animator>().speed = 2f;
				heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
				heroine.GetComponent<HeroineStats>().GainOrg(10f);
				return;
			}
			if (pantiesLickSex && HeroineStats.orgasm)
			{
				animator.speed = 1f;
				heroine.GetComponent<Animator>().speed = 1f;
				heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
				heroine.GetComponent<Animator>().SetBool("LickLickFuckOrg", value: true);
				animator.SetBool("isLickOrg", value: true);
				sexSound.Stop();
				sex = false;
			}
			if (!HeroineStats.orgasm && !licksex)
			{
				if (!lick)
				{
					StartCoroutine(IdleToGrab());
					sexSound.Stop();
				}
				else
				{
					StartCoroutine(LickToLickSex());
					if (!sexSound.isPlaying)
					{
						sexSound.Play();
					}
				}
			}
			else if (HeroineStats.orgasm && licksex)
			{
				animator.SetBool("isLickOrg", value: true);
				heroine.GetComponent<Animator>().SetBool("LickLickFuckOrg", value: true);
				sexSound.Stop();
				sex = false;
			}
			if (grabbing)
			{
				waitForLick += Time.deltaTime;
				if (waitForLick >= 5f)
				{
					lick = true;
					grabbing = false;
				}
			}
		}
		else
		{
			StartCoroutine(Release());
		}
	}

	private IEnumerator IdleToGrab()
	{
		animator.SetBool("isIdle", value: true);
		heroine.GetComponent<Animator>().SetBool("falled", value: true);
		yield return new WaitForSeconds(5f);
		heroine.GetComponent<Animator>().SetBool("LickBeforeLicking", value: true);
		animator.SetBool("isGrab", value: true);
		grabbing = true;
	}

	private IEnumerator LickToLickSex()
	{
		animator.SetBool("isLicking", value: true);
		heroine.GetComponent<Animator>().SetBool("LickLicking", value: true);
		yield return new WaitForSeconds(5f);
		HeroineStats.creampied = false;
		HeroineStats.lustyCum = false;
		HeroineStats.addictiveCum = false;
		HeroineStats.fertileCum = false;
		Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMesh);
		if (!pantiesAreOn)
		{
			licksex = true;
			animator.SetBool("isLickFucking", value: true);
			PlayerManager.IsVirgin = false;
			heroine.GetComponent<Animator>().SetBool("LickLickFucking", value: true);
		}
		else
		{
			pantiesLickSex = true;
		}
	}

	private IEnumerator Release()
	{
		yield return new WaitForSeconds(2f);
		heroine.GetComponent<Animator>().SetBool("LickBeforeLicking", value: false);
		heroine.GetComponent<Animator>().SetBool("LickLicking", value: false);
		heroine.GetComponent<Animator>().SetBool("LickLickFucking", value: false);
		heroine.GetComponent<Animator>().SetBool("LickRelease", value: true);
		heroine.GetComponent<Animator>().SetBool("falled", value: true);
		heroine.GetComponent<Animator>().SetBool("LickLickFuckOrg", value: false);
		animator.speed = 1f;
		CameraFollow.target = heroine.transform;
		PlayerController.iGetFucked = false;
		HeroineStats.stunned = false;
		HeroineStats.aroused = false;
		animator.SetBool("isLickOrg", value: false);
		animator.SetBool("isGrab", value: false);
		heroine.GetComponent<PlayerController>().enabled = true;
		GetComponent<LickerFuckMe>().enabled = false;
		StartCoroutine(Vanish());
	}

	private IEnumerator Vanish()
	{
		yield return new WaitForSeconds(2f);
		heroine.GetComponent<Animator>().SetBool("LickRelease", value: false);
		heroine.GetComponent<Animator>().SetBool("isAhegao", value: false);
		heroine.GetComponent<Animator>().speed = 1f;
		heroine.GetComponent<PlayerController>().enabled = true;
		GetComponent<LickerFuckMe>().enabled = false;
		hospDoor.GetComponent<Animator>().SetBool("isOpen", value: true);
		progControl.GetComponent<ChallageRoomProgControl>().gangBangDone = true;
		Object.Destroy(base.gameObject);
	}

	private void LickEvent()
	{
		if (sex)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(4.5f);
		}
		sexSound.Play();
	}
}
