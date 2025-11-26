using UnityEngine;

public class FutaCollisionDetector : MonoBehaviour
{
	public GameObject Me;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Me.GetComponent<FutaChaser>().gotHer = true;
			GetComponent<CapsuleCollider>().enabled = false;
		}
	}
}
