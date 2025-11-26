using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EffectControl : MonoBehaviour
{
	public TextMeshProUGUI text1;

	public TextMeshProUGUI text2;

	public Image img;

	public float fadeDuration = 2f;

	public Volume volume;

	private ShadowsMidtonesHighlights shadow;

	private UnityEngine.Rendering.PostProcessing.Bloom bloom;

	public AudioSource heheSound;

	public AudioSource notHeheSound;

	public AudioSource endingSong;

	public GameObject Masks;

	public GameObject Credits;

	private void Start()
	{
		Masks.SetActive(value: false);
		Credits.SetActive(value: false);
		Color color = text1.color;
		color.a = 0f;
		text1.color = color;
		Color color2 = text2.color;
		color2.a = 0f;
		text2.color = color2;
		volume.profile.TryGet<ShadowsMidtonesHighlights>(out shadow);
		heheSound.Play();
		StartCoroutine(FadeOutBlack());
	}

	private IEnumerator FadeInText()
	{
		float elapsedTime = 0f;
		Color color = text1.color;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
			text1.color = color;
			yield return null;
		}
		color.a = 1f;
		text1.color = color;
		StartCoroutine(DecreaseShadowsXOverTime(5f));
	}

	private IEnumerator FadeInBlack()
	{
		float elapsedTime = 0f;
		Color color = img.color;
		while (elapsedTime < 6f)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Clamp01(elapsedTime / 6f);
			img.color = color;
			yield return null;
		}
		color.a = 255f;
		img.color = color;
		StartCoroutine(ShowCredits());
	}

	private IEnumerator ShowCredits()
	{
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("EndingCredits");
	}

	private IEnumerator FadeOutBlack()
	{
		float startAlpha = img.color.a;
		float rate = 1f / 7f;
		float progress = 0f;
		while (progress < 1f)
		{
			Color color = img.color;
			color.a = Mathf.Lerp(startAlpha, 0f, progress);
			img.color = color;
			progress += rate * Time.deltaTime;
			yield return null;
		}
		Color color2 = img.color;
		color2.a = 0f;
		img.color = color2;
		StartCoroutine(FadeInText());
	}

	private IEnumerator FadeInText2()
	{
		float elapsedTime = 0f;
		Color color = text2.color;
		heheSound.Stop();
		notHeheSound.Play();
		Color color2 = text1.color;
		color2.a = 0f;
		text1.color = color2;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
			text2.color = color;
			yield return null;
		}
		color.a = 1f;
		text2.color = color;
		Debug.Log("DURATION2 ENDED");
		StartCoroutine(FadeInBlack());
	}

	private IEnumerator DecreaseShadowsXOverTime(float duration)
	{
		float elapsedTime = 0f;
		heheSound.volume -= Time.deltaTime * 10f;
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float num = Mathf.Lerp(0f, -0.85f, elapsedTime / duration);
			Debug.Log(num);
			shadow.shadows.value = new Vector4(1f, 1f, 1f, num);
			yield return null;
		}
		StartCoroutine(FadeInText2());
	}
}
