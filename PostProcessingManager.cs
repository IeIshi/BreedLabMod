using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostProcessingManager : MonoBehaviour
{
	public static PostProcessingManager instance;

	public Volume volume;

	public static Vignette vignette;

	private LiftGammaGain gamma;

	public Bloom bloom;

	public GameObject ps;

	private Image pinkScreen;

	private Color c;

	private Color c2;

	public float fadeSpeed = 0.5f;

	public float pinkIntensity = 0.1f;

	public static bool thrusted;

	private bool down;

	private float gammaValue;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		volume.profile.TryGet<Vignette>(out vignette);
		volume.profile.TryGet<LiftGammaGain>(out gamma);
		volume.profile.TryGet<Bloom>(out bloom);
		bloom.active = true;
		gammaValue = 0.3f * PlayerPrefs.GetFloat("Gamma");
		gamma.gamma.Override(new Vector4(1f, 1f, 1f, gammaValue));
		pinkScreen = ps.GetComponent<Image>();
		c = pinkScreen.color;
		c2 = pinkScreen.color;
		ps.SetActive(value: false);
		c.a = 0f;
	}

	private void FixedUpdate()
	{
		if (PlayerController.iGetFucked)
		{
			if (HeroineStats.orgasm)
			{
				ps.SetActive(value: true);
				OrgasmEffect();
			}
			else if (HeroineStats.currentOrg > 50f)
			{
				float num = (HeroineStats.currentOrg - 50f) * 100f / 50f;
				float a = 0.3f * num / 100f;
				c2.a = a;
				pinkScreen.color = c2;
				ps.SetActive(value: true);
			}
			else
			{
				ps.SetActive(value: false);
			}
		}
	}

	public void VingetteEffectLow()
	{
		vignette.intensity.value = Mathf.PingPong(Time.time / 2f, 0.4f);
	}

	public void VingetteEffectHigh()
	{
		vignette.intensity.value = Mathf.PingPong(Time.time / 2f, 0.5f);
	}

	public void OrgasmEffect()
	{
		pinkScreen.color = c;
		c.a = Mathf.PingPong(Time.time / 2f, 0.3f);
	}

	private void ThrustEffect()
	{
		if (!thrusted)
		{
			return;
		}
		ps.SetActive(value: true);
		pinkScreen.color = c;
		if (down)
		{
			c.a -= Time.deltaTime * fadeSpeed * 2f;
			if (c.a <= 0.05f)
			{
				down = false;
				thrusted = false;
				ps.SetActive(value: false);
			}
		}
		else if (c.a <= pinkIntensity)
		{
			c.a += Time.deltaTime * fadeSpeed;
		}
		else
		{
			down = true;
		}
	}
}
