using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WolfTakeMe : MonoBehaviour
{
	public GameObject heroine;

	public Transform mountPos;

	public Transform camPos;

	private Animator animator;

	public AudioSource sexOne;

	public AudioSource sexTwo;

	public AudioSource cumSound;

	public bool sex;

	private bool fastSex;

	private bool cum;

	private float normalSexTimer;

	private float fastSexTimer;

	public float normalSexDuration = 10f;

	public float fastSexDuration = 10f;

	private bool cumSoundPlayed;

	private bool pantieCheck;

	private Image humImage;

	private void Start()
	{
		animator = GetComponent<Animator>();
		pantieCheck = false;
		fastSex = false;
		humImage = GameObject.Find("ManagerAndUI/UI/Canvas/Inventory/HumSprite").GetComponent<Image>();
	}

	private void Update()
	{
		if (sex)
		{
			PlayerController.iFalled = true;
			PlayerController.iGetFucked = true;
			HeroineStats.stunned = true;
			CameraFollow.target = camPos;
			HeroineStats.aroused = true;
			PlayerManager.IsVirgin = false;
			heroine.GetComponent<PlayerController>().enabled = false;
			heroine.GetComponent<HeroineStats>().GainLust(1f);
			heroine.transform.rotation = Quaternion.Lerp(heroine.transform.rotation, mountPos.transform.rotation, 0.2f);
			heroine.transform.position = Vector3.Lerp(heroine.transform.position, mountPos.transform.position, 0.2f);
			if (!pantieCheck)
			{
				if (EquipmentManager.instance.currentEquipment[3] != null)
				{
					EquipmentManager.instance.RipPantsu();
				}
				pantieCheck = true;
			}
			if (!cum)
			{
				if (!fastSex)
				{
					heroine.GetComponent<Animator>().SetBool("falled", value: true);
					animator.SetBool("isResting", value: true);
					StartCoroutine(GrabToSex());
					return;
				}
				animator.SetBool("isSexingFront2", value: true);
				heroine.GetComponent<Animator>().SetBool("HumSexFront2", value: true);
				heroine.GetComponent<Animator>().SetBool("isAhegao", value: true);
				fastSexTimer += Time.deltaTime;
				if (fastSexTimer >= fastSexDuration)
				{
					cum = true;
				}
			}
			else
			{
				StartCoroutine(CumToRelease());
			}
		}
		else
		{
			heroine.GetComponent<Animator>().SetBool("HumCumFront2", value: false);
			heroine.GetComponent<Animator>().SetBool("HumTeaseFront", value: false);
			heroine.GetComponent<Animator>().SetBool("HumSexFront1", value: false);
			heroine.GetComponent<Animator>().SetBool("HumSexFront2", value: false);
			heroine.GetComponent<Animator>().SetBool("falled", value: true);
			heroine.GetComponent<Animator>().SetBool("isAhegao", value: false);
			PlayerController.iGetFucked = false;
			HeroineStats.stunned = false;
			HeroineStats.aroused = false;
			CameraFollow.target = heroine.transform;
			heroine.GetComponent<PlayerController>().enabled = true;
			GetComponent<WolfTakeMe>().enabled = false;
			base.gameObject.SetActive(value: false);
			Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator GrabToSex()
	{
		animator.SetBool("isInsertFront", value: true);
		heroine.GetComponent<Animator>().SetBool("HumTeaseFront", value: true);
		yield return new WaitForSeconds(2f);
		animator.SetBool("isSexingFront1", value: true);
		heroine.GetComponent<Animator>().SetBool("HumSexFront1", value: true);
		normalSexTimer += Time.deltaTime * 1f;
		if (normalSexTimer >= normalSexDuration)
		{
			fastSex = true;
		}
	}

	private IEnumerator CumToRelease()
	{
		animator.SetBool("isCummingFront2", value: true);
		heroine.GetComponent<Animator>().SetBool("HumCumFront2", value: true);
		HeroineStats.HumanoidBuff = true;
		HeroineStats.creampied = true;
		HeroineStats.hugeAmount = true;
		heroine.GetComponent<Animator>().SetBool("isCumFilled", value: true);
		humImage.enabled = true;
		if (!cumSoundPlayed)
		{
			cumSound.Play();
			cumSoundPlayed = true;
		}
		yield return new WaitForSeconds(3f);
		cumSound.Stop();
		sex = false;
	}

	private void ThrustEvent()
	{
		heroine.GetComponent<HeroineStats>().GainOrgInstant(1.5f);
		sexOne.Play();
	}

	private void ThrustEventHard()
	{
		if (sex)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(2.5f);
			heroine.GetComponent<HeroineStats>().GainExp(5f);
		}
		sexTwo.Play();
	}
}
