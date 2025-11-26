using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public int level;

		public int xp;
	}

	[SerializeField]
	private int level = 1;

	[SerializeField]
	private int xp = 100;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.level = level;
		saveData.xp = xp;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		level = saveData.level;
		xp = saveData.xp;
	}
}
