using UnityEngine;

public class CumFlow : MonoBehaviour
{
	public GameObject cumStrain;

	public float mSize;

	private float speed = 1f;

	private void Start()
	{
		cumStrain.SetActive(value: false);
	}

	private void FixedUpdate()
	{
		if (AbortBed.fastFuck)
		{
			PlayerController.animator.SetBool("isAhegao", value: true);
		}
		if (cumStrain.activeSelf && !(mSize > 300f))
		{
			cumStrain.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize += speed);
		}
	}
}
