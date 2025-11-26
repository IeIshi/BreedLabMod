using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterOutsideWest : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public Dialogue dialogue;

	public override void Interact()
	{
		base.Interact();
		if (HeroineStats.pregnant)
		{
			TriggerDialoge();
			return;
		}
		PlayerManager.loadedrly = false;
		LoadLevel("OutdoorWest");
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

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
