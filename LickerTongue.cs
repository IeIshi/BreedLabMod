using UnityEngine;

public class LickerTongue : MonoBehaviour
{
	private GameObject gameManager;

	public GameObject Licker;

	public GameObject deadStatusCheck;

	private void Start()
	{
		gameManager = GameObject.Find("ManagerAndUI/Game Manager");
	}

	public void OnTriggerEnter(Collider other)
	{
		if (PlayerController.iFalled || !(other.tag == "Player") || deadStatusCheck.GetComponent<EnemSavState>().isDead)
		{
			return;
		}
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
		if (!EquipmentManager.heroineIsNaked && gameManager.GetComponent<EquipmentManager>().currentEquipment[1] != null)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id == 3648532)
			{
				if (!PlayerController.iFalled)
				{
					Licker.GetComponent<NewLickerControl>().sheMine = true;
					PlayerController.iFalled = true;
					PlayerController.gotHitFront = true;
				}
			}
			else if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id != 3648532)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(1);
			}
		}
		else if (!PlayerController.iFalled)
		{
			Licker.GetComponent<NewLickerControl>().sheMine = true;
			PlayerController.iFalled = true;
			PlayerController.gotHitFront = true;
		}
	}
}
