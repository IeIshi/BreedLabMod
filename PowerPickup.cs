using System;
using UnityEngine;

public class PowerPickup : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool pickedUp;
	}

	public int gainAmount;

	public bool willPw;

	public bool speedPw;

	public bool expPw;

	public GameObject heroine;

	public GameObject container;

	public AudioSource pickupSound;

	[SerializeField]
	private bool pickedUp;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.pickedUp = pickedUp;
		return saveData;
	}

	public void RestoreState(object state)
	{
		pickedUp = ((SaveData)state).pickedUp;
	}

	private void Start()
	{
		if (pickedUp)
		{
			UnityEngine.Object.Destroy(container);
			GetComponent<BoxCollider>().enabled = false;
			base.gameObject.layer = 0;
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!pickedUp)
		{
			pickupSound.Play();
			UnityEngine.Object.Destroy(container);
			base.gameObject.layer = 0;
			GetComponent<BoxCollider>().enabled = false;
			giveDaPowa();
		}
	}

	private void giveDaPowa()
	{
		if (willPw)
		{
			HeroineStats.maxStamina += 10f;
			if (HeroineStats.debuffedStam > 10f)
			{
				HeroineStats.debuffedStam -= 10f;
			}
		}
		pickedUp = true;
	}
}
