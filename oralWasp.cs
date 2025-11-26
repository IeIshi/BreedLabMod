using UnityEngine;

public class oralWasp : MonoBehaviour
{
	public GameObject Leader;

	public GameObject Heroine;

	private AudioSource oralSound;

	private AudioSource cumSound;

	private void Start()
	{
		oralSound = base.transform.Find("OralSound").GetComponent<AudioSource>();
		cumSound = base.transform.Find("CumSound").GetComponent<AudioSource>();
	}

	private void OralEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(1f);
		oralSound.Play();
	}

	private void CumEvent()
	{
		cumSound.Play();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(2f);
		Leader.GetComponent<WaspGallery>().LoseCumInstant(3f);
	}
}
