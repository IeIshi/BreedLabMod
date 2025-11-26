using UnityEngine;

public class LichtDingensAus : MonoBehaviour
{
	public GameObject LichtDingens;

	private void Start()
	{
		if (PlayerManager.enteredChallangeRoom)
		{
			LichtDingens.SetActive(value: false);
		}
	}
}
