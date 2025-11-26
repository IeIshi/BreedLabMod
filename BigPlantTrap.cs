using System.Collections;
using UnityEngine;

public class BigPlantTrap : MonoBehaviour
{
	public GameObject Smoke;

	public GameObject TheSwarm;

	public GameObject myEgg1;

	public GameObject myEgg2;

	public GameObject myEgg3;

	public GameObject myEgg4;

	public GameObject myEgg5;

	public GameObject PanelRiddle;

	private bool entered;

	public AudioSource smokeSound;

	private void Start()
	{
		entered = false;
		Smoke.SetActive(value: false);
		TheSwarm.SetActive(value: false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !PanelRiddle.GetComponent<PlantControlPanel>().bag && !entered)
		{
			Smoke.SetActive(value: true);
			if (!smokeSound.isPlaying)
			{
				smokeSound.Play();
			}
			StartCoroutine(ActivateTheSwarm());
			StartCoroutine(DeactivateTheSmoke());
			entered = true;
		}
	}

	private IEnumerator ActivateTheSwarm()
	{
		yield return new WaitForSeconds(3f);
		OpenEggs();
	}

	private IEnumerator DeactivateTheSmoke()
	{
		yield return new WaitForSeconds(10f);
		Smoke.GetComponent<ParticleSystem>().Stop();
		smokeSound.Stop();
	}

	private void OpenEggs()
	{
		myEgg1.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		myEgg2.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		myEgg3.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		myEgg4.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		myEgg5.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		TheSwarm.SetActive(value: true);
	}
}
