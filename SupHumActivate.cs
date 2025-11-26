using System.Collections;
using UnityEngine;

public class SupHumActivate : MonoBehaviour
{
	private SecuritySchalterDoor test;

	public GameObject wolf;

	private void Start()
	{
		test = GetComponent<SecuritySchalterDoor>();
		wolf.GetComponent<SupHum>().enabled = false;
	}

	private void FixedUpdate()
	{
		if (test.open && !wolf.GetComponent<SupHum>().sleeping)
		{
			StartCoroutine(ActivateWolf());
		}
	}

	private IEnumerator ActivateWolf()
	{
		yield return new WaitForSeconds(2f);
		if (wolf.activeSelf)
		{
			wolf.GetComponent<SupHum>().enabled = true;
		}
	}
}
