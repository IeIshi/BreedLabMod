using UnityEngine;

public class TentButtom : MonoBehaviour
{
	public AudioSource sexSound1;

	public float maxCum;

	public float maxStamina;

	public float currentCum;

	public float currentStamina;

	public bool thrusted;

	public bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	public float lstDmg;

	public float orgDmg;

	private void Start()
	{
		currentCum = 0f;
		currentStamina = maxStamina;
	}

	public void DrainStaminaInstant(float drainValue)
	{
		currentStamina -= drainValue;
		EnemyUI.instance.TakeDamage(drainValue);
	}

	public void DrainStamina(float drainValue)
	{
		currentStamina -= drainValue * Time.deltaTime;
		EnemyUI.instance.TakeDamage(drainValue * Time.deltaTime);
	}

	public void GainStamina(float gainValue)
	{
		currentStamina += gainValue * Time.deltaTime;
	}

	private void GainCumInstant(float gainValue)
	{
		currentCum += gainValue;
		EnemyUI.instance.GainCum(gainValue);
	}

	public void DrainCum(float drainValue)
	{
		currentCum -= drainValue * Time.deltaTime;
		EnemyUI.instance.LoseCum(drainValue * Time.deltaTime);
	}

	public void DrainCumInstant(float drainValue)
	{
		currentCum -= drainValue;
		EnemyUI.instance.LoseCum(drainValue);
	}

	public void VignetteEffect()
	{
		if (!thrusted)
		{
			return;
		}
		if (max)
		{
			PostProcessingManager.vignette.intensity.value -= Time.deltaTime * pulsingSpeed;
			if (PostProcessingManager.vignette.intensity.value <= 0f)
			{
				thrusted = false;
				max = false;
			}
		}
		else if (PostProcessingManager.vignette.intensity.value <= vignetteIntensity)
		{
			PostProcessingManager.vignette.intensity.value += Time.deltaTime * pulsingSpeed;
		}
		else
		{
			max = true;
		}
	}

	public void ThrustEvent()
	{
		GainCumInstant(1.5f);
		sexSound1.Play();
		thrusted = true;
		if (currentCum > 25f)
		{
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(lstDmg);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(orgDmg);
	}

	public void CumEvent()
	{
		DrainCumInstant(2f);
		thrusted = true;
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(lstDmg * 2f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(orgDmg / 2f);
	}
}
