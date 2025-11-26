using UnityEngine;

public class FemHugSoundEvents : MonoBehaviour
{
	public AudioSource thrustSound;

	public AudioSource thrustOutSound;

	public AudioSource cumSound;

	public void ThrustEvent()
	{
		thrustSound.Play();
	}

	public void ThrustOutEvent()
	{
		thrustOutSound.Play();
	}

	public void MilkEvent()
	{
		cumSound.Play();
	}
}
