using UnityEngine;

public class PulsingEgg : MonoBehaviour
{
	private float mSize;

	private bool scaleDown;

	private void Update()
	{
		if (!PlayerManager.KeyDoorChillArea)
		{
			Scale();
			return;
		}
		GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
		GetComponent<PulsingEgg>().enabled = false;
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
