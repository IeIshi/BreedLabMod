using UnityEngine;

public class ListGaserHitDet : MonoBehaviour
{
	public ParticleSystem explosionParticle;

	public GameObject triggerPoint;

	public bool dead;

	private void Start()
	{
		explosionParticle.Stop();
	}
}
