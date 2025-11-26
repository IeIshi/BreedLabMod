using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateFinalLevel : MonoBehaviour
{
	public GameObject blackScreen;

	private Image bsImage;

	private float duration = 6f;

	private void Start()
	{
		blackScreen.SetActive(value: true);
		bsImage = blackScreen.GetComponent<Image>();
		StartCoroutine(FadeImage());
	}

	private IEnumerator FadeImage()
	{
		float startAlpha = bsImage.color.a;
		float rate = 1f / duration;
		float progress = 0f;
		while (progress < 1f)
		{
			Color color = bsImage.color;
			color.a = Mathf.Lerp(startAlpha, 0f, progress);
			bsImage.color = color;
			progress += rate * Time.deltaTime;
			yield return null;
		}
		Color color2 = bsImage.color;
		color2.a = 0f;
		bsImage.color = color2;
	}
}
