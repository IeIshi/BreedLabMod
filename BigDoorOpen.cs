using System.Collections;
using UnityEngine;

public class BigDoorOpen : Interactable
{
	private Animator animator;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	public AudioSource doorSound;

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
	}

	public override void Interact()
	{
		base.Interact();
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			if (doorSound != null)
			{
				doorSound.Play();
			}
			open = true;
			StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
		}
		IEnumerator doSomethingAfterTime(float time)
		{
			yield return new WaitForSeconds(time);
			animator.SetBool("isOpen", value: false);
			if (doorSound != null)
			{
				doorSound.Play();
			}
			open = false;
		}
	}
}
