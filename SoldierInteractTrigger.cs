using System.Collections;
using UnityEngine;

public class SoldierInteractTrigger : MonoBehaviour
{
	public bool hasPenis;

	public GameObject Soldier;

	public GameObject MatingPartner;

	private Animator soldierAnimator;

	private Animator matingPartnerAnimator;

	public GameObject SoldierCloth;

	public GameObject SoldierWeapon;

	public GameObject SoldierBody;

	public GameObject Lights;

	public Dialogue dialogue;

	public Transform creatureSpawnPlace;

	private Vector3 spawnPoint;

	private Quaternion spawnPointRotation;

	public GameObject femHugPref;

	private bool entered;

	private bool triggered;

	private bool routineStarted;

	public GameObject DemonHeroine;

	private Animator demonAnimator;

	private GameObject DecailEffect;

	private Color baseColorValue;

	private Color fadingColorValue;

	private void Start()
	{
		soldierAnimator = Soldier.GetComponent<Animator>();
		matingPartnerAnimator = MatingPartner.GetComponent<Animator>();
		demonAnimator = DemonHeroine.GetComponent<Animator>();
		Lights.SetActive(value: false);
		MatingPartner.SetActive(value: false);
	}

	private void FixedUpdate()
	{
		if (triggered && !entered)
		{
			FaceTarget(PlayerManager.instance.player.transform);
			DemonFaceTarget(Soldier.transform);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (!entered && other.tag == "Player")
		{
			soldierAnimator.SetBool("anschlag", value: true);
			if (!triggered)
			{
				TriggerDialoge();
				triggered = true;
			}
			else if (!DialogManager.inDialogue && !routineStarted)
			{
				StartCoroutine(PrepareToMate());
				routineStarted = true;
			}
		}
	}

	private void SpawnCreature()
	{
		spawnPoint = new Vector3(creatureSpawnPlace.position.x, creatureSpawnPlace.position.y, creatureSpawnPlace.position.z);
		spawnPointRotation = creatureSpawnPlace.rotation;
		GameObject obj = Object.Instantiate(femHugPref, spawnPoint, spawnPointRotation);
		obj.GetComponent<RunToSoldier>().Soldier = Soldier;
		obj.GetComponent<RunToSoldier>().Control = base.gameObject;
	}

	private void ResetTransform()
	{
		Soldier.transform.localPosition = Vector3.zero;
		Soldier.transform.localRotation = Quaternion.identity;
	}

	private IEnumerator PrepareToMate()
	{
		DemonHeroine.GetComponent<PlayerController>().enabled = false;
		demonAnimator.SetBool("cast", value: true);
		yield return new WaitForSeconds(3f);
		SoldierCloth.SetActive(value: false);
		SoldierWeapon.SetActive(value: false);
		SoldierBody.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
		soldierAnimator.SetBool("anschlag", value: false);
		yield return new WaitForSeconds(2f);
		demonAnimator.SetBool("cast", value: false);
		SpawnCreature();
		DemonHeroine.GetComponent<PlayerController>().enabled = true;
	}

	public void StartMating()
	{
		ResetTransform();
		MatingPartner.SetActive(value: true);
		Lights.SetActive(value: true);
		if (hasPenis)
		{
			StartCoroutine(HuggerSoldierRape());
		}
		else
		{
			soldierAnimator.Play("rig|FSoldierGettingMated");
			matingPartnerAnimator.Play("rig|DoggyBackSex2");
		}
		entered = true;
	}

	private IEnumerator HuggerSoldierRape()
	{
		soldierAnimator.Play("rig|SoldierHuggerRaped");
		matingPartnerAnimator.Play("rig|SoldierPumping");
		yield return new WaitForSeconds(10f);
		soldierAnimator.Play("rig|SoldierMilked");
		matingPartnerAnimator.Play("rig|SoldierMilking");
		yield return new WaitForSeconds(5f);
		soldierAnimator.Play("rig|SoldierHuggerRaped");
		matingPartnerAnimator.Play("rig|SoldierPumping");
		yield return new WaitForSeconds(10f);
		soldierAnimator.Play("rig|SoldierMilked");
		matingPartnerAnimator.Play("rig|SoldierMilking");
		yield return new WaitForSeconds(5f);
		soldierAnimator.Play("rig|DefeatedMilked");
		matingPartnerAnimator.Play("rig|DefeatedSoldierMilking");
	}

	private void FaceTarget(Transform target)
	{
		Vector3 normalized = (target.transform.position - Soldier.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		Soldier.transform.rotation = Quaternion.Slerp(Soldier.transform.rotation, b, Time.deltaTime * 10f);
	}

	private void DemonFaceTarget(Transform target)
	{
		Vector3 normalized = (target.transform.position - DemonHeroine.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		DemonHeroine.transform.rotation = Quaternion.Slerp(DemonHeroine.transform.rotation, b, Time.deltaTime * 10f);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
