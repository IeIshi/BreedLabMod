using UnityEngine;

public class ThrustListener : MonoBehaviour
{
	public AudioSource[] plaps;

	public AudioSource[] slides;

	public bool IsScientist;

	public void ThrustEvent()
	{
		plaps[Random.Range(0, plaps.Length)].Play();
		slides[Random.Range(0, plaps.Length)].Play();
		if (IsScientist)
		{
			PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(2f);
		}
	}
}
