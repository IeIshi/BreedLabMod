using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScientistGallery : Interactable
{
	private Animator animator;

	public GameObject heroine;

	public Dialogue dialogue;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	public static bool scGallery;

	private bool triggered;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		if (DialogManager.inDialogue)
		{
			FaceTarget();
			animator.SetBool("lookAt", value: true);
			triggered = true;
		}
		else if (triggered)
		{
			scGallery = true;
			LoadLevel("ScientistXHeroineEvent");
		}
	}

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void FaceTarget()
	{
		Vector3 normalized = (heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
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
