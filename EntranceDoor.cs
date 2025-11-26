using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntranceDoor : Interactable
{
	public Transform schalter;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public Dialogue dialogue1;

	public Dialogue dialogue2;

	public override void Interact()
	{
		base.Interact();
		if (schalter.GetComponent<Schalter>().isOn)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else
		{
			TriggerDialoge2();
		}
	}

	public void TriggerDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue1);
	}

	public void TriggerDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(int sceneIndex)
	{
		loadingScreen.SetActive(value: true);
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
