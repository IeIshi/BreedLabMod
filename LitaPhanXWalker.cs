using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LitaPhanXWalker : MonoBehaviour
{
	public enum SexState
	{
		IDLE,
		KISS,
		LYINGKISS,
		GANGBANG,
		PLACEHOLDER
	}

	public SexState state;

	public GameObject Lita;

	public GameObject Addict;

	public GameObject Heroine;

	public Transform heroinePhanMount;

	private Animator litaAnim;

	private Animator addictAnim;

	private Animator heroineAnim;

	private float distance;

	public float range;

	private float timer;

	private bool spotted;

	private bool kissed;

	public AudioSource ambientMusic;

	public Transform cam1;

	public Transform cam2;

	public GameObject loadingScreen;

	public Slider loadingSlider;

	private float pumpSpeed = 1f;

	private void Start()
	{
		state = SexState.IDLE;
		pumpSpeed = 1f;
		litaAnim = Lita.GetComponent<Animator>();
		addictAnim = Addict.GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		SexRoutine(state);
	}

	private void SexRoutine(SexState state)
	{
		switch (state)
		{
		case SexState.IDLE:
			Idle();
			break;
		case SexState.KISS:
			Kiss();
			break;
		case SexState.LYINGKISS:
			LyingKiss();
			break;
		case SexState.GANGBANG:
			GangBang();
			break;
		case SexState.PLACEHOLDER:
			break;
		}
	}

	private void Idle()
	{
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (distance <= range)
		{
			if (!ambientMusic.isPlaying)
			{
				ambientMusic.Play();
			}
			LitaFaceTarget();
			AddictFaceTarget();
			timer += Time.deltaTime;
			Heroine.GetComponent<PlayerController>().enabled = false;
			if (!spotted)
			{
				heroineAnim.SetBool("isNaked", value: true);
				heroineAnim.Play("rig|Idle_Naked");
				spotted = true;
			}
			if (timer > 3f)
			{
				state = SexState.KISS;
				timer = 0f;
			}
		}
	}

	private void Kiss()
	{
		Lita.transform.position = heroinePhanMount.transform.position;
		Lita.transform.rotation = heroinePhanMount.transform.rotation;
		timer += Time.deltaTime;
		Heroine.GetComponent<PlayerController>().enabled = true;
		PlayerController.iGetFucked = true;
		HeroineStats.stunned = true;
		if (!kissed)
		{
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
			heroineAnim.Play("rig|Lita_Kiss");
			litaAnim.Play("rig|Kiss");
			kissed = true;
		}
		if (timer > 5f)
		{
			heroineAnim.SetBool("kissGiveIn", value: true);
		}
		if (timer > 10f)
		{
			state = SexState.LYINGKISS;
			timer = 0f;
		}
	}

	private void LyingKiss()
	{
		heroineAnim.SetBool("kissLying", value: true);
		litaAnim.SetBool("kissLying", value: true);
		timer += Time.deltaTime;
		CameraFollow.target = cam2;
		if (timer > 5f)
		{
			state = SexState.GANGBANG;
			timer = 0f;
		}
	}

	private void GangBang()
	{
		heroineAnim.SetBool("litaGangbang", value: true);
		addictAnim.SetBool("litaGangbang", value: true);
		Addict.transform.position = heroinePhanMount.transform.position;
		Addict.transform.rotation = heroinePhanMount.transform.rotation;
		timer += Time.deltaTime;
		if (timer > 5f)
		{
			if (pumpSpeed < 2f)
			{
				pumpSpeed += Time.deltaTime / 8f;
			}
			addictAnim.speed = pumpSpeed;
			litaAnim.speed = pumpSpeed;
			heroineAnim.speed = pumpSpeed;
		}
		if (timer > 30f)
		{
			LoadLevel("BreedingEnding");
		}
	}

	private void LitaFaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - Lita.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Lita.transform.rotation = Quaternion.Slerp(Lita.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void AddictFaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - Addict.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Addict.transform.rotation = Quaternion.Slerp(Addict.transform.rotation, b, Time.deltaTime * 10f);
		addictAnim.SetBool("standing", value: true);
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

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, range);
	}
}
