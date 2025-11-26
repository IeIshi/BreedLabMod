using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromTwoToTwoTwo : Interactable
{
	public GameObject loadingScreen;

	public GameObject Lita;

	public Slider loadingSlider;

	public static bool insideLevelTwoTwo;

	public Transform startPlace;

	private Inventory inventory;

	public Dialogue dialogue;

	public Dialogue dialogueTwo;

	public Dialogue dialogueNoLight;

	public GameObject fleshLightChest;

	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "Level_3_DarkTunnelLvl" && PlayerManager.finishedDarkTunnelTwo)
		{
			fleshLightChest.GetComponent<ChestPickup>().opened = true;
			Lita.SetActive(value: false);
		}
	}

	private void Start()
	{
		inventory = Inventory.instance;
		if (SceneManager.GetActiveScene().name == "Level_3_DarkTunnelLvl" && insideLevelTwoTwo)
		{
			if (startPlace != null)
			{
				PlayerManager.instance.player.transform.localPosition = startPlace.localPosition;
				PlayerManager.instance.player.transform.localRotation = startPlace.localRotation;
				PlayerManager.instance.player.GetComponent<CharacterController>().enabled = true;
			}
			insideLevelTwoTwo = false;
		}
		if (SceneManager.GetActiveScene().name == "DarkTunnelTwo")
		{
			insideLevelTwoTwo = true;
		}
	}

	public override void Interact()
	{
		base.Interact();
		PlayerManager.loadedrly = false;
		if (SceneManager.GetActiveScene().name == "Level_3_DarkTunnelLvl")
		{
			if (PlayerManager.finishedDarkTunnelTwo)
			{
				TriggerDialogeTwo();
				return;
			}
			for (int i = 0; i < inventory.items.Count; i++)
			{
				if (inventory.items[i].name == "NeckFlash")
				{
					LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
					PlayerManager.loadedrly = false;
					return;
				}
			}
			if (EquipmentManager.flashNeckOn)
			{
				LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
				PlayerManager.loadedrly = false;
			}
			else
			{
				TriggerDialogeNoLight();
			}
		}
		else if (!PlayerManager.id1)
		{
			TriggerDialoge();
		}
		else
		{
			PlayerManager.id1 = true;
			PlayerManager.id2 = false;
			PlayerManager.id11 = false;
			PlayerManager.smallKey = false;
			PlayerManager.KeyB = false;
			PlayerManager.Office = false;
			PlayerManager.Key = false;
			PlayerManager.finishedDarkTunnelTwo = true;
			PlayerManager.loadedrly = false;
			LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
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

	public void TriggerDialogeTwo()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueTwo);
	}

	public void TriggerDialogeNoLight()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueNoLight);
	}
}
