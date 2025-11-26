using UnityEngine;

public class BigDoorTriggerShalt : MonoBehaviour
{
	public Material[] material;

	private Renderer rend;

	private Animator animator;

	public AudioSource openDoor;

	public Transform schalter;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rend = GetComponent<Renderer>();
		rend.enabled = true;
	}

	private void Update()
	{
		if (schalter.GetComponent<Schalter>().isOn)
		{
			rend.sharedMaterial = material[1];
		}
		else
		{
			rend.sharedMaterial = material[0];
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && schalter.GetComponent<Schalter>().isOn)
		{
			animator.SetBool("isOpen", value: true);
			openDoor.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (schalter.GetComponent<Schalter>().isOn)
		{
			animator.SetBool("isOpen", value: false);
			openDoor.Play();
		}
	}
}
