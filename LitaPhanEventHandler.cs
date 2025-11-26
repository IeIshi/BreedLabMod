using UnityEngine;

public class LitaPhanEventHandler : MonoBehaviour
{
	public AudioSource KissSound1;

	public AudioSource KissSound2;

	public AudioSource KissSound3;

	private void KissEvent1()
	{
		float num = Random.Range(1, 4);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(1f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(5f);
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
