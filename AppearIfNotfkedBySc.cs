using UnityEngine;

public class AppearIfNotfkedBySc : MonoBehaviour
{
	private void Start()
	{
		if (PlayerManager.FkedBySc)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
