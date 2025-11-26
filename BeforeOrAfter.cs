using UnityEngine;

public class BeforeOrAfter : MonoBehaviour
{
	public GameObject BeforeMF;

	public GameObject AfterMF;

	public bool debugAfter;

	private void Start()
	{
		if (HeroineStats.corrupted)
		{
			AfterMF.SetActive(value: true);
			BeforeMF.SetActive(value: false);
		}
		else
		{
			AfterMF.SetActive(value: false);
			BeforeMF.SetActive(value: true);
		}
		if (debugAfter)
		{
			AfterMF.SetActive(value: true);
			BeforeMF.SetActive(value: false);
		}
	}
}
