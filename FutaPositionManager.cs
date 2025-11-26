using UnityEngine;

public class FutaPositionManager : MonoBehaviour
{
	private void Start()
	{
		PlayerManager.instance.player.GetComponent<CharacterController>().enabled = false;
		PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
		PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
		PlayerManager.instance.player.GetComponent<CharacterController>().enabled = true;
	}
}
