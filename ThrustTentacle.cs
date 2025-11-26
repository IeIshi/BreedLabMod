using UnityEngine;

public class ThrustTentacle : MonoBehaviour
{
	public GameObject heroine;

	public AudioSource sexSound;

	public float thrustDmg = 1.5f;

	public float fustFuckDmg = 3f;

	public float expPerThrust = 2f;

	public void ThrustEvent()
	{
		if (!AbortBed.fastFuck)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
		}
		else
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			heroine.GetComponent<HeroineStats>().GainOrgInstant(fustFuckDmg);
		}
		heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
		sexSound.Play();
		heroine.GetComponent<HeroineStats>().GainLustInstant(20f);
	}
}
