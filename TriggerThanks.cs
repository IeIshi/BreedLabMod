using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerThanks : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			SceneManager.LoadScene("GameOver");
		}
	}
}
