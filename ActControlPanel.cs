using UnityEngine;

public class ActControlPanel : Interactable
{
	public Material[] material;

	private Renderer rend;

	public GameObject screen;

	public AudioSource onSound;

	public bool act_1;

	public bool act_2;

	public bool act_3;

	public bool act_4;

	public GameObject Act1;

	public GameObject Act2;

	public GameObject Act3;

	public GameObject Act4;

	private void Start()
	{
		rend = screen.GetComponent<Renderer>();
		act_1 = true;
		rend.sharedMaterial = material[0];
		Act1Active();
	}

	public override void Interact()
	{
		base.Interact();
		onSound.Play();
		if (act_1)
		{
			act_1 = false;
			act_2 = true;
			act_3 = false;
			act_4 = false;
			rend.sharedMaterial = material[1];
			Act2Active();
		}
		else if (act_2)
		{
			act_1 = false;
			act_2 = false;
			act_3 = true;
			act_4 = false;
			rend.sharedMaterial = material[2];
			Act3Active();
		}
		else if (act_3)
		{
			act_1 = false;
			act_2 = false;
			act_3 = false;
			act_4 = true;
			rend.sharedMaterial = material[3];
			Act4Active();
		}
		else if (act_4)
		{
			act_1 = true;
			act_2 = false;
			act_3 = false;
			act_4 = false;
			rend.sharedMaterial = material[0];
			Act1Active();
		}
	}

	private void Act1Active()
	{
		Act1.SetActive(value: true);
		Act2.SetActive(value: false);
		Act3.SetActive(value: false);
		Act4.SetActive(value: false);
	}

	private void Act2Active()
	{
		Act1.SetActive(value: false);
		Act2.SetActive(value: true);
		Act3.SetActive(value: false);
		Act4.SetActive(value: false);
	}

	private void Act3Active()
	{
		Act1.SetActive(value: false);
		Act2.SetActive(value: false);
		Act3.SetActive(value: true);
		Act4.SetActive(value: false);
	}

	private void Act4Active()
	{
		Act1.SetActive(value: false);
		Act2.SetActive(value: false);
		Act3.SetActive(value: false);
		Act4.SetActive(value: true);
	}
}
