using UnityEngine;

public class ActivateGangBangifFkH : MonoBehaviour
{
	private void Start()
	{
		if (!PlayerManager.FkedBySc)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
