using UnityEngine;

public class BodyMatChange : MonoBehaviour
{
	public Material[] material;

	private Renderer rend;

	private static bool changed;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		rend.sharedMaterial = material[0];
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (!changed)
			{
				changed = true;
			}
			else
			{
				changed = false;
			}
			Debug.Log("CHANGED?: " + changed);
		}
	}

	private void FixedUpdate()
	{
		if (!changed)
		{
			if (HeroineStats.horny)
			{
				if (rend.sharedMaterial != material[1])
				{
					rend.sharedMaterial = material[1];
				}
			}
			else if (HeroineStats.aroused)
			{
				if (rend.sharedMaterial != material[1])
				{
					rend.sharedMaterial = material[1];
				}
			}
			else if (rend.sharedMaterial != material[0])
			{
				rend.sharedMaterial = material[0];
			}
		}
		else if (HeroineStats.horny)
		{
			if (rend.sharedMaterial != material[3])
			{
				rend.sharedMaterial = material[3];
			}
		}
		else if (HeroineStats.aroused)
		{
			if (rend.sharedMaterial != material[3])
			{
				rend.sharedMaterial = material[3];
			}
		}
		else if (rend.sharedMaterial != material[2])
		{
			rend.sharedMaterial = material[2];
		}
	}
}
