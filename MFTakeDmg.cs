using UnityEngine;

public class MFTakeDmg : MonoBehaviour
{
	public GameObject MindFleyer;

	public void TakeDamage(float amount)
	{
		Debug.Log("MINDFLEYER TOOK DAMAGE");
		MindFleyer.GetComponent<MindFleyerControl>().TakeDamage(amount);
	}
}
