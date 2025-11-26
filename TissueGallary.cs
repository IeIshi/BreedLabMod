using UnityEngine;

public class TissueGallary : Interactable
{
	public AudioSource wipeSound;

	public override void Interact()
	{
		base.Interact();
		wipeSound.Play();
		HeroineStats.creampied = false;
		HeroineStats.oralCreampie = false;
		HeroineStats.fertileCum = false;
		HeroineStats.lustyCum = false;
		HeroineStats.hugeAmount = false;
		HeroineStats.addictiveCum = false;
		PlayerManager.instance.player.GetComponent<Animator>().SetBool("isCumFilled", value: false);
		PlayerManager.instance.player.GetComponent<HeroineStats>().cumStrain.Stop();
		Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMesh);
		PlayerManager.instance.player.GetComponent<HeroineStats>().showCumSpot = false;
		Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMouth);
		PlayerManager.instance.player.GetComponent<HeroineStats>().showCumMouth = false;
	}
}
