using System;
using UnityEngine;

public class EnergyEngineOne : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool engineIsOn;
	}

	public Transform schalter;

	public Material[] material;

	private Renderer rendLampe;

	public Dialogue dialogue;

	public GameObject lampe;

	public GameObject lampMesh;

	private Renderer rendLampMesh;

	public GameObject lamp1;

	public GameObject lamp2;

	public GameObject lamp3;

	public GameObject areaLights;

	public GameObject strangeLight;

	public GameObject guard;

	public AudioSource motorSound;

	public GameObject lyingScientists;

	public static bool engineIsOn;

	public GameObject door_1;

	public GameObject door_2;

	public GameObject scientists;

	public GameObject challangeDoor;

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
		rendLampe = lampe.GetComponent<Renderer>();
		rendLampMesh = lampMesh.GetComponent<Renderer>();
		rendLampe.enabled = true;
		if (engineIsOn)
		{
			guard.SetActive(value: false);
			areaLights.SetActive(value: true);
			scientists.SetActive(value: true);
			rendLampe.sharedMaterial = material[1];
			lyingScientists.SetActive(value: false);
			lamp1.GetComponent<Light>().enabled = true;
			lamp2.GetComponent<Light>().enabled = true;
			lamp3.GetComponent<Light>().enabled = true;
			door_1.GetComponent<Animator>().SetBool("isOpen", value: true);
			door_2.GetComponent<Animator>().SetBool("isOpen", value: true);
			motorSound.Play();
			if (!PlayerManager.enteredChallangeRoom)
			{
				challangeDoor.SetActive(value: false);
			}
		}
		else
		{
			rendLampe.sharedMaterial = material[0];
			if (!PlayerManager.enteredChallangeRoom)
			{
				lamp1.GetComponent<Light>().enabled = false;
				lamp2.GetComponent<Light>().enabled = false;
				lamp3.GetComponent<Light>().enabled = false;
				strangeLight.SetActive(value: false);
				areaLights.SetActive(value: false);
			}
			scientists.SetActive(value: false);
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (schalter.GetComponent<Schalter>().isOn)
		{
			rendLampe.sharedMaterial = material[1];
			lamp1.GetComponent<Light>().enabled = true;
			lamp2.GetComponent<Light>().enabled = true;
			lamp3.GetComponent<Light>().enabled = true;
			areaLights.SetActive(value: true);
			strangeLight.SetActive(value: true);
			guard.SetActive(value: false);
			rendLampMesh.sharedMaterial = material[2];
			engineIsOn = true;
			door_1.GetComponent<Animator>().SetBool("isOpen", value: true);
			door_2.GetComponent<Animator>().SetBool("isOpen", value: true);
			scientists.SetActive(value: true);
			lyingScientists.SetActive(value: false);
			challangeDoor.SetActive(value: false);
			motorSound.Play();
		}
		else
		{
			TriggerDialoge();
		}
	}

	public void TriggerDialoge()
	{
		UnityEngine.Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
