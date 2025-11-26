using System.Collections;
using UnityEngine;

public class OpenEndDoorTwo : Interactable
{
	private Animator animator;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	public GameObject schalter1;

	public GameObject schalter2;

	public GameObject schalter3;

	public Dialogue dialogue;

	public GameObject scientist;

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
		scientist.SetActive(value: false);
	}

	public override void Interact()
	{
		base.Interact();
		if (PlayerManager.ScSexAfterMath || open)
		{
			return;
		}
		if (schalter1.GetComponent<SchalterRiddle>().isOn && schalter2.GetComponent<SchalterRiddle>().isOn && schalter3.GetComponent<SchalterRiddle>().isOn)
		{
			animator.SetBool("isOpen", value: true);
			open = true;
			if (!PlayerManager.ScSexAfterMath)
			{
				scientist.SetActive(value: true);
			}
			GetComponent<BoxCollider>().isTrigger = true;
			StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
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
		GetComponent<BoxCollider>().isTrigger = false;
		open = false;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
