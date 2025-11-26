using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackDtToInBetween : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public static bool backFromDt;

	public static bool backFromInBetween;

	public bool dtToInBetween;

	public bool InBetweenToMainHall2;

	public TextMeshProUGUI actHeader;

	public TextMeshProUGUI actName;

	private string act2String = "Act 3";

	private string actNameString = "Mothers";

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
		if (dtToInBetween)
		{
			backFromDt = true;
		}
		if (InBetweenToMainHall2)
		{
			backFromInBetween = true;
		}
		if (dtToInBetween)
		{
			LoadLevelAct3(SceneManager.GetActiveScene().buildIndex - 1);
		}
		if (InBetweenToMainHall2)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	public void LoadLevelAct3(int sceneIndex)
	{
		StartCoroutine(LoadAsynchAct3(sceneIndex));
	}

	private IEnumerator LoadAsynch(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			loadingSlider.value = value;
			yield return null;
		}
	}

	private IEnumerator LoadAsynchAct3(int sceneIndex)
	{
		loadingScreen.SetActive(value: true);
		actHeader.text = act2String;
		actName.text = actNameString;
		yield return new WaitForSeconds(3f);
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			loadingSlider.value = value;
			yield return null;
		}
	}
}
