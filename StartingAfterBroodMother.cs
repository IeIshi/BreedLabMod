using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingAfterBroodMother : MonoBehaviour
{
	public Dialogue dialogue1;

	public Dialogue dialogue2;

	public Dialogue dialogue3;

	private bool d1Triggered;

	private bool d2Triggered;

	private bool d3Triggered;

	private float timer;

	private float camChangeTimer;

	public Camera MainCamera;

	public Camera StoryCamera1;

	public Camera StoryCamera2;

	public Camera StoryCamera3;

	public AudioSource SlickSound;

	public GameObject bs;

	private Image blackScreen;

	private Color c;

	private bool bs1Done;

	public GameObject Mayu;

	public GameObject MayuTwo;

	public GameObject PeakControls;

	public GameObject GameManager;

	public GameObject stuff1;

	public GameObject stuff2;

	public GameObject stuff3;

	public GameObject stuff4;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	private void Awake()
	{
		if (!PlayerManager.MasturbationFinished && !PlayerManager.BroodmotherMet)
		{
			PeakControls.SetActive(value: false);
			StoryCamera2.enabled = false;
			StoryCamera2.GetComponent<AudioListener>().enabled = false;
			StoryCamera3.enabled = false;
			StoryCamera3.GetComponent<AudioListener>().enabled = false;
			StoryCamera1.enabled = false;
			StoryCamera1.GetComponent<AudioListener>().enabled = false;
			base.gameObject.SetActive(value: false);
			Mayu.SetActive(value: false);
			MayuTwo.SetActive(value: false);
		}
		else if (!PlayerManager.MasturbationFinished && PlayerManager.BroodmotherMet)
		{
			stuff1.SetActive(value: false);
			stuff2.SetActive(value: false);
			stuff3.SetActive(value: false);
			stuff4.SetActive(value: false);
			GameManager.GetComponent<EquipmentManager>().enabled = false;
		}
	}

	private void Start()
	{
		if (!PlayerManager.MasturbationFinished && !PlayerManager.BroodmotherMet)
		{
			PeakControls.SetActive(value: false);
			StoryCamera2.enabled = false;
			StoryCamera2.GetComponent<AudioListener>().enabled = false;
			StoryCamera3.enabled = false;
			StoryCamera3.GetComponent<AudioListener>().enabled = false;
			StoryCamera1.enabled = false;
			StoryCamera1.GetComponent<AudioListener>().enabled = false;
			base.gameObject.SetActive(value: false);
			Mayu.SetActive(value: false);
			MayuTwo.SetActive(value: false);
		}
		else if (PlayerManager.MasturbationFinished)
		{
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
			PlayerController.iFalled = true;
			HeroineStats.masturbating = false;
			PeakControls.SetActive(value: false);
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("falled", value: true);
			HeroineStats.currentLust = 0f;
			PlayerController.iGetFucked = false;
			HeroineStats.stunned = false;
			StoryCamera2.enabled = false;
			StoryCamera2.GetComponent<AudioListener>().enabled = false;
			StoryCamera3.enabled = false;
			StoryCamera3.GetComponent<AudioListener>().enabled = false;
			StoryCamera1.enabled = false;
			StoryCamera1.GetComponent<AudioListener>().enabled = false;
			Mayu.SetActive(value: false);
			MayuTwo.SetActive(value: true);
			base.enabled = false;
		}
		else if (PlayerManager.BroodmotherMet)
		{
			PeakControls.SetActive(value: false);
			MayuTwo.SetActive(value: false);
			if (HeroineStats.debuffedStam >= HeroineStats.maxStamina)
			{
				HeroineStats.GameOver = false;
				HeroineStats.debuffedStam -= 10f;
			}
			HeroineStats.pregnant = false;
			HeroineStats.creampied = false;
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("isPregnant", value: false);
			HeroineStats.masturbating = true;
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
			PlayerController.iFalled = true;
			PlayerManager.instance.player.GetComponent<Animator>().Play("rig|Masturbating");
			PlayerManager.instance.player.GetComponent<PlayerController>().enabled = false;
			HeroineStats.currentLust = 100f;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
			HeroineStats.debuffedStam = 0f;
			MainCamera.enabled = false;
			MainCamera.GetComponent<AudioListener>().enabled = false;
			StoryCamera2.enabled = false;
			StoryCamera2.GetComponent<AudioListener>().enabled = false;
			StoryCamera3.enabled = false;
			StoryCamera3.GetComponent<AudioListener>().enabled = false;
			StoryCamera1.enabled = true;
			StoryCamera1.GetComponent<AudioListener>().enabled = true;
			if (!SlickSound.isPlaying)
			{
				SlickSound.Play();
			}
			bs.SetActive(value: true);
			blackScreen = bs.GetComponent<Image>();
			c = blackScreen.color;
		}
		else
		{
			Mayu.SetActive(value: false);
			StoryCamera1.enabled = false;
			StoryCamera1.GetComponent<AudioListener>().enabled = false;
			StoryCamera2.enabled = false;
			StoryCamera2.GetComponent<AudioListener>().enabled = false;
			StoryCamera3.enabled = false;
			StoryCamera3.GetComponent<AudioListener>().enabled = false;
			PeakControls.SetActive(value: false);
		}
	}

	private void Update()
	{
		Debug.Log(HeroineStats.masturbating);
		CamChangeFuntion();
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrg(2.4f);
		if (HeroineStats.currentOrg > 70f || HeroineStats.orgasm)
		{
			PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrg(1.6f);
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("mastFast", value: true);
			SlickSound.pitch = 2.2f;
		}
		else
		{
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("mastFast", value: false);
			SlickSound.pitch = 2f;
		}
		if (bs1Done)
		{
			timer += Time.deltaTime;
			if (!d1Triggered)
			{
				TriggerDialoge1();
			}
			if (timer >= 40f)
			{
				DialogManager.instance.EndDialogue();
				PeakControls.SetActive(value: true);
				if (Input.GetKeyDown(KeyCode.X))
				{
					PlayerPrefs.SetInt("ReachedEnd", 1);
					PlayerPrefs.Save();
					StoryCamera1.enabled = true;
					MainCamera.GetComponent<AudioListener>().enabled = true;
					PeakControls.SetActive(value: false);
					PlayerManager.instance.player.GetComponent<PlayerController>().enabled = true;
					StoryCamera1.enabled = false;
					StoryCamera1.GetComponent<AudioListener>().enabled = false;
					StoryCamera2.enabled = false;
					StoryCamera2.GetComponent<AudioListener>().enabled = false;
					StoryCamera3.enabled = false;
					StoryCamera3.GetComponent<AudioListener>().enabled = false;
					PlayerManager.MasturbationFinished = true;
					LoadLevel("AfterBMMH");
					return;
				}
			}
			if (timer >= 30f)
			{
				if (!d3Triggered)
				{
					TriggerDialoge3();
				}
			}
			else if (timer >= 10f && !d2Triggered)
			{
				TriggerDialoge2();
			}
		}
		else
		{
			StartCoroutine(FadeBlackScreen());
		}
	}

	public void CamChangeFuntion()
	{
		camChangeTimer += Time.deltaTime;
		if (camChangeTimer < 15f)
		{
			TriggerCam1();
		}
		if (camChangeTimer > 15f && camChangeTimer < 30f)
		{
			TriggerCam2();
		}
		if (camChangeTimer > 30f)
		{
			TriggerCam3();
		}
		if (camChangeTimer > 45f)
		{
			camChangeTimer = 0f;
		}
	}

	private void TriggerCam1()
	{
		StoryCamera1.enabled = true;
		StoryCamera1.GetComponent<AudioListener>().enabled = true;
		StoryCamera2.enabled = false;
		StoryCamera2.GetComponent<AudioListener>().enabled = false;
		StoryCamera3.enabled = false;
		StoryCamera3.GetComponent<AudioListener>().enabled = false;
	}

	private void TriggerCam2()
	{
		StoryCamera1.enabled = false;
		StoryCamera1.GetComponent<AudioListener>().enabled = false;
		StoryCamera2.enabled = true;
		StoryCamera2.GetComponent<AudioListener>().enabled = true;
		StoryCamera3.enabled = false;
		StoryCamera3.GetComponent<AudioListener>().enabled = false;
	}

	private void TriggerCam3()
	{
		StoryCamera1.enabled = false;
		StoryCamera1.GetComponent<AudioListener>().enabled = false;
		StoryCamera2.enabled = false;
		StoryCamera2.GetComponent<AudioListener>().enabled = false;
		StoryCamera3.enabled = true;
		StoryCamera3.GetComponent<AudioListener>().enabled = true;
	}

	public void TriggerDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue1);
		d1Triggered = true;
	}

	public void TriggerDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue2);
		d2Triggered = true;
	}

	public void TriggerDialoge3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue3);
		d3Triggered = true;
	}

	private IEnumerator FadeBlackScreen()
	{
		yield return new WaitForSeconds(3f);
		if (c.a > 0f)
		{
			blackScreen.color = c;
			c.a -= 0.1f * Time.deltaTime;
			if (c.a <= 0f)
			{
				bs.SetActive(value: false);
				bs1Done = true;
			}
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
