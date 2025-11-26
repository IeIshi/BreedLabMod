using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{
	private PlayerManager playerManager;

	private CharacterStats myStats;

	private void Start()
	{
		playerManager = PlayerManager.instance;
		myStats = GetComponent<CharacterStats>();
	}

	public override void Interact()
	{
		base.Interact();
		CharacterCombat component = playerManager.player.GetComponent<CharacterCombat>();
		if (component != null)
		{
			component.Attack(myStats);
		}
	}
}
