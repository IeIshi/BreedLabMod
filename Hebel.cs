using System;
using System.Collections;
using UnityEngine;

public class Hebel : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool mantisIsDead;
	}

	public GameObject Gate;

	public GameObject TriggerHebel;

	public GameObject Mantis;

	public AudioSource openHebelSound;

	public AudioSource openGateSound;

	private Animator hebelAnim;

	private Animator gateAnim;

	public GameObject SpawnAmmo;

	public GameObject Futa;

	public GameObject GoodJob;

	public bool mantisIsDead;

	private bool opened;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.mantisIsDead = mantisIsDead;
		return saveData;
	}

	public void RestoreState(object state)
	{
		mantisIsDead = ((SaveData)state).mantisIsDead;
	}

	private void Awake()
	{
		Mantis.SetActive(value: false);
	}

	private void Start()
	{
		hebelAnim = TriggerHebel.GetComponent<Animator>();
		if (Gate != null)
		{
			gateAnim = Gate.GetComponent<Animator>();
		}
		SpawnAmmo.SetActive(value: false);
		if (mantisIsDead)
		{
			hebelAnim.SetBool("isOpen", value: true);
			gateAnim.SetBool("isOpen", value: true);
			PlayerManager.MantisDown = true;
			MantisAi.isDed = true;
			GoodJob.SetActive(value: false);
			Futa.SetActive(value: false);
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		if (!opened)
		{
			base.Interact();
			hebelAnim.SetBool("isOpen", value: true);
			openHebelSound.Play();
			StartCoroutine(activateGate());
			Mantis.SetActive(value: true);
			opened = true;
			base.gameObject.layer = 0;
			Futa.gameObject.layer = 0;
		}
	}

	private IEnumerator activateGate()
	{
		yield return new WaitForSeconds(1f);
		gateAnim.SetBool("isOpen", value: true);
		openGateSound.Play();
		yield return new WaitForSeconds(3.5f);
		openGateSound.Stop();
		openHebelSound.Stop();
	}
}
