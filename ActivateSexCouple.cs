using UnityEngine;

public class ActivateSexCouple : MonoBehaviour
{
	public GameObject SexCouple;

	private Inventory inventory;

	private bool gotTheCard;

	public bool coupleActivated;

	private void Start()
	{
		inventory = Inventory.instance;
		SexCouple.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !gotTheCard && PlayerManager.id1)
		{
			if (!coupleActivated)
			{
				SexCouple.SetActive(value: true);
				coupleActivated = true;
			}
			gotTheCard = true;
		}
	}
}
