using UnityEngine;

public class DoorTriggerSchalt : MonoBehaviour
{
	private Animator animator;

	public Transform schalter;

	public AudioSource openDoor;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && schalter.GetComponent<PasswordKeyGet>().opened)
		{
			animator.SetBool("isOpen", value: true);
			openDoor.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && schalter.GetComponent<PasswordKeyGet>().opened)
		{
			animator.SetBool("isOpen", value: false);
			openDoor.Play();
		}
	}
}
