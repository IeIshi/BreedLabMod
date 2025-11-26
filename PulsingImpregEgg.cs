using UnityEngine;

public class PulsingImpregEgg : MonoBehaviour
{
	private float mSize;

	private bool scaleDown;

	private void Update()
	{
		if (!BodestSchaltung.allActivated)
		{
			Scale();
			return;
		}
		GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		GetComponent<PulsingImpregEgg>().enabled = false;
	}

	private void Scale()
	{
		if (!scaleDown)
		{
			GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize++);
		}
		if (mSize > 99f)
		{
			scaleDown = true;
		}
		if (scaleDown)
		{
			GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize--);
			if (mSize < 1f)
			{
				scaleDown = false;
			}
		}
	}
}
