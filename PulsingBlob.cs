using UnityEngine;

public class PulsingBlob : MonoBehaviour
{
	private float mSize;

	private bool scaleDown;

	public float pulsingSpeed = 0.5f;

	private void Update()
	{
		Scale();
	}

	private void Scale()
	{
		if (!scaleDown)
		{
			GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize += pulsingSpeed);
		}
		if (mSize > 99f)
		{
			scaleDown = true;
		}
		if (scaleDown)
		{
			GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize -= pulsingSpeed);
			if (mSize < 1f)
			{
				scaleDown = false;
			}
		}
	}
}
