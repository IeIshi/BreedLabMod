using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlantWalkerControl : MonoBehaviour
{
	public enum State
	{
		IDLE,
		HIDE,
		MISSSEX,
		BLOWJOB,
		REVERSERAPED,
		EXHAUST
	}

	public State state;

	public float maxStamina;

	public float maxCum;

	public float thrustDmg;

	public float lustDmg;

	public float gainCumPerThrust;

	public float drainStamPerThrust;

	public float expPerThrust;

	public float recoveryTime;

	private float currentStamina;

	private float currentCum;

	public GameObject LitaPhanPref;

	private GameObject LitaSpawn;

	private Animator animator;

	private Animator heroineAnim;

	private NavMeshAgent heroineAgent;

	private float distance;

	public bool standing;

	public bool sitting;

	public float influenceRange = 5f;

	public GameObject Heroine;

	public Transform HeroineMount;

	public Transform BjPos;

	public Transform MastPos;

	public Transform LyingPos;

	public Transform CamBj;

	public Transform CamCow;

	public Transform CamMission;

	public Transform LitaSpawnPoint;

	private float timer;

	private CamShaker shake;

	private bool cumming;

	private bool thrusted;

	private bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private bool reverseRaped;

	public ParticleSystem cumParticle;

	public AudioSource blowJobSound1;

	public AudioSource blowJobSound2;

	public AudioSource cumSound;

	public AudioSource sexSound1;

	public AudioSource sexSound2;

	public AudioSource sexSound3;

	public AudioSource collideSound;

	public AudioSource wetSound;

	private bool adjustPos;

	private bool passion;

	private bool enableNav;

	public List<GameObject> BodyParts = new List<GameObject>();

	private GameObject screenEffect;

	private Color baseColorValue;

	private Color fadingColorValue;

	private void Start()
	{
		animator = GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		heroineAgent = Heroine.GetComponent<NavMeshAgent>();
		state = State.IDLE;
		currentStamina = maxStamina;
		currentCum = 0f;
		shake = Object.FindObjectOfType<CamShaker>();
		if (heroineAgent != null)
		{
			heroineAgent.enabled = false;
		}
		screenEffect = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect");
		baseColorValue = screenEffect.GetComponent<Image>().color;
		fadingColorValue = screenEffect.GetComponent<Image>().color;
		if (standing)
		{
			animator.Play("rig|Idle");
		}
		if (sitting)
		{
			animator.Play("rig|Sit");
		}
		if ((standing && sitting) || (!standing && !sitting))
		{
			animator.Play("rig|Idle");
		}
	}

	private void Update()
	{
		StateMachine(state);
	}

	private void StateMachine(State state)
	{
		switch (state)
		{
		case State.IDLE:
			Idle();
			break;
		case State.MISSSEX:
			Misssex();
			break;
		case State.BLOWJOB:
			BlowJob();
			break;
		case State.REVERSERAPED:
			ReverseRaped();
			break;
		case State.EXHAUST:
			Exhaust();
			break;
		case State.HIDE:
			break;
		}
	}

	private void Idle()
	{
		distance = Vector3.Distance(Heroine.transform.position, base.transform.position);
		if (!(distance <= influenceRange))
		{
			return;
		}
		if (HeroineStats.GameOver)
		{
			recoveryTime = 10f;
		}
		if (!(HeroineStats.currentLust > 80f))
		{
			return;
		}
		InitiateUI();
		if (Heroine.GetComponent<HeroineStats>().mySexPartner == null)
		{
			Heroine.GetComponent<HeroineStats>().mySexPartner = base.gameObject;
			BodyParts[0].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
			BodyParts[1].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
			BodyParts[2].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
			BodyParts[3].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
			BodyParts[4].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		}
		if (!(Heroine.GetComponent<HeroineStats>().mySexPartner == base.gameObject))
		{
			return;
		}
		if (heroineAgent != null && !enableNav)
		{
			heroineAgent.enabled = true;
			enableNav = true;
		}
		if (heroineAgent.stoppingDistance >= distance)
		{
			if (!PlayerController.iGetFucked)
			{
				heroineAnim.Play("rig|Walk");
				PlayerController.iGetFucked = true;
				PlayerController.iFalled = true;
			}
			FaceTarget();
			if (standing)
			{
				state = State.BLOWJOB;
				reverseRaped = true;
				PlayerController.iFalled = true;
			}
			if (sitting)
			{
				heroineAnim.SetBool("luring", value: true);
				PlayerController.iFalled = true;
			}
			heroineAgent.ResetPath();
			heroineAgent.isStopped = true;
			timer += Time.deltaTime;
			if (timer > 3f)
			{
				state = State.MISSSEX;
				timer = 0f;
			}
		}
		else
		{
			PlayerController.iGetFucked = true;
			PlayerController.iFalled = true;
			heroineAnim.Play("rig|Walk");
			heroineAgent.SetDestination(base.gameObject.transform.position);
		}
	}

	private void Exhaust()
	{
		timer += Time.deltaTime;
		if (timer > recoveryTime)
		{
			animator.SetBool("lyingBack", value: false);
			animator.SetBool("lyingStomach", value: false);
			GetComponent<CapsuleCollider>().enabled = true;
			standing = false;
			sitting = true;
			state = State.IDLE;
			timer = 0f;
		}
	}

	private void Release()
	{
		cumming = false;
		if (reverseRaped)
		{
			if (!HeroineStats.GameOver)
			{
				PlayerController.iFalled = false;
				heroineAnim.SetBool("falled", value: false);
				reverseRaped = false;
			}
			else
			{
				heroineAnim.SetBool("masturbatingLying", value: true);
				reverseRaped = false;
				Heroine.transform.position = BjPos.position;
				Heroine.transform.rotation = BjPos.rotation;
			}
		}
		heroineAnim.SetBool("luring", value: false);
		heroineAnim.SetBool("addictCowgirl", value: false);
		heroineAnim.SetBool("addictOralCum", value: false);
		heroineAnim.SetBool("addictOral", value: false);
		heroineAnim.SetBool("missInsert", value: false);
		heroineAnim.SetBool("mastStandingHard", value: false);
		heroineAnim.SetBool("missPass", value: false);
		animator.SetBool("addictCowgirl", value: false);
		animator.SetBool("addictCowCum", value: false);
		animator.SetBool("addictOralCum", value: false);
		animator.SetBool("addictOral", value: false);
		animator.SetBool("missInsert", value: false);
		animator.SetBool("missPass", value: false);
		animator.SetBool("missCum", value: false);
		heroineAnim.SetBool("missCum", value: false);
		animator.SetBool("lyingBack", value: true);
		animator.SetBool("lyingStomach", value: true);
		cumSound.Stop();
		cumParticle.Stop();
		wetSound.Stop();
		DisableUI();
		PlayerController.iGetFucked = false;
		HeroineStats.stunned = false;
		enableNav = false;
		GetComponent<CapsuleCollider>().enabled = false;
		GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
		CameraFollow.target = Heroine.transform;
		if (heroineAgent != null)
		{
			heroineAgent.enabled = false;
		}
		animator.speed = 1f;
		heroineAnim.speed = 1f;
		currentCum = 0f;
		adjustPos = false;
		timer = 0f;
		if (heroineAgent.enabled)
		{
			heroineAgent.isStopped = false;
			heroineAgent.enabled = false;
		}
		if (passion)
		{
			Heroine.transform.position = LyingPos.position;
			Heroine.transform.rotation = LyingPos.rotation;
			passion = false;
		}
		else if (!HeroineStats.GameOver)
		{
			Heroine.transform.position = base.gameObject.transform.position;
			Heroine.transform.rotation = base.gameObject.transform.rotation;
		}
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		BodyParts[0].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[1].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[2].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[3].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		BodyParts[4].GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
		state = State.EXHAUST;
	}

	private void ReverseRaped()
	{
		InitiateUI();
		HeroineStats.stunned = true;
		CameraFollow.target = CamCow;
		if (!cumming)
		{
			cumSound.Stop();
		}
		if (timer > 5f)
		{
			if (!adjustPos)
			{
				Heroine.transform.position = BjPos.position;
				Heroine.transform.rotation = BjPos.rotation;
				adjustPos = true;
			}
			base.gameObject.transform.position = HeroineMount.position;
			base.gameObject.transform.rotation = HeroineMount.rotation;
			animator.SetBool("addictCowgirl", value: true);
			heroineAnim.SetBool("addictCowgirl", value: true);
			PlayerManager.IsVirgin = false;
			if (cumming)
			{
				if (currentCum <= 0f)
				{
					if (!cumSound.isPlaying)
					{
						cumSound.Play();
					}
					HeroineStats.lustyCum = true;
					animator.speed = 0.3f;
					heroineAnim.speed = 0.3f;
					cumParticle.Play();
					timer += Time.deltaTime;
					if (timer > 9f)
					{
						Release();
					}
					return;
				}
			}
			else if (currentCum > 40f)
			{
				animator.speed = 1.5f;
				heroineAnim.speed = 1.5f;
			}
			if (currentCum >= 100f)
			{
				animator.SetBool("addictCowCum", value: true);
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
				}
				HeroineStats.creampied = true;
				animator.speed = 2f;
				heroineAnim.speed = 2f;
				cumming = true;
			}
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	private void BlowJob()
	{
		InitiateUI();
		HeroineStats.stunned = true;
		Heroine.transform.position = BjPos.transform.position;
		Heroine.transform.rotation = BjPos.transform.rotation;
		CameraFollow.target = CamBj;
		if (cumming && currentCum <= 0f)
		{
			if (timer > 3f)
			{
				animator.SetBool("lyingBack", value: true);
				animator.SetBool("addictOralCum", value: false);
				heroineAnim.SetBool("addictOralCum", value: false);
				Heroine.transform.position = MastPos.position;
				Heroine.transform.rotation = MastPos.rotation;
				state = State.REVERSERAPED;
				animator.speed = 1f;
				heroineAnim.speed = 1f;
				cumParticle.Stop();
				timer = 0f;
				cumming = false;
			}
			else
			{
				timer += Time.deltaTime;
				animator.speed = 0.1f;
				heroineAnim.speed = 0.1f;
				cumParticle.Play();
			}
		}
		else
		{
			if (cumming)
			{
				animator.SetBool("addictOralCum", value: true);
				heroineAnim.SetBool("addictOralCum", value: true);
				HeroineStats.oralCreampie = true;
				animator.speed = 2f;
				heroineAnim.speed = 2f;
				DrainCum(1f);
			}
			else if (currentCum > 40f)
			{
				animator.speed = 1.5f;
				heroineAnim.speed = 1.5f;
			}
			animator.SetBool("addictOral", value: true);
			heroineAnim.SetBool("addictOral", value: true);
		}
	}

	private void Misssex()
	{
		InitiateUI();
		HeroineStats.stunned = true;
		base.gameObject.transform.position = HeroineMount.position;
		base.gameObject.transform.rotation = HeroineMount.rotation;
		PlayerManager.IsVirgin = false;
		CameraFollow.target = CamMission;
		heroineAnim.SetBool("falled", value: true);
		if (currentCum >= 100f)
		{
			cumming = true;
			HeroineStats.lustyCum = true;
		}
		if (cumming)
		{
			HeroineStats.creampied = true;
			animator.speed = 1f;
			heroineAnim.speed = 1f;
			if (currentCum <= 0f)
			{
				Release();
				state = State.EXHAUST;
			}
			else
			{
				animator.SetBool("missCum", value: true);
				heroineAnim.SetBool("missCum", value: true);
				DrainCum(1f);
			}
			return;
		}
		if (currentCum > 60f)
		{
			animator.speed = 1.5f;
			heroineAnim.speed = 1.5f;
		}
		if (timer > 15f)
		{
			animator.SetBool("missPass", value: true);
			heroineAnim.SetBool("missPass", value: true);
			passion = true;
			if (!wetSound.isPlaying)
			{
				wetSound.Play();
			}
		}
		animator.SetBool("missInsert", value: true);
		heroineAnim.SetBool("missInsert", value: true);
		timer += Time.deltaTime;
	}

	public void SpawnLita()
	{
		StartCoroutine(SpawnLitas());
	}

	private IEnumerator SpawnLitas()
	{
		yield return new WaitForSeconds(1f);
		LitaSpawn = Object.Instantiate(LitaPhanPref, new Vector3(LitaSpawnPoint.position.x, LitaSpawnPoint.position.y, LitaSpawnPoint.position.z), Quaternion.identity);
		shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		StartCoroutine(PlayScreenEffect());
	}

	private IEnumerator PlayScreenEffect()
	{
		screenEffect.SetActive(value: true);
		_ = screenEffect.GetComponent<Image>().color;
		yield return new WaitForSeconds(1.5f);
		screenEffect.SetActive(value: false);
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.maxHealth = maxStamina;
		EnemyUI.instance.maxCum = maxCum;
		EnemyUI.instance.health = currentStamina;
		EnemyUI.instance.cum = currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
		EnemyUI.instance.portraitHugger.enabled = false;
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void DrainStamina(float drainValue)
	{
		currentStamina -= drainValue * Time.deltaTime;
		EnemyUI.instance.TakeDamage(drainValue * Time.deltaTime);
	}

	public void GainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	private void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
	}

	private void DrainCumInstant(float drainValue)
	{
		currentCum -= drainValue;
		EnemyUI.instance.LoseCum(drainValue);
	}

	private void VignetteEffect()
	{
		if (!thrusted)
		{
			return;
		}
		if (max)
		{
			PostProcessingManager.vignette.intensity.value -= Time.deltaTime * pulsingSpeed;
			if (PostProcessingManager.vignette.intensity.value <= 0f)
			{
				thrusted = false;
				max = false;
			}
		}
		else if (PostProcessingManager.vignette.intensity.value <= vignetteIntensity)
		{
			PostProcessingManager.vignette.intensity.value += Time.deltaTime * pulsingSpeed;
		}
		else
		{
			max = true;
		}
	}

	public void ThrustEvent()
	{
		thrusted = true;
		if (state == State.BLOWJOB)
		{
			if ((float)Random.Range(0, 2) == 1f)
			{
				blowJobSound1.Play();
			}
			else
			{
				blowJobSound2.Play();
			}
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 5f);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
			Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
			GainCumInstant(gainCumPerThrust);
		}
		if (state == State.MISSSEX)
		{
			if (passion)
			{
				sexSound3.Play();
				Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
				Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
				Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
				GainCumInstant(gainCumPerThrust);
			}
			else
			{
				sexSound2.Play();
				Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 3f);
				Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
				Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
				GainCumInstant(gainCumPerThrust / 3f);
			}
			if (EquipmentManager.instance.currentEquipment[3] != null)
			{
				if (EquipmentManager.instance.currentEquipment[3].id == 454874)
				{
					return;
				}
				EquipmentManager.instance.RipPantsu();
			}
		}
		if (state == State.REVERSERAPED)
		{
			sexSound1.Play();
			Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg);
			Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
			Heroine.GetComponent<HeroineStats>().GainExp(expPerThrust);
			GainCumInstant(gainCumPerThrust);
			if (EquipmentManager.instance.currentEquipment[3] != null)
			{
				if (EquipmentManager.instance.currentEquipment[3].id == 454874)
				{
					return;
				}
				EquipmentManager.instance.RipPantsu();
			}
		}
		if (currentCum >= maxCum)
		{
			cumming = true;
		}
	}

	public void CumEvent()
	{
		shake.StartShake(new CamShaker.Properties(0.05f, 0.003f, 0.3f, 0.05f, 0.1f, 0.035f, 0.035f, 0.035f));
		Heroine.GetComponent<HeroineStats>().GainOrgInstant(thrustDmg / 2f);
		Heroine.GetComponent<HeroineStats>().GainLustInstant(lustDmg);
		if (state == State.BLOWJOB)
		{
			if ((float)Random.Range(0, 2) == 1f)
			{
				blowJobSound1.Play();
			}
			else
			{
				blowJobSound2.Play();
			}
		}
		if (state == State.MISSSEX)
		{
			sexSound2.Play();
		}
		if (!cumSound.isPlaying)
		{
			cumSound.Play();
		}
		DrainCumInstant(10f);
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Heroine.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, influenceRange);
	}
}
