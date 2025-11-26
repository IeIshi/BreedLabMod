using UnityEngine;

public class BlobWiggle : MonoBehaviour
{
	private Renderer rend;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void Update()
	{
		float value = Mathf.PingPong(Time.time / 8f, 0.08f);
		rend.material.SetFloat("_Parallax", value);
	}
}
