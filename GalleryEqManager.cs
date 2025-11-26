using UnityEngine;

public class GalleryEqManager : MonoBehaviour
{
	[SerializeField]
	public bool[] eqIdArray;

	public SkinnedMeshRenderer[] currentMeshes;

	private void Start()
	{
		currentMeshes = new SkinnedMeshRenderer[8];
	}
}
