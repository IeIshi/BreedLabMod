using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadEndDemoOnTrig : MonoBehaviour
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerManager.loadedrly = false;
			HeroineStats.currentOrg = 0f;
			HeroineStats.orgasm = false;
			LoadLevel("BotanicMain");
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
