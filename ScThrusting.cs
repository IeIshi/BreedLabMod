using UnityEngine;

public class ScThrusting : MonoBehaviour
{
	public GameObject sceneManager;

	public AudioSource vaginaSound;

	public AudioSource vaginaSoundFast;

	public AudioSource bodyCollide;

	public AudioSource impact;

	public AudioSource squish;

	private void ThrustEvent()
	{
		if (sceneManager.GetComponent<SceneController>().state != SexState.FASTSEX)
		{
			impact.Play();
		}
		squish.Play();
	}

	private void OutEvent()
	{
		if (sceneManager.GetComponent<SceneController>().state == SexState.FASTSEX)
		{
			bodyCollide.Play();
		}
	}
}
