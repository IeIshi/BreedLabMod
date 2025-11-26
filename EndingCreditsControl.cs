using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCreditsControl : MonoBehaviour
{
	private void Start()
	{
		Cursor.visible = true;
		PlayerPrefs.SetInt("Finished", 1);
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("TrickLevel");
	}
}
