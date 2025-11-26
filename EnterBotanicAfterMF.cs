using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterBotanicAfterMF : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public static bool AfterMindFleyer;

	public override void Interact()
	{
		base.Interact();
		EnterBotanicMain.enteringFromOutside = true;
		PlayerManager.loadedrly = false;
		LoadLevel("BotanicCorrupted");
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
			yield return null;
		}
	}
}
