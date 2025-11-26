using System;
using UnityEngine;

public class OpenBlubDoors : Interactable, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool interacted;
	}

	public GameObject blobDoor;

	public AudioSource slimeSound;

	public GameObject crystal;

	public Material[] material;

	private Renderer rend;

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
		rend = crystal.GetComponent<Renderer>();
		rend.enabled = true;
		if (!interacted)
		{
			rend.sharedMaterial = material[0];
			return;
		}
		UnityEngine.Object.Destroy(blobDoor);
		rend.sharedMaterial = material[1];
		base.gameObject.layer = 0;
	}

	public override void Interact()
	{
		base.Interact();
		interacted = true;
		slimeSound.Play();
		if (blobDoor != null)
		{
			UnityEngine.Object.Destroy(blobDoor);
		}
		rend.sharedMaterial = material[1];
		base.gameObject.layer = 0;
	}
}
