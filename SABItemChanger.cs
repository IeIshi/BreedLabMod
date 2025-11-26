using UnityEngine;

public class SABItemChanger : MonoBehaviour
{
	public GameObject normalCloth;

	public GameObject sabCloth;

	private void Start()
	{
		if (PlayerManager.SAB)
		{
			sabCloth.SetActive(value: true);
			normalCloth.SetActive(value: false);
		}
		else
		{
			sabCloth.SetActive(value: false);
			normalCloth.SetActive(value: true);
		}
	}
}
