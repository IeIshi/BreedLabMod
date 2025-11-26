using UnityEngine;

public class OctoEventHandler : MonoBehaviour
{
	public AudioSource mouthFuckSound;

	public AudioSource cumPumpSound;

	public GameObject Heroine;

	public void MouthFuckEvent()
	{
		mouthFuckSound.Play();
		Heroine.GetComponent<HeroineStats>().GainLustInstant(5f);
	}

	public void FacePumpEvent()
	{
		Heroine.GetComponent<HeroineStats>().GainLustInstant(15f);
		cumPumpSound.Play();
	}
}
