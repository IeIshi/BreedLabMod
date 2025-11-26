using UnityEngine;

public class ScXPhansCam : Interactable
{
	public Camera mainCamera;

	public Camera peakCamera1;

	public GameObject PeakControls;

	private bool peakCamActive;

	private void Start()
	{
		mainCamera.enabled = true;
		PeakControls.SetActive(value: false);
		peakCamera1.enabled = false;
		peakCamera1.GetComponent<AudioListener>().enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		Debug.Log("INTERACTED");
		ActivePeakCam();
	}

	private void LateUpdate()
	{
		if (peakCamActive && Input.GetKeyDown(KeyCode.X))
		{
			DeactivatePeakCam();
		}
	}

	private void ActivePeakCam()
	{
		mainCamera.enabled = false;
		mainCamera.GetComponent<AudioListener>().enabled = false;
		peakCamera1.enabled = true;
		peakCamera1.GetComponent<AudioListener>().enabled = true;
		PeakControls.SetActive(value: true);
		PlayerManager.instance.player.GetComponent<PlayerController>().enabled = false;
		peakCamActive = true;
	}

	private void DeactivatePeakCam()
	{
		mainCamera.enabled = true;
		mainCamera.GetComponent<AudioListener>().enabled = true;
		peakCamera1.enabled = false;
		peakCamera1.GetComponent<AudioListener>().enabled = false;
		PeakControls.SetActive(value: false);
		PlayerManager.instance.player.GetComponent<PlayerController>().enabled = true;
		peakCamActive = false;
	}
}
