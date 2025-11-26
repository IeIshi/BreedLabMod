using UnityEngine;

public class LitaXAddictEvent : Interactable
{
	public enum SexState
	{
		BEFORETALK,
		DICKSUCK,
		HANDJOB,
		CUM,
		IDLE
	}

	public Transform CameraTarget;

	public SexState state;

	public GameObject Lita;

	public GameObject Addict;

	public GameObject Heroine;

	private Animator litaAnim;

	private Animator addictAnim;

	private float animSpeed = 1f;

	public Dialogue dialogue;

	public Dialogue dialogueTwo;

	public Dialogue dialogueThree;

	public Dialogue dialogueFour;

	private bool dialogueTriggered;

	private bool finished;

	private bool d1Finished;

	private bool d2Finished;

	private float timer;

	private bool dickSuckTwo;

	private bool xx;

	public AudioSource wetSound;

	public AudioSource cumSound;

	private void Start()
	{
		if (PlayerManager.litaGaveKey)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		state = SexState.BEFORETALK;
		litaAnim = Lita.GetComponent<Animator>();
		addictAnim = Addict.GetComponent<Animator>();
		wetSound.Play();
	}

	public override void Interact()
	{
		base.Interact();
		if (finished)
		{
			if (!d1Finished)
			{
				TriggerDialogeTwo();
				d1Finished = true;
				return;
			}
			if (d1Finished && !d2Finished)
			{
				TriggerDialogueThree();
				PlayerManager.smallKey = true;
				GameObject.Find("KeyBox").GetComponent<KeyBox>().smallKey.SetActive(value: true);
				GameObject.Find("AmmoPickup").GetComponent<AudioSource>().Play();
				PlayerManager.litaGaveKey = true;
				d2Finished = true;
				return;
			}
			if (d1Finished && d2Finished)
			{
				TriggerDialogueFour();
				return;
			}
		}
		if (!dialogueTriggered)
		{
			TriggerDialoge();
			state = SexState.DICKSUCK;
			dialogueTriggered = true;
		}
	}

	private void FixedUpdate()
	{
		SexRoutine(state);
	}

	private void SexRoutine(SexState state)
	{
		switch (state)
		{
		case SexState.DICKSUCK:
			DickSuck();
			break;
		case SexState.HANDJOB:
			HandJob();
			break;
		case SexState.CUM:
			Cum();
			break;
		case SexState.IDLE:
			Idle();
			break;
		case SexState.BEFORETALK:
			break;
		}
	}

	private void DickSuck()
	{
		if (DialogManager.inDialogue)
		{
			return;
		}
		if (dickSuckTwo)
		{
			if (!wetSound.isPlaying)
			{
				wetSound.Play();
			}
			if ((double)animSpeed < 3.5)
			{
				animSpeed += Time.deltaTime / 5f;
				litaAnim.speed = animSpeed;
				addictAnim.speed = animSpeed;
				return;
			}
			timer += Time.deltaTime;
			if (timer > 3f)
			{
				timer = 0f;
				animSpeed = 1f;
				state = SexState.CUM;
			}
			return;
		}
		CameraFollow.target = CameraTarget;
		PlayerController.iFalled = true;
		Heroine.GetComponent<HeroineStats>().enabled = false;
		Heroine.GetComponent<PlayerController>().enabled = false;
		Heroine.GetComponent<Animator>().SetBool("isNaked", value: true);
		Heroine.gameObject.layer = 11;
		HeroineFaceTarget();
		base.gameObject.layer = 0;
		if (animSpeed < 2f)
		{
			animSpeed += Time.deltaTime / 10f;
			litaAnim.speed = animSpeed;
			addictAnim.speed = animSpeed;
			return;
		}
		timer += Time.deltaTime;
		if (timer > 5f)
		{
			timer = 0f;
			animSpeed = 1f;
			state = SexState.HANDJOB;
		}
	}

	private void HandJob()
	{
		wetSound.Stop();
		if (animSpeed < 3f)
		{
			litaAnim.SetBool("handjob", value: true);
			addictAnim.SetBool("handjob", value: true);
			animSpeed += Time.deltaTime / 10f;
			litaAnim.speed = animSpeed;
			addictAnim.speed = animSpeed;
			return;
		}
		timer += Time.deltaTime;
		if (timer > 5f)
		{
			litaAnim.SetBool("handjob", value: false);
			addictAnim.SetBool("handjob", value: false);
			litaAnim.SetBool("dicksuck", value: true);
			addictAnim.SetBool("dicksuck", value: true);
			timer = 0f;
			animSpeed = 2f;
			dickSuckTwo = true;
			state = SexState.DICKSUCK;
		}
	}

	private void Cum()
	{
		if (timer > 15f)
		{
			finished = true;
			CameraFollow.target = Heroine.transform;
			PlayerController.iFalled = false;
			Heroine.GetComponent<HeroineStats>().enabled = true;
			Heroine.GetComponent<PlayerController>().enabled = true;
			Heroine.GetComponent<Animator>().SetBool("isNaked", value: false);
			cumSound.Stop();
			base.gameObject.layer = 9;
			Heroine.gameObject.layer = 12;
			state = SexState.IDLE;
		}
		else
		{
			if (!cumSound.isPlaying)
			{
				cumSound.Play();
			}
			wetSound.Stop();
			litaAnim.SetBool("cum", value: true);
			addictAnim.SetBool("cum", value: true);
			timer += Time.deltaTime;
			litaAnim.speed = animSpeed;
			addictAnim.speed = animSpeed;
		}
	}

	private void Idle()
	{
		litaAnim.SetBool("idle", value: true);
		addictAnim.SetBool("idle", value: true);
		FaceTarget();
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - Lita.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Lita.transform.rotation = Quaternion.Slerp(Lita.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void HeroineFaceTarget()
	{
		Vector3 normalized = base.transform.position.normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Heroine.transform.rotation = Quaternion.Slerp(Heroine.transform.rotation, b, Time.deltaTime * 10f);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}

	public void TriggerDialogeTwo()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueTwo);
	}

	public void TriggerDialogueThree()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueThree);
	}

	public void TriggerDialogueFour()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogueFour);
	}
}
