using UnityEngine;
using UnityEngine.UI;

public class MovementManipulator : MonoBehaviour
{
	public static MovementManipulator instance;

	private PassiveStats speed;

	private HeroineStats lust;

	private float runMalus;

	public static bool mouthClaimed;

	public static bool assClaimed;

	public static bool vaginaClaimed;

	public static bool mouthInject;

	public static bool assInject;

	public static bool occupied;

	public static int chasingWaspCount;

	public static bool getsTeased;

	public Image img;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		img = GameObject.Find("Intercourse").GetComponent<Image>();
		chasingWaspCount = 0;
		speed = PlayerManager.instance.player.GetComponent<PassiveStats>();
		lust = PlayerManager.instance.player.GetComponent<HeroineStats>();
	}

	private void FixedUpdate()
	{
		if (speed.speed.baseValue >= -50f)
		{
			speed.speed.baseValue = 0f - HeroineStats.currentLust;
		}
		if (speed.speed.baseValue < -50f)
		{
			speed.speed.baseValue = -50f;
		}
	}
}
