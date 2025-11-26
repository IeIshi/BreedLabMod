using UnityEngine;

public class BotanicMainAreaControl : MonoBehaviour
{
	public GameObject LeftTunnelStuff;

	private void Start()
	{
		if (PlayerManager.litaGaveKey)
		{
			LeftTunnelStuff.SetActive(value: false);
		}
	}
}
