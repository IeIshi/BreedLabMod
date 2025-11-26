using UnityEngine;

public class TotemEnemyBlock : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Wasp")
		{
			Debug.Log("WASP ENTERED");
			other.GetComponent<WaspControl>().currentStamina = 0f;
			other.GetComponent<WaspControl>().resting = true;
			other.GetComponent<WaspControl>().state = WaspControl.MyState.PATROL;
		}
		if (other.tag == "Player")
		{
			Debug.Log("Player ENTERED");
		}
	}
}
