using UnityEngine;

public class TrickLevelActivate : MonoBehaviour
{
	private void Start()
	{
		Pause.instance.BackToStartScreen();
	}
}
