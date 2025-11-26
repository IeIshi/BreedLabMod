using UnityEngine;

public class FromOutsideHeroinePlace : MonoBehaviour
{
	private void Start()
	{
		Debug.Log("Entering from outside? " + EnterBotanicMain.enteringFromOutside);
		if (EnterBotanicMain.enteringFromOutside)
		{
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.position;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.rotation;
			EnterBotanicMain.enteringFromOutside = false;
		}
	}
}
