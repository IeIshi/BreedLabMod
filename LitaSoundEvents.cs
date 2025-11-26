using UnityEngine;

public class LitaSoundEvents : MonoBehaviour
{
	public AudioSource blowSound1;

	public AudioSource blowSound2;

	public AudioSource handSound;

	public AudioSource swallowSound;

	private void BlowEvent()
	{
		if ((float)Random.Range(0, 2) == 1f)
		{
			blowSound1.Play();
		}
		else
		{
			blowSound2.Play();
		}
	}

	private void HandEvent()
	{
		handSound.Play();
	}

	private void SwallowEvent()
	{
		swallowSound.Play();
	}
}
