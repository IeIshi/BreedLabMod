using System.Collections;
using UnityEngine;

public class OpenTrapDoor : Interactable
{
	private Animator animator;

	public AudioSource openDoor;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	public GameObject Scientist;

	public GameObject heroine;

	public bool locked;

	public GameObject smoke;

	public AudioSource gasSound;

	private void Start()
	{
		smoke.SetActive(value: false);
		if (animHolder != null)
		{
			animator = animHolder.GetComponent<Animator>();
		}
		else
		{
			animator = GetComponent<Animator>();
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!Scientist.GetComponent<ScDialogues>().nonVirginDialogueTriggered)
		{
			if (!open)
			{
				animator.SetBool("isOpen", value: true);
				if (openDoor != null)
				{
					openDoor.Play();
				}
				open = true;
				StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
			}
		}
		else if (!InsideOrOutside.inside)
		{
			if (!open)
			{
				animator.SetBool("isOpen", value: true);
				if (openDoor != null)
				{
					openDoor.Play();
				}
				open = true;
				StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
			}
		}
		else
		{
			smoke.SetActive(value: true);
			if (!gasSound.isPlaying)
			{
				gasSound.Play();
			}
		}
	}

	private IEnumerator doSomethingAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		animator.SetBool("isOpen", value: false);
		if (openDoor != null)
		{
			openDoor.Play();
		}
		open = false;
	}
}
