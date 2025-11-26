using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Image SexImage;

	public Image SexCircle;

	public Image HumBuff;

	private void Start()
	{
		SexImage.enabled = false;
		SexCircle.enabled = false;
		HumBuff.enabled = false;
	}
}
