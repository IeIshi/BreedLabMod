using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadActTwo : Interactable
{
	private Inventory inventory;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public TextMeshProUGUI actHeader;

	public TextMeshProUGUI actName;

	private string act2String = "Act 2";

	private string actNameString = "Scholoarship";

	public Image coverImage;

	private void Start()
	{
		inventory = Inventory.instance;
	}

	public override void Interact()
	{
		base.Interact();
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "IDCard")
			{
				inventory.items[i].RemoveFromInventory();
			}
		}
		PlayerManager.loadedrly = false;
		InventoryUI.heroineIsChased = false;
		LoadLevel("Level_3");
	}

	public void LoadLevel(string sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(string sceneIndex)
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
