using System;
using UnityEngine;

public class SpecialScientist : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool fucked;
	}

	private Animator anim;

	private ScientistNewControl control;

	public bool fucked;

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.fucked = fucked;
		return saveData;
	}

	public void RestoreState(object state)
	{
		fucked = ((SaveData)state).fucked;
	}

	private void Awake()
	{
		anim = GetComponent<Animator>();
		if (fucked)
		{
			anim.SetBool("isMasturbating", value: true);
		}
	}

	private void Start()
	{
		control = GetComponent<ScientistNewControl>();
	}

	private void FixedUpdate()
	{
		if (control.currentStamina == 0f)
		{
			fucked = true;
		}
		if (control.getCowGirlAction)
		{
			control.staminaRegRate = 5f;
		}
		else
		{
			control.staminaRegRate = 0f;
		}
	}
}
