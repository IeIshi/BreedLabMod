using System;
using UnityEngine;

public class EnemSavState : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool isDead;
	}

	public bool isDead;

	public GameObject theEnemy;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.isDead = isDead;
		return saveData;
	}

	public void RestoreState(object state)
	{
		isDead = ((SaveData)state).isDead;
	}

	private void Start()
	{
		if (isDead)
		{
			UnityEngine.Object.Destroy(theEnemy);
		}
	}
}
