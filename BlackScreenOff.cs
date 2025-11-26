using UnityEngine;

public class BlackScreenOff : MonoBehaviour
{
	public GameObject bs;

	private void Start()
	{
		bs = GameObject.Find("Canvas/BlackScreen");
		bs.SetActive(value: false);
	}
}
