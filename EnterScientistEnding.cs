using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterScientistEnding : MonoBehaviour
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (PlayerManager.IsVirgin)
			{
				EndingHandler.triggerVirginEnding = true;
			}
			else
			{
				EndingHandler.triggerScientistEnding = true;
			}
			LoadLevel("FinalLevel");
		}
	}

	public void LoadLevel(string sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(string sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingScreen.SetActive(value: true);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			loadingSlider.value = value;
			loadingSlider.value = value;
			yield return null;
		}
	}
}
