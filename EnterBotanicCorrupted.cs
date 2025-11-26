using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterBotanicCorrupted : Interactable
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public Dialogue dialogue;

	public Dialogue dialogeNoCon;

	private Inventory inventory;

	private void Start()
	{
		inventory = Inventory.instance;
	}

	public override void Interact()
	{
		base.Interact();
		if (LarveBirth.larveImpregnated)
		{
			TriggerDialoge();
			return;
		}
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "GreenOrbContainer")
			{
				PlayerManager.loadedrly = false;
				MasturbationArea.mastArea = false;
				LoadLevel("BotanicCorrupted");
				return;
			}
		}
		TriggerDialogeNoCon();
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

	public void TriggerDialogeNoCon()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogeNoCon);
	}
}
