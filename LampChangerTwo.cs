using UnityEngine;

public class LampChangerTwo : MonoBehaviour
{
	public Transform schalter;

	public Material[] material;

	public Renderer rend;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		rend.enabled = true;
	}

	private void Update()
	{
	}
}
