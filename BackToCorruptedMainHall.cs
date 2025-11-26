using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToCorruptedMainHall : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	private void Start()
	{
		if (!PlayerManager.ScSexAfterMath)
		{
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		PlayerManager.loadedrly = false;
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 6);
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
