using UnityEngine;

public class goodJobActive : MonoBehaviour
{
	private void Start()
	{
		if (PlayerManager.MantisDown)
		{
			base.gameObject.SetActive(value: true);
		}
	}
}
