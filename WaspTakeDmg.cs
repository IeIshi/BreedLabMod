using UnityEngine;

public class WaspTakeDmg : MonoBehaviour
{
	public GameObject Wasp;

	public void TakeDamage(float amount)
	{
		Debug.Log("WASP TOOK DAMAGE");
		Wasp.GetComponent<WaspControl>().TakeDamage(amount);
	}
}
