using UnityEngine;

public class ConvertToRegularMesh : MonoBehaviour
{
	[ContextMenu("Convert to regular mesh")]
	private void Convert()
	{
		SkinnedMeshRenderer component = GetComponent<SkinnedMeshRenderer>();
		MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		base.gameObject.AddComponent<MeshFilter>().sharedMesh = component.sharedMesh;
		meshRenderer.sharedMaterials = component.sharedMaterials;
		Object.DestroyImmediate(component);
		Object.DestroyImmediate(this);
	}
}
