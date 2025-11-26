using UnityEngine;

public class TentTrap : MonoBehaviour
{
	public GameObject Tentacle;

	private TentacleThrustEvent tentControl;

	private GameObject Heroine;

	private Animator tentAnim;

	private Animator heroineAnim;

	private float mSize;

	private bool scaleDown;

	public float blobPulsingSpeed = 0.5f;

	public float openSpeed = 1f;

	private SkinnedMeshRenderer meshRenderer;

	private SkinnedMeshRenderer penisRenderer;

	public GameObject Penis;

	private float key1;

	private float key2;

	private bool open;

	private float sexDelay;

	private bool inserted;

	public bool cumming;

	public AudioSource slideInsideSound;

	public AudioSource blubSound;

	public AudioSource swallowSound;

	public AudioSource cumSound;

	private bool released;

	public float ressurectTime = 10f;

	private float resTime;

	public GameObject Lights;

	private int cumCounter;

	private void Start()
	{
		Lights.SetActive(value: false);
		tentControl = Tentacle.GetComponent<TentacleThrustEvent>();
		Heroine = GameObject.Find("Heroine");
		tentAnim = Tentacle.GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		meshRenderer = GetComponent<SkinnedMeshRenderer>();
		penisRenderer = Penis.GetComponent<SkinnedMeshRenderer>();
		key1 = 100f;
		meshRenderer.SetBlendShapeWeight(0, key1);
		if (EquipmentManager.antiTentOn)
		{
			meshRenderer.enabled = true;
			penisRenderer.enabled = true;
		}
		else
		{
			meshRenderer.enabled = false;
			penisRenderer.enabled = false;
		}
	}

	private void FixedUpdate()
	{
		if (!open)
		{
			if (EquipmentManager.antiTentOn)
			{
				meshRenderer.enabled = true;
				penisRenderer.enabled = true;
			}
			else
			{
				meshRenderer.enabled = false;
				penisRenderer.enabled = false;
			}
		}
	}

	private void Update()
	{
		if (tentControl.currentStamina <= 0f)
		{
			Close();
			resTime += Time.deltaTime;
			if (resTime >= ressurectTime)
			{
				GetComponent<BoxCollider>().enabled = true;
				released = false;
				tentControl.currentStamina = tentControl.maxStamina;
			}
		}
		else if (PlayerManager.SAB && !HeroineStats.GameOver && cumCounter == 2)
		{
			Close();
			resTime += Time.deltaTime;
			if (resTime >= ressurectTime)
			{
				GetComponent<BoxCollider>().enabled = true;
				released = false;
				tentControl.currentStamina = tentControl.maxStamina;
			}
		}
		else
		{
			if (!open)
			{
				return;
			}
			meshRenderer.enabled = true;
			penisRenderer.enabled = true;
			Heroine.GetComponent<HeroineStats>().mySexPartner = Tentacle.gameObject;
			PlayerController.iGetFucked = true;
			PlayerController.iFalled = true;
			InitiateUI();
			Open();
			Pulse();
			if (!blubSound.isPlaying)
			{
				blubSound.Play();
			}
			if (sexDelay > 3f)
			{
				tentAnim.SetBool("rub", value: true);
				if (!EquipmentManager.heroineIsNaked)
				{
					Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
					return;
				}
				if (cumming)
				{
					tentAnim.speed = 1f;
					heroineAnim.speed = 1f;
					tentControl.DrainCum(10f);
					HeroineStats.creampied = true;
					HeroineStats.hugeAmount = true;
					heroineAnim.SetBool("isCumFilled", value: true);
					Heroine.GetComponent<HeroineStats>().LargeCumDrip();
					if (!cumSound.isPlaying)
					{
						cumSound.Play();
					}
					if (tentControl.currentCum <= 0f)
					{
						cumming = false;
						cumCounter++;
						cumSound.Stop();
					}
					return;
				}
				if (tentControl.currentCum > 100f)
				{
					cumming = true;
				}
				if (tentControl.currentCum > 20f)
				{
					tentControl.VignetteEffect();
					tentAnim.SetBool("fuckHard", value: true);
					tentAnim.SetBool("fuckSoft", value: false);
					heroineAnim.SetBool("trapSoftFuck", value: false);
					heroineAnim.SetBool("trapHardFuck", value: true);
					if (tentControl.currentCum > 75f)
					{
						heroineAnim.SetBool("isAhegao", value: true);
						tentAnim.speed = 2.5f;
						heroineAnim.speed = 2.5f;
					}
					return;
				}
				if (!inserted)
				{
					slideInsideSound.Play();
					inserted = true;
					PlayerManager.IsVirgin = false;
				}
				tentAnim.SetBool("fuckSoft", value: true);
				tentAnim.SetBool("fuckHard", value: false);
				heroineAnim.SetBool("trapSoftFuck", value: true);
				heroineAnim.SetBool("trapHardFuck", value: false);
				heroineAnim.SetBool("isScared", value: false);
				HeroineStats.aroused = true;
			}
			else
			{
				sexDelay += Time.deltaTime;
			}
		}
	}

	private void Pulse()
	{
		if (!scaleDown)
		{
			meshRenderer.SetBlendShapeWeight(1, mSize += blobPulsingSpeed);
		}
		if (mSize > 99f)
		{
			scaleDown = true;
		}
		if (scaleDown)
		{
			meshRenderer.SetBlendShapeWeight(1, mSize -= blobPulsingSpeed);
			if (mSize < 1f)
			{
				scaleDown = false;
			}
		}
	}

	private void Open()
	{
		if (key1 > 0f)
		{
			resTime = 0f;
			heroineAnim.SetBool("isScared", value: true);
			meshRenderer.SetBlendShapeWeight(0, key1 -= openSpeed);
		}
	}

	private void Close()
	{
		if (key1 < 100f)
		{
			meshRenderer.SetBlendShapeWeight(0, key1 += openSpeed);
		}
		if (!released)
		{
			cumSound.Stop();
			blubSound.Stop();
			Lights.SetActive(value: false);
			heroineAnim.SetBool("isFalledBack", value: true);
			heroineAnim.speed = 1f;
			tentAnim.speed = 1f;
			PlayerController.iFalledBack = true;
			heroineAnim.SetBool("isScared", value: false);
			heroineAnim.SetBool("isAhegao", value: false);
			heroineAnim.SetBool("trapSoftFuck", value: false);
			heroineAnim.SetBool("trapHardFuck", value: false);
			heroineAnim.SetBool("trapped", value: false);
			tentAnim.SetBool("fuckHard", value: false);
			tentAnim.SetBool("fuckSoft", value: false);
			tentAnim.SetBool("rub", value: false);
			tentAnim.SetBool("idle", value: false);
			GetComponent<BoxCollider>().enabled = false;
			PlayerManager.instance.player.GetComponent<HeroineStats>().mySexPartner = null;
			PlayerController.iGetFucked = false;
			Heroine.GetComponent<PlayerController>().enabled = true;
			HeroineStats.aroused = false;
			tentControl.thrusted = false;
			sexDelay = 0f;
			tentControl.currentCum = 0f;
			DisableUI();
			open = false;
			cumCounter = 0;
			released = true;
		}
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.maxHealth = Tentacle.GetComponent<TentacleThrustEvent>().maxStamina;
		EnemyUI.instance.maxCum = Tentacle.GetComponent<TentacleThrustEvent>().maxCum;
		EnemyUI.instance.health = Tentacle.GetComponent<TentacleThrustEvent>().currentStamina;
		EnemyUI.instance.cum = Tentacle.GetComponent<TentacleThrustEvent>().currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			open = true;
			Lights.SetActive(value: true);
			tentAnim.SetBool("idle", value: true);
			heroineAnim.SetBool("trapped", value: true);
			Heroine.GetComponent<PlayerController>().enabled = false;
			Heroine.transform.position = base.gameObject.transform.position;
			Tentacle.transform.rotation = Heroine.transform.rotation;
			swallowSound.Play();
		}
	}
}
