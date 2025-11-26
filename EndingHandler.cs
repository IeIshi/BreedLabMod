using UnityEngine;

public class EndingHandler : MonoBehaviour
{
	public static bool triggerTrueEnding;

	public static bool triggerScientistEnding;

	public static bool triggerVirginEnding;

	public GameObject DemonHeroine;

	public GameObject NormalHeroine;

	public GameObject NormalHeroineCameraFollow;

	public GameObject Camera;

	public GameObject BlackScreen;

	public GameObject TrueEndingStuff;

	public GameObject ScientistEndingStuff;

	public GameObject VirginEndingStuff;

	private void Awake()
	{
		if (triggerScientistEnding)
		{
			ScientistEndingStuff.SetActive(value: true);
			Object.Destroy(DemonHeroine);
			Camera.GetComponent<CameraFollow>().cameraFollowObj = NormalHeroineCameraFollow;
			Camera.GetComponent<CameraFollow>().heroinePos = NormalHeroine;
			BlackScreen.SetActive(value: false);
			TrueEndingStuff.SetActive(value: false);
			VirginEndingStuff.SetActive(value: false);
		}
		if (triggerTrueEnding)
		{
			Object.Destroy(NormalHeroine);
			ScientistEndingStuff.SetActive(value: false);
			TrueEndingStuff.SetActive(value: true);
			VirginEndingStuff.SetActive(value: false);
		}
		if (triggerVirginEnding)
		{
			Object.Destroy(DemonHeroine);
			ScientistEndingStuff.SetActive(value: false);
			BlackScreen.SetActive(value: false);
			TrueEndingStuff.SetActive(value: false);
			Camera.GetComponent<CameraFollow>().cameraFollowObj = NormalHeroineCameraFollow;
			Camera.GetComponent<CameraFollow>().heroinePos = NormalHeroine;
			VirginEndingStuff.SetActive(value: true);
		}
	}
}
