using UnityEngine;

public class ActivateMasturbatingCouple : MonoBehaviour
{
	public GameObject masturbatingCouple;

	public GameObject progControl;

	public GameObject theLicekrs;

	public GameObject licker1;

	public GameObject licker2;

	public AudioSource switchOn;

	private void Start()
	{
		masturbatingCouple.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player") || progControl.GetComponent<ChallageRoomProgControl>().doubleDoorOpened)
		{
			return;
		}
		if (masturbatingCouple != null)
		{
			masturbatingCouple.SetActive(value: true);
			switchOn.Play();
		}
		if (theLicekrs.gameObject.activeSelf)
		{
			licker1.GetComponent<NewLickerControl>().staminaUi.gameObject.SetActive(value: false);
			licker2.GetComponent<NewLickerControl>().staminaUi.gameObject.SetActive(value: false);
			licker1.GetComponent<NewLickerControl>().healthUI.gameObject.SetActive(value: false);
			licker2.GetComponent<NewLickerControl>().healthUI.gameObject.SetActive(value: false);
			Object.Destroy(theLicekrs);
			progControl.GetComponent<ChallageRoomProgControl>().lickersSpawned = false;
			if (InventoryUI.heroineIsChased)
			{
				InventoryUI.heroineIsChased = false;
			}
		}
	}
}
