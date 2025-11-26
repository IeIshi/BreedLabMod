using UnityEngine;

public class GotHit : MonoBehaviour
{
	public GameObject Mantis;

	public void TakeDamage(float amount)
	{
		Mantis.GetComponent<MantisAi>().TakeDamage(amount);
	}
}
