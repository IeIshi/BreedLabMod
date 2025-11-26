using UnityEngine;

public class KeyBox : MonoBehaviour
{
	public GameObject id1;

	public GameObject id11;

	public GameObject id2;

	public GameObject smallKey;

	public GameObject KeyB;

	public GameObject Office;

	public GameObject Key;

	private void Start()
	{
		if (PlayerManager.id1)
		{
			id1.SetActive(value: true);
		}
		else
		{
			id1.SetActive(value: false);
		}
		if (PlayerManager.id11)
		{
			id11.SetActive(value: true);
		}
		else
		{
			id11.SetActive(value: false);
		}
		if (PlayerManager.id2)
		{
			id2.SetActive(value: true);
		}
		else
		{
			id2.SetActive(value: false);
		}
		if (PlayerManager.smallKey)
		{
			smallKey.SetActive(value: true);
		}
		else
		{
			smallKey.SetActive(value: false);
		}
		if (PlayerManager.KeyB)
		{
			KeyB.SetActive(value: true);
		}
		else
		{
			KeyB.SetActive(value: false);
		}
		if (PlayerManager.Office)
		{
			Office.SetActive(value: true);
		}
		else
		{
			Office.SetActive(value: false);
		}
		if (PlayerManager.Key)
		{
			Key.SetActive(value: true);
		}
		else
		{
			Key.SetActive(value: false);
		}
	}
}
