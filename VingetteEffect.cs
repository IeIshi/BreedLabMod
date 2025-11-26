using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VingetteEffect : MonoBehaviour
{
	public PostProcessVolume volume;

	private Vignette _vignette;

	private AmbientOcclusion ambient;

	private Bloom bloom;

	public float speed = 1f;

	private float lustCalc;

	public AudioSource beatFast;

	public AudioSource beatSlow;

	private bool soundPlayed;

	private float startValue;

	private float endValueLow = 0.2f;

	private float endValueMid = 0.35f;

	private float endValueIntense = 0.5f;

	private float transitionTimeSlow = 1.5f;

	private float transitionTimeMid = 1f;

	private float transitionTimeFast = 0.8f;

	private float t;

	private float b;

	private void Start()
	{
		bool num = PlayerPrefs.GetInt("AmbientOcclusion") == 1;
		bool flag = PlayerPrefs.GetInt("Bloom") == 1;
		volume.profile.TryGetSettings<Vignette>(out _vignette);
		_vignette.intensity.value = 0f;
		volume.profile.TryGetSettings<AmbientOcclusion>(out ambient);
		if (num)
		{
			ambient.active = true;
		}
		else
		{
			ambient.active = false;
		}
		volume.profile.TryGetSettings<Bloom>(out bloom);
		if (flag)
		{
			bloom.active = true;
		}
		else
		{
			bloom.active = false;
		}
	}

	private void Update()
	{
	}

	private void PulseLow()
	{
		if (b == 0f)
		{
			t += Time.deltaTime / transitionTimeSlow;
			_vignette.intensity.value = Mathf.SmoothStep(startValue, endValueLow, t);
		}
		if (t >= transitionTimeSlow)
		{
			b += Time.deltaTime / transitionTimeSlow;
			_vignette.intensity.value = Mathf.SmoothStep(endValueLow, startValue, b);
			if (b >= transitionTimeSlow)
			{
				b = 0f;
				t = 0f;
			}
		}
	}

	private void PulseIntense()
	{
		if (b == 0f)
		{
			t += Time.deltaTime / transitionTimeMid;
			_vignette.intensity.value = Mathf.SmoothStep(startValue, endValueMid, t);
		}
		if (t >= transitionTimeMid)
		{
			b += Time.deltaTime / transitionTimeMid;
			_vignette.intensity.value = Mathf.SmoothStep(endValueMid, startValue, b);
			if (b >= transitionTimeMid)
			{
				b = 0f;
				t = 0f;
			}
		}
		else
		{
			t = transitionTimeMid;
		}
	}

	private void PulseOnCum()
	{
		if (b == 0f)
		{
			t += Time.deltaTime / transitionTimeFast;
			_vignette.intensity.value = Mathf.SmoothStep(startValue, endValueIntense, t);
		}
		if (t >= transitionTimeFast)
		{
			b += Time.deltaTime / transitionTimeFast;
			_vignette.intensity.value = Mathf.SmoothStep(endValueIntense, startValue, b);
			if (b >= transitionTimeFast)
			{
				b = 0f;
				t = 0f;
			}
		}
	}

	private void Heartbeat()
	{
	}
}
