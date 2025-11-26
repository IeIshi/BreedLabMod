using System;
using UnityEngine;

public class TunnelSaveable : MonoBehaviour
{
	[Serializable]
	private struct SaveData
	{
		public bool normalOn;

		public bool distortedOn;
	}

	public GameObject hallNormal;

	public GameObject hallDistorted;

	[SerializeField]
	private bool normalOn;

	[SerializeField]
	private bool distortedOn;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.normalOn = normalOn;
		saveData.distortedOn = distortedOn;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		normalOn = saveData.normalOn;
		distortedOn = saveData.distortedOn;
	}

	private void Start()
	{
		if (normalOn)
		{
			hallNormal.SetActive(value: true);
			hallDistorted.SetActive(value: false);
		}
		if (distortedOn)
		{
			hallNormal.SetActive(value: false);
			hallDistorted.SetActive(value: true);
		}
	}
}
