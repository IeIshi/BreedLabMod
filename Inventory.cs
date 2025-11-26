using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable
{
	public delegate void OnItemChanged();

	[Serializable]
	private struct SaveData
	{
		public int[] itemSaveArray;

		public float[] heroineLocation;

		public int energyDrinkCount;

		public int lovePotionCount;
	}

	public static Inventory instance;

	public static List<Item> itemSave = new List<Item>();

	public OnItemChanged onItemChangedCallback;

	public int space = 12;

	public List<Item> items = new List<Item>();

	public List<Item> itemList;

	public List<Equipment> equipmentList;

	[SerializeField]
	public float[] heroineLocation = new float[4];

	public static int[] itemSaveArray = new int[12];

	public static int energyDrinkCount;

	public static int lovePotionCount;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of Inventory found!");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		for (int i = 0; i < itemSaveArray.Length; i++)
		{
			if (itemSaveArray[i] == 0)
			{
				continue;
			}
			for (int j = 0; j < itemList.Count; j++)
			{
				if (itemSaveArray[i] == itemList[j].id)
				{
					LoadItem(itemList[j]);
				}
			}
		}
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.itemSaveArray = itemSaveArray;
		saveData.heroineLocation = heroineLocation;
		saveData.energyDrinkCount = energyDrinkCount;
		saveData.lovePotionCount = lovePotionCount;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		itemSaveArray = saveData.itemSaveArray;
		heroineLocation = saveData.heroineLocation;
		energyDrinkCount = saveData.energyDrinkCount;
		lovePotionCount = saveData.lovePotionCount;
	}

	public bool Add(Item item)
	{
		if (!item.isDefaultItem)
		{
			if (items.Count >= space)
			{
				Debug.Log("Not enough room.");
				return false;
			}
			items.Add(item);
			for (int i = 0; i < itemSaveArray.Length; i++)
			{
				if (itemSaveArray[i] == 0)
				{
					itemSaveArray[i] = item.id;
					break;
				}
			}
			if (onItemChangedCallback != null)
			{
				onItemChangedCallback();
			}
		}
		return true;
	}

	public bool LoadItem(Item item)
	{
		items.Add(item);
		if (onItemChangedCallback != null)
		{
			onItemChangedCallback();
		}
		return true;
	}

	public void Remove(Item item)
	{
		items.Remove(item);
		itemSave.Remove(item);
		for (int i = 0; i < space; i++)
		{
			if (itemSaveArray[i] == item.id)
			{
				itemSaveArray[i] = 0;
				break;
			}
		}
		if (onItemChangedCallback != null)
		{
			onItemChangedCallback();
		}
	}
}
