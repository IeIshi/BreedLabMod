using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public static DialogManager instance;

	public Text nameText;

	public Text dialogueText;

	public Image dialogueImage;

	public Image defaultImage;

	public Animator animator;

	private Queue<string> sentences;

	public static bool inDialogue;

	public AudioSource typeSound;

	private void Start()
	{
		sentences = new Queue<string>();
		instance = this;
	}

	public void StartDialogue(Dialogue dialogue)
	{
		inDialogue = true;
		animator.SetBool("IsOpen", value: true);
		nameText.text = dialogue.name;
		if (dialogue.image != null)
		{
			dialogueImage.sprite = dialogue.image.sprite;
		}
		else
		{
			dialogueImage.sprite = defaultImage.sprite;
		}
		sentences.Clear();
		string[] array = dialogue.sentences;
		foreach (string item in array)
		{
			sentences.Enqueue(item);
		}
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	private IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		char[] array = sentence.ToCharArray();
		foreach (char c in array)
		{
			dialogueText.text += c;
			typeSound.Play();
			yield return new WaitForSeconds(0.02f);
		}
	}

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", value: false);
		StartCoroutine(wait());
		inDialogue = false;
	}

	private IEnumerator wait()
	{
		yield return new WaitForSeconds(1f);
		dialogueImage.sprite = defaultImage.sprite;
	}
}
