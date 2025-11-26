using System.Collections;
using UnityEngine;

public class FutaBlowjober : MonoBehaviour
{
	private GameObject Heroine;

	private GameObject mountTargetFront;

	private GameObject mountTargetBehind;

	private GameObject preBlowTargetFront;

	public bool startFucking;

	private Animator anim;

	private bool setPosition;

	private bool startStroking;

	private AudioSource port;

	private AudioSource blowjobSound;

	public float pleasureValue;

	public float lustValue;

	private void Start()
	{
		port = base.transform.GetChild(0).GetComponent<AudioSource>();
		blowjobSound = base.transform.GetChild(6).GetComponent<AudioSource>();
		port.Play();
		Heroine = GameObject.Find("Heroine");
		mountTargetFront = GameObject.Find("Heroine/FutaMount");
		mountTargetBehind = GameObject.Find("Heroine/FutaMountBehind");
		preBlowTargetFront = GameObject.Find("Heroine/FutaPreBlowjobFront");
		anim = GetComponent<Animator>();
		StartCoroutine(StrokeYourself());
	}

	private void Update()
	{
		if (!startFucking)
		{
			if (!startStroking)
			{
				FaceTarget();
			}
		}
		else if (!setPosition)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
			port.Play();
			if (PlayerController.iFalledFront)
			{
				base.transform.position = mountTargetFront.transform.position;
				base.transform.eulerAngles = new Vector3(Heroine.transform.eulerAngles.x, Heroine.transform.eulerAngles.y, 0f - Heroine.transform.eulerAngles.z);
				anim.Play("rig|Futa_FrontBlowjob");
			}
			if (PlayerController.iFalledBack)
			{
				base.transform.position = mountTargetBehind.transform.position;
				base.transform.eulerAngles = mountTargetBehind.transform.eulerAngles;
				anim.Play("rig|Futa_Behind_Blowjob");
			}
			setPosition = true;
		}
	}

	private IEnumerator StrokeYourself()
	{
		yield return new WaitForSeconds(3f);
		startStroking = true;
		port.Play();
		if (PlayerController.iFalledBack)
		{
			base.transform.position = mountTargetBehind.transform.position;
			base.transform.eulerAngles = mountTargetBehind.transform.eulerAngles;
			anim.Play("rig|Futa_Behind_PreBlowjob");
		}
		if (PlayerController.iFalledFront)
		{
			base.transform.position = preBlowTargetFront.transform.position;
			base.transform.eulerAngles = preBlowTargetFront.transform.eulerAngles;
			anim.Play("rig|Futa_Behind_PreBlowjob");
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void ThrustEvent()
	{
		blowjobSound.Play();
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustValue);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(pleasureValue);
	}
}
