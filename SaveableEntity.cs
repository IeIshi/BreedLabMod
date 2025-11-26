using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
	[SerializeField]
	private string id = string.Empty;

	public string Id => id;

	[ContextMenu("Generate Id")]
	private void GenerateId()
	{
		id = Guid.NewGuid().ToString();
	}

	public object CaptureState()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		ISaveable[] components = GetComponents<ISaveable>();
		foreach (ISaveable saveable in components)
		{
			dictionary[saveable.GetType().ToString()] = saveable.CaptureState();
		}
		return dictionary;
	}

	public void RestoreState(object state)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)state;
		ISaveable[] components = GetComponents<ISaveable>();
		foreach (ISaveable saveable in components)
		{
			string key = saveable.GetType().ToString();
			if (dictionary.TryGetValue(key, out var value))
			{
				saveable.RestoreState(value);
			}
		}
	}
}
