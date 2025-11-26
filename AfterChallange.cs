using UnityEngine;

public class AfterChallange : MonoBehaviour
{
	public GameObject heroine;

	public GameObject interactDoorOpen;

	public GameObject interactDoorClosed;

	public GameObject LurkerGuardHideG1;

	public GameObject LurkerGuardHideG2;

	public GameObject LurkerGuardOutside;

	public GameObject spawningBoxes;

	public GameObject lamp1;

	private void Start()
	{
		if (PlayerManager.enteredChallangeRoom && !PlayerManager.ScSexAfterMath)
		{
			heroine.transform.localPosition = base.gameObject.transform.localPosition;
			interactDoorOpen.SetActive(value: true);
			interactDoorClosed.SetActive(value: false);
			LurkerGuardHideG1.SetActive(value: true);
			LurkerGuardHideG2.SetActive(value: true);
			LurkerGuardOutside.SetActive(value: false);
			spawningBoxes.SetActive(value: true);
			lamp1.SetActive(value: true);
			Debug.Log("Loaded from AfterChallange");
		}
		if (!PlayerManager.enteredChallangeRoom)
		{
			interactDoorOpen.SetActive(value: false);
			interactDoorClosed.SetActive(value: true);
			LurkerGuardHideG1.SetActive(value: false);
			LurkerGuardHideG2.SetActive(value: false);
			LurkerGuardOutside.SetActive(value: true);
			spawningBoxes.SetActive(value: false);
		}
	}
}
