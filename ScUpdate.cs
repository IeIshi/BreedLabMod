using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScUpdate : MonoBehaviour
{
	public GameObject heroine;

	public GameObject bs;

	private Image blackScreen;

	private Color c;

	public Transform defaultTarget;

	private Animator animator;

	public AudioSource gasSound;

	private bool setAlphaToZero;

	private void Start()
	{
		animator = GetComponent<Animator>();
		blackScreen = bs.GetComponent<Image>();
		c = blackScreen.color;
	}

	private void Update()
	{
		if (DialogManager.inDialogue)
		{
			FaceTarget();
		}
		else
		{
			FaceDefaultTarget();
			animator.SetBool("lookAt", value: false);
		}
		if (gasSound.isPlaying)
		{
			heroine.GetComponent<HeroineStats>().GainLust(15f);
		}
		if (HeroineStats.currentLust >= 100f && InsideOrOutside.inside)
		{
			heroine.GetComponent<PlayerController>().enabled = false;
			heroine.GetComponent<Animator>().SetBool("isFalledBack", value: true);
			StartCoroutine(JumpToNextScene());
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void FaceDefaultTarget()
	{
		Vector3 normalized = (defaultTarget.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
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
		SceneManager.LoadScene("ScientistXHeroineEvent");
	}
}
