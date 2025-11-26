using UnityEngine;

public class PosAndStateManager : MonoBehaviour
{
	public GameObject Mayu;

	private void Start()
	{
		if (BackDtToInBetween.backFromDt)
		{
			PlayerManager.instance.player.GetComponent<CharacterController>().enabled = false;
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
			PlayerManager.instance.player.GetComponent<CharacterController>().enabled = true;
			BackDtToInBetween.backFromDt = false;
		}
		if (BackDtToInBetween.backFromInBetween)
		{
			PlayerManager.instance.player.GetComponent<CharacterController>().enabled = false;
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
			PlayerManager.instance.player.GetComponent<CharacterController>().enabled = true;
			if (Mayu != null)
			{
				Mayu.SetActive(value: false);
			}
			BackDtToInBetween.backFromInBetween = false;
		}
	}
}
