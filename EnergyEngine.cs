using System;
using UnityEngine;

public class EnergyEngine : Interactable, ISaveable
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

	public GameObject lamp1;

	public GameObject lampMesh1;

	private Renderer rendLamp1;

	public GameObject lamp2;

	public GameObject lampMesh2;

	private Renderer rendLamp2;

	public GameObject lamp3;

	public GameObject lampMesh3;

	private Renderer rendLamp3;

	public GameObject lamp4;

	public GameObject lampMesh4;

	private Renderer rendLamp4;

	public AudioSource motorSound;

	[SerializeField]
	public static bool engineIsOn;

	public GameObject door_1;

	public GameObject door_2;

	public GameObject enemy_1;

	public GameObject enemy_2;

	public GameObject rightTunnelHugger1;

	public GameObject rightTunnelHugger2;

	public GameObject tunnel_normal;

	public GameObject tunnel_distorted;

	public GameObject labor_Insects;

	public GameObject Mayu;

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
		rendLampe.enabled = true;
		rendLamp1 = lampMesh1.GetComponent<Renderer>();
		rendLamp1.enabled = true;
		rendLamp2 = lampMesh2.GetComponent<Renderer>();
		rendLamp2.enabled = true;
		rendLamp3 = lampMesh3.GetComponent<Renderer>();
		rendLamp3.enabled = true;
		rendLamp4 = lampMesh4.GetComponent<Renderer>();
		rendLamp4.enabled = true;
		if (!engineIsOn)
		{
			rendLampe.sharedMaterial = material[0];
			rendLamp1.sharedMaterial = material[2];
			lamp1.GetComponent<Light>().enabled = false;
			rendLamp2.sharedMaterial = material[2];
			lamp2.GetComponent<Light>().enabled = false;
			rendLamp3.sharedMaterial = material[2];
			lamp3.GetComponent<Light>().enabled = false;
			rendLamp4.sharedMaterial = material[2];
			lamp4.GetComponent<Light>().enabled = false;
			labor_Insects.SetActive(value: false);
			tunnel_distorted.SetActive(value: false);
		}
		else
		{
			schalter.GetComponent<Schalter>().isOn = true;
			schalter.GetComponent<Schalter>().repaired = true;
			rendLampe.sharedMaterial = material[1];
			lamp1.GetComponent<Light>().enabled = true;
			rendLamp1.sharedMaterial = material[3];
			lamp2.GetComponent<Light>().enabled = true;
			rendLamp2.sharedMaterial = material[3];
			lamp3.GetComponent<Light>().enabled = true;
			rendLamp3.sharedMaterial = material[3];
			lamp4.GetComponent<Light>().enabled = true;
			rendLamp4.sharedMaterial = material[3];
			door_1.GetComponent<Animator>().SetBool("isOpen", value: true);
			door_2.GetComponent<Animator>().SetBool("isOpen", value: true);
			enemy_1.gameObject.SetActive(value: true);
			enemy_2.gameObject.SetActive(value: true);
			UnityEngine.Object.Destroy(tunnel_normal);
			tunnel_distorted.gameObject.SetActive(value: true);
			labor_Insects.gameObject.SetActive(value: true);
			rightTunnelHugger1.SetActive(value: false);
			rightTunnelHugger2.SetActive(value: false);
			motorSound.Play();
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (schalter.GetComponent<Schalter>().isOn)
		{
			rendLampe.sharedMaterial = material[1];
			lamp1.GetComponent<Light>().enabled = true;
			rendLamp1.sharedMaterial = material[3];
			lamp2.GetComponent<Light>().enabled = true;
			rendLamp2.sharedMaterial = material[3];
			lamp3.GetComponent<Light>().enabled = true;
			rendLamp3.sharedMaterial = material[3];
			lamp4.GetComponent<Light>().enabled = true;
			rendLamp4.sharedMaterial = material[3];
			engineIsOn = true;
			door_1.GetComponent<Animator>().SetBool("isOpen", value: true);
			door_2.GetComponent<Animator>().SetBool("isOpen", value: true);
			enemy_1.gameObject.SetActive(value: true);
			enemy_2.gameObject.SetActive(value: true);
			UnityEngine.Object.Destroy(tunnel_normal);
			tunnel_distorted.gameObject.SetActive(value: true);
			labor_Insects.gameObject.SetActive(value: true);
			Mayu.SetActive(value: false);
			rightTunnelHugger1.SetActive(value: false);
			rightTunnelHugger2.SetActive(value: false);
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
