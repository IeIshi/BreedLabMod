using UnityEngine;

public class LightHimWhenH : MonoBehaviour
{
	public GameObject Light1;

	public GameObject Light2;

	private NewEnemControl control;

	private void Start()
	{
		control = GetComponent<NewEnemControl>();
	}

	private void FixedUpdate()
	{
		if (!control.sexyTime)
		{
			Light1.SetActive(value: false);
			Light2.SetActive(value: false);
		}
		else
		{
			Light1.SetActive(value: true);
			Light2.SetActive(value: true);
		}
	}
}
