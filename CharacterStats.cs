using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
	public int maxHealth = 100;

	public Stat damage;

	public Stat armor;

	public int currentHealth { get; private set; }

	public event Action<int, int> OnHealthChanged;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			TakeDamage(10);
		}
	}

	public void TakeDamage(int damage)
	{
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		currentHealth -= damage;
		Debug.Log(base.transform.name + " takes " + damage + " damage.");
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(maxHealth, currentHealth);
		}
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public virtual void Die()
	{
		Debug.Log(base.transform.name + " died.");
	}
}
