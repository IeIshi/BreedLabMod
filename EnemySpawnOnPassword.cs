using System;
using UnityEngine;

public class EnemySpawnOnPassword : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool activated;
	}

	public GameObject insectEnemyOne;

	public GameObject insectEnemyTwo;

	public AudioSource awakeSound;

	private bool activated;

	private void Start()
	{
		if (activated)
		{
			if (!insectEnemyOne.GetComponent<EnemyFieldOfView>().isDed)
			{
				insectEnemyOne.gameObject.SetActive(value: true);
			}
			if (!insectEnemyOne.GetComponent<EnemyFieldOfView>().isDed)
			{
				insectEnemyTwo.gameObject.SetActive(value: true);
			}
			base.gameObject.SetActive(value: false);
		}
		else
		{
			insectEnemyOne.gameObject.SetActive(value: false);
			insectEnemyTwo.gameObject.SetActive(value: false);
		}
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.activated = activated;
		return saveData;
	}

	public void RestoreState(object state)
	{
		activated = ((SaveData)state).activated;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && PlayerManager.KeyDoorChillArea && !activated)
		{
			insectEnemyOne.gameObject.SetActive(value: true);
			insectEnemyTwo.gameObject.SetActive(value: true);
			awakeSound.Play();
			activated = true;
		}
	}
}
