using UnityEngine;

public class LampChanger : MonoBehaviour
{
	public Transform schalter;

	public Material[] material;

	public Renderer rend;

	public bool firstDoor;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		if (firstDoor)
		{
			if (schalter.GetComponent<PasswordKeyGet>().opened)
			{
				rend.sharedMaterial = material[1];
			}
			else
			{
				rend.sharedMaterial = material[0];
			}
		}
	}
}
