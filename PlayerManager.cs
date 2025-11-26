using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool KeyDoorChillArea;

		public bool enteredChallangeRoom;

		public bool ScSexAfterMath;

		public bool finishedDarkTunnelTwo;

		public bool MantisDown;

		public bool BroodmotherMet;

		public bool MasturbationFinished;

		public bool id1;

		public bool id11;

		public bool id2;

		public bool smallKey;

		public bool Office;

		public bool KeyB;

		public bool Key;

		public bool firstTimeEnterBotanicMain;

		public bool litaGaveKey;

		public bool firstHoul;

		public bool SAB;

		public bool AfterMindFleyer;

		public bool IsVirgin;

		public bool FkedBySc;
	}

	public static PlayerManager instance;

	public Image sexImage;

	public Image circle;

	public Image skullImage;

	public GameObject player;

	public static bool KeyDoorChillArea;

	public static bool enteredChallangeRoom;

	public static bool ScSexAfterMath;

	public static bool finishedDarkTunnelTwo;

	public static bool MantisDown;

	public static bool BroodmotherMet;

	public static bool MasturbationFinished;

	public static bool firstTimeEnterBotanicMain;

	public static bool litaGaveKey;

	public static bool firstHoul;

	public static bool AfterMindFleyer;

	public static bool IsVirgin;

	public static bool FkedBySc;

	public static bool spawnedFutaMounter;

	public static bool spawnedFutaChaser;

	public static bool loaded;

	public static bool loadedrly;

	public List<GameObject> enemyTurnOrder;

	public GameObject lastScientistWhoFuckedMe;

	public static bool heroineIsMounted;

	public static bool cheatModeOn;

	public static bool infCloth;

	public static bool SAB;

	public static bool id1;

	public static bool id11;

	public static bool id2;

	public static bool smallKey;

	public static bool KeyB;

	public static bool Office;

	public static bool Key;

	private string SavePath => Application.persistentDataPath + "/save.txt";

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.KeyDoorChillArea = KeyDoorChillArea;
		saveData.enteredChallangeRoom = enteredChallangeRoom;
		saveData.ScSexAfterMath = ScSexAfterMath;
		saveData.finishedDarkTunnelTwo = finishedDarkTunnelTwo;
		saveData.MantisDown = MantisDown;
		saveData.BroodmotherMet = BroodmotherMet;
		saveData.MasturbationFinished = MasturbationFinished;
		saveData.id1 = id1;
		saveData.id11 = id11;
		saveData.id2 = id2;
		saveData.smallKey = smallKey;
		saveData.KeyB = KeyB;
		saveData.Office = Office;
		saveData.Key = Key;
		saveData.firstTimeEnterBotanicMain = firstTimeEnterBotanicMain;
		saveData.litaGaveKey = litaGaveKey;
		saveData.firstHoul = firstHoul;
		saveData.SAB = SAB;
		saveData.AfterMindFleyer = AfterMindFleyer;
		saveData.IsVirgin = IsVirgin;
		saveData.FkedBySc = FkedBySc;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData obj = (SaveData)state;
		KeyDoorChillArea = obj.KeyDoorChillArea;
		enteredChallangeRoom = obj.enteredChallangeRoom;
		ScSexAfterMath = obj.ScSexAfterMath;
		finishedDarkTunnelTwo = obj.finishedDarkTunnelTwo;
		MantisDown = obj.MantisDown;
		BroodmotherMet = obj.BroodmotherMet;
		MasturbationFinished = obj.MasturbationFinished;
		id1 = obj.id1;
		id11 = obj.id11;
		id2 = obj.id2;
		smallKey = obj.smallKey;
		KeyB = obj.KeyB;
		Office = obj.Office;
		Key = obj.Key;
		firstTimeEnterBotanicMain = obj.firstTimeEnterBotanicMain;
		litaGaveKey = obj.litaGaveKey;
		firstHoul = obj.firstHoul;
		SAB = obj.SAB;
		AfterMindFleyer = obj.AfterMindFleyer;
		IsVirgin = obj.IsVirgin;
		FkedBySc = obj.FkedBySc;
	}

	private void Awake()
	{
		if (Screen.currentResolution.refreshRate > 200)
		{
			Application.targetFrameRate = 120;
		}
		else
		{
			Application.targetFrameRate = Screen.currentResolution.refreshRate;
		}
		instance = this;
		if (PlayerPrefs.GetInt("CheatMode") == 1)
		{
			cheatModeOn = true;
			Debug.Log("CHEAT MODE ON");
		}
		else
		{
			cheatModeOn = false;
			Debug.Log("CHEAT MODE OFF");
		}
		if (PlayerPrefs.GetInt("InfiniteCloth") == 1)
		{
			infCloth = true;
			Debug.Log("INFINITE CLOTH ON");
		}
		else
		{
			infCloth = false;
			Debug.Log("INFINITE CLOTH OFF");
		}
		if (loaded)
		{
			Load();
			loaded = false;
		}
	}

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "BotanicMain" && !firstTimeEnterBotanicMain)
		{
			HeroineStats.masturbating = false;
			HeroineStats.stunned = false;
			HeroineStats.debuffedStam = 0f;
			PlayerController.iFalled = true;
			player.GetComponent<Animator>().SetBool("isSitting", value: true);
			firstTimeEnterBotanicMain = true;
		}
		heroineIsMounted = false;
		sexImage.enabled = false;
		circle.enabled = false;
		try
		{
			skullImage = GameObject.Find("skull").GetComponent<Image>();
			skullImage.enabled = false;
		}
		catch
		{
			Debug.Log("No Image found");
		}
		enemyTurnOrder.Clear();
		Debug.Log("Loaded?: " + loadedrly);
	}

	public void KillPlayer()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void Load()
	{
		Dictionary<string, object> state = LoadFile();
		loadedrly = true;
		RestoreState(state);
	}

	private Dictionary<string, object> LoadFile()
	{
		if (!File.Exists(SavePath))
		{
			return new Dictionary<string, object>();
		}
		using FileStream serializationStream = File.Open(SavePath, FileMode.Open);
		return (Dictionary<string, object>)new BinaryFormatter().Deserialize(serializationStream);
	}

	private void CaptureState(Dictionary<string, object> state)
	{
		SaveableEntity[] array = UnityEngine.Object.FindObjectsOfType<SaveableEntity>();
		foreach (SaveableEntity saveableEntity in array)
		{
			state[saveableEntity.Id] = saveableEntity.CaptureState();
		}
	}

	private void RestoreState(Dictionary<string, object> state)
	{
		SaveableEntity[] array = UnityEngine.Object.FindObjectsOfType<SaveableEntity>();
		foreach (SaveableEntity saveableEntity in array)
		{
			if (state.TryGetValue(saveableEntity.Id, out var value))
			{
				saveableEntity.RestoreState(value);
			}
		}
	}
}
