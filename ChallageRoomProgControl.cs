using System;
using UnityEngine;

public class ChallageRoomProgControl : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool doubleDoorOpened;

		public bool phantomTwoOpenedDoor;

		public bool phantomThreeOpenedDoor;

		public bool gangBangDone;

		public bool phantomFourOpenedDoor;

		public bool lickersSpawned;
	}

	public bool doubleDoorOpened;

	public bool phantomTwoOpenedDoor;

	public bool phantomThreeOpenedDoor;

	public bool gangBangDone;

	public bool phantomFourOpenedDoor;

	public bool lickersSpawned;

	public GameObject phantomTwo;

	public GameObject phantomThree;

	public GameObject phantomFour;

	public GameObject theLickers;

	public GameObject blobDoorThree;

	public GameObject blobDoorFour;

	public GameObject doubeDoor;

	public GameObject doubleDoorTwo;

	public GameObject oldTv;

	public GameObject blobDoorFive;

	public GameObject activeOut;

	public GameObject anotherBlobDoor;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.doubleDoorOpened = doubleDoorOpened;
		saveData.phantomTwoOpenedDoor = phantomTwoOpenedDoor;
		saveData.phantomThreeOpenedDoor = phantomThreeOpenedDoor;
		saveData.gangBangDone = gangBangDone;
		saveData.phantomFourOpenedDoor = phantomFourOpenedDoor;
		saveData.lickersSpawned = lickersSpawned;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		doubleDoorOpened = saveData.doubleDoorOpened;
		phantomTwoOpenedDoor = saveData.phantomTwoOpenedDoor;
		phantomThreeOpenedDoor = saveData.phantomThreeOpenedDoor;
		gangBangDone = saveData.gangBangDone;
		phantomFourOpenedDoor = saveData.phantomFourOpenedDoor;
		lickersSpawned = saveData.lickersSpawned;
	}

	private void Start()
	{
		if (doubleDoorOpened)
		{
			doubeDoor.GetComponent<Animator>().SetBool("isOpen", value: true);
			if (!phantomTwoOpenedDoor)
			{
				phantomTwo.SetActive(value: true);
			}
		}
		if (lickersSpawned)
		{
			theLickers.SetActive(value: true);
		}
		else
		{
			theLickers.SetActive(value: false);
		}
		if (phantomTwoOpenedDoor)
		{
			UnityEngine.Object.Destroy(blobDoorThree);
		}
		if (phantomThreeOpenedDoor)
		{
			if (!gangBangDone)
			{
				UnityEngine.Object.Destroy(blobDoorFour);
			}
			UnityEngine.Object.Destroy(phantomThree);
		}
		if (gangBangDone)
		{
			oldTv.layer = 0;
			doubleDoorTwo.GetComponent<Animator>().SetBool("isOpen", value: true);
			UnityEngine.Object.Destroy(blobDoorFour);
		}
		if (phantomFourOpenedDoor)
		{
			UnityEngine.Object.Destroy(blobDoorFive);
			UnityEngine.Object.Destroy(phantomFour);
			UnityEngine.Object.Destroy(anotherBlobDoor);
			if (activeOut != null)
			{
				activeOut.SetActive(value: true);
			}
		}
	}
}
