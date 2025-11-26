using System.Collections;
using UnityEngine;

public class ClassicShalterDoor : Interactable
{
	private Animator animator;

	public Transform schalter;

	public Dialogue dialogue;

	public float closeAfterSeconds = 3f;

	public AudioSource openDoor;

	private bool open;

	public GameObject animHolder;

	public GameObject lampMesh;

	private Renderer rendLamp;

	public Material[] material;

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
		if (rendLamp != null)
		{
			rendLamp = lampMesh.GetComponent<Renderer>();
			rendLamp.enabled = true;
			if (!schalter.GetComponent<Schalter>().isOn)
			{
				rendLamp.sharedMaterial = material[0];
			}
			else
			{
				rendLamp.sharedMaterial = material[1];
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (schalter.GetComponent<Schalter>().isOn)
		{
			if (!open)
			{
				if (rendLamp != null)
				{
					rendLamp.sharedMaterial = material[1];
				}
				animator.SetBool("isOpen", value: true);
				openDoor.Play();
				open = true;
				StartCoroutine(doSomethingAfterTime(closeAfterSeconds));
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
