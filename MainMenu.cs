using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject mainMenu;

	public GameObject optionsMenu;

	public GameObject creditScreen;

	public GameObject loadingScreen;

	public GameObject settingOptions;

	public GameObject resMenu;

	public GameObject backButton;

	public GameObject continueButton;

	public GameObject difficulty;

	public GameObject enterGallery;

	public GameObject ActSkips;

	public GameObject SubMode;

	private Slider mouseSilder;

	private Slider gammaSlider;

	public Slider loadingSlider;

	public TextMeshProUGUI mouseValueText;

	public TextMeshProUGUI gammaValueText;

	public Toggle[] resolutionToggles;

	public int[] screenWidths;

	public Toggle ambientOc;

	public Toggle bloom;

	public Toggle cheatMode;

	public Toggle infiniteCloth;

	private int activeScreenResIndex;

	public bool LoadLevel_1;

	public bool LoadLevel_2;

	public static bool levelSkipped;

	public static bool skippedToAct4;

	public static bool skippedToAct4Two;

	private string savePath;

	public GameObject cheatOptions;

	public static bool NewGamePlus;

	private bool clicked;

	private void Start()
	{
		cheatOptions.SetActive(value: true);
		Cursor.visible = true;
		savePath = Application.persistentDataPath + "/save.txt";
		if (File.Exists(savePath))
		{
			continueButton.SetActive(value: true);
		}
		else
		{
			continueButton.SetActive(value: false);
		}
		difficulty.SetActive(value: false);
		activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
		PlayerPrefs.GetInt("fullscreen");
		_ = 1;
		bool flag = PlayerPrefs.GetInt("AmbientOcclusion") == 1;
		bool flag2 = PlayerPrefs.GetInt("Bloom") == 1;
		bool flag3 = PlayerPrefs.GetInt("CheatMode") == 1;
		bool flag4 = PlayerPrefs.GetInt("InfiniteCloth") == 1;
		if (PlayerPrefs.GetInt("Finished") == 1)
		{
			ActSkips.SetActive(value: true);
			SubMode.SetActive(value: true);
		}
		else
		{
			ActSkips.SetActive(value: false);
			SubMode.SetActive(value: false);
		}
		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].isOn = i == activeScreenResIndex;
		}
		if (flag)
		{
			ambientOc.isOn = true;
		}
		else
		{
			ambientOc.isOn = false;
		}
		if (flag2)
		{
			bloom.isOn = true;
		}
		else
		{
			bloom.isOn = false;
		}
		if (flag3)
		{
			cheatMode.isOn = true;
		}
		else
		{
			cheatMode.isOn = false;
		}
		if (flag4)
		{
			infiniteCloth.isOn = true;
		}
		else
		{
			infiniteCloth.isOn = false;
		}
		mouseSilder = GameObject.Find("MouseSlider").GetComponent<Slider>();
		gammaSlider = GameObject.Find("GammaSlider").GetComponent<Slider>();
		float num = Mathf.Round(gammaSlider.value * 10f) * 0.1f;
		mouseValueText.text = mouseSilder.value.ToString();
		gammaValueText.text = num.ToString();
		settingOptions.SetActive(value: false);
		creditScreen.SetActive(value: false);
		Mods.CreateInstance();
	}

	public void AdjustMouseSens()
	{
		mouseValueText.text = mouseSilder.value.ToString();
		PlayerPrefs.SetInt("MouseSens", (int)mouseSilder.value);
		Debug.Log(mouseSilder.value);
	}

	public void AdjustGamma()
	{
		float num = Mathf.Round(gammaSlider.value * 10f) * 0.1f;
		gammaValueText.text = num.ToString();
		PlayerPrefs.SetFloat("Gamma", gammaSlider.value);
	}

	public void PlayGame()
	{
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		enterGallery.SetActive(value: false);
		if (LoadLevel_1)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		}
		if (LoadLevel_2)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	public void OpenDifficulty()
	{
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: true);
		backButton.SetActive(value: true);
	}

	public void PlayGameHard()
	{
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		enterGallery.SetActive(value: false);
		if (LoadLevel_1)
		{
			PlayerManager.SAB = true;
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		}
		if (LoadLevel_2)
		{
			LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	public void EnterGallery()
	{
		SceneManager.LoadScene("Gallery");
	}

	public void GoToNextLevel()
	{
		levelSkipped = true;
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 3);
		PlayerManager.IsVirgin = true;
	}

	public void GoToAfterScientist()
	{
		levelSkipped = true;
		PlayerManager.ScSexAfterMath = true;
		BackDtToInBetween.backFromDt = true;
		PlayerManager.IsVirgin = true;
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 3);
	}

	public void GoToAct4()
	{
		skippedToAct4 = true;
		PlayerManager.ScSexAfterMath = true;
		BackDtToInBetween.backFromDt = true;
		PlayerManager.MantisDown = true;
		PlayerManager.BroodmotherMet = true;
		PlayerManager.MasturbationFinished = true;
		PlayerManager.IsVirgin = true;
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 12);
	}

	public void GoToAct4Two()
	{
		skippedToAct4Two = true;
		PlayerManager.ScSexAfterMath = true;
		BackDtToInBetween.backFromDt = true;
		PlayerManager.MantisDown = true;
		PlayerManager.BroodmotherMet = true;
		PlayerManager.MasturbationFinished = true;
		PlayerManager.IsVirgin = true;
		mainMenu.SetActive(value: false);
		difficulty.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		backButton.SetActive(value: false);
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 13);
	}

	public void Continue()
	{
		mainMenu.SetActive(value: false);
		cheatOptions.SetActive(value: false);
		enterGallery.SetActive(value: false);
		PlayerManager.loaded = true;
		LoadLevel(SceneManager.GetActiveScene().buildIndex + PlayerPrefs.GetInt("CurrentScene"));
	}

	public void GoToSettingsMenu()
	{
		mainMenu.SetActive(value: false);
		optionsMenu.SetActive(value: true);
		backButton.SetActive(value: true);
	}

	public void GoToMainMenu()
	{
		optionsMenu.SetActive(value: false);
		backButton.SetActive(value: false);
		creditScreen.SetActive(value: false);
		resMenu.SetActive(value: false);
		mainMenu.SetActive(value: true);
		difficulty.SetActive(value: false);
	}

	public void GoToCredits()
	{
		mainMenu.SetActive(value: false);
		creditScreen.SetActive(value: true);
		backButton.SetActive(value: true);
	}

	public void GoToResMenu()
	{
		settingOptions.SetActive(value: false);
		resMenu.SetActive(value: true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void SetScreenResolution(int i)
	{
		if (resolutionToggles[i].isOn)
		{
			activeScreenResIndex = i;
			float num = 1.7777778f;
			Screen.SetResolution(screenWidths[i], (int)((float)screenWidths[i] / num), fullscreen: false);
			PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
			PlayerPrefs.Save();
		}
	}

	public void SetPostProcessing()
	{
		if (ambientOc.isOn)
		{
			PlayerPrefs.SetInt("AmbientOcclusion", 1);
		}
		else
		{
			PlayerPrefs.SetInt("AmbientOcclusion", 0);
		}
		if (bloom.isOn)
		{
			PlayerPrefs.SetInt("Bloom", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Bloom", 0);
		}
		PlayerPrefs.Save();
	}

	public void SetCheatMode()
	{
		if (cheatMode.isOn)
		{
			PlayerPrefs.SetInt("CheatMode", 1);
		}
		else
		{
			PlayerPrefs.SetInt("CheatMode", 0);
		}
		PlayerPrefs.Save();
	}

	public void SetInfiniteCloth()
	{
		if (infiniteCloth.isOn)
		{
			PlayerPrefs.SetInt("InfiniteCloth", 1);
		}
		else
		{
			PlayerPrefs.SetInt("InfiniteCloth", 0);
		}
	}

	public void SetFullscreen(bool isFullscreen)
	{
		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].interactable = !isFullscreen;
		}
		if (isFullscreen)
		{
			Resolution resolution = Screen.resolutions[Screen.resolutions.Length -1];
			Screen.SetResolution(resolution.width, resolution.height, fullscreen: true);
		}
		else
		{
			SetScreenResolution(activeScreenResIndex);
		}
		PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynch(sceneIndex));
	}

	public void LoadLevelByName(string sceneIndex)
	{
		StartCoroutine(LoadAsynchName(sceneIndex));
	}

	public void GoToSSPage()
	{
		Application.OpenURL("https://subscribestar.adult/moey");
	}

	public void GoToPatreonPage()
	{
		Application.OpenURL("https://www.patreon.com/moeymoey");
	}

	public void GoToTwitterPage()
	{
		Application.OpenURL("https://twitter.com/moey_dev");
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

	private IEnumerator LoadAsynchName(string sceneIndex)
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
