using UnityEngine;

public class Flashlight : MonoBehaviour
{
	public GameObject flashlight;

	private GameObject flashLightIcon;

	private AudioSource onSound;

	public static bool FlashOn;

	private void Start()
	{
		flashLightIcon = GameObject.Find("ManagerAndUI/UI/Canvas/StatImages/FlashlightIcon");
		flashLightIcon.SetActive(value: false);
		onSound = GameObject.Find("ManagerAndUI/Audio/InvClose").GetComponent<AudioSource>();
		flashlight.SetActive(value: false);
		FlashOn = false;
	}

	private void Update()
	{
		if (!EquipmentManager.flashNeckOn)
		{
			flashlight.SetActive(value: false);
		}
		else if (PlayerController.iFalled)
		{
			FlashOn = false;
			flashlight.SetActive(value: false);
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			if (!FlashOn)
			{
				FlashOn = true;
				flashlight.SetActive(value: true);
				onSound.Play();
			}
			else
			{
				FlashOn = false;
				flashlight.SetActive(value: false);
				onSound.Play();
			}
		}
	}
}
