using UnityEngine;

public class WaspPregEffect : MonoBehaviour
{
	private void FixedUpdate()
	{
		PostProcessingManager.instance.bloom.dirtIntensity.value = Mathf.PingPong(Time.time * 100f, 200f);
	}
}
