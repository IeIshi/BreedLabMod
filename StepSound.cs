using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepSound : MonoBehaviour
{
	public AudioSource stepSound;

	public AudioSource snapSound;

	private Animator animator;

	private Image imageToFade;

	public float fadeDuration = 2f;

	private GameObject DecailEffect;

	private Color preserveColor;

	private void Start()
	{
		animator = GetComponent<Animator>();
		DecailEffect = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect");
		imageToFade = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect").GetComponent<Image>();
		preserveColor = imageToFade.color;
	}

	public void StepEvent()
	{
		if (animator.GetFloat("speedPercent") > 0.2f || animator.GetFloat("speedPercent") < 0.6f)
		{
			stepSound.Play();
		}
		else if (animator.GetFloat("speedPercent") > 0.6f)
		{
			stepSound.Play();
		}
	}

	public void SnapEvent()
	{
		snapSound.Play();
		DecailEffect.SetActive(value: true);
		StartCoroutine(FadeOutCoroutine());
	}

	private IEnumerator FadeOutCoroutine()
	{
		Color originalColor = imageToFade.color;
		float elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			float a = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
			imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);
			yield return null;
		}
		imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
		imageToFade.color = preserveColor;
		imageToFade.gameObject.SetActive(value: false);
		DecailEffect.SetActive(value: false);
	}
}
