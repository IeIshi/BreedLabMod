using UnityEngine;

public class OnCollision : MonoBehaviour
{
	public GameObject myBody;

	private GameObject gameManager;

	private bool hitFront;

	private bool hitBack;

	private EnemyFieldOfView myState;

	private void Start()
	{
		gameManager = GameObject.Find("Game Manager");
		myState = myBody.GetComponent<EnemyFieldOfView>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (PlayerController.iFalled || !(other.tag == "Player"))
		{
			return;
		}
		CheckHitAngle();
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.24f, 0.1f, 3f, 1f, 0.3f, 0.233f, 0.335f, 0.122f));
		if (!EquipmentManager.heroineIsNaked && gameManager.GetComponent<EquipmentManager>().currentEquipment[1] != null)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[1].id != 3648532)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(1);
				return;
			}
			PlayerController.iFalled = true;
			myState.claimedHer = true;
			if (hitFront)
			{
				if (!HeroineStats.pregnant)
				{
					PlayerController.gotHitFront = true;
					PlayerController.gotHitBack = false;
				}
				else
				{
					PlayerController.gotHitFront = false;
					PlayerController.gotHitBack = true;
				}
			}
			if (hitBack)
			{
				PlayerController.gotHitFront = false;
				PlayerController.gotHitBack = true;
			}
		}
		PlayerController.iFalled = true;
		myState.claimedHer = true;
		if (hitFront)
		{
			if (!HeroineStats.pregnant)
			{
				PlayerController.gotHitFront = true;
				PlayerController.gotHitBack = false;
			}
			else
			{
				PlayerController.gotHitFront = false;
				PlayerController.gotHitBack = true;
			}
		}
		if (hitBack)
		{
			PlayerController.gotHitFront = false;
			PlayerController.gotHitBack = true;
		}
	}

	private void CheckHitAngle()
	{
		Vector3 forward = PlayerManager.instance.player.transform.forward;
		Vector3 to = PlayerManager.instance.player.transform.position - base.transform.position;
		float f = Vector3.SignedAngle(forward, to, Vector3.up);
		if (Mathf.Abs(f) > 80f)
		{
			hitFront = true;
			hitBack = false;
			Debug.Log("Hit Front");
		}
		if (Mathf.Abs(f) <= 80f)
		{
			hitFront = false;
			hitBack = true;
			Debug.Log("Hit Back");
		}
	}
}
