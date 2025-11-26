using UnityEngine;

public class LustGenOnEnter : MonoBehaviour
{
	private bool inside;

	public GameObject Heroine;

	private void FixedUpdate()
	{
		if (inside)
		{
			Heroine.GetComponent<HeroineStats>().GainLust(2f);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			inside = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			inside = false;
		}
	}
}
