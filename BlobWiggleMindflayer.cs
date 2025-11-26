using UnityEngine;

public class BlobWiggleMindflayer : MonoBehaviour
{
	private Renderer rend;

	public Material material;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		rend.sharedMaterial = material;
	}

	private void Update()
	{
		float value = Mathf.PingPong(Time.time / 8f, 0.08f);
		material.SetFloat("_Parallax", value);
	}
}
