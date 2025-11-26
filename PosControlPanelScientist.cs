using UnityEngine;

public class PosControlPanelScientist : Interactable
{
	public Material[] material;

	private Renderer rend;

	public GameObject screen;

	public AudioSource onSound;

	public bool blowjob;

	public bool cowgirl;

	public bool lifted;

	private void Start()
	{
		rend = screen.GetComponent<Renderer>();
		blowjob = true;
		rend.sharedMaterial = material[0];
	}

	public override void Interact()
	{
		base.Interact();
		onSound.Play();
		if (blowjob)
		{
			blowjob = false;
			cowgirl = true;
			rend.sharedMaterial = material[1];
		}
		else if (cowgirl)
		{
			lifted = true;
			cowgirl = false;
			rend.sharedMaterial = material[2];
		}
		else if (lifted)
		{
			lifted = false;
			blowjob = true;
			rend.sharedMaterial = material[0];
		}
	}
}
