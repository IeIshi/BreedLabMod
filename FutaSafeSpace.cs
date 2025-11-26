using UnityEngine;

public class FutaSafeSpace : MonoBehaviour
{
	public GameObject FutaController;

	public GameObject FutaSafeState;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && FutaController.activeSelf)
		{
			FutaController.SetActive(value: false);
			FutaSafeState.GetComponent<FutaAreaSafeState>().passedFutaArea = true;
		}
	}
}
