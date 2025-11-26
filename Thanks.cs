using UnityEngine;

public class Thanks : MonoBehaviour
{
	private void Start()
	{
		Cursor.visible = true;
	}

	public void GoToMainMenu()
	{
		Application.Quit();
	}

	public void GoToSSPage()
	{
		Application.OpenURL("https://subscribestar.adult/moey");
	}

	public void GoToPatreonPage()
	{
		Application.OpenURL("https://www.patreon.com/moeymoey");
	}
}
