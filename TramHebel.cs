using System.Collections;
using UnityEngine;

public class TramHebel : Interactable
{
	public Dialogue dialogue;

	public GameObject TramDoor;

	public GameObject TriggerHebel;

	public GameObject Terminal;

	public AudioSource openHebelSound;

	public AudioSource openTramSound;

	private Animator hebelAnim;

	private Animator tramAnim;

	private bool opened;

	private void Start()
	{
		hebelAnim = TriggerHebel.GetComponent<Animator>();
		if (TramDoor != null)
		{
			tramAnim = TramDoor.GetComponent<Animator>();
		}
		if (Terminal.GetComponent<SecuritySchalter>().isOn)
		{
			hebelAnim.SetBool("isOpen", value: true);
			tramAnim.SetBool("isOpen", value: true);
		}
	}

	public override void Interact()
	{
		if (!opened)
		{
			base.Interact();
			if (!Terminal.GetComponent<SecuritySchalter>().isOn)
			{
				TriggerDialoge();
				return;
			}
			hebelAnim.SetBool("isOpen", value: true);
			openHebelSound.Play();
			StartCoroutine(activateGate());
			opened = true;
			base.gameObject.layer = 0;
		}
	}

	private IEnumerator activateGate()
	{
		yield return new WaitForSeconds(1f);
		tramAnim.SetBool("isOpen", value: true);
		openTramSound.Play();
		yield return new WaitForSeconds(1.5f);
		openTramSound.Stop();
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
