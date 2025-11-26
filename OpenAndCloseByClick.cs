using UnityEngine;

public class OpenAndCloseByClick : Interactable
{
	private Animator animator;

	public AudioSource openDoor;

	public AudioSource closeDoor;

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
	}

	public override void Interact()
	{
		base.Interact();
		if (!open)
		{
			animator.SetBool("isOpen", value: true);
			if (openDoor != null)
			{
				openDoor.Play();
			}
			base.gameObject.layer = 0;
			open = true;
		}
	}
}
