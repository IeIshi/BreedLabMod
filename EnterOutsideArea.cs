using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterOutsideArea : Interactable, ISaveable
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

		public bool item7;

		public bool item8;

		public bool item9;

		public bool item10;

		public bool item11;

		public bool item12;
	}

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public List<GameObject> BotanicMainItems = new List<GameObject>();

	public static bool item1;

	public static bool item2;

	public static bool item3;

	public static bool item4;

	public static bool item5;

	public static bool item6;

	public static bool item7;

	public static bool item8;

	public static bool item9;

	public static bool item10;

	public static bool item11;

	public static bool item12;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.item1 = item1;
		saveData.item2 = item2;
		saveData.item3 = item3;
		saveData.item4 = item4;
		saveData.item5 = item5;
		saveData.item6 = item6;
		saveData.item7 = item7;
		saveData.item8 = item8;
		saveData.item9 = item9;
		saveData.item10 = item10;
		saveData.item11 = item11;
		saveData.item12 = item12;
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
		item7 = obj.item7;
		item8 = obj.item8;
		item9 = obj.item9;
		item10 = obj.item10;
		item11 = obj.item11;
		item12 = obj.item12;
	}

	private void Awake()
	{
		if (item1)
		{
			BotanicMainItems[0].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item2)
		{
			BotanicMainItems[1].GetComponent<ChestPickup>().opened = true;
		}
		if (item3)
		{
			BotanicMainItems[2].GetComponent<ChestPickup>().opened = true;
		}
		if (item4)
		{
			BotanicMainItems[3].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item5)
		{
			BotanicMainItems[4].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item6)
		{
			BotanicMainItems[5].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item7)
		{
			BotanicMainItems[6].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item8)
		{
			BotanicMainItems[7].GetComponent<ItemPickup>().itemPickedUp = true;
		}
		if (item9)
		{
			BotanicMainItems[8].GetComponent<SecuritySchalter>().repaired = true;
			BotanicMainItems[8].GetComponent<SecuritySchalter>().isOn = true;
		}
		if (item10)
		{
			BotanicMainItems[9].GetComponent<SecuritySchalter>().repaired = true;
			BotanicMainItems[9].GetComponent<SecuritySchalter>().isOn = true;
		}
		if (item11)
		{
			BotanicMainItems[10].GetComponent<ChestPickup>().opened = true;
		}
		if (item12)
		{
			BotanicMainItems[11].GetComponent<ItemPickup>().itemPickedUp = true;
		}
	}

	public override void Interact()
	{
		base.Interact();
		PlayerManager.loadedrly = false;
		EnterBotanicMain.enteringFromOutside = true;
		if (BotanicMainItems[0].GetComponent<ItemPickup>().itemPickedUp)
		{
			item1 = true;
		}
		if (BotanicMainItems[1].GetComponent<ChestPickup>().opened)
		{
			item2 = true;
		}
		if (BotanicMainItems[2].GetComponent<ChestPickup>().opened)
		{
			item3 = true;
		}
		if (BotanicMainItems[3].GetComponent<ItemPickup>().itemPickedUp)
		{
			item4 = true;
		}
		if (BotanicMainItems[4].GetComponent<ItemPickup>().itemPickedUp)
		{
			item5 = true;
		}
		if (BotanicMainItems[5].GetComponent<ItemPickup>().itemPickedUp)
		{
			item6 = true;
		}
		if (BotanicMainItems[6].GetComponent<ItemPickup>().itemPickedUp)
		{
			item7 = true;
		}
		if (BotanicMainItems[7].GetComponent<ItemPickup>().itemPickedUp)
		{
			item8 = true;
		}
		if (BotanicMainItems[8].GetComponent<SecuritySchalter>().isOn)
		{
			item9 = true;
		}
		if (BotanicMainItems[9].GetComponent<SecuritySchalter>().isOn)
		{
			item10 = true;
		}
		if (BotanicMainItems[10].GetComponent<ChestPickup>().opened)
		{
			item11 = true;
		}
		if (BotanicMainItems[11].GetComponent<ItemPickup>().itemPickedUp)
		{
			item12 = true;
		}
		PlayerManager.loadedrly = false;
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
