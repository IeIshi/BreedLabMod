using UnityEngine;

public class SkipChestTwo : MonoBehaviour
{
	private void Start()
	{
		if (MainMenu.skippedToAct4Two)
		{
			base.gameObject.SetActive(value: true);
			MainMenu.skippedToAct4Two = false;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
