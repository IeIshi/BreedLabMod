using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroineStats : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public float currentOrg;

		public float currentLust;

		public float currentStamina;

		public float maxStamina;

		public int lewd;

		public bool pregnant;

		public bool HumanoidBuff;

		public bool MantisBuff;

		public float debuffedStam;

		public float currentPreg;

		public float currentExp;

		public int myLevel;

		public bool fertileCum;

		public bool lustyCum;

		public bool addictiveCum;

		public bool creampied;

		public bool corrupted;
	}

	public static HeroineStats instance;

	public static float currentStamina;

	public static float maxStamina;

	public static float currentLust;

	public float maxLust;

	public static float currentOrg;

	public float maxOrg;

	public static float currentPower;

	public float maxPower = 15f;

	public static float currentPreg;

	public float maxPreg;

	public static float currentExp;

	public float maxExp;

	public static int myLevel;

	private Slider Stamina;

	private Slider Lust;

	private Slider Org;

	private Slider OrgBackBar;

	private Slider Power;

	private Slider Malus;

	private CharacterController controller;

	public GameObject Slug;

	public GameObject RescuePhan;

	private int curStam;

	private int maxStam;

	private int curLust;

	private int curOrg;

	private int curExp;

	private int maxExpInt;

	private int curPower;

	private float curPreg;

	private int pregInt;

	private Text staminaAmount;

	private Text maxStaminaAmount;

	private Text lustAmount;

	private Text orgAmount;

	private Text energyAmount;

	private Text ammoAmount;

	public Text expAmount;

	public Text maxExpAmount;

	private Text pregPercent;

	private Text powerAmount;

	public float chargeSpeed = 10f;

	public float fartigecd = 5f;

	public float lovePotionCd = 30f;

	public float gainPregSpeed = 0.005f;

	public static bool buttonHeldDown;

	public static bool calculated;

	public static bool creampied;

	public static bool oralCreampie;

	public static bool fartiged;

	public static bool pregnant;

	public static Image fartigeImage;

	private Image fartigeMainImage;

	private Image pregnantImage;

	[SerializeField]
	public Image pregCircleImage;

	private Image loveImage;

	private Image loveCircle;

	private Image hornyImage;

	[SerializeField]
	public GameObject pregnantWholeImage;

	private float currentTimeElapsed;

	private float lovePotionTimeElapsed;

	private float pct;

	private float lovepct;

	public static bool staminaLow;

	public float lustDeregRate = 5f;

	public float orgDeregRate = 2f;

	public float lustResistance = 10f;

	public float orgResistance = 10f;

	public float StaminaGainRate = 3.5f;

	public static int lewd;

	public int numOfHearts;

	public Image[] hearts;

	public Sprite fullHeart;

	public Sprite emptyHeart;

	public static bool lovePotion;

	public static bool pregByHum;

	public static bool HumanoidBuff;

	public static bool MantisBuff;

	public static bool orgasm;

	private float orgTime;

	public static bool immune;

	private float immuneTimer;

	public static float debuffedStam;

	public ParticleSystem heartFlash;

	private bool heartIsPlaying;

	public static bool stunned;

	private Image humImage;

	private Image mantisImage;

	private AudioSource struggleSound;

	private AudioSource drinkSound;

	public static bool horny;

	public static bool aroused;

	public static bool wet;

	public static bool edging;

	public static bool masturbating;

	private AudioSource heartBeatSlow;

	private AudioSource heartBeatFast;

	private GameObject shoesDuration;

	public static Image shoesDurSlider;

	public static Image skirtDurSlider;

	public static Image stockingsDurSlider;

	private GameObject gameManager;

	private float orgCalculatedValue;

	public Slider Exp;

	public Text levelVal;

	public static bool GameOver;

	public GameObject gameOverUI;

	private Animator animator;

	private bool slugCummed;

	public static bool birth;

	private float cumTimer;

	private float sexTimer;

	private float birthSceneTimer;

	private float phanPopTime;

	public AudioSource birthSound;

	public AudioSource cumSound;

	public Transform slugSexCamPos;

	private float startPregTimer;

	private bool impregRolette;

	public static Image PantiesCircle;

	public static Image PantiesIcon;

	public GameObject mySexPartner;

	public GameObject cumMeshPref;

	public GameObject cumMouthPref;

	public ParticleSystem cumStrain;

	public AudioSource cumDripSound;

	public AudioSource cumDripSound2;

	public AudioSource slikSound;

	public AudioSource wetSound;

	public SkinnedMeshRenderer cumMesh;

	public SkinnedMeshRenderer cumMouth;

	private bool timer;

	private bool decleared;

	private float time;

	private float randomTime;

	public bool showCumSpot;

	public bool showCumMouth;

	public static bool fertileCum;

	public static bool hugeAmount;

	public static bool lustyCum;

	public static bool addictiveCum;

	private float fertileCumTimer;

	private float cumDripOutTime;

	private float lerpTimer;

	private float chipSpeed = 1f;

	private GameObject OrgBar;

	private GameObject ContClimaxTxt;

	private TextMeshProUGUI climaxCountTxt;

	private int climaxCount;

	public GameObject Eyes;

	public Mesh normalEyes;

	public Mesh monsterEyes;

	public Material[] material;

	public Renderer rend;

	public static bool corrupted;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.maxStamina = maxStamina;
		saveData.currentStamina = currentStamina;
		saveData.currentLust = currentLust;
		saveData.currentOrg = currentOrg;
		saveData.lewd = lewd;
		saveData.pregnant = pregnant;
		saveData.HumanoidBuff = HumanoidBuff;
		saveData.MantisBuff = MantisBuff;
		saveData.debuffedStam = debuffedStam;
		saveData.currentPreg = currentPreg;
		saveData.currentExp = currentExp;
		saveData.myLevel = myLevel;
		saveData.fertileCum = fertileCum;
		saveData.lustyCum = lustyCum;
		saveData.addictiveCum = addictiveCum;
		saveData.creampied = creampied;
		saveData.corrupted = corrupted;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData obj = (SaveData)state;
		currentLust = obj.currentLust;
		currentStamina = obj.currentStamina;
		maxStamina = obj.maxStamina;
		currentOrg = obj.currentOrg;
		lewd = obj.lewd;
		pregnant = obj.pregnant;
		HumanoidBuff = obj.HumanoidBuff;
		MantisBuff = obj.MantisBuff;
		debuffedStam = obj.debuffedStam;
		currentPreg = obj.currentPreg;
		currentExp = obj.currentExp;
		myLevel = obj.myLevel;
		fertileCum = obj.fertileCum;
		lustyCum = obj.lustyCum;
		addictiveCum = obj.addictiveCum;
		creampied = obj.creampied;
		corrupted = obj.corrupted;
	}

	private void Awake()
	{
		shoesDurSlider = GameObject.Find("ManagerAndUI/UI/Canvas/shoesDuration/durBarEmpty/durBarFull").GetComponent<Image>();
		skirtDurSlider = GameObject.Find("ManagerAndUI/UI/Canvas/skirtDuration/durBarSkirtEmpty/durBarSkirtFull").GetComponent<Image>();
		stockingsDurSlider = GameObject.Find("ManagerAndUI/UI/Canvas/stockingsDuration/durBarStockingsEmpty/durBarStockingsFull").GetComponent<Image>();
		PantiesCircle = GameObject.Find("ManagerAndUI/UI/Canvas/Pantsu/PantiesCircle").GetComponent<Image>();
		gameOverUI = GameObject.Find("ManagerAndUI/UI/Canvas/GameOverUI");
		pregnantWholeImage = GameObject.Find("ManagerAndUI/UI/Canvas/Pregnant");
	}

	private void Start()
	{
		if (corrupted)
		{
			rend = Eyes.GetComponent<Renderer>();
			Eyes.GetComponent<SkinnedMeshRenderer>().sharedMesh = monsterEyes;
			rend.sharedMaterial = material[1];
		}
		if (PlayerManager.IsVirgin)
		{
			Debug.Log("You are still a virgin");
		}
		else
		{
			Debug.Log("You are not a virgin anymore");
		}
		Debug.Log("SAB?: " + PlayerManager.SAB);
		cumDripOutTime = 0f;
		animator = GetComponent<Animator>();
		maxExp = 100f + (float)((myLevel - 1) * 50);
		gameManager = GameObject.Find("ManagerAndUI/Game Manager");
		gameOverUI.SetActive(value: false);
		Stamina = GameObject.Find("StaminaBar").GetComponent<Slider>();
		Lust = GameObject.Find("LustBar").GetComponent<Slider>();
		Org = GameObject.Find("OrgBar").GetComponent<Slider>();
		OrgBackBar = GameObject.Find("BackOrgBar").GetComponent<Slider>();
		Power = GameObject.Find("PowerBar").GetComponent<Slider>();
		Malus = GameObject.Find("MalusBar").GetComponent<Slider>();
		staminaAmount = GameObject.Find("stamAmount").GetComponent<Text>();
		maxStaminaAmount = GameObject.Find("maxStamAmount").GetComponent<Text>();
		lustAmount = GameObject.Find("lustAmount").GetComponent<Text>();
		orgAmount = GameObject.Find("orgAmount").GetComponent<Text>();
		pregPercent = GameObject.Find("PregText").GetComponent<Text>();
		powerAmount = GameObject.Find("PowerValueTxt").GetComponent<Text>();
		energyAmount = GameObject.Find("energyAmount").GetComponent<Text>();
		ammoAmount = GameObject.Find("ammoAmount").GetComponent<Text>();
		fartigeImage = GameObject.Find("circle").GetComponent<Image>();
		fartigeMainImage = GameObject.Find("fartige").GetComponent<Image>();
		pregnantImage = GameObject.Find("Pregnant").GetComponent<Image>();
		pregCircleImage = GameObject.Find("PregCircle").GetComponent<Image>();
		loveImage = GameObject.Find("LovePotion").GetComponent<Image>();
		loveCircle = GameObject.Find("LoveCircle").GetComponent<Image>();
		hornyImage = GameObject.Find("horny").GetComponent<Image>();
		struggleSound = GameObject.Find("AudioSounds/Struggle").GetComponent<AudioSource>();
		humImage = GameObject.Find("ManagerAndUI/UI/Canvas/Inventory/HumSprite").GetComponent<Image>();
		mantisImage = GameObject.Find("ManagerAndUI/UI/Canvas/Inventory/MantisSprite").GetComponent<Image>();
		heartBeatSlow = GameObject.Find("ManagerAndUI/Audio/HeartBeat_Slow").GetComponent<AudioSource>();
		heartBeatFast = GameObject.Find("ManagerAndUI/Audio/HeartBeat_Fast").GetComponent<AudioSource>();
		OrgBar = GameObject.Find("OrgBars");
		ContClimaxTxt = GameObject.Find("ClimaxText");
		climaxCountTxt = GameObject.Find("ClimaxNumbers").GetComponent<TextMeshProUGUI>();
		if (HumanoidBuff)
		{
			humImage.enabled = true;
		}
		else
		{
			humImage.enabled = false;
		}
		if (MantisBuff)
		{
			mantisImage.enabled = true;
		}
		else
		{
			mantisImage.enabled = false;
		}
		if (maxStamina == 0f)
		{
			maxStamina = 100f;
			currentStamina = maxStamina;
		}
		maxLust = 100f;
		maxOrg = 100f;
		currentPower = 0f;
		heartIsPlaying = false;
		Power.value = CalculatePower();
		Stamina.value = CalculateStamina();
		Lust.value = CalculateLust();
		Org.value = CalculateOrg();
		controller = GetComponent<CharacterController>();
		Lust.value = CalculateLust();
		stunned = false;
		Slug.SetActive(value: false);
		RescuePhan.SetActive(value: false);
		currentLust = Mathf.Clamp(currentLust, 0f, 100f);
		currentOrg = Mathf.Clamp(currentOrg, 0f, 100f);
		currentPreg = Mathf.Clamp(currentPreg, 0f, 1f);
		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
		currentExp = Mathf.Clamp(currentExp, 0f, maxExp);
		fartigeMainImage.enabled = false;
		hornyImage.enabled = false;
		impregRolette = false;
		animator.SetFloat("pregSpeed", 1f);
		if (pregnant)
		{
			StartCoroutine(GetBelly());
		}
		OrgBar.SetActive(value: false);
		ContClimaxTxt.SetActive(value: false);
		pregnantWholeImage.SetActive(value: false);
	}

	private void Update()
	{
		if (InventoryUI.inventoryIsOpen)
		{
			Exp.value = CalculateExp();
			levelVal.text = myLevel.ToString();
		}
		ClampStats();
		if (!GameOver)
		{
			if (!addictiveCum)
			{
				GainStamina(StaminaGainRate);
			}
			else
			{
				GainStamina(StaminaGainRate / 2f);
			}
		}
		Malus.value = CalculateMalus();
		ShowStatsNumbers();
		PragnancyImage();
		CheckMyState(currentLust, currentOrg);
		if (birth)
		{
			mySexPartner = null;
			masturbating = false;
			if (DialogManager.inDialogue)
			{
				DialogManager.instance.EndDialogue();
			}
			if (slugCummed)
			{
				if (!GameOver)
				{
					RescuePhan.SetActive(value: true);
					phanPopTime += Time.deltaTime;
					if (phanPopTime > 0.5f)
					{
						GetComponent<PlayerController>().enabled = true;
						RescuePhan.SetActive(value: false);
						Slug.SetActive(value: false);
						animator.SetBool("birthStart", value: false);
						animator.SetBool("birth", value: false);
						animator.SetBool("slugHardSex", value: false);
						animator.SetBool("slugInsert", value: false);
						animator.SetBool("falled", value: true);
						Slug.GetComponent<Animator>().SetBool("insert", value: false);
						Slug.GetComponent<Animator>().SetBool("slugHardSex", value: false);
						Slug.GetComponent<Animator>().SetBool("birthStart", value: false);
						PlayerController.iGetFucked = false;
						aroused = false;
						phanPopTime = 0f;
						birthSceneTimer = 0f;
						slugCummed = false;
						birth = false;
						startPregTimer = 0f;
						return;
					}
				}
				else
				{
					SlugSexCycle();
				}
			}
			else
			{
				birthScene();
			}
		}
		if (immune)
		{
			immuneTimer += Time.deltaTime;
			if (immuneTimer >= 3f)
			{
				immune = false;
				immuneTimer = 0f;
			}
		}
		if (!PlayerController.iFalled)
		{
			if (!lustyCum)
			{
				if (!EquipmentManager.heroineIsNaked)
				{
					DrainLust(PassiveStats.instance.lustdecrease.GetValue());
				}
				else
				{
					DrainLust(PassiveStats.instance.lustdecrease.GetValue() / 2f);
				}
			}
			if (OrgBar.activeSelf)
			{
				OrgBar.SetActive(value: false);
			}
		}
		else
		{
			UpdateOrgMeter();
			if (!OrgBar.activeSelf && currentOrg > 1f)
			{
				OrgBar.SetActive(value: true);
			}
		}
		DrainOrg(PassiveStats.instance.orgdecrease.GetValue());
		if (currentOrg >= 99f && !lovePotion)
		{
			orgasm = true;
			currentOrg = 0f;
			orgTime = 0f;
			climaxCount++;
			Debug.Log("Climax Count: " + climaxCount);
		}
		if (orgasm)
		{
			animator.SetBool("isAhegao", value: true);
			if (!heartIsPlaying)
			{
				heartFlash.Play();
				heartIsPlaying = true;
			}
			OrgasmRoutine();
		}
		if (!fartiged && currentStamina >= 10f)
		{
			if (PlayerController.iGetInserted || PlayerController.iGetFucked)
			{
				if (!stunned && !PlayerManager.SAB)
				{
					Defence();
				}
			}
			else if (PlayerController.iFalled && !birth && !LarveBirth.larveBirthing && !orgasm)
			{
				StandUp();
			}
		}
		if (PlayerController.iFalled)
		{
			Power.gameObject.SetActive(value: true);
		}
		else
		{
			Power.gameObject.SetActive(value: false);
		}
		if (fartigeMainImage.enabled && !fartiged)
		{
			fartigeImage.fillAmount = 0f;
			pct = 0f;
			currentTimeElapsed = 0f;
			fartigeMainImage.enabled = false;
		}
		if (fartiged)
		{
			fartigeMainImage.enabled = true;
			currentTimeElapsed += Time.deltaTime;
			pct = currentTimeElapsed / fartigecd;
			fartigeImage.fillAmount = pct;
			if (fartigeImage.fillAmount >= 1f)
			{
				fartigeMainImage.enabled = false;
				fartigeImage.fillAmount = 0f;
				pct = 0f;
				currentTimeElapsed = 0f;
				fartiged = false;
			}
		}
		if (lovePotion)
		{
			loveImage.enabled = true;
			loveCircle.enabled = true;
			lovePotionTimeElapsed += Time.deltaTime;
			lovepct = lovePotionTimeElapsed / lovePotionCd;
			loveCircle.fillAmount = lovepct;
			if (loveCircle.fillAmount >= 1f)
			{
				lovePotion = false;
				loveCircle.fillAmount = 0f;
				lovePotionTimeElapsed = 0f;
			}
		}
		else
		{
			loveImage.enabled = false;
			loveCircle.enabled = false;
		}
		if (!HumanoidBuff)
		{
			if (currentStamina <= 10f && !PlayerController.iGetFucked && !PlayerController.iGetInserted && debuffedStam > maxStamina - 10f)
			{
				debuffedStam -= Time.deltaTime;
			}
		}
		else if (currentStamina <= 30f && !PlayerController.iGetInserted && !PlayerController.iGetFucked && debuffedStam > maxStamina - 30f)
		{
			debuffedStam -= Time.deltaTime;
		}
		if (debuffedStam >= maxStamina)
		{
			gameOverUI.SetActive(value: true);
			GameOver = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (GameOver)
			{
				return;
			}
			if (PlayerManager.cheatModeOn && debuffedStam != 0f)
			{
				drinkSound = GameObject.Find("DrinkSound").GetComponent<AudioSource>();
				drinkSound.Play();
				debuffedStam -= 20f;
				currentStamina = maxStamina;
				return;
			}
			if (Inventory.energyDrinkCount > 0 && debuffedStam != 0f)
			{
				drinkSound = GameObject.Find("DrinkSound").GetComponent<AudioSource>();
				drinkSound.Play();
				debuffedStam -= 20f;
				currentStamina = maxStamina;
				Inventory.energyDrinkCount--;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (GameOver || lovePotion)
			{
				return;
			}
			if (Inventory.lovePotionCount > 0)
			{
				drinkSound = GameObject.Find("DrinkSound").GetComponent<AudioSource>();
				drinkSound.Play();
				lovePotion = true;
				Inventory.lovePotionCount--;
			}
		}
		if (creampied)
		{
			if (!PlayerController.iFalled)
			{
				if (EquipmentManager.heroineIsNaked)
				{
					CumDrip();
					if (hugeAmount)
					{
						cumDripOutTime += Time.deltaTime;
						if (cumDripOutTime > 6f)
						{
							if (!pregnant)
							{
								animator.SetBool("isCumFilled", value: false);
							}
							ParticleSystem.MainModule main = cumStrain.main;
							main.loop = false;
							hugeAmount = false;
							cumDripOutTime = 0f;
						}
					}
				}
				else
				{
					cumStrain.Stop();
				}
			}
			else
			{
				cumStrain.Stop();
			}
			if (!showCumSpot)
			{
				cumMesh = UnityEngine.Object.Instantiate(cumMeshPref.GetComponent<SkinnedMeshRenderer>());
				cumMesh.transform.parent = EquipmentManager.instance.targetMesh.transform;
				cumMesh.bones = EquipmentManager.instance.targetMesh.bones;
				cumMesh.rootBone = EquipmentManager.instance.targetMesh.rootBone;
				showCumSpot = true;
			}
			if (fertileCum)
			{
				FertileCumInside();
			}
			if (hugeAmount)
			{
				PlayerController.walking = true;
				ParticleSystem.MainModule main2 = cumStrain.main;
				main2.loop = true;
			}
		}
		if (oralCreampie && !showCumMouth)
		{
			cumMouth = UnityEngine.Object.Instantiate(cumMouthPref.GetComponent<SkinnedMeshRenderer>());
			cumMouth.transform.parent = EquipmentManager.instance.targetMesh.transform;
			cumMouth.bones = EquipmentManager.instance.targetMesh.bones;
			cumMouth.rootBone = EquipmentManager.instance.targetMesh.rootBone;
			showCumMouth = true;
		}
	}

	private void FertileCumInside()
	{
		fertileCumTimer += Time.deltaTime;
		if (!(fertileCumTimer > 15f))
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, 100);
		if (PlayerManager.SAB)
		{
			if (50 >= num)
			{
				pregnant = true;
				animator.SetBool("isPregnant", value: true);
				fertileCum = false;
			}
			else
			{
				Debug.Log(num);
				Debug.Log("NOT PREGNANT");
			}
		}
		else if (25 >= num)
		{
			pregnant = true;
			animator.SetBool("isPregnant", value: true);
			fertileCum = false;
		}
		else
		{
			Debug.Log(num);
			Debug.Log("NOT PREGNANT");
		}
		fertileCumTimer = 0f;
	}

	public void CumDrip()
	{
		if (!decleared)
		{
			if (!hugeAmount)
			{
				randomTime = UnityEngine.Random.Range(0f, 10f);
			}
			else
			{
				randomTime = UnityEngine.Random.Range(0f, 4f);
			}
			decleared = true;
		}
		time += Time.deltaTime;
		if (time >= randomTime)
		{
			timer = true;
			cumStrain.Play();
			if ((float)UnityEngine.Random.Range(0, 2) == 0f)
			{
				cumDripSound.Play();
			}
			else
			{
				cumDripSound2.Play();
			}
			randomTime = 0f;
			time = 0f;
			decleared = false;
		}
		if (timer)
		{
			time += Time.deltaTime;
			if (time >= 1f)
			{
				timer = false;
				time = 0f;
			}
		}
	}

	public void LargeCumDrip()
	{
		randomTime = UnityEngine.Random.Range(0f, 10f);
		time += Time.deltaTime;
		if (time >= randomTime)
		{
			timer = true;
			cumStrain.Play();
			if ((float)UnityEngine.Random.Range(0, 2) == 0f)
			{
				cumDripSound.Play();
			}
			else
			{
				cumDripSound2.Play();
			}
			randomTime = 0f;
			time = 0f;
			decleared = false;
		}
		if (timer)
		{
			time += Time.deltaTime;
			if (time >= 1f)
			{
				timer = false;
				time = 0f;
			}
		}
	}

	private IEnumerator GetBelly()
	{
		animator.SetBool("isPregnant", value: true);
		animator.SetFloat("pregSpeed", 50f);
		yield return new WaitForSeconds(2f);
		animator.SetFloat("pregSpeed", 1f);
	}

	private void OrgasmRoutine()
	{
		orgTime += Time.deltaTime;
		if (!masturbating)
		{
			float num = (float)climaxCount * 0.3f;
			if (!corrupted)
			{
				if (!PlayerManager.SAB)
				{
					debuffedStam += Time.deltaTime * (1f + num);
				}
				else
				{
					debuffedStam += Time.deltaTime * (1f + num) / 2f;
				}
			}
		}
		else if (!lustyCum)
		{
			debuffedStam -= Time.deltaTime * 5f;
		}
		if (climaxCount > 1)
		{
			ContClimaxTxt.SetActive(value: true);
			climaxCountTxt.text = climaxCount.ToString();
		}
		if (!(orgTime >= 6f))
		{
			return;
		}
		animator.SetBool("isAhegao", value: false);
		orgasm = false;
		heartFlash.Stop();
		heartIsPlaying = false;
		if (masturbating)
		{
			if (!lustyCum)
			{
				DrainLustInstant(60f);
			}
			animator.SetBool("mastStanding", value: false);
			animator.SetBool("mastStandingHard", value: false);
			wetSound.Stop();
			if (!PlayerController.iGetFucked)
			{
				masturbating = false;
				PlayerController.iFalled = false;
			}
		}
		Malus.value = CalculateMalus();
		lewd++;
		orgTime = 0f;
		ContClimaxTxt.SetActive(value: false);
		climaxCount = 0;
		orgasm = false;
	}

	private void PragnancyImage()
	{
		if (pregnant)
		{
			Pregnancy();
		}
		else if (!LarveBirth.larveImpregnated)
		{
			pregnantWholeImage.SetActive(value: false);
			currentPreg = 0f;
		}
	}

	private void Pregnancy()
	{
		if (startPregTimer < 6f)
		{
			startPregTimer += Time.deltaTime;
		}
		if (startPregTimer > 5f)
		{
			pregnantWholeImage.SetActive(value: true);
			GainPreg(gainPregSpeed);
			pregCircleImage.fillAmount = currentPreg;
			animator.SetBool("isPregnant", value: true);
			if (currentPreg >= 1f && !PlayerController.iGetInserted && !PlayerController.iGetFucked)
			{
				GetComponent<PlayerController>().enabled = false;
				birth = true;
				fertileCum = false;
				PlayerController.iFalled = true;
				PostProcessingManager.instance.bloom.dirtIntensity.value = 0f;
				pregnant = false;
			}
		}
	}

	private void birthScene()
	{
		birthSceneTimer += Time.deltaTime;
		animator.SetBool("mastStanding", value: false);
		animator.SetBool("mastStandingHard", value: false);
		animator.SetBool("birthStart", value: true);
		CameraFollow.target = slugSexCamPos;
		PlayerController.iFalledFront = true;
		PlayerController.iFalled = true;
		PlayerController.iGetFucked = true;
		if (birthSceneTimer > 3f && birthSceneTimer < 11f)
		{
			if (gameManager.GetComponent<EquipmentManager>().currentEquipment[3] != null)
			{
				gameManager.GetComponent<EquipmentManager>().RipPantsu();
			}
			aroused = true;
			animator.SetBool("isAhegao", value: true);
			animator.SetBool("birth", value: true);
			animator.SetBool("isPregnant", value: false);
			animator.SetBool("isCumFilled", value: false);
			GainLust(2f);
			GainOrg(10f);
			if (!birthSound.isPlaying)
			{
				birthSound.Play();
			}
			impregRolette = false;
			Slug.SetActive(value: true);
		}
		else
		{
			birthSound.Stop();
		}
		if (birthSceneTimer > 18f)
		{
			Slug.GetComponent<Animator>().SetBool("insert", value: true);
			animator.SetBool("slugInsert", value: true);
			GainLust(2f);
		}
		if (birthSceneTimer > 33f)
		{
			Slug.GetComponent<Animator>().SetBool("hardSex", value: true);
			animator.SetBool("slugHardSex", value: true);
			if (birthSceneTimer > 45f && birthSceneTimer < 60f)
			{
				Slug.GetComponent<Animator>().speed = 2f;
				animator.speed = 2f;
			}
		}
		if (birthSceneTimer > 60f)
		{
			Slug.GetComponent<Animator>().SetBool("cum", value: true);
			animator.SetBool("slugCum", value: true);
			Slug.GetComponent<Animator>().speed = 1f;
			animator.speed = 1f;
			if (!slugCummed && !cumSound.isPlaying)
			{
				cumSound.Play();
				if (!impregRolette)
				{
					creampied = true;
					lustyCum = true;
					impregRolette = true;
				}
			}
		}
		if (birthSceneTimer > 65f)
		{
			cumSound.Stop();
			Slug.GetComponent<Animator>().SetBool("cum", value: false);
			animator.SetBool("slugCum", value: false);
			slugCummed = true;
		}
	}

	private void SlugSexCycle()
	{
		GainLust(2f);
		if (sexTimer <= 30f)
		{
			sexTimer += Time.deltaTime;
			Debug.Log(sexTimer);
			Slug.GetComponent<Animator>().SetBool("hardSex", value: true);
			animator.SetBool("slugHardSex", value: true);
			if (sexTimer > 20f && sexTimer < 30f)
			{
				Slug.GetComponent<Animator>().speed = 2f;
				animator.speed = 2f;
			}
			Slug.GetComponent<Animator>().SetBool("cum", value: false);
			animator.SetBool("slugCum", value: false);
			cumSound.Stop();
		}
		if (sexTimer >= 30f)
		{
			cumTimer += Time.deltaTime;
			Slug.GetComponent<Animator>().speed = 1f;
			animator.speed = 1f;
			Slug.GetComponent<Animator>().SetBool("cum", value: true);
			animator.SetBool("slugCum", value: true);
			if (!cumSound.isPlaying)
			{
				cumSound.Play();
			}
		}
		if (cumTimer >= 5f)
		{
			sexTimer = 0f;
			cumTimer = 0f;
		}
	}

	private void ShowStatsNumbers()
	{
		curStam = (int)currentStamina;
		maxStam = (int)maxStamina;
		curLust = (int)currentLust;
		curOrg = (int)currentOrg;
		curPreg = currentPreg * 100f;
		pregInt = (int)curPreg;
		staminaAmount.text = curStam.ToString();
		maxStaminaAmount.text = maxStam.ToString();
		lustAmount.text = curLust.ToString();
		orgAmount.text = curOrg.ToString();
		pregPercent.text = pregInt.ToString();
		energyAmount.text = Inventory.energyDrinkCount.ToString();
		ammoAmount.text = Inventory.lovePotionCount.ToString();
		curExp = (int)currentExp;
		expAmount.text = curExp.ToString();
		maxExpAmount.text = maxExp.ToString();
	}

	private void OnSprint()
	{
		Vector3 velocity = controller.velocity;
		float magnitude = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
		_ = controller.velocity;
		if (magnitude >= 3f)
		{
			DrainStamina(0.5f);
		}
		else
		{
			GainStamina(StaminaGainRate);
		}
	}

	public void UpdateStats()
	{
		curPower = (int)currentPower;
		powerAmount.text = curPower.ToString();
	}

	private void Defence()
	{
		currentPower = Mathf.Clamp(currentPower, 0f, currentStamina);
		Power.value = CalculatePower();
		curPower = (int)currentPower;
		powerAmount.text = curPower.ToString();
		buttonHeldDown = Input.GetKey(KeyCode.Space);
		if (buttonHeldDown)
		{
			if (aroused)
			{
				currentPower += Time.deltaTime * chargeSpeed / 2f;
				return;
			}
			if (horny)
			{
				currentPower += Time.deltaTime * chargeSpeed / 3f;
				return;
			}
			currentPower += Time.deltaTime * chargeSpeed;
		}
		else if (!buttonHeldDown)
		{
			if (currentPower > 0f)
			{
				currentStamina -= currentPower;
				Stamina.value = CalculateStamina();
				struggleSound.Play();
				try
				{
					float damage = currentPower + currentPower * PassiveStats.instance.damage.GetValue();
					mySexPartner.GetComponent<DamageTranslator>().RevieveDamage(damage);
					Debug.Log("My Struggle Damage: " + damage);
					calculated = true;
					fartiged = true;
					UnityEngine.Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				}
				catch
				{
					calculated = true;
					fartiged = true;
					UnityEngine.Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0f, 0.3f, 5f, 2f, 0.7f, 0.3f, 0.6f, 0.3f));
				}
			}
			if (calculated)
			{
				currentPower = 0f;
				curPower = (int)currentPower;
				powerAmount.text = curPower.ToString();
				Power.value = CalculatePower();
				calculated = false;
			}
		}
		if (buttonHeldDown && currentStamina < 10f)
		{
			Debug.Log("Im to tired");
		}
	}

	private void StandUp()
	{
		currentPower = Mathf.Clamp(currentPower, 0f, maxPower);
		Power.value = CalculatePower();
		curPower = (int)currentPower;
		powerAmount.text = curPower.ToString();
		buttonHeldDown = Input.GetKey(KeyCode.Space);
		if (buttonHeldDown)
		{
			currentPower += Time.deltaTime * (chargeSpeed * 2f);
		}
		else if (!buttonHeldDown)
		{
			if (currentPower == 10f)
			{
				currentStamina -= currentPower;
				Stamina.value = CalculateStamina();
				PlayerController.iFalled = false;
				PlayerController.iFalledFront = false;
				PlayerController.iFalledBack = false;
				PlayerController.animator.SetBool("isSitting", value: false);
				PlayerController.animator.SetBool("originMasturbate", value: false);
				fartigeMainImage.enabled = false;
				PlayerController.claimed = false;
				immune = true;
				GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>().ps.SetActive(value: false);
				animator.SetBool("isAhegao", value: false);
				animator.SetBool("falled", value: false);
				animator.SetBool("isFalledBack", value: false);
				animator.SetBool("isInserting", value: false);
				animator.SetBool("isScared", value: false);
				animator.SetBool("isHorny", value: false);
				animator.SetBool("octoDoubleFinished", value: false);
				animator.SetBool("mastStanding", value: false);
				animator.SetBool("mastStandingHard", value: false);
				masturbating = false;
				wetSound.Stop();
				PlayerController.heIsFuckingHard = false;
				PlayerController.soundplayed = false;
				CameraFollow.target = base.gameObject.transform;
				calculated = true;
				fartiged = true;
			}
			if (calculated)
			{
				currentPower = 0f;
				curPower = (int)currentPower;
				powerAmount.text = curPower.ToString();
				Power.value = CalculatePower();
				calculated = false;
			}
		}
		if (buttonHeldDown && currentStamina < 10f)
		{
			Debug.Log("Im to tired");
		}
	}

	private void GainStamina(float gainValue)
	{
		Stamina.value = CalculateStamina();
		if (!(currentStamina >= maxStamina - debuffedStam))
		{
			currentStamina += gainValue * Time.deltaTime;
		}
	}

	public void DrainStamina(float drainValue)
	{
		currentStamina -= drainValue * Time.deltaTime;
		Stamina.value = CalculateStamina();
	}

	private void DebuffStamina(float debuffValue)
	{
		maxStamina -= debuffValue;
		Stamina.value = CalculateStamina();
	}

	private void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		Stamina.value = CalculateStamina();
	}

	public void GainLust(float gainValue)
	{
		float num = gainValue - PassiveStats.instance.lustRes.GetValue() / 100f * gainValue;
		currentLust += num * Time.deltaTime;
		Lust.value = CalculateLust();
	}

	public void GainLustInstant(float gainValue)
	{
		float num = gainValue - PassiveStats.instance.lustRes.GetValue() / 100f * gainValue;
		currentLust += num;
		Lust.value = CalculateLust();
	}

	private void DrainLust(float drainValue)
	{
		currentLust -= drainValue * Time.deltaTime;
		Lust.value = CalculateLust();
	}

	private void DrainLustInstant(float drainValue)
	{
		currentLust -= drainValue;
		Lust.value = CalculateLust();
	}

	public void GainOrg(float gainValue)
	{
		currentOrg += gainValue * Time.deltaTime;
		Org.value = CalculateOrg();
	}

	public void GainExp(float gainValue)
	{
		currentExp += gainValue;
		Exp.value = CalculateExp();
		if (currentExp >= maxExp && myLevel != 10)
		{
			currentExp = 0f;
			myLevel++;
			maxStamina += 5f;
			PassiveStats.instance.orgRes.AddModifier(0.2f);
			PassiveStats.instance.damage.AddModifier(0.05f);
			maxExp = 100f + (float)((myLevel - 1) * 50);
		}
	}

	public void LosePantiesDurability(float gainValue)
	{
		float num = gainValue / 100f * Time.deltaTime;
		EquipmentManager.pantiesDurability -= num;
		PantiesCircle.fillAmount = EquipmentManager.pantiesDurability;
		if (EquipmentManager.pantiesDurability <= 0f)
		{
			EquipmentManager.heroineIsNaked = true;
			gameManager.GetComponent<EquipmentManager>().RipPantsu();
		}
	}

	public void GainOrgInstant(float gainValue)
	{
		float num = gainValue + gainValue * (2f * currentLust / 100f) - PassiveStats.instance.orgRes.GetValue() / 100f * gainValue;
		currentOrg += num;
		lerpTimer = 0f;
		if (masturbating || PlayerManager.infCloth)
		{
			return;
		}
		if (EquipmentManager.skirtOn)
		{
			EquipmentManager.skirtDurability -= 0.01f;
			skirtDurSlider.fillAmount = EquipmentManager.skirtDurability;
			if (EquipmentManager.skirtDurability <= 0f)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(2);
			}
		}
		if (EquipmentManager.stockingsOn)
		{
			EquipmentManager.stockingsDurability -= 0.005f;
			stockingsDurSlider.fillAmount = EquipmentManager.stockingsDurability;
			if (EquipmentManager.stockingsDurability <= 0f)
			{
				gameManager.GetComponent<EquipmentManager>().RipOff(4);
			}
		}
	}

	private void DrainOrg(float drainValue)
	{
		if (!orgasm)
		{
			currentOrg -= drainValue * Time.deltaTime;
		}
	}

	public void GainPreg(float gainValue)
	{
		currentPreg += gainValue * Time.deltaTime;
	}

	private float CalculateStamina()
	{
		return currentStamina / maxStamina;
	}

	private float CalculateLust()
	{
		return currentLust / maxLust;
	}

	private float CalculateOrg()
	{
		return currentOrg / maxOrg;
	}

	private float CalculateExp()
	{
		return currentExp / maxExp;
	}

	private float CalculatePower()
	{
		return currentPower / currentStamina;
	}

	private float CalculateStandupPower()
	{
		return currentPower / maxPower;
	}

	private float CalculatePreg()
	{
		return currentPreg / maxPreg;
	}

	private float CalculateMalus()
	{
		return debuffedStam / maxStamina;
	}

	private void ClampStats()
	{
		currentLust = Mathf.Clamp(currentLust, 0f, 100f);
		currentOrg = Mathf.Clamp(currentOrg, 0f, 100f);
		currentPreg = Mathf.Clamp(currentPreg, 0f, 1f);
		debuffedStam = Mathf.Clamp(debuffedStam, 0f, maxStamina);
		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina - debuffedStam);
	}

	private void UpdateOrgMeter()
	{
		float value = Org.value;
		float value2 = OrgBackBar.value;
		float num = currentOrg / maxOrg;
		if (value2 > num)
		{
			Org.value = num;
			lerpTimer += Time.deltaTime;
			float num2 = lerpTimer / chipSpeed;
			num2 *= num2;
			OrgBackBar.value = Mathf.Lerp(value2, num, num2);
		}
		if (value < num)
		{
			OrgBackBar.value = num;
			lerpTimer += Time.deltaTime;
			float num3 = lerpTimer / chipSpeed;
			num3 *= num3;
			Org.value = Mathf.Lerp(value, OrgBackBar.value, num3);
		}
	}

	private void CheckMyState(float curLust, float curOrg)
	{
		if (curLust > 79f)
		{
			horny = true;
			hornyImage.enabled = true;
		}
		else
		{
			horny = false;
			hornyImage.enabled = false;
		}
		if (curOrg > 50f && curOrg < 80f)
		{
			if (!heartBeatSlow.isPlaying)
			{
				heartBeatSlow.Play();
			}
		}
		else
		{
			heartBeatSlow.Stop();
		}
		if (curOrg > 79f)
		{
			if (!heartBeatFast.isPlaying)
			{
				heartBeatFast.Play();
			}
		}
		else if (!orgasm)
		{
			heartBeatFast.Stop();
		}
	}

	private void FingerEvent()
	{
		slikSound.Play();
		if (!wetSound.isPlaying)
		{
			wetSound.Play();
		}
		if (orgasm)
		{
			GainOrgInstant(0.1f);
			return;
		}
		if (currentLust >= 80f)
		{
			GainOrgInstant(1.25f);
		}
		else
		{
			GainOrgInstant(0.1f);
		}
		if (currentOrg > 50f)
		{
			animator.SetBool("mastStandingHard", value: true);
		}
	}
}
