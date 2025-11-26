using UnityEngine;

public class TentacleThrustEvent : MonoBehaviour
{
	public GameObject tentTrap;

	public float currentCum;

	public float maxCum;

	public float currentStamina;

	public float maxStamina;

	public AudioSource sexSound1;

	public AudioSource sexSound2;

	public AudioSource sexSound3;

	public float orgDmg = 5f;

	public float lstDmg = 1f;

	[SerializeField]
	public bool thrusted;

	[SerializeField]
	public bool max;

	public float vignetteIntensity = 0.2f;

	public float pulsingSpeed = 1.5f;

	private void Start()
	{
		currentCum = 0f;
		maxCum = 100f;
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

	private void ThrustEvent()
	{
		GainCumInstant(1.5f);
		int num = Random.Range(0, 2);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(lstDmg);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(orgDmg);
		if (num == 1)
		{
			sexSound1.Play();
		}
		else
		{
			sexSound2.Play();
		}
	}

	private void ThrustHardEvent()
	{
		if (!tentTrap.GetComponent<TentTrap>().cumming)
		{
			GainCumInstant(1.5f);
		}
		thrusted = true;
		Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		GainCumInstant(1.5f);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(lstDmg);
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainOrgInstant(orgDmg);
		sexSound3.Play();
	}
}
