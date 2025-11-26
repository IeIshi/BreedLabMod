using UnityEngine;
using UnityEngine.UI;

public class PlantGaser : MonoBehaviour
{
	public enum MyState
	{
		GAS,
		NOGAS
	}

	public float timer;

	private float isTimer;

	private GameObject Heroine;

	public ParticleSystem gas;

	public float lustDamage;

	public GameObject ps;

	private Image pinkScreen;

	private Color c;

	public MyState state;

	private bool gasing;

	private bool noGasing;

	private AudioSource heartBeat;

	private void Start()
	{
		heartBeat = GameObject.Find("HeartBeatGlobal").GetComponent<AudioSource>();
		Heroine = PlayerManager.instance.player;
		noGasing = true;
		pinkScreen = ps.GetComponent<Image>();
		c = pinkScreen.color;
		c.a = 0f;
	}

	private void FixedUpdate()
	{
		isTimer += Time.deltaTime;
		if (isTimer > timer)
		{
			if (noGasing)
			{
				gasing = true;
				noGasing = false;
				state = MyState.GAS;
				isTimer = 0f;
			}
			else if (gasing)
			{
				gasing = false;
				noGasing = true;
				state = MyState.NOGAS;
				isTimer = 0f;
			}
		}
		GaserState(state);
	}

	private void GaserState(MyState state)
	{
		switch (state)
		{
		case MyState.GAS:
			Gas();
			break;
		case MyState.NOGAS:
			NoGas();
			break;
		}
	}

	private void Gas()
	{
		if (!gas.isPlaying)
		{
			gas.Play();
		}
	}

	private void NoGas()
	{
		gas.Stop();
	}

	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && gas.particleCount > 150)
		{
			ps.SetActive(value: true);
			pinkScreen.color = c;
			c.a = Mathf.PingPong(Time.deltaTime / 2f, 0.3f);
			PostProcessingManager.instance.VingetteEffectHigh();
			if (!heartBeat.isPlaying)
			{
				heartBeat.Play();
			}
			Heroine.GetComponent<HeroineStats>().GainLust(lustDamage);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			Debug.Log("EXIT");
			ps.SetActive(value: false);
		}
	}
}
