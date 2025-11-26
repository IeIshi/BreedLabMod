using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntranceLvlTwo : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public Dialogue dialogue;

	private void Start()
	{
		BackDtToInBetween.backFromInBetween = false;
	}

	public override void Interact()
	{
		base.Interact();
		if (PlayerManager.ScSexAfterMath)
		{
			TriggerDialoge();
			return;
		}
		PlayerManager.loadedrly = false;
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(int sceneIndex)
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
