using System;
using UnityEngine;

public class EnergyEngineBotanic : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool engineIsOn;
	}

	public GameObject door1;

	public GameObject door2;

	public Material[] material;

	public GameObject lampMesh;

	private Renderer rendLampMesh;

	public static bool engineIsOn;

	public AudioSource motorSound;

	public GameObject ScXLitaScene;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.engineIsOn = engineIsOn;
		return saveData;
	}

	public void RestoreState(object state)
	{
		engineIsOn = ((SaveData)state).engineIsOn;
	}

	private void Start()
	{
		rendLampMesh = lampMesh.GetComponent<Renderer>();
		if (engineIsOn)
		{
			rendLampMesh.sharedMaterial = material[1];
			door1.GetComponent<OpenPoweredDoors>().doorClosed = false;
			door2.GetComponent<OpenPoweredDoors>().doorClosed = false;
			ScXLitaScene.SetActive(value: true);
			motorSound.Play();
		}
		else
		{
			rendLampMesh.sharedMaterial = material[0];
			door1.GetComponent<OpenPoweredDoors>().doorClosed = true;
			door2.GetComponent<OpenPoweredDoors>().doorClosed = true;
			ScXLitaScene.SetActive(value: false);
		}
	}

	public override void Interact()
	{
		base.Interact();
		rendLampMesh.sharedMaterial = material[1];
		engineIsOn = true;
		ScXLitaScene.SetActive(value: true);
		door1.GetComponent<OpenPoweredDoors>().doorClosed = false;
		door2.GetComponent<OpenPoweredDoors>().doorClosed = false;
		motorSound.Play();
	}
}
