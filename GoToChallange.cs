using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToChallange : MonoBehaviour
{
	public GameObject loadingScreen;

	public Slider loadingSlider;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerManager.loadedrly = false;
			InventoryUI.heroineIsChased = false;
			PlayerManager.enteredChallangeRoom = true;
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 2);
		}
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	private IEnumerator LoadAsynch(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingScreen.SetActive(value: true);
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			loadingSlider.value = value;
			yield return null;
		}
	}
}
