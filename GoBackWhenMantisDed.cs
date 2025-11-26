using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoBackWhenMantisDed : Interactable
{
	public Dialogue dialogue;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	private Inventory inventory;

	private void Start()
	{
		inventory = Inventory.instance;
	}

	public override void Interact()
	{
		base.Interact();
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "AntiTent")
			{
				LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
				PlayerManager.loadedrly = false;
				return;
			}
		}
		if (EquipmentManager.antiTentOn)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
			PlayerManager.loadedrly = false;
		}
		else
		{
			TriggerDialoge();
		}
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

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
