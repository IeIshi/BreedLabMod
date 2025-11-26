using UnityEngine;

public class SpecialDoor : Interactable
{
	public Dialogue dialogue;

	public Dialogue dia1;

	public Dialogue dia2;

	public Dialogue dia3;

	public Dialogue dia4;

	public Dialogue dia5;

	public Camera mainCamera;

	public Camera peakCamera1;

	public Camera peakCamera2;

	public Camera peakCamera3;

	public Camera peakCamera4;

	public AudioSource ambientMusic;

	public GameObject activateCoupleTrigger;

	public GameObject Heroine;

	public GameObject Scientist;

	public GameObject Mayu;

	private bool peaked;

	private bool startMayuScScene;

	private float progressTimer;

	public float firstDiaIn;

	public float secondDiaIn;

	public float thirdDiaIn;

	public float fourthDiaIn;

	public float fifthDiaIn;

	private bool d1;

	private bool d2;

	private bool d3;

	private bool d4;

	private bool d5;

	private bool stopDialogue;

	private bool controlPanelActive;

	public GameObject PeakControls;

	private void Start()
	{
		mainCamera.enabled = true;
		PeakControls.SetActive(value: false);
		peakCamera1.enabled = false;
		peakCamera1.GetComponent<AudioListener>().enabled = false;
		peakCamera2.enabled = false;
		peakCamera2.GetComponent<AudioListener>().enabled = false;
		peakCamera3.enabled = false;
		peakCamera3.GetComponent<AudioListener>().enabled = false;
		peakCamera4.enabled = false;
		peakCamera4.GetComponent<AudioListener>().enabled = false;
	}

	private void LateUpdate()
	{
		if (startMayuScScene)
		{
			if (!ambientMusic.isPlaying)
			{
				ambientMusic.Play();
			}
			if (!controlPanelActive)
			{
				PeakControls.SetActive(value: true);
				controlPanelActive = true;
			}
			progressTimer += Time.deltaTime;
			Debug.Log("Progress Timer: " + progressTimer);
			if (!stopDialogue)
			{
				ProgressDialogue();
			}
			ChangeCameraView();
		}
		else if (peaked && !DialogManager.inDialogue)
		{
			mainCamera.enabled = false;
			mainCamera.GetComponent<AudioListener>().enabled = false;
			peakCamera1.enabled = true;
			peakCamera1.GetComponent<AudioListener>().enabled = true;
			Heroine.GetComponent<PlayerController>().enabled = false;
			startMayuScScene = true;
		}
	}

	private void ChangeCameraView()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			ambientMusic.Stop();
			mainCamera.enabled = true;
			mainCamera.GetComponent<AudioListener>().enabled = true;
			PeakControls.SetActive(value: false);
			Heroine.GetComponent<PlayerController>().enabled = true;
			peakCamera1.enabled = false;
			peakCamera1.GetComponent<AudioListener>().enabled = false;
			peakCamera2.enabled = false;
			peakCamera2.GetComponent<AudioListener>().enabled = false;
			peakCamera3.enabled = false;
			peakCamera3.GetComponent<AudioListener>().enabled = false;
			peakCamera4.enabled = false;
			peakCamera4.GetComponent<AudioListener>().enabled = false;
			stopDialogue = true;
			base.gameObject.GetComponent<SpecialDoor>().enabled = false;
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			if (peakCamera1.isActiveAndEnabled)
			{
				peakCamera1.enabled = false;
				peakCamera1.GetComponent<AudioListener>().enabled = false;
				peakCamera2.enabled = true;
				peakCamera2.GetComponent<AudioListener>().enabled = true;
			}
			else if (peakCamera2.isActiveAndEnabled)
			{
				peakCamera2.enabled = false;
				peakCamera2.GetComponent<AudioListener>().enabled = false;
				peakCamera3.enabled = true;
				peakCamera3.GetComponent<AudioListener>().enabled = true;
			}
			else if (peakCamera3.isActiveAndEnabled)
			{
				peakCamera3.enabled = false;
				peakCamera3.GetComponent<AudioListener>().enabled = false;
				peakCamera4.enabled = true;
				peakCamera4.GetComponent<AudioListener>().enabled = true;
			}
			else if (peakCamera4.isActiveAndEnabled)
			{
				peakCamera4.enabled = false;
				peakCamera4.GetComponent<AudioListener>().enabled = false;
				peakCamera1.enabled = true;
				peakCamera1.GetComponent<AudioListener>().enabled = true;
			}
		}
	}

	private void ProgressDialogue()
	{
		if (progressTimer > fifthDiaIn)
		{
			if (!d5)
			{
				TriggerMayuXScDia5();
				Mayu.GetComponent<Animator>().speed = 1.5f;
				Scientist.GetComponent<Animator>().speed = 1.5f;
				d5 = true;
			}
		}
		else if (progressTimer > fourthDiaIn)
		{
			if (!d4)
			{
				TriggerMayuXScDia4();
				d4 = true;
			}
		}
		else if (progressTimer > thirdDiaIn)
		{
			if (!d3)
			{
				TriggerMayuXScDia3();
				d3 = true;
			}
		}
		else if (progressTimer > secondDiaIn)
		{
			if (!d2)
			{
				TriggerMayuXScDia2();
				d2 = true;
			}
		}
		else if (progressTimer > firstDiaIn && !d1)
		{
			TriggerMayuXScDia1();
			d1 = true;
		}
	}

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
		if (activateCoupleTrigger.GetComponent<ActivateSexCouple>().coupleActivated)
		{
			peaked = true;
		}
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerMayuXScDia1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dia1);
	}

	public void TriggerMayuXScDia2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dia2);
	}

	public void TriggerMayuXScDia3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dia3);
	}

	public void TriggerMayuXScDia4()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dia4);
	}

	public void TriggerMayuXScDia5()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dia5);
	}
}
