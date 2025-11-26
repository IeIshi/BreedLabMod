using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FutaArea : MonoBehaviour
{
	public GameObject FutaChaserPrefab;

	public GameObject FutaMounterPrefab;

	private GameObject screenEffect;

	public Waypoint spawnpoint;

	public Transform HeroineTarget;

	private GameObject FutaSpawn;

	private Color baseColorValue;

	private Color fadingColorValue;

	private bool chaserSpawned;

	public bool screenHandler;

	private void Start()
	{
		screenEffect = GameObject.Find("ManagerAndUI/UI/Canvas/DecailEffect");
		baseColorValue = screenEffect.GetComponent<Image>().color;
		fadingColorValue = screenEffect.GetComponent<Image>().color;
	}

	private void FixedUpdate()
	{
		if (Safespace.heroineSafe)
		{
			return;
		}
		if (PlayerController.iFalled && !PlayerManager.spawnedFutaMounter && !HeroineStats.birth && !PlayerController.iGetFucked)
		{
			StartCoroutine(SpawnMountFuta());
			PlayerManager.spawnedFutaMounter = true;
		}
		if (screenHandler)
		{
			if (screenEffect.activeSelf)
			{
				fadingColorValue.a -= Time.deltaTime / 2f;
				screenEffect.GetComponent<Image>().color = fadingColorValue;
			}
			else
			{
				screenEffect.GetComponent<Image>().color = baseColorValue;
				fadingColorValue = baseColorValue;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !PlayerManager.spawnedFutaChaser)
		{
			if (!chaserSpawned)
			{
				StartCoroutine(PlayScreenEffect());
				FutaSpawn = Object.Instantiate(FutaChaserPrefab, new Vector3(spawnpoint.transform.position.x, spawnpoint.transform.position.y, spawnpoint.transform.position.z), Quaternion.identity);
				PlayerManager.spawnedFutaChaser = true;
				chaserSpawned = true;
			}
			else if (FutaSpawn == null)
			{
				StartCoroutine(Reactivate());
			}
		}
	}

	private IEnumerator PlayScreenEffect()
	{
		screenEffect.SetActive(value: true);
		_ = screenEffect.GetComponent<Image>().color;
		yield return new WaitForSeconds(3f);
		screenEffect.SetActive(value: false);
	}

	private IEnumerator Reactivate()
	{
		yield return new WaitForSeconds(5f);
		chaserSpawned = false;
	}

	private IEnumerator SpawnMountFuta()
	{
		yield return new WaitForSeconds(1f);
		FutaSpawn = Object.Instantiate(FutaMounterPrefab, new Vector3(HeroineTarget.transform.position.x - 1f, HeroineTarget.transform.position.y, HeroineTarget.transform.position.z), Quaternion.identity);
	}
}
