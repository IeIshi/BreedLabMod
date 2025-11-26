using System.Collections;
using UnityEngine;

public class LustGaserScript : MonoBehaviour
{
	public ParticleSystem gas;

	public AudioSource gasSound;

	private bool gasPlaying;

	public PostProcessingManager postProcess;

	public GameObject hitDetector;

	private void Start()
	{
		gas.Stop();
		postProcess = GameObject.Find("ManagerAndUI/Global Volume").GetComponent<PostProcessingManager>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !hitDetector.GetComponent<ListGaserHitDet>().dead && !gasPlaying)
		{
			StartCoroutine(ReleaseGas());
		}
	}

	public void FixedUpdate()
	{
		if (postProcess.ps.activeSelf)
		{
			postProcess.OrgasmEffect();
		}
	}

	private IEnumerator ReleaseGas()
	{
		gas.Play();
		gasPlaying = true;
		postProcess.ps.SetActive(value: true);
		if (!gasSound.isPlaying)
		{
			gasSound.Play();
		}
		PlayerManager.instance.player.GetComponent<HeroineStats>().GainLustInstant(40f);
		yield return new WaitForSeconds(2f);
		gas.Stop();
		gasSound.Stop();
		gasPlaying = false;
		yield return new WaitForSeconds(5f);
		postProcess.ps.SetActive(value: false);
	}
}
