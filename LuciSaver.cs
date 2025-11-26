using System;
using UnityEngine;

public class LuciSaver : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool interacted;
	}

	public GameObject Luci;

	public GameObject Key;

	public bool interacted;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.interacted = interacted;
		return saveData;
	}

	public void RestoreState(object state)
	{
		interacted = ((SaveData)state).interacted;
	}

	private void Start()
	{
		if (interacted)
		{
			UnityEngine.Object.Destroy(Luci);
			UnityEngine.Object.Destroy(Key);
		}
	}
}
