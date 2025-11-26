using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	private Animator animator;

	public AudioSource openDoor;

	private bool open;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!open && (other.tag == "Player" || other.tag == "LickerEnemy"))
		{
			animator.SetBool("isOpen", value: true);
			if (openDoor != null)
			{
				openDoor.Play();
			}
			open = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (open)
		{
			StartCoroutine(waitSeconds());
		}
	}

	private IEnumerator waitSeconds()
	{
		yield return new WaitForSeconds(2f);
		animator.SetBool("isOpen", value: false);
		if (openDoor != null)
		{
			openDoor.Play();
		}
		open = false;
	}
}
