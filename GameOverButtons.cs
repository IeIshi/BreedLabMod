using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
	private void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene("TrickLevel");
	}
}
