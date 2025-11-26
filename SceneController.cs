using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
	public GameObject scientist;

	public GameObject heroine;

	public AudioSource sexInsert;

	public AudioSource cumSound;

	public AudioSource heartBeatSlow;

	public AudioSource heartBeatFast;

	public GameObject phantomHeroine;

	public GameObject bs;

	private Image blackScreen;

	private Color c;

	private Animator scientistAnim;

	private Animator heroineAnim;

	public Dialogue dialogueOne;

	public Dialogue dialogueTwo;

	public Dialogue dialogue3;

	public Dialogue dialogue4;

	public Dialogue dialogue5;

	public Dialogue dialogue6;

	public Dialogue dialogue7;

	public Dialogue dialogue8;

	public Dialogue dialogue9;

	public Dialogue dialogue10;

	public SexState state;

	private bool d1;

	private bool d2;

	private bool d3;

	private bool d4;

	private bool d5;

	private bool d6;

	private bool d7;

	private bool d8;

	private bool d9;

	public float insertionTime = 10f;

	public float sexTime = 10f;

	public float fastSexTime = 10f;

	public GameObject cam;

	private float yaw;

	private float pitch;

	private float mouseSensitivity = 500f;

	private Vector3 currentRotation;

	private Vector3 rotationSmoothVelocity;

	private float rotationSmoothTime = 0.12f;

	private bool bs1Done;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	private void Start()
	{
		scientistAnim = scientist.GetComponent<Animator>();
		heroineAnim = heroine.GetComponent<Animator>();
		state = SexState.WAKE;
		blackScreen = bs.GetComponent<Image>();
		c = blackScreen.color;
		phantomHeroine.SetActive(value: false);
		PlayerManager.ScSexAfterMath = true;
		PlayerManager.loaded = false;
		PlayerManager.FkedBySc = true;
		pitch = 0f;
		yaw = 0f;
	}

	private void Update()
	{
		CamRot();
		SexRoutine(state);
		if (!bs1Done && c.a > 0f)
		{
			blackScreen.color = c;
			c.a -= 0.1f * Time.deltaTime;
			if (c.a <= 0f)
			{
				bs1Done = true;
			}
		}
	}

	private void SexRoutine(SexState state)
	{
		switch (state)
		{
		case SexState.WAKE:
			StartCoroutine(Begin());
			break;
		case SexState.SLOWSEX:
			StartCoroutine(SlowSexDialogue());
			break;
		case SexState.SEX:
			StartCoroutine(SexDialogue());
			break;
		case SexState.FASTSEX:
			StartCoroutine(FastSex());
			break;
		case SexState.CUM:
			StartCoroutine(Cum());
			break;
		case SexState.GRIND:
			break;
		}
	}

	private IEnumerator Begin()
	{
		yield return new WaitForSeconds(5f);
		if (!d1)
		{
			TriggerDialogeOne();
			d1 = true;
		}
		scientistAnim.SetBool("isTeasing", value: true);
		heroineAnim.SetBool("isTeasing", value: true);
		if (!sexInsert.isPlaying && state != SexState.SLOWSEX)
		{
			sexInsert.Play();
		}
		yield return new WaitForSeconds(5f);
		if (!d2)
		{
			TriggerDialogeTwo();
			d2 = true;
		}
		yield return new WaitForSeconds(5f);
		if (!d3)
		{
			TriggerDialoge3();
			d3 = true;
		}
		yield return new WaitForSeconds(3f);
		state = SexState.SLOWSEX;
	}

	private IEnumerator SlowSexDialogue()
	{
		if (sexInsert.isPlaying)
		{
			sexInsert.Stop();
		}
		if (!heartBeatSlow.isPlaying)
		{
			heartBeatSlow.Play();
		}
		scientistAnim.SetBool("isFucking", value: true);
		heroineAnim.SetBool("isFucking", value: true);
		scientistAnim.speed = 0.3f;
		heroineAnim.speed = 0.3f;
		yield return new WaitForSeconds(5f);
		if (!d4)
		{
			TriggerDialoge4();
			d4 = true;
		}
		yield return new WaitForSeconds(5f);
		if (!d5)
		{
			TriggerDialoge5();
			d5 = true;
		}
		yield return new WaitForSeconds(5f);
		if (!d6)
		{
			if (!PlayerManager.SAB)
			{
				TriggerDialoge6();
			}
			else
			{
				TriggerDialoge10();
			}
			d6 = true;
		}
		state = SexState.SEX;
	}

	private IEnumerator SexDialogue()
	{
		scientistAnim.speed = 1f;
		heroineAnim.speed = 1f;
		yield return new WaitForSeconds(5f);
		if (!d7)
		{
			TriggerDialoge7();
			d7 = true;
		}
		state = SexState.FASTSEX;
	}

	private IEnumerator FastSex()
	{
		yield return new WaitForSeconds(fastSexTime);
		if (!d8)
		{
			TriggerDialoge8();
			d8 = true;
		}
		phantomHeroine.SetActive(value: true);
		scientistAnim.speed = 2f;
		heroineAnim.speed = 2f;
		if (!heartBeatFast.isPlaying)
		{
			heartBeatSlow.Stop();
			heartBeatFast.Play();
		}
		yield return new WaitForSeconds(10f);
		state = SexState.CUM;
	}

	private IEnumerator Cum()
	{
		yield return new WaitForSeconds(3f);
		scientistAnim.speed = 1f;
		heroineAnim.speed = 1f;
		scientistAnim.SetBool("isCumming", value: true);
		heroineAnim.SetBool("isCumming", value: true);
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		yield return new WaitForSeconds(7f);
		if (!d9)
		{
			TriggerDialoge9();
			d9 = true;
		}
		yield return new WaitForSeconds(8f);
		if (!(c.a <= 1f))
		{
			yield break;
		}
		blackScreen.color = c;
		c.a += 0.1f * Time.deltaTime;
		if (c.a >= 1f)
		{
			if (!ScientistGallery.scGallery)
			{
				LoadLevel(SceneManager.GetActiveScene().buildIndex - 3);
				yield break;
			}
			LoadLevelString("Gallery");
			PlayerManager.ScSexAfterMath = false;
			PlayerManager.loaded = false;
			ScientistGallery.scGallery = false;
		}
	}

	public void TriggerDialogeOne()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueOne);
	}

	public void TriggerDialogeTwo()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueTwo);
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

	public void TriggerDialoge7()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue7);
	}

	public void TriggerDialoge8()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue8);
	}

	public void TriggerDialoge9()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue9);
	}

	public void TriggerDialoge10()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue10);
	}

	private void CamRot()
	{
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
		pitch = Mathf.Clamp(pitch, -12f, 25f);
		yaw = Mathf.Clamp(yaw, 50f, 130f);
		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		cam.transform.eulerAngles = currentRotation;
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	public void LoadLevelString(string sceneIndex)
	{
		StartCoroutine(LoadAsynchString(sceneIndex));
	}

	private IEnumerator LoadAsynchString(string sceneIndex)
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

	private IEnumerator LoadAsynch(int sceneIndex)
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
