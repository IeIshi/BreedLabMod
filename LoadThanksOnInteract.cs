using UnityEngine.SceneManagement;

public class LoadThanksOnInteract : Interactable
{
	public override void Interact()
	{
		base.Interact();
		SceneManager.LoadScene("GameOver");
	}
}
