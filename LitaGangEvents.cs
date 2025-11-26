using UnityEngine;

public class LitaGangEvents : MonoBehaviour
{
	public AudioSource[] suckSoundArray;

	private void SuckEvent()
	{
		suckSoundArray[Random.Range(0, suckSoundArray.Length)].Play();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(0.5f);
	}
}
