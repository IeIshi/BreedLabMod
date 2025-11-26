using UnityEngine;

public class EquipCloth : Interactable
{
	public Equipment equipment;

	public SkinnedMeshRenderer targetMesh;

	public GameObject gallaryEqManager;

	public AudioSource clothSound;

	public override void Interact()
	{
		base.Interact();
		Equip(equipment);
	}

	public void Equip(Equipment equipment)
	{
		int equipSlot = (int)equipment.equipSlot;
		if (!gallaryEqManager.GetComponent<GalleryEqManager>().eqIdArray[equipSlot])
		{
			SkinnedMeshRenderer skinnedMeshRenderer = Object.Instantiate(equipment.mesh);
			skinnedMeshRenderer.transform.parent = targetMesh.transform;
			skinnedMeshRenderer.bones = targetMesh.bones;
			skinnedMeshRenderer.rootBone = targetMesh.rootBone;
			gallaryEqManager.GetComponent<GalleryEqManager>().currentMeshes[equipSlot] = skinnedMeshRenderer;
			clothSound.Play();
			gallaryEqManager.GetComponent<GalleryEqManager>().eqIdArray[equipSlot] = true;
		}
		else
		{
			Object.Destroy(gallaryEqManager.GetComponent<GalleryEqManager>().currentMeshes[equipSlot].gameObject);
			gallaryEqManager.GetComponent<GalleryEqManager>().eqIdArray[equipSlot] = false;
		}
	}
}
