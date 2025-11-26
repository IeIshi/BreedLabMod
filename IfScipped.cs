using UnityEngine;

public class IfScipped : MonoBehaviour
{
	public GameObject neckLace;

	private void Start()
	{
		if (MainMenu.levelSkipped)
		{
			base.gameObject.SetActive(value: true);
			if (!PlayerManager.ScSexAfterMath && !BackDtToInBetween.backFromDt && neckLace != null)
			{
				neckLace.SetActive(value: false);
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
