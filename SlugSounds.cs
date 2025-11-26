using UnityEngine;

public class SlugSounds : MonoBehaviour
{
	public AudioSource slugSuck1;

	public AudioSource slugSuck2;

	public AudioSource insert1;

	public AudioSource insert2;

	public AudioSource bodyCollide;

	public GameObject heroine;

	private void SlugSuck()
	{
		int num = Random.Range(0, 2);
		heroine.GetComponent<HeroineStats>().GainOrgInstant(2f);
		if (num == 0)
		{
			slugSuck1.Play();
		}
		else
		{
			slugSuck2.Play();
		}
	}

	private void SlugInsert()
	{
		int num = Random.Range(0, 2);
		heroine.GetComponent<HeroineStats>().GainOrgInstant(2f);
		if (num == 0)
		{
			insert1.Play();
		}
		else
		{
			insert2.Play();
		}
	}

	private void SlugCollide()
	{
		bodyCollide.Play();
		if (GetComponent<Animator>().speed > 1f)
		{
			heroine.GetComponent<HeroineStats>().GainOrgInstant(1f);
			Object.FindObjectOfType<CamShaker>().StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		}
	}
}
