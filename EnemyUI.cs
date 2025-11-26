using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
	private float lerpTimer;

	private float lerpTimerCum;

	public float chipSpeed = 2f;

	public float health;

	public float maxHealth;

	public float cum;

	public float maxCum;

	public Image frontHealthBar;

	public Image backHealthBar;

	public Image frontCumBar;

	public Image backCumBar;

	public Image portraitHugger;

	public Image portraitWolf;

	public static EnemyUI instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of Inventory found!");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		health = maxHealth;
		cum = 0f;
		portraitHugger.enabled = false;
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		health = Mathf.Clamp(health, 0f, maxHealth);
		cum = Mathf.Clamp(cum, 0f, maxCum);
		UpdateHealthUI();
		UpdateCumUI();
	}

	public void UpdateHealthUI()
	{
		float fillAmount = frontHealthBar.fillAmount;
		float fillAmount2 = backHealthBar.fillAmount;
		float num = health / maxHealth;
		if (fillAmount2 > num)
		{
			frontHealthBar.fillAmount = num;
			backHealthBar.color = Color.red;
			lerpTimer += Time.deltaTime;
			float num2 = lerpTimer / chipSpeed;
			num2 *= num2;
			backHealthBar.fillAmount = Mathf.Lerp(fillAmount2, num, num2);
		}
		if (fillAmount < num)
		{
			backHealthBar.color = Color.green;
			backHealthBar.fillAmount = num;
			lerpTimer += Time.deltaTime;
			float num3 = lerpTimer / chipSpeed;
			num3 *= num3;
			frontHealthBar.fillAmount = Mathf.Lerp(fillAmount, backHealthBar.fillAmount, num3);
		}
	}

	public void UpdateCumUI()
	{
		float fillAmount = frontCumBar.fillAmount;
		float fillAmount2 = backCumBar.fillAmount;
		float num = cum / maxCum;
		if (fillAmount2 > num)
		{
			frontCumBar.fillAmount = num;
			backCumBar.color = Color.red;
			lerpTimerCum += Time.deltaTime;
			float num2 = lerpTimerCum / chipSpeed;
			num2 *= num2;
			backCumBar.fillAmount = Mathf.Lerp(fillAmount2, num, num2);
		}
		if (fillAmount < num)
		{
			backCumBar.color = Color.magenta;
			backCumBar.fillAmount = num;
			lerpTimerCum += Time.deltaTime;
			float num3 = lerpTimerCum / chipSpeed;
			num3 *= num3;
			frontCumBar.fillAmount = Mathf.Lerp(fillAmount, backCumBar.fillAmount, num3);
		}
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		lerpTimer = 0f;
	}

	public void RestoreHealth(float healAmount)
	{
		health += healAmount;
		lerpTimer = 0f;
	}

	public void GainCum(float gain)
	{
		cum += gain;
		lerpTimerCum = 0f;
	}

	public void LoseCum(float lose)
	{
		cum -= lose;
		lerpTimerCum = 0f;
	}
}
