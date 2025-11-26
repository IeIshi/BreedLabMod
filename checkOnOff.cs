using UnityEngine;

public class checkOnOff : MonoBehaviour
{
	public Material[] material;

	public Renderer rend;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		rend.sharedMaterial = material[0];
	}

	private void FixedUpdate()
	{
		if (Flashlight.FlashOn)
		{
			rend.sharedMaterial = material[1];
		}
		else
		{
			rend.sharedMaterial = material[0];
		}
	}
}
