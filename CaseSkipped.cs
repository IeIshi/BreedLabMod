using UnityEngine;

public class CaseSkipped : MonoBehaviour
{
	public GameObject chest;

	public GameObject weapon;

	private void Start()
	{
		if (MainMenu.levelSkipped)
		{
			chest.SetActive(value: true);
			weapon.SetActive(value: true);
		}
		else
		{
			chest.SetActive(value: false);
			weapon.SetActive(value: false);
		}
	}
}
