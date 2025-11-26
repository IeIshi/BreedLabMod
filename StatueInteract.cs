using System;
using UnityEngine;

public class StatueInteract : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool interacted;
	}

	public GameObject fence;

	public AudioSource statueClick;

	public GameObject scientist;

	public GameObject pumpingTentacle;

	public bool interacted;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.interacted = interacted;
		return saveData;
	}

	public void RestoreState(object state)
	{
		interacted = ((SaveData)state).interacted;
	}

	private void Start()
	{
		if (interacted)
		{
			UnityEngine.Object.Destroy(pumpingTentacle);
			fence.GetComponent<Animator>().SetBool("isOpen", value: true);
			GetComponent<StatueInteract>().enabled = false;
			base.gameObject.layer = 0;
		}
		else if (scientist != null)
		{
			scientist.GetComponent<ScientistNewControl>().enabled = false;
			scientist.GetComponent<SpecialScientist>().enabled = false;
		}
	}

	public override void Interact()
	{
		if (!interacted)
		{
			base.Interact();
			statueClick.Play();
			interacted = true;
			if (pumpingTentacle != null)
			{
				UnityEngine.Object.Destroy(pumpingTentacle);
			}
			fence.GetComponent<Animator>().SetBool("isOpen", value: true);
			GetComponent<StatueInteract>().enabled = false;
			base.gameObject.layer = 0;
			if (fence.name == "Tor3")
			{
				scientist.GetComponent<ScientistNewControl>().enabled = true;
				scientist.GetComponent<SpecialScientist>().enabled = true;
			}
		}
	}
}
