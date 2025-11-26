using System.Collections;
using UnityEngine;

public class LitaPhanDefender : MonoBehaviour
{
	private Transform mountPos;

	private bool kissing;

	private float timer;

	public AudioSource KissSound1;

	public AudioSource KissSound2;

	public AudioSource KissSound3;

	public AudioSource PortSound;

	private void Start()
	{
		PortSound.Play();
		mountPos = GameObject.Find("Heroine/MantisMountFront").transform;
		PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		StartCoroutine(KissAttack());
		timer = 0f;
	}

	private void FixedUpdate()
	{
		if (!kissing)
		{
			FaceTarget();
			return;
		}
		base.gameObject.transform.position = mountPos.position;
		base.gameObject.transform.rotation = mountPos.rotation;
		timer += Time.deltaTime;
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLust(10f);
		if ((double)timer > 17.8 && !PortSound.isPlaying)
		{
			PortSound.Play();
		}
		if (timer > 18f)
		{
			PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = null;
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("kissGiveIn", value: false);
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
			PlayerManager.instance.player.GetComponent<Gun>().litaPhanSpawned = false;
			PlayerManager.instance.player.GetComponent<Animator>().speed = 1f;
			base.gameObject.GetComponent<Animator>().speed = 1f;
			PlayerController.heIsFuckingHard = false;
			HeroineStats.stunned = false;
			PlayerController.iGetFucked = false;
			Object.Destroy(base.gameObject);
		}
		else
		{
			if (timer > 5f)
			{
				PlayerController.heIsFuckingHard = true;
			}
			if (timer > 10f)
			{
				PlayerManager.instance.player.GetComponent<Animator>().speed = 2f;
				base.gameObject.GetComponent<Animator>().speed = 2f;
				PlayerManager.instance.player.GetComponent<Animator>().SetBool("kissGiveIn", value: true);
			}
		}
	}

	private IEnumerator KissAttack()
	{
		yield return new WaitForSeconds(1f);
		PlayerController.iFalled = true;
		HeroineStats.stunned = true;
		PlayerController.iGetFucked = true;
		kissing = true;
		base.gameObject.transform.position = mountPos.position;
		base.gameObject.transform.rotation = mountPos.rotation;
		PlayerManager.instance.player.GetComponent<Animator>().Play("rig|Lita_Kiss");
		base.gameObject.GetComponent<Animator>().Play("rig|Kiss");
		PlayerManager.instance.player.GetComponent<Animator>().SetBool("falled", value: true);
	}

	private void FaceTarget()
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - base.gameObject.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.gameObject.transform.rotation = Quaternion.Slerp(base.gameObject.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void KissEvent1()
	{
		float num = Random.Range(1, 4);
		Debug.Log(num);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(2f);
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
