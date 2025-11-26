using UnityEngine;

public class PhanOpenDoor : MonoBehaviour
{
	public Transform target;

	public GameObject door;

	public GameObject theLickers;

	public GameObject nextPhantom;

	public GameObject progControl;

	private float distance;

	private void Update()
	{
		distance = Vector3.Distance(target.position, base.transform.position);
		if (distance < 5f)
		{
			target.GetComponent<HeroineStats>().GainLust(3f);
		}
		if (GetComponent<PhantomControl>().interacted)
		{
			if (!DialogManager.inDialogue)
			{
				Object.Destroy(base.gameObject);
				Object.Destroy(theLickers);
				InventoryUI.heroineIsChased = false;
				nextPhantom.SetActive(value: true);
				door.GetComponent<Animator>().SetBool("isOpen", value: true);
				progControl.GetComponent<ChallageRoomProgControl>().doubleDoorOpened = true;
			}
			else
			{
				FaceTarget();
			}
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (target.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}
}
