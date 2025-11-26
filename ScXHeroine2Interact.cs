using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScXHeroine2Interact : Interactable
{
	public Dialogue dialogue;

	public bool dialogueTriggered;

	public GameObject Cum;

	public GameObject Heroine;

	public Transform WallSexPos;

	public Transform AfterSexPos;

	public Transform struggleFallPoint;

	public Animator scAnimator;

	public Animator heroineAnimator;

	private float timer;

	public bool triggerTease;

	private bool triggerSex;

	private bool triggerCum;

	public Dialogue sexDialogue1;

	public Dialogue sexDialogue2;

	public Dialogue sexDialogue3;

	private bool dialogueRoutineTriggered;

	public float maxStamina = 200f;

	private float currentStamina;

	private bool sheCummed;

	private bool lewdDialogueEnded;

	public ParticleSystem cumParticle;

	public GameObject BlackScreen;

	private Image BlackScreenImage;

	private float blackScreenFadeDuration = 10f;

	private bool startFade;

	private Color originalColor;

	private float timer2;

	private bool chanceSpeedOnce;

	private bool triggerEnding;

	private bool triggerFade;

	private float endingTimer;

	private bool struggledFree;

	private void Start()
	{
		scAnimator = GetComponent<Animator>();
		heroineAnimator = Heroine.GetComponent<Animator>();
		sheCummed = false;
		BlackScreenImage = BlackScreen.GetComponent<Image>();
		currentStamina = maxStamina;
		originalColor = BlackScreenImage.color;
	}

	public override void Interact()
	{
		base.Interact();
		TriggerDialoge();
	}

	private void FixedUpdate()
	{
		if (currentStamina <= 0f && !triggerSex)
		{
			if (!struggledFree)
			{
				Release();
				struggledFree = true;
			}
			return;
		}
		if (triggerEnding)
		{
			endingTimer += Time.deltaTime;
			if (endingTimer > 3f)
			{
				HeroineStats.masturbating = false;
				heroineAnimator.SetBool("isFalledBack", value: true);
				heroineAnimator.Play("rig|futaBehindAfterCum");
				cumParticle.Stop();
				Heroine.GetComponent<PlayerController>().enabled = true;
				PlayerController.iGetFucked = false;
				PlayerController.iFalledBack = true;
				PlayerController.iFalled = true;
				InventoryUI.heroineIsChased = false;
				Heroine.transform.position = AfterSexPos.position;
				Heroine.transform.rotation = AfterSexPos.rotation;
				scAnimator.speed = 1f;
				heroineAnimator.speed = 1f;
				HeroineStats.stunned = false;
				Cum.SetActive(value: true);
				PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = null;
				BlackScreen.SetActive(value: false);
				Object.Destroy(base.gameObject);
			}
			return;
		}
		if (dialogueTriggered && !DialogManager.inDialogue)
		{
			if (triggerTease)
			{
				TeaseTime();
			}
			if (triggerSex)
			{
				if (!startFade)
				{
					scAnimator.speed = 0f;
					heroineAnimator.speed = 0f;
					StartCoroutine(FadeOutAndDisable());
					startFade = true;
				}
				SexTime();
			}
			if (triggerCum)
			{
				CumTime();
			}
		}
		if (triggerSex)
		{
			PlayerManager.instance.player.GetComponent<HeroineStats>().GainLust(4f);
		}
	}

	private void Release()
	{
		HeroineStats.masturbating = false;
		heroineAnimator.SetBool("isFalledBack", value: true);
		cumParticle.Stop();
		Heroine.GetComponent<PlayerController>().enabled = true;
		PlayerController.iGetFucked = false;
		PlayerController.iFalled = true;
		InventoryUI.heroineIsChased = false;
		Heroine.transform.position = struggleFallPoint.position;
		base.gameObject.layer = 9;
		scAnimator.Play("rig|Idle");
		scAnimator.speed = 1f;
		heroineAnimator.speed = 1f;
		DisableUI();
		HeroineStats.stunned = false;
		PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = null;
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	private void TeaseTime()
	{
		base.gameObject.layer = 0;
		Heroine.transform.position = base.transform.position;
		Heroine.transform.rotation = base.transform.rotation;
		scAnimator.Play("rig|GrabHeroine");
		heroineAnimator.Play("rig|Scientist2_Tease");
		PlayerController.iGetFucked = true;
		PlayerController.iFalled = true;
		InventoryUI.heroineIsChased = true;
		PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLust(2f);
		InitiateUI();
		if (HeroineStats.currentOrg < 15f && !sheCummed)
		{
			scAnimator.speed = 0.5f;
			heroineAnimator.speed = 0.5f;
		}
		else if (HeroineStats.currentOrg > 90f || sheCummed)
		{
			scAnimator.speed = 2f;
			heroineAnimator.speed = 2f;
		}
		else
		{
			scAnimator.speed = 1f;
			heroineAnimator.speed = 1f;
		}
		if (HeroineStats.currentOrg > 50f)
		{
			HeroineStats.aroused = true;
			heroineAnimator.SetBool("isScared", value: false);
		}
		if (HeroineStats.orgasm)
		{
			sheCummed = true;
			HeroineStats.currentPower = 0f;
			HeroineStats.stunned = true;
		}
		if (sheCummed)
		{
			timer += Time.deltaTime;
			if (timer > 4f)
			{
				triggerTease = false;
				triggerSex = true;
				timer = 0f;
				BlackScreen.SetActive(value: true);
			}
		}
	}

	private void SexTime()
	{
		timer2 += Time.deltaTime;
		if (!(timer2 > 2f))
		{
			return;
		}
		scAnimator.Play("rig|FuckHeroine");
		heroineAnimator.Play("rig|Scientist2_Sex");
		base.gameObject.transform.position = WallSexPos.position;
		base.gameObject.transform.rotation = WallSexPos.rotation;
		Heroine.transform.position = base.transform.position;
		Heroine.transform.rotation = base.transform.rotation;
		PlayerManager.FkedBySc = true;
		PlayerManager.IsVirgin = false;
		if (!chanceSpeedOnce)
		{
			scAnimator.speed = 1f;
			heroineAnimator.speed = 1f;
			chanceSpeedOnce = true;
		}
		PlayerManager.instance.player.GetComponent<PlayerController>().enabled = false;
		DisableUI();
		if (!dialogueRoutineTriggered)
		{
			StartCoroutine(SexyDialogue());
			dialogueRoutineTriggered = true;
		}
		if (lewdDialogueEnded)
		{
			timer += Time.deltaTime;
			if (timer > 5f)
			{
				triggerSex = false;
				triggerCum = true;
				timer = 0f;
			}
		}
	}

	private void CumTime()
	{
		scAnimator.Play("rig|CumInHeroine");
		heroineAnimator.Play("rig|Scientist2_Cum");
		base.gameObject.transform.position = WallSexPos.position;
		base.gameObject.transform.rotation = WallSexPos.rotation;
		Heroine.transform.position = base.transform.position;
		Heroine.transform.rotation = base.transform.rotation;
		HeroineStats.creampied = true;
		if (!cumParticle.isPlaying)
		{
			cumParticle.Play();
		}
		PlayerManager.instance.player.GetComponent<HeroineStats>().CumDrip();
		scAnimator.speed = 1f;
		heroineAnimator.speed = 1f;
		timer += Time.deltaTime;
		if (timer > 10f && !triggerFade)
		{
			StartCoroutine(FadeInAndEnable());
			triggerFade = true;
		}
	}

	private IEnumerator SexyDialogue()
	{
		yield return new WaitForSeconds(5f);
		TriggerSexDialoge1();
		yield return new WaitForSeconds(15f);
		TriggerSexDialoge2();
		scAnimator.speed = 1f;
		heroineAnimator.speed = 1f;
		yield return new WaitForSeconds(15f);
		TriggerSexDialoge3();
		scAnimator.speed = 1.5f;
		heroineAnimator.speed = 1.5f;
		yield return new WaitForSeconds(5f);
		scAnimator.speed = 2f;
		heroineAnimator.speed = 2f;
		lewdDialogueEnded = true;
		DialogManager.instance.EndDialogue();
	}

	public void TriggerSexDialoge1()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(sexDialogue1);
	}

	public void TriggerSexDialoge2()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(sexDialogue2);
	}

	public void TriggerSexDialoge3()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(sexDialogue3);
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.portraitHugger.enabled = true;
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = 100f;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = 0f;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
	}

	public void drainStamina(float drainValue)
	{
		currentStamina -= drainValue;
		_ = currentStamina / maxStamina;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public IEnumerator FadeOutAndDisable()
	{
		BlackScreen.SetActive(value: true);
		Color imageColor = BlackScreenImage.color;
		float startAlpha = imageColor.a;
		yield return new WaitForSeconds(3f);
		for (float t = 0f; t < blackScreenFadeDuration; t += Time.deltaTime)
		{
			float t2 = t / blackScreenFadeDuration;
			imageColor.a = Mathf.Lerp(startAlpha, 0f, t2);
			BlackScreenImage.color = imageColor;
			yield return null;
		}
		imageColor.a = 0f;
		BlackScreenImage.color = imageColor;
		BlackScreen.SetActive(value: false);
		BlackScreenImage.color = originalColor;
	}

	private IEnumerator FadeInAndEnable()
	{
		BlackScreen.SetActive(value: true);
		Color imageColor = BlackScreenImage.color;
		float targetAlpha = originalColor.a;
		for (float t = 0f; t < blackScreenFadeDuration; t += Time.deltaTime)
		{
			float t2 = t / blackScreenFadeDuration;
			imageColor.a = Mathf.Lerp(0f, targetAlpha, t2);
			BlackScreenImage.color = imageColor;
			yield return null;
		}
		imageColor.a = targetAlpha;
		BlackScreenImage.color = imageColor;
		scAnimator.speed = 0f;
		heroineAnimator.speed = 0f;
		triggerEnding = true;
	}
}
