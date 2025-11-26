using System.Collections;
using UnityEngine;

public class OpenPoweredDoors : Interactable
{
	public Dialogue dialogue;

	public float closeAfterSeconds = 3f;

	public AudioSource openDoor;

	public bool doorClosed;

	private bool open;

	private Animator animator;

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
	}

	public override void Interact()
	{
		base.Interact();
		if (doorClosed)
		{
			TriggerDialoge();
		}
		else if (!open)
		{
			animator.SetBool("isOpen", value: true);
			GetComponent<BoxCollider>().enabled = false;
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
		if (openDoor != null)
		{
			openDoor.Play();
		}
		GetComponent<BoxCollider>().enabled = true;
		open = false;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
