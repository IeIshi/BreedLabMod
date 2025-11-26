using UnityEngine;

public class ObedientHound : MonoBehaviour
{
	private Animator anim;

	private bool exit;

	private float timer;

	private float timerT = 3f;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (exit)
		{
			timer += Time.deltaTime;
			if (timer > timerT)
			{
				anim.SetBool("bow", value: false);
				timer = 0f;
				exit = false;
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			anim.SetBool("bow", value: true);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			exit = true;
		}
	}
}
