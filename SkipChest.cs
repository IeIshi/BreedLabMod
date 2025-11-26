using UnityEngine;

public class SkipChest : MonoBehaviour
{
	private void Start()
	{
		if (MainMenu.skippedToAct4)
		{
			base.gameObject.SetActive(value: true);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
