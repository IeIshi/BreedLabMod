using UnityEngine;

public class PosControlPanel : Interactable
{
	public Material[] material;

	private Renderer rend;

	public GameObject screen;

	public AudioSource onSound;

	public bool doggy;

	private void Start()
	{
		rend = screen.GetComponent<Renderer>();
		if (doggy)
		{
			rend.sharedMaterial = material[1];
		}
		else
		{
			rend.sharedMaterial = material[0];
		}
	}

	public override void Interact()
	{
		base.Interact();
		onSound.Play();
		if (!doggy)
		{
			doggy = true;
			rend.sharedMaterial = material[1];
		}
		else
		{
			doggy = false;
			rend.sharedMaterial = material[0];
		}
	}
}
