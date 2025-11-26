using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
	public float attackSpeed = 1f;

	private float attackCooldown;

	public float attackDelay = 0.6f;

	private CharacterStats myStats;

	public event Action OnAttack;

	private void Start()
	{
		myStats = GetComponent<CharacterStats>();
	}

	private void Update()
	{
		attackCooldown -= Time.deltaTime;
	}

	public void Attack(CharacterStats targetStats)
	{
		if (attackCooldown <= 0f)
		{
			StartCoroutine(DoDamage(targetStats, attackDelay));
			if (this.OnAttack != null)
			{
				this.OnAttack();
			}
			attackCooldown = 1f / attackSpeed;
		}
	}

	private IEnumerator DoDamage(CharacterStats stats, float delay)
	{
		yield return new WaitForSeconds(delay);
	}
}
