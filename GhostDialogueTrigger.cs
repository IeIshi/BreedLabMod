using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GhostDialogueTrigger : Interactable
{
	public Dialogue dialogue;

	public AudioSource ghostSound;

	public AudioSource heartBeat;

	private bool dialogueTriggered;

	public GameObject postProcessMng;

	private PostProcessingManager postProcess;

	public GameObject Heroine;

	public GameObject bs;

	private Image blackScreen;

	private Color c;

	private bool setAlphaToZero;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public bool isSecondBroothmother;

	private void Start()
	{
		postProcess = postProcessMng.GetComponent<PostProcessingManager>();
		blackScreen = bs.GetComponent<Image>();
		c = blackScreen.color;
	}

	private void FixedUpdate()
	{
		if (!dialogueTriggered || DialogManager.inDialogue)
		{
			return;
		}
		base.gameObject.layer = 0;
		postProcess.ps.SetActive(value: true);
		if (isSecondBroothmother)
		{
			postProcess.OrgasmEffect();
			if (!heartBeat.isPlaying)
			{
				heartBeat.Play();
				StartCoroutine(JumpToNextScene());
			}
			return;
		}
		postProcess.OrgasmEffect();
		Heroine.GetComponent<HeroineStats>().GainLust(35f);
		Heroine.GetComponent<HeroineStats>().GainOrg(35f);
		if (!heartBeat.isPlaying)
		{
			heartBeat.Play();
		}
		if (HeroineStats.orgasm)
		{
			Heroine.GetComponent<PlayerController>().enabled = false;
			Heroine.GetComponent<PlayerController>().enabled = false;
			PlayerController.iFalled = true;
			Heroine.GetComponent<Animator>().SetBool("isFalledBack", value: true);
			StartCoroutine(JumpToNextScene());
		}
	}

	public override void Interact()
	{
		base.Interact();
		PlayerManager.BroodmotherMet = true;
		PlayerManager.loadedrly = false;
		TriggerDialoge();
		ghostSound.Play();
		dialogueTriggered = true;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private IEnumerator JumpToNextScene()
	{
		bs.SetActive(value: true);
		blackScreen.color = c;
		if (!setAlphaToZero)
		{
			c.a = 0f;
			setAlphaToZero = true;
		}
		c.a += 0.1f * Time.deltaTime;
		yield return new WaitForSeconds(6f);
		HeroineStats.oralCreampie = false;
		HeroineStats.creampied = false;
		if (isSecondBroothmother)
		{
			EndingHandler.triggerTrueEnding = true;
			SceneManager.LoadScene("FinalLevel");
		}
		else
		{
			SceneManager.LoadScene("AfterBMMH");
		}
	}

	public void LoadLevel(string sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(string sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingScreen.SetActive(value: true);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			loadingSlider.value = value;
			loadingSlider.value = value;
			yield return null;
		}
	}
}
