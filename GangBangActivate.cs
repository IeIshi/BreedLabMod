using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GangBangActivate : MonoBehaviour
{
	public bool firstTrigger;

	public bool secondTrigger;

	private bool triggered;

	public GameObject Soldier1;

	public GameObject Soldier2;

	public GameObject Soldier3;

	public GameObject Soldier4;

	public GameObject Soldier5;

	public GameObject Soldier6;

	public GameObject Soldier7;

	public GameObject MatingP1;

	public GameObject MatingP2;

	public GameObject MatingP3;

	public GameObject MatingP4;

	public GameObject MatingP5;

	public GameObject MatingP6;

	public GameObject MatingP7;

	public GameObject HuggerPref;

	public GameObject HoundPref;

	public GameObject Body1;

	public GameObject Body2;

	public GameObject Body3;

	public GameObject Body4;

	public GameObject Wepon1;

	public GameObject Wepon2;

	public GameObject Wepon3;

	public GameObject Wepon4;

	public GameObject Wepon5;

	public GameObject Wepon6;

	public GameObject Wepon7;

	public GameObject Cloth1;

	public GameObject Cloth2;

	public GameObject Cloth3;

	public GameObject Cloth4;

	public GameObject Cloth5;

	public GameObject Cloth6;

	public GameObject Cloth7;

	private Image imageToFade;

	public float fadeDuration = 5f;

	private GameObject DecailEffect;

	private Color preserveColor;

	public bool goAnschlag;

	public GameObject FirstTrigger;

	private void Start()
	{
		MatingP1.SetActive(value: false);
		MatingP2.SetActive(value: false);
		MatingP3.SetActive(value: false);
		MatingP4.SetActive(value: false);
		MatingP5.SetActive(value: false);
		MatingP6.SetActive(value: false);
		MatingP7.SetActive(value: false);
		DecailEffect = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect");
		imageToFade = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect").GetComponent<Image>();
		preserveColor = imageToFade.color;
	}

	public void FixedUpdate()
	{
		if (goAnschlag)
		{
			FaceTarget(Soldier1);
			FaceTarget(Soldier2);
			FaceTarget(Soldier3);
			FaceTarget(Soldier4);
			FaceTarget(Soldier5);
			FaceTarget(Soldier6);
			FaceTarget(Soldier7);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !triggered)
		{
			if (firstTrigger)
			{
				Soldier1.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier2.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier3.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier4.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier5.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier6.GetComponent<Animator>().SetBool("anschlag", value: true);
				Soldier7.GetComponent<Animator>().SetBool("anschlag", value: true);
				goAnschlag = true;
			}
			if (secondTrigger)
			{
				UndressAndScreenEffect();
				DecailEffect.SetActive(value: true);
				StartCoroutine(FadeOutCoroutine());
				Soldier1.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier2.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier3.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier4.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier5.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier6.GetComponent<Animator>().SetBool("anschlag", value: false);
				Soldier7.GetComponent<Animator>().SetBool("anschlag", value: false);
				SpawnCreatureOnSoldier(HuggerPref, Soldier1, MatingP1, isMale: true);
				SpawnCreatureOnSoldier(HuggerPref, Soldier2, MatingP2, isMale: true);
				SpawnCreatureOnSoldier(HuggerPref, Soldier3, MatingP3, isMale: true);
				SpawnCreatureOnSoldier(HuggerPref, Soldier4, MatingP4, isMale: true);
				SpawnCreatureOnSoldier(HoundPref, Soldier5, MatingP5, isMale: false);
				SpawnCreatureOnSoldier(HoundPref, Soldier6, MatingP6, isMale: false);
				SpawnCreatureOnSoldier(HoundPref, Soldier7, MatingP7, isMale: false);
				FirstTrigger.GetComponent<GangBangActivate>().goAnschlag = false;
			}
			triggered = true;
		}
	}

	private void FaceTarget(GameObject Soldier)
	{
		Vector3 normalized = (PlayerManager.instance.player.transform.position - Soldier.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Soldier.transform.rotation = Quaternion.Slerp(Soldier.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void UndressAndScreenEffect()
	{
		Body1.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
		Body2.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
		Body3.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
		Body4.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
		Wepon1.SetActive(value: false);
		Wepon2.SetActive(value: false);
		Wepon3.SetActive(value: false);
		Wepon4.SetActive(value: false);
		Wepon5.SetActive(value: false);
		Wepon6.SetActive(value: false);
		Wepon7.SetActive(value: false);
		Cloth1.SetActive(value: false);
		Cloth2.SetActive(value: false);
		Cloth3.SetActive(value: false);
		Cloth4.SetActive(value: false);
		Cloth5.SetActive(value: false);
		Cloth6.SetActive(value: false);
		Cloth7.SetActive(value: false);
	}

	private void SpawnCreatureOnSoldier(GameObject CreaturePref, GameObject Soldier, GameObject MatingPartner, bool isMale)
	{
		Vector3 position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
		Quaternion rotation = base.gameObject.transform.rotation;
		GameObject obj = Object.Instantiate(CreaturePref, position, rotation);
		obj.GetComponent<RunToSoldier>().gangBangScenario = true;
		obj.GetComponent<RunToSoldier>().Soldier = Soldier;
		obj.GetComponent<RunToSoldier>().MatingPActivate = MatingPartner;
		obj.GetComponent<RunToSoldier>().isMale = isMale;
		obj.GetComponent<RunToSoldier>().Control = base.gameObject;
	}

	public void StartMating(GameObject Soldier, bool isMale, GameObject MatingPartner)
	{
		ResetTransform(Soldier);
		if (isMale)
		{
			StartCoroutine(HuggerSoldierRape(Soldier, MatingPartner));
			return;
		}
		Soldier.GetComponent<Animator>().Play("rig|FSoldierGettingMated");
		MatingPartner.GetComponent<Animator>().Play("rig|DoggyBackSex2");
	}

	private void ResetTransform(GameObject Soldier)
	{
		Soldier.transform.localPosition = Vector3.zero;
		Soldier.transform.localRotation = Quaternion.identity;
	}

	private IEnumerator HuggerSoldierRape(GameObject Soldier, GameObject MatingPartner)
	{
		Soldier.GetComponent<Animator>().Play("rig|SoldierHuggerRaped");
		MatingPartner.GetComponent<Animator>().Play("rig|SoldierPumping");
		yield return new WaitForSeconds(10f);
		Soldier.GetComponent<Animator>().Play("rig|SoldierMilked");
		MatingPartner.GetComponent<Animator>().Play("rig|SoldierMilking");
		yield return new WaitForSeconds(5f);
		Soldier.GetComponent<Animator>().Play("rig|SoldierHuggerRaped");
		MatingPartner.GetComponent<Animator>().Play("rig|SoldierPumping");
		yield return new WaitForSeconds(10f);
		Soldier.GetComponent<Animator>().Play("rig|SoldierMilked");
		MatingPartner.GetComponent<Animator>().Play("rig|SoldierMilking");
		yield return new WaitForSeconds(5f);
		Soldier.GetComponent<Animator>().Play("rig|DefeatedMilked");
		MatingPartner.GetComponent<Animator>().Play("rig|DefeatedSoldierMilking");
	}

	private IEnumerator FadeOutCoroutine()
	{
		Color originalColor = imageToFade.color;
		float elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			float a = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
			imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);
			yield return null;
		}
		imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
		imageToFade.color = preserveColor;
		imageToFade.gameObject.SetActive(value: false);
		DecailEffect.SetActive(value: false);
	}
}
