using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	[SerializeField]
	public float baseValue;

	private List<float> modifiers = new List<float>();

	public float GetValue()
	{
		float finalValue = baseValue;
		modifiers.ForEach(delegate(float x)
		{
			finalValue += x;
		});
		return finalValue;
	}

	public void AddModifier(float modifier)
	{
		if (modifier != 0f)
		{
			modifiers.Add(modifier);
		}
	}

	public void RemoveModifier(float modifier)
	{
		if (modifier != 0f)
		{
			modifiers.Remove(modifier);
		}
	}
}
