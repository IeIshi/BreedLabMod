using UnityEngine;

public class MayuGangEvents : MonoBehaviour
{
	public AudioSource[] slickSoundArray;

	public AudioSource[] plapSoundArray;

	private void ThrustEvent()
	{
		slickSoundArray[Random.Range(0, slickSoundArray.Length)].Play();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(1f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(1f);
	}

	private void PlapEvent()
	{
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(0.2f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(1f);
		plapSoundArray[Random.Range(0, plapSoundArray.Length)].Play();
	}
}
