using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTrueEnding : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (EndingHandler.triggerScientistEnding)
			{
				SceneManager.LoadScene("ScEnding");
			}
			if (EndingHandler.triggerTrueEnding)
			{
				SceneManager.LoadScene("TrueEnding");
			}
		}
	}

	private IEnumerator GoTo()
	{
		yield return null;
	}
}
