using System.Collections;
using UnityEngine;

public class OpenDoorClickEngine : Interactable
{
	private Animator animator;

	public AudioSource openDoor;

	public float closeAfterSeconds = 3f;

	private bool open;

	public GameObject animHolder;

	public Dialogue dialogue;

	public GameObject enemyspawn;

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
		if (EnergyEngine.engineIsOn)
		{
			if (!open)
			{
				animator.SetBool("isOpen", value: true);
				GetComponent<BoxCollider>().isTrigger = true;
				if (enemyspawn != null)
				{
					enemyspawn.SetActive(value: true);
				}
				openDoor.Play();
				open = true;
			}
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
		openDoor.Play();
		open = false;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
