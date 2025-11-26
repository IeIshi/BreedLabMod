using UnityEngine;

public class PhantomHugger : MonoBehaviour
{
	public AudioSource thrustSound;

	public GameObject Heroine;

	public void ThrustEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainLustInstant(2f);
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
		thrustSound.Play();
	}
}
