using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TentacleWall : MonoBehaviour
{
	public AudioSource grabSound;

	public AudioSource slideSound;

	public AudioSource cumSound;

	public ParticleSystem cumStrain;

	public GameObject tentTop;

	public GameObject tentButtom;

	public GameObject baseWall;

	public GameObject armTraps;

	public GameObject Heroine;

	public GameObject dropPoint;

	public GameObject camPos;

	private Animator tentTopAnim;

	private Animator tentButtomAnim;

	private Animator heroineAnim;

	private TentTop tentTopScript;

	private TentButtom tentButtomScript;

	private bool slideOut;

	private SkinnedMeshRenderer armTrapMeshRenderer;

	private SkinnedMeshRenderer baseWallRenderer;

	private float armTrapMSize;

	private float baseWallMSize;

	private bool scaleDown;

	private bool grabbedHer;

	public Image circle;

	public Image SexImage;

	private float fuckDelay;

	public float fuckTime;

	private float pct;

	private bool cumming;

	private bool recovering;

	public float recoveryTime;

	private float timer;

	private bool grabSoundPlayed;

	private bool extendWall;

	private int cumCounter;

	private void Start()
	{
		tentTopAnim = tentTop.GetComponent<Animator>();
		tentButtomAnim = tentButtom.GetComponent<Animator>();
		heroineAnim = Heroine.GetComponent<Animator>();
		tentTopScript = tentTop.GetComponent<TentTop>();
		tentButtomScript = tentButtom.GetComponent<TentButtom>();
		armTrapMeshRenderer = armTraps.GetComponent<SkinnedMeshRenderer>();
		baseWallRenderer = baseWall.GetComponent<SkinnedMeshRenderer>();
		baseWallMSize = 100f;
		baseWallRenderer.SetBlendShapeWeight(0, baseWallMSize);
		cumStrain.Stop();
	}

	private void FixedUpdate()
	{
		if (extendWall)
		{
			if (baseWallMSize > 0f)
			{
				baseWallRenderer.SetBlendShapeWeight(0, baseWallMSize -= 200f * Time.deltaTime);
			}
			if (baseWallMSize < 0f)
			{
				baseWallMSize = 0f;
				baseWallRenderer.SetBlendShapeWeight(0, baseWallMSize);
				extendWall = false;
			}
		}
		if (recovering)
		{
			if (baseWallMSize < 100f)
			{
				baseWallRenderer.SetBlendShapeWeight(0, baseWallMSize += 200f * Time.deltaTime);
			}
			timer += Time.deltaTime;
			if (timer >= recoveryTime)
			{
				tentButtomScript.currentStamina = tentButtomScript.maxStamina;
				timer = 0f;
				recovering = false;
			}
		}
		else if (tentButtomScript.currentStamina <= 0f)
		{
			Release();
		}
		else if (PlayerManager.SAB && cumCounter == 2 && !HeroineStats.GameOver)
		{
			Release();
		}
		else
		{
			if (!tentTopScript.grabbed)
			{
				return;
			}
			if (!grabSoundPlayed)
			{
				grabSound.Play();
				grabSoundPlayed = true;
			}
			InitiateUI();
			tentButtomScript.VignetteEffect();
			float maxDistanceDelta = 5f * Time.deltaTime;
			Heroine.transform.position = Vector3.MoveTowards(Heroine.transform.position, base.transform.position, maxDistanceDelta);
			Heroine.transform.rotation = base.transform.rotation;
			StartCoroutine(ExtendGrabArms());
			if (!slideOut)
			{
				StartCoroutine(ExtendTentacle());
				return;
			}
			if (!EquipmentManager.heroineIsNaked)
			{
				Heroine.GetComponent<HeroineStats>().LosePantiesDurability(5f);
				return;
			}
			if (!PlayerController.iGetFucked)
			{
				InitiateSex();
				return;
			}
			if (tentButtomScript.currentCum >= 100f)
			{
				cumming = true;
				HeroineStats.creampied = true;
				HeroineStats.fertileCum = true;
				HeroineStats.hugeAmount = true;
				tentButtomAnim.SetBool("fuck", value: false);
				heroineAnim.SetBool("tWallFuck", value: false);
				tentButtomAnim.SetBool("doggyFuck", value: false);
				heroineAnim.SetBool("tWallDoggyFuck", value: false);
				tentTopAnim.SetBool("doggyFuck", value: false);
				tentTopAnim.SetBool("grab", value: false);
				heroineAnim.SetBool("isCumFilled", value: true);
				heroineAnim.speed = 1f;
				tentButtomAnim.speed = 1f;
				tentTopAnim.speed = 1f;
			}
			if (cumming)
			{
				tentButtomAnim.SetBool("cum", value: true);
				tentTopAnim.SetBool("cum", value: true);
				heroineAnim.SetBool("tWallCum", value: true);
				if (!cumSound.isPlaying)
				{
					cumSound.Play();
					cumStrain.Play();
				}
				if (tentButtomScript.currentCum <= 0f)
				{
					tentButtomAnim.SetBool("fuck", value: true);
					heroineAnim.SetBool("tWallFuck", value: true);
					tentTopAnim.SetBool("grab", value: true);
					tentButtomAnim.SetBool("cum", value: false);
					tentTopAnim.SetBool("cum", value: false);
					heroineAnim.SetBool("tWallCum", value: false);
					armTrapMeshRenderer.SetBlendShapeWeight(0, 0f);
					armTrapMSize = 0f;
					cumSound.Stop();
					cumStrain.Stop();
					cumCounter++;
					cumming = false;
				}
			}
			else if (tentButtomScript.currentCum > 70f)
			{
				if (armTrapMSize < 100f)
				{
					armTrapMeshRenderer.SetBlendShapeWeight(0, armTrapMSize += 500f * Time.deltaTime);
				}
				tentButtomAnim.SetBool("doggyFuck", value: true);
				heroineAnim.SetBool("tWallDoggyFuck", value: true);
				tentTopAnim.SetBool("doggyFuck", value: true);
			}
			else
			{
				tentButtomAnim.SetBool("fuck", value: true);
				heroineAnim.SetBool("tWallFuck", value: true);
				if (tentButtomScript.currentCum > 25f)
				{
					heroineAnim.speed = 2f;
					tentButtomAnim.speed = 2f;
					tentTopAnim.speed = 2f;
				}
			}
		}
	}

	private void InitiateSex()
	{
		if (EquipmentManager.heroineIsNaked)
		{
			SexImage.enabled = true;
			circle.enabled = true;
			heroineAnim.SetBool("isScared", value: false);
			HeroineStats.aroused = true;
			fuckDelay += Time.deltaTime;
			pct = fuckDelay / fuckTime;
			circle.fillAmount = pct;
			if (fuckDelay >= fuckTime)
			{
				SexImage.enabled = false;
				circle.enabled = false;
				circle.fillAmount = 0f;
				pct = 0f;
				PlayerManager.IsVirgin = false;
				PlayerController.iGetFucked = true;
				PlayerController.iGetInserted = false;
			}
		}
	}

	private void InitiateUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: true);
		EnemyUI.instance.maxHealth = tentButtom.GetComponent<TentButtom>().maxStamina;
		EnemyUI.instance.maxCum = tentButtom.GetComponent<TentButtom>().maxCum;
		EnemyUI.instance.health = tentButtom.GetComponent<TentButtom>().currentStamina;
		EnemyUI.instance.cum = tentButtom.GetComponent<TentButtom>().currentCum;
	}

	private void DisableUI()
	{
		EnemyUI.instance.gameObject.SetActive(value: false);
	}

	private void Release()
	{
		Heroine.GetComponent<HeroineStats>().mySexPartner = null;
		StopAllCoroutines();
		tentButtomAnim.SetBool("fuck", value: false);
		heroineAnim.SetBool("tWallFuck", value: false);
		tentButtomAnim.SetBool("doggyFuck", value: false);
		heroineAnim.SetBool("tWallDoggyFuck", value: false);
		tentTopAnim.SetBool("doggyFuck", value: false);
		tentButtomAnim.SetBool("cum", value: false);
		tentTopAnim.SetBool("cum", value: false);
		heroineAnim.SetBool("tWallCum", value: false);
		heroineAnim.SetBool("tWallGrabbed", value: false);
		tentTopAnim.SetBool("grab", value: false);
		tentButtomAnim.SetBool("slideOut", value: false);
		heroineAnim.SetBool("isFalledBack", value: true);
		heroineAnim.SetBool("isScared", value: false);
		DisableUI();
		fuckDelay = 0f;
		heroineAnim.speed = 1f;
		tentButtomAnim.speed = 1f;
		tentTopAnim.speed = 1f;
		cumCounter = 0;
		cumming = false;
		tentButtomScript.currentCum = 0f;
		armTrapMSize = 0f;
		armTrapMeshRenderer.SetBlendShapeWeight(0, armTrapMSize);
		PlayerController.iGetInserted = false;
		PlayerController.iGetFucked = false;
		HeroineStats.aroused = false;
		tentTopScript.grabbed = false;
		slideOut = false;
		Heroine.transform.position = dropPoint.transform.position;
		Heroine.GetComponent<PlayerController>().enabled = true;
		CameraFollow.target = Heroine.transform;
		SexImage.enabled = false;
		circle.enabled = false;
		slideSound.Stop();
		cumSound.Stop();
		cumStrain.Stop();
		recovering = true;
		grabSoundPlayed = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!recovering && !PlayerController.iFalled && other.gameObject.tag == "Player")
		{
			extendWall = true;
			tentTopAnim.SetBool("grab", value: true);
			heroineAnim.SetBool("isNaked", value: true);
			heroineAnim.SetBool("isScared", value: true);
			Heroine.GetComponent<PlayerController>().enabled = false;
			CameraFollow.target = camPos.transform;
			PlayerController.iFalled = true;
			PlayerController.iGetInserted = true;
			Heroine.GetComponent<HeroineStats>().mySexPartner = tentButtom;
		}
	}

	private void ExtendTrapArms()
	{
		if (grabbedHer)
		{
			return;
		}
		if (!scaleDown)
		{
			armTrapMeshRenderer.SetBlendShapeWeight(0, armTrapMSize += 700f * Time.deltaTime);
		}
		if (armTrapMSize > 80f)
		{
			scaleDown = true;
		}
		if (scaleDown)
		{
			armTrapMeshRenderer.SetBlendShapeWeight(0, armTrapMSize -= 700f * Time.deltaTime);
			if (armTrapMSize < 1f)
			{
				scaleDown = false;
				grabbedHer = true;
			}
		}
	}

	private IEnumerator ExtendGrabArms()
	{
		yield return new WaitForSeconds(1f);
		ExtendTrapArms();
	}

	private IEnumerator ExtendTentacle()
	{
		yield return new WaitForSeconds(4f);
		tentButtomAnim.SetBool("slideOut", value: true);
		if (!slideSound.isPlaying)
		{
			slideSound.Play();
		}
		yield return new WaitForSeconds(4f);
		slideOut = true;
		slideSound.Stop();
	}
}
