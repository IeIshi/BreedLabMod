using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
	public GameObject blackScreen;

	public GameObject QuitButton;

	public GameObject GameOverTxt;

	public GameObject MainMenuButton;

	private Image bsImage;

	public float duration = 6f;

	private void Start()
	{
		bsImage = blackScreen.GetComponent<Image>();
		StartCoroutine(FadeImage3());
		QuitButton.SetActive(value: false);
		GameOverTxt.SetActive(value: false);
		MainMenuButton.SetActive(value: false);
	}

	private IEnumerator FadeImage3()
	{
		yield return new WaitForSeconds(3f);
		float startAlpha = bsImage.color.a;
		float rate = 1f / duration;
		float progress = 0f;
		while (progress < 1f)
		{
			Color color = bsImage.color;
			color.a = Mathf.Lerp(startAlpha, 0f, progress);
			bsImage.color = color;
			progress += rate * Time.deltaTime;
			if (progress > 0.7f)
			{
				QuitButton.SetActive(value: true);
				GameOverTxt.SetActive(value: true);
				MainMenuButton.SetActive(value: true);
			}
			yield return null;
		}
		Color color2 = bsImage.color;
		color2.a = 0f;
		bsImage.color = color2;
		blackScreen.SetActive(value: false);
		GameOverTxt.SetActive(value: true);
		QuitButton.SetActive(value: true);
	}
}
