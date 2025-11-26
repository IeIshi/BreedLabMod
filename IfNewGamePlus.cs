using UnityEngine;

public class IfNewGamePlus : MonoBehaviour
{
	private void Start()
	{
		if (MainMenu.NewGamePlus)
		{
			base.gameObject.SetActive(value: true);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
