using System.Collections;
using UnityEngine;

public class SecuritySchalterDoor : Interactable
{
	private Animator animator;

	public Transform schalter;

	public Dialogue dialogue;

	public float closeAfterSeconds = 3f;

	public AudioSource openDoor;

	public bool open;

	public GameObject animHolder;

	public bool dontClose;

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
		if (schalter.GetComponent<SecuritySchalter>().isOn)
		{
			if (!open)
			{
				animator.SetBool("isOpen", value: true);
				openDoor.Play();
				open = true;
				if (!dontClose)
				{
					StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
				}
			}
		}
		else
		{
			TriggerDialoge();
		}
	}

	private IEnumerator doSomethingAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		animator.SetBool("isOpen", value: false);
		openDoor.Play();
		open = false;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
