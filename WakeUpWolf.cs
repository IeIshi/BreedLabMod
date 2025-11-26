using System;
using UnityEngine;

public class WakeUpWolf : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool wakedUp;
	}

	public GameObject WolfWaked;

	public GameObject WolfSleeping;

	private Inventory inventory;

	private bool wakedUp;

	private bool gotTheCard;

	public AudioSource growl;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.wakedUp = wakedUp;
		return saveData;
	}

	public void RestoreState(object state)
	{
		wakedUp = ((SaveData)state).wakedUp;
	}

	private void Start()
	{
		inventory = Inventory.instance;
		if (wakedUp)
		{
			WolfWaked.SetActive(value: true);
			WolfSleeping.SetActive(value: false);
		}
		else
		{
			WolfWaked.SetActive(value: false);
			WolfSleeping.SetActive(value: true);
		}
	}

	private void WakeUpTheWolf()
	{
		wakedUp = true;
		growl.Play();
		WolfWaked.SetActive(value: true);
		WolfSleeping.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !gotTheCard && PlayerManager.id1)
		{
			if (!wakedUp)
			{
				WakeUpTheWolf();
			}
			gotTheCard = true;
		}
	}
}
