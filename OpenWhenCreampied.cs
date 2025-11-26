using UnityEngine;

public class OpenWhenCreampied : MonoBehaviour
{
	public Material[] material;

	private Renderer rend;

	private Animator animator;

	public AudioSource enterDarkness;

	public AudioSource openDoor;

	private bool openedOnce;

	private bool played;

	private void Start()
	{
		openedOnce = false;
		played = false;
		animator = GetComponent<Animator>();
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		if (HeroineStats.pregnant)
		{
			rend.sharedMaterial = material[1];
		}
		else
		{
			rend.sharedMaterial = material[0];
		}
	}

	private void Update()
	{
		if (HeroineStats.pregnant)
		{
			rend.sharedMaterial = material[1];
		}
		else
		{
			rend.sharedMaterial = material[0];
		}
		if (openedOnce && !enterDarkness.isPlaying && !played)
		{
			enterDarkness.Play();
			played = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (HeroineStats.pregnant)
		{
			animator.SetBool("isOpen", value: true);
			openDoor.Play();
			openedOnce = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (HeroineStats.pregnant)
		{
			animator.SetBool("isOpen", value: false);
			openDoor.Play();
		}
	}
}
