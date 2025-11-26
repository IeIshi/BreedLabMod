using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterBotanicMain : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool item1;

		public bool item2;

		public bool item3;

		public bool item4;

		public bool item5;

		public bool item6;
	}

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public static bool enteringFromOutside;

	public List<GameObject> BotanicOutdoorItems = new List<GameObject>();

	public static bool item1;

	public static bool item2;

	public static bool item3;

	public static bool item4;

	public static bool item5;

	public static bool item6;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.item1 = item1;
		saveData.item2 = item2;
		saveData.item3 = item3;
		saveData.item4 = item4;
		saveData.item5 = item5;
		saveData.item6 = item6;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData obj = (SaveData)state;
		item1 = obj.item1;
		item2 = obj.item2;
		item3 = obj.item3;
		item4 = obj.item4;
		item5 = obj.item5;
		item6 = obj.item6;
	}

	private void Awake()
	{
		if (item1)
		{
			BotanicOutdoorItems[0].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item2)
		{
			BotanicOutdoorItems[1].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item3)
		{
			BotanicOutdoorItems[2].GetComponent<FreezerLock>().open = true;
		}
		if (item4)
		{
			BotanicOutdoorItems[3].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item5)
		{
			BotanicOutdoorItems[4].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item6)
		{
			BotanicOutdoorItems[5].GetComponent<ItemPickup>().itemPickedUp = true;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (BotanicOutdoorItems[0].GetComponent<ItemPickup>().itemPickedUp)
		{
			item1 = true;
		}
		if (BotanicOutdoorItems[1].GetComponent<ItemPickup>().itemPickedUp)
		{
			item2 = true;
		}
		if (BotanicOutdoorItems[2].GetComponent<FreezerLock>().open)
		{
			item3 = true;
		}
		if (BotanicOutdoorItems[3].GetComponent<ItemPickup>().itemPickedUp)
		{
			item4 = true;
		}
		if (BotanicOutdoorItems[4].GetComponent<ItemPickup>().itemPickedUp)
		{
			item5 = true;
		}
		if (BotanicOutdoorItems[5].GetComponent<ItemPickup>().itemPickedUp)
		{
			item6 = true;
		}
		enteringFromOutside = true;
		PlayerManager.loadedrly = false;
		LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
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
