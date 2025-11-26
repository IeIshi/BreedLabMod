using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStation : Interactable
{
	public AudioSource activateSound;

	private GameObject saved;

	private string SavePath => Application.persistentDataPath + "/save.txt";

	[ContextMenu("Save")]
	private void Save()
	{
		Dictionary<string, object> state = LoadFile();
		CaptureState(state);
		if (!File.Exists(SavePath))
		{
			SaveFile(state);
			StartCoroutine(showSavedText());
		}
		PlayerManager.loadedrly = false;
		Debug.Log("Saved");
	}

	[ContextMenu("Load")]
	private void Load()
	{
		Dictionary<string, object> state = LoadFile();
		RestoreState(state);
	}

	private Dictionary<string, object> LoadFile()
	{
		if (!File.Exists(SavePath))
		{
			return new Dictionary<string, object>();
		}
		using FileStream serializationStream = File.Open(SavePath, FileMode.Open);
		return (Dictionary<string, object>)new BinaryFormatter().Deserialize(serializationStream);
	}

	private void SaveFile(object state)
	{
		using FileStream serializationStream = File.Open(SavePath, FileMode.Create);
		new BinaryFormatter().Serialize(serializationStream, state);
	}

	private void CaptureState(Dictionary<string, object> state)
	{
		SaveableEntity[] array = Object.FindObjectsOfType<SaveableEntity>();
		foreach (SaveableEntity saveableEntity in array)
		{
			state[saveableEntity.Id] = saveableEntity.CaptureState();
		}
	}

	private void RestoreState(Dictionary<string, object> state)
	{
		SaveableEntity[] array = Object.FindObjectsOfType<SaveableEntity>();
		foreach (SaveableEntity saveableEntity in array)
		{
			if (state.TryGetValue(saveableEntity.Id, out var value))
			{
				saveableEntity.RestoreState(value);
			}
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (!InventoryUI.heroineIsChased)
		{
			File.Delete(SavePath);
			activateSound.Play();
			Inventory.instance.heroineLocation[0] = PlayerManager.instance.player.transform.position.x;
			Inventory.instance.heroineLocation[1] = PlayerManager.instance.player.transform.position.y;
			Inventory.instance.heroineLocation[2] = PlayerManager.instance.player.transform.position.z;
			Inventory.instance.heroineLocation[3] = PlayerManager.instance.player.transform.eulerAngles.y;
			PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
			Save();
		}
		Debug.Log(Inventory.instance.heroineLocation[0]);
		Debug.Log(Inventory.instance.heroineLocation[1]);
		Debug.Log(Inventory.instance.heroineLocation[2]);
	}

	private IEnumerator showSavedText()
	{
		saved = GameObject.Find("ManagerAndUI/UI/Canvas/Saved");
		saved.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		saved.SetActive(value: false);
	}
}
