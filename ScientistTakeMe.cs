using System.Collections;
using UnityEngine;

public class ScientistTakeMe : MonoBehaviour
{
	public GameObject heroine;

	public Transform mountPos;

	private Animator animator;

	public AudioSource sexSound;

	public bool sex;

	private bool cum;

	private bool pantieCheck;

	private float startTimer;

	public float sexDuration = 10f;

	private void Start()
	{
		pantieCheck = false;
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (sex)
		{
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
			HeroineStats.aroused = true;
			PlayerManager.IsVirgin = false;
			if (!pantieCheck)
			{
				if (EquipmentManager.instance.currentEquipment[3] != null)
				{
					EquipmentManager.instance.RipPantsu();
				}
				pantieCheck = true;
			}
			heroine.GetComponent<PlayerController>().enabled = false;
			heroine.GetComponent<HeroineStats>().GainLust(1f);
			heroine.transform.rotation = Quaternion.Lerp(heroine.transform.rotation, mountPos.transform.rotation, 0.2f);
			heroine.transform.position = Vector3.Lerp(heroine.transform.position, mountPos.transform.position, 0.2f);
			if (!cum)
			{
				StartCoroutine(GrabToSex());
			}
			else
			{
				StartCoroutine(CumToRelease());
			}
		}
		else
		{
			heroine.GetComponent<Animator>().SetBool("ScBehindCum", value: false);
			heroine.GetComponent<Animator>().SetBool("behindGrinded", value: false);
			heroine.GetComponent<Animator>().SetBool("ScBehindFuck", value: false);
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: false);
			PlayerController.iFalled = false;
			PlayerController.iGetFucked = false;
			HeroineStats.stunned = false;
			HeroineStats.aroused = false;
			PlayerController.iGetFucked = false;
			heroine.GetComponent<PlayerController>().enabled = true;
			GetComponent<ScientistTakeMe>().enabled = false;
			base.gameObject.SetActive(value: false);
			Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator GrabToSex()
	{
		animator.SetBool("isGrinding", value: true);
		heroine.GetComponent<Animator>().SetBool("behindGrinded", value: true);
		yield return new WaitForSeconds(2f);
		animator.SetBool("isBehindFucking", value: true);
		heroine.GetComponent<Animator>().SetBool("ScBehindFuck", value: true);
		startTimer += Time.deltaTime * 1f;
		if (startTimer >= sexDuration)
		{
			cum = true;
		}
	}

	private IEnumerator CumToRelease()
	{
		animator.SetBool("isCummingBehind", value: true);
		heroine.GetComponent<Animator>().SetBool("ScBehindCum", value: true);
		heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
		heroine.GetComponent<HeroineStats>().GainOrg(5f);
		HeroineStats.creampied = true;
		HeroineStats.fertileCum = false;
		HeroineStats.pregnant = false;
		HeroineStats.lustyCum = true;
		yield return new WaitForSeconds(5f);
		sex = false;
	}

	private void ThrustEvent()
	{
		if (sex)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(2f);
			heroine.GetComponent<HeroineStats>().GainExp(5f);
		}
		sexSound.Play();
	}
}
