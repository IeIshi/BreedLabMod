using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Dialogue
{
	public string name;

	public Image image;

	[TextArea(3, 10)]
	public string[] sentences;
}
