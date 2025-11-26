using UnityEngine.SceneManagement;

public class LoadThanks : Interactable
{
	private Inventory inventory;

	private void Start()
	{
		inventory = Inventory.instance;
	}

	public override void Interact()
	{
		base.Interact();
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "IDCard")
			{
				inventory.items[i].RemoveFromInventory();
			}
		}
		PlayerManager.loadedrly = false;
		InventoryUI.heroineIsChased = false;
		SceneManager.LoadScene("Level_3");
	}
}
