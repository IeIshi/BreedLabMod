using System.Collections;
using UnityEngine;

public class OpenDoorbyClick : Interactable
{
	private Animator animator;

	public AudioSource openDoor;

	public float closeAfterSeconds = 3f;

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
		Debug.Log("registered");
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			GetComponent<BoxCollider>().isTrigger = true;
			if (openDoor != null)
			{
				openDoor.Play();
			}
			open = true;
			StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
		}
	}

	private IEnumerator doSomethingAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		animator.SetBool("isOpen", value: false);
		GetComponent<BoxCollider>().isTrigger = false;
		if (openDoor != null)
		{
			openDoor.Play();
		}
		open = false;
	}
}
