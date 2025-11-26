using System.Collections;
using UnityEngine;

public class OpenConDoor : Interactable
{
	private Animator animator;

	public AudioSource openDoor;

	public float closeAfterSeconds = 1f;

	private bool open;

	public GameObject animHolder;

	private void Start()
	{
		if (animHolder != null)
		{
			animator = animHolder.GetComponent<Animator>();
		}
		else
		{
			animator = GetComponent<Animator>();
		}
		GetComponent<BoxCollider>().isTrigger = false;
	}

	public override void Interact()
	{
		base.Interact();
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			animHolder.GetComponent<BoxCollider>().isTrigger = true;
			GetComponent<BoxCollider>().isTrigger = true;
			if (openDoor != null)
			{
				openDoor.Play();
			}
			open = true;
			StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
		}
		else
		{
			animator.SetBool("isOpen", value: false);
			animHolder.GetComponent<BoxCollider>().isTrigger = true;
			GetComponent<BoxCollider>().isTrigger = true;
			if (openDoor != null)
			{
				openDoor.Play();
			}
			open = false;
			StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
		}
	}

	private IEnumerator doSomethingAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		GetComponent<BoxCollider>().isTrigger = false;
		animHolder.GetComponent<BoxCollider>().isTrigger = false;
	}
}
