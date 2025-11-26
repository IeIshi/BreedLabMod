using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingLoading : MonoBehaviour
{
	private string SavePath => Application.persistentDataPath + "/save.txt";

	[ContextMenu("Save")]
	private void Save()
	{
		Dictionary<string, object> state = LoadFile();
		CaptureState(state);
		SaveFile(state);
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
}
