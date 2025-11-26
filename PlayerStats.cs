using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public static float CurrentStamina;

	public float MaxStamina;

	public float coolDown;

	public bool isDraining;

	public Slider stamBar;

	private void Start()
	{
		MaxStamina = 1000f;
		CurrentStamina = MaxStamina;
		stamBar.value = CalculateStamina();
	}

	private void Update()
	{
		GainStamina(1f);
	}

	private void GainStamina(float gainValue)
	{
		if (CurrentStamina < MaxStamina && !isDraining)
		{
			if (coolDown > 0f)
			{
				coolDown -= Time.deltaTime;
			}
			if (coolDown <= 0f)
			{
				CurrentStamina += gainValue;
				stamBar.value = CalculateStamina();
			}
		}
	}

	private float CalculateStamina()
	{
		return CurrentStamina / MaxStamina;
	}
}
