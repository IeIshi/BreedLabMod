using UnityEngine;

public class PlantControlPanel : Interactable
{
	public Material[] material;

	private Renderer rend;

	public GameObject screen;

	public AudioSource onSound;

	public bool cocumba;

	public bool coral;

	public bool egg;

	public bool bag;

	private void Start()
	{
		rend = screen.GetComponent<Renderer>();
		cocumba = true;
		rend.sharedMaterial = material[0];
	}

	public override void Interact()
	{
		base.Interact();
		onSound.Play();
		if (cocumba)
		{
			rend.sharedMaterial = material[1];
			coral = true;
			cocumba = false;
		}
		else if (coral)
		{
			rend.sharedMaterial = material[2];
			egg = true;
			coral = false;
		}
		else if (egg)
		{
			rend.sharedMaterial = material[3];
			bag = true;
			egg = false;
		}
		else if (bag)
		{
			rend.sharedMaterial = material[0];
			cocumba = true;
			bag = false;
		}
	}
}
