using System.Collections;
using UnityEngine;

public class LarveBirth : MonoBehaviour
{
	private Animator heroineAnimator;

	public Transform larveSpawnTransform;

	public GameObject LarvePref;

	private GameObject Larve;

	private float startPregTimer;

	public float pregnancySpeed;

	[SerializeField]
	public static bool larveImpregnated;

	public static bool larveBirthing;

	private void Start()
	{
		heroineAnimator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (larveImpregnated)
		{
			LarvePregnancy();
		}
	}

	private void LarvePregnancy()
	{
		if (startPregTimer < 6f)
		{
			startPregTimer += Time.deltaTime;
			MovementManipulator.instance.img.enabled = false;
		}
		if (!(startPregTimer > 5f))
		{
			return;
		}
		GetComponent<HeroineStats>().pregnantWholeImage.SetActive(value: true);
		GetComponent<HeroineStats>().GainPreg(pregnancySpeed);
		GetComponent<HeroineStats>().pregCircleImage.fillAmount = HeroineStats.currentPreg;
		PostProcessingManager.instance.bloom.dirtIntensity.value = Mathf.PingPong(Time.time * 100f, 60f);
		if (HeroineStats.currentPreg >= 1f && !HeroineStats.masturbating && !PlayerController.iGetInserted && !PlayerController.iGetFucked)
		{
			heroineAnimator.SetBool("isCumFilled", value: false);
			PlayerController.iFalled = true;
			HeroineStats.stunned = true;
			larveBirthing = true;
			CameraFollow.target = GetComponent<HeroineStats>().slugSexCamPos;
			heroineAnimator.SetBool("isScared", value: true);
			heroineAnimator.SetBool("larveBirth", value: true);
			if (EquipmentManager.instance.GetComponent<EquipmentManager>().currentEquipment[3] != null)
			{
				EquipmentManager.instance.GetComponent<EquipmentManager>().RipPantsu();
			}
			MovementManipulator.occupied = true;
			if (heroineAnimator.GetCurrentAnimatorStateInfo(0).IsName("rig|LarveBirth") && heroineAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
			{
				heroineAnimator.SetBool("falled", value: true);
				heroineAnimator.SetBool("larveBirth", value: false);
				heroineAnimator.SetBool("isScared", value: false);
				MovementManipulator.occupied = false;
				HeroineStats.stunned = false;
				PostProcessingManager.instance.bloom.dirtIntensity.value = 0f;
				HeroineStats.currentPreg = 0f;
				StartCoroutine(waitPls());
				GetComponent<HeroineStats>().pregnantWholeImage.SetActive(value: false);
				MovementManipulator.instance.img.enabled = true;
				larveImpregnated = false;
			}
		}
	}

	private IEnumerator waitPls()
	{
		yield return new WaitForSeconds(7f);
		larveBirthing = false;
	}

	private void SpawnEvent()
	{
		Larve = Object.Instantiate(LarvePref, new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z), Quaternion.identity);
		Larve.transform.rotation = base.transform.rotation;
		GetComponent<HeroineStats>().GainLustInstant(-40f);
		GetComponent<HeroineStats>().GainOrgInstant(30f);
	}
}
