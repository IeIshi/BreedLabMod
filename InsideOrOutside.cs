using UnityEngine;

public class InsideOrOutside : MonoBehaviour
{
	public static bool inside;

	public bool insideChecker;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (insideChecker)
			{
				inside = true;
			}
			else
			{
				inside = false;
			}
		}
	}
}
