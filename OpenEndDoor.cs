using System.Collections;
using UnityEngine;

public class OpenEndDoor : Interactable
{
	private Animator animator;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	public GameObject skulptur1;

	public GameObject skulptur2;

	public GameObject skulptur3;

	public Dialogue dialogue;

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
			if (skulptur1.GetComponent<BodestSchaltNeo>().right && skulptur2.GetComponent<BodestSchaltNeo>().right && skulptur3.GetComponent<BodestSchaltNeo>().right)
			{
				animator.SetBool("isOpen", value: true);
				GetComponent<BoxCollider>().isTrigger = true;
				open = true;
				StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
			}
			else
			{
				TriggerDialoge();
			}
		}
	}

	private IEnumerator doSomethingAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		animator.SetBool("isOpen", value: false);
		GetComponent<BoxCollider>().isTrigger = false;
		open = false;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
