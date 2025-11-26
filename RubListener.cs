using UnityEngine;

public class RubListener : MonoBehaviour
{
	public AudioSource[] rubs;

	public AudioSource[] cums;

	public void RubEvent()
	{
		rubs[Random.Range(0, rubs.Length)].Play();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(1f);
	}

	public void CumEvent()
	{
		cums[Random.Range(0, cums.Length)].Play();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(5f);
	}
}
