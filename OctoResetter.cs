using UnityEngine;

public class OctoResetter : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log("PATH RESETTED");
			other.gameObject.GetComponent<OctoControl>().resetPath = true;
		}
	}
}
