using UnityEngine;

public class TentacleThrust : MonoBehaviour
{
	public AudioSource thrust;

	public GameObject tent;

	private SkinnedMeshRenderer tentMesh;

	public float pulsingSpeed = 0.1f;

	private float weigt;

	private void Start()
	{
		tentMesh = tent.GetComponent<SkinnedMeshRenderer>();
	}

	private void Update()
	{
		tentMesh.SetBlendShapeWeight(0, weigt += pulsingSpeed);
	}

	private void ThrustEvent()
	{
		thrust.Play();
	}
}
