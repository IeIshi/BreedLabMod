using System;
using UnityEngine;

public class OutsideItemSafer : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool lockOpen;

		public bool idCardPicked;

		public bool ammoPicked;

		public bool willpowerPicked;

		public bool lovePotionPicked;

		public bool redConPicked;
	}

	public static bool lockOpen;

	public static bool idCardPicked;

	public static bool ammoPicked;

	public static bool willpowerPicked;

	public static bool lovePotionPicked;

	public static bool redConPicked;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.lockOpen = lockOpen;
		saveData.idCardPicked = idCardPicked;
		saveData.ammoPicked = ammoPicked;
		saveData.willpowerPicked = willpowerPicked;
		saveData.lovePotionPicked = lovePotionPicked;
		saveData.redConPicked = redConPicked;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData obj = (SaveData)state;
		lockOpen = obj.lockOpen;
		idCardPicked = obj.idCardPicked;
		ammoPicked = obj.ammoPicked;
		willpowerPicked = obj.willpowerPicked;
		lovePotionPicked = obj.lovePotionPicked;
		redConPicked = obj.redConPicked;
	}
}
