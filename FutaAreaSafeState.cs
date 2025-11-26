using System;
using UnityEngine;

public class FutaAreaSafeState : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool passedFutaArea;
	}

	public GameObject Futas;

	public GameObject TV;

	public GameObject spawnPoint;

	public bool passedFutaArea;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.passedFutaArea = passedFutaArea;
		return saveData;
	}

	public void RestoreState(object state)
	{
		passedFutaArea = ((SaveData)state).passedFutaArea;
	}

	private void Start()
	{
		if (passedFutaArea)
		{
			Futas.SetActive(value: false);
			TV.SetActive(value: false);
			spawnPoint.SetActive(value: false);
		}
	}
}
