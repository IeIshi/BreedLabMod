using UnityEngine;

public class DoorAlwaysOpen : MonoBehaviour
{
	private Animator animator;

	public Transform schalter;

	public static bool doorOpen;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (schalter.GetComponent<Schalter>().isOn)
		{
			animator.SetBool("isOpen", value: true);
			doorOpen = true;
		}
		else
		{
			animator.SetBool("isOpen", value: false);
			doorOpen = false;
		}
	}
}
