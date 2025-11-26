using UnityEngine;

public class RayControl : MonoBehaviour
{
	public bool mindbreak;

	public bool inRay;

	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			inRay = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			inRay = false;
		}
	}
}
