using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VirginSoldierDialogue : MonoBehaviour
{
	private Animator animator;

	public GameObject Heroine;

	public GameObject FemSoldier;

	public GameObject FemSWeapon;

	public GameObject Scanner;

	public AudioSource raySound;

	public Dialogue dialogue1;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	public Dialogue dialogue4;

	public Dialogue dialogue5;

	public Dialogue dialogue6;

	public GameObject blackScreen;

	private Image bsImage;

	private float duration = 6f;

	private bool dialogueTriggered;

	private bool routineStarted;

	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("anschlag", value: true);
		Heroine.GetComponent<PlayerController>().enabled = false;
		Heroine.GetComponent<Animator>().SetBool("isNaked", value: true);
		Scanner.SetActive(value: false);
		bsImage = blackScreen.GetComponent<Image>();
		StartCoroutine(StartDialogue());
	}

	private IEnumerator StartDialogue()
	{
		yield return new WaitForSeconds(5f);
		TriggerDialoge1();
		dialogueTriggered = true;
		Debug.Log(bsImage.color);
		yield return new WaitForSeconds(5f);
		TriggerDialoge2();
		yield return new WaitForSeconds(5f);
		TriggerDialoge3();
		yield return new WaitForSeconds(5f);
		FemSoldier.GetComponent<Animator>().SetBool("scan", value: true);
		Scanner.SetActive(value: true);
		FemSWeapon.SetActive(value: false);
		raySound.Play();
		yield return new WaitForSeconds(6f);
		TriggerDialoge4();
		yield return new WaitForSeconds(5f);
		TriggerDialoge5();
		yield return new WaitForSeconds(3f);
		FemSoldier.GetComponent<Animator>().SetBool("scan", value: false);
		Scanner.SetActive(value: false);
		FemSWeapon.SetActive(value: true);
		raySound.Stop();
		yield return new WaitForSeconds(3f);
		animator.SetBool("backTo", value: true);
		animator.SetBool("anschlag", value: false);
		TriggerDialoge6();
		yield return new WaitForSeconds(3f);
		DialogManager.instance.EndDialogue();
		StartCoroutine(FadeImageThenEnd());
	}

	private IEnumerator FadeImageThenEnd()
	{
		float startAlpha = 0f;
		float rate = 1f / duration;
		float progress = 0f;
		while (progress < 1f)
		{
			Debug.Log(progress);
			Color color = bsImage.color;
			color.a = Mathf.Lerp(startAlpha, 1f, progress);
			bsImage.color = color;
			progress += rate * Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadScene("VirginEnding");
	}

	public void TriggerDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue1);
	}

	public void TriggerDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
	}

	public void TriggerDialoge3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
	}

	public void TriggerDialoge4()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue4);
	}

	public void TriggerDialoge5()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue5);
	}

	public void TriggerDialoge6()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue6);
	}
}
