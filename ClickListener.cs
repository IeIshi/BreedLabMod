using UnityEngine;

public class ClickListener : MonoBehaviour
{
	public AudioSource click;

	private void OnClickEvent()
	{
		click.Play();
	}
}
