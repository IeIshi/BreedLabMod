using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbortBed : Interactable
{
	private Animator anim;

	private Animator tentanim;

	private Animator heroanim;

	private GameObject bs;

	public Image humImage;

	public GameObject tentacle;

	public GameObject heroine;

	public Transform sitPlace;

	public Transform standPlace;

	public Dialogue dialogue;

	public AudioSource insertSound;

	public AudioSource cumSound;

	public static bool fastFuck;

	public bool cumStrainShow;

	public static bool abortBedSitting;

	private void Start()
	{
		anim = GetComponent<Animator>();
		tentanim = tentacle.GetComponent<Animator>();
		heroanim = heroine.GetComponent<Animator>();
		bs = GameObject.Find("Canvas/BlackScreen");
		bs.SetActive(value: false);
		heroine.GetComponent<CumStrains>().enabled = false;
		GetComponent<CumFlow>().enabled = false;
	}

	public override void Interact()
	{
		if (EquipmentManager.heroineIsNaked)
		{
			base.Interact();
			HeroineStats.stunned = true;
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			abortBedSitting = true;
			heroanim.SetBool("AbortSit", value: true);
			bs.SetActive(value: true);
			StartCoroutine(BlackScreenOff());
		}
		else
		{
			TriggerDialoge();
		}
	}

	private IEnumerator BlackScreenOff()
	{
		yield return new WaitForSeconds(1f);
		heroine.GetComponent<CharacterController>().enabled = false;
		heroine.transform.rotation = sitPlace.rotation;
		heroine.transform.position = sitPlace.position;
		bs.SetActive(value: false);
		StartCoroutine(ChairOpen());
	}

	private IEnumerator BlackScreenOffOff()
	{
		yield return new WaitForSeconds(1f);
		heroine.GetComponent<CumStrains>().enabled = false;
		heroanim.SetBool("AbortAfter", value: false);
		heroanim.SetBool("AbortSit", value: false);
		heroanim.SetBool("AbortSitOpen", value: false);
		heroanim.SetBool("AbortFuck", value: false);
		heroanim.SetBool("isCumFilled", value: false);
		heroanim.SetBool("isPregnant", value: false);
		heroine.transform.position = standPlace.position;
		heroine.GetComponent<CharacterController>().enabled = true;
		GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
		bs.SetActive(value: false);
	}

	private IEnumerator ChairOpen()
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(TentActiv());
	}

	private IEnumerator TentActiv()
	{
		yield return new WaitForSeconds(5f);
		GetComponent<CumFlow>().enabled = false;
		tentanim.SetBool("sleep", value: false);
		tentanim.SetBool("idle", value: true);
		anim.SetBool("isOpen", value: true);
		heroanim.SetBool("AbortSitOpen", value: true);
		heroanim.SetBool("isScared", value: true);
		StartCoroutine(TentTeasing());
	}

	private IEnumerator TentTeasing()
	{
		yield return new WaitForSeconds(3f);
		tentanim.SetBool("insert", value: true);
		heroanim.SetBool("AbortSitOpen", value: true);
		heroanim.SetBool("AbortAfter", value: false);
		insertSound.Play();
		StartCoroutine(TentFucking());
	}

	private IEnumerator TentFucking()
	{
		yield return new WaitForSeconds(2f);
		PlayerManager.IsVirgin = false;
		GetComponent<CumFlow>().cumStrain.SetActive(value: false);
		insertSound.Stop();
		heroanim.SetBool("isScared", value: false);
		heroanim.SetBool("AbortFuck", value: true);
		tentanim.SetBool("fuck", value: true);
		HeroineStats.aroused = true;
		StartCoroutine(TentFastFucking());
	}

	private IEnumerator TentFastFucking()
	{
		yield return new WaitForSeconds(8f);
		GetComponent<CumFlow>().enabled = true;
		tentanim.speed = 2f;
		PlayerController.animator.speed = 2f;
		fastFuck = true;
		PlayerController.heIsFuckingHard = true;
		PlayerController.isAheago = true;
		StartCoroutine(TentCumming());
	}

	private IEnumerator TentCumming()
	{
		if (HeroineStats.lovePotion)
		{
			yield return new WaitForSeconds(10f);
		}
		else
		{
			yield return new WaitForSeconds(30f);
		}
		fastFuck = false;
		cumSound.Play();
		tentanim.speed = 1f;
		PlayerController.animator.speed = 1f;
		if (!PlayerManager.SAB)
		{
			HeroineStats.pregnant = false;
		}
		HeroineStats.creampied = false;
		HeroineStats.oralCreampie = false;
		HeroineStats.fertileCum = false;
		HeroineStats.lustyCum = false;
		HeroineStats.hugeAmount = false;
		HeroineStats.addictiveCum = false;
		PlayerManager.instance.player.GetComponent<Animator>().SetBool("isCumFilled", value: false);
		PlayerManager.instance.player.GetComponent<HeroineStats>().cumStrain.Stop();
		Object.Destroy(PlayerManager.instance.player.GetComponent<HeroineStats>().cumMesh);
		PlayerManager.instance.player.GetComponent<HeroineStats>().showCumSpot = false;
		tentanim.SetBool("cum", value: true);
		heroanim.SetBool("AbortAfter", value: true);
		heroanim.SetBool("AbortFuck", value: false);
		StartCoroutine(TentSleep());
	}

	private IEnumerator TentSleep()
	{
		yield return new WaitForSeconds(5f);
		cumSound.Stop();
		tentanim.SetBool("cum", value: false);
		tentanim.SetBool("fuck", value: false);
		tentanim.SetBool("insert", value: false);
		tentanim.SetBool("idle", value: false);
		tentanim.SetBool("sleep", value: true);
		heroanim.SetBool("isAhegao", value: false);
		PlayerController.heIsFuckingHard = false;
		HeroineStats.aroused = false;
		GetComponent<CumFlow>().cumStrain.SetActive(value: true);
		GetComponent<CumFlow>().mSize = 0f;
		GetComponent<CumFlow>().enabled = true;
		if (!HeroineStats.GameOver)
		{
			StartCoroutine(CloseBed());
		}
		else
		{
			StartCoroutine(TentActiv());
		}
	}

	private IEnumerator CloseBed()
	{
		yield return new WaitForSeconds(5f);
		anim.SetBool("isOpen", value: false);
		PlayerController.iFalled = false;
		HeroineStats.stunned = false;
		cumStrainShow = false;
		PlayerController.iGetFucked = false;
		GetComponent<CumFlow>().cumStrain.SetActive(value: false);
		GetComponent<CumFlow>().enabled = false;
		heroanim.SetBool("isAhegao", value: false);
		bs.SetActive(value: true);
		abortBedSitting = false;
		StartCoroutine(BlackScreenOffOff());
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
