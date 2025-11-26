using UnityEngine;

public class TentTop : MonoBehaviour
{
	public bool grabbed;

	public void GrabbedEvent()
	{
		Debug.Log("GRABBED");
		grabbed = true;
		PlayerManager.instance.player.GetComponent<Animator>().SetBool("tWallGrabbed", value: true);
	}
}
