using UnityEngine;

public class ScSexAfterMath : MonoBehaviour
{
	public GameObject Scientist;

	public GameObject ScCameAlot;

	public GameObject blockingKisten;

	public GameObject verteileKisten;

	public GameObject blockingWall;

	public GameObject scientsits;

	private void Start()
	{
		if (PlayerManager.ScSexAfterMath)
		{
			PlayerManager.instance.player.transform.localPosition = base.gameObject.transform.localPosition;
			PlayerManager.instance.player.transform.localRotation = base.gameObject.transform.localRotation;
			PlayerController.iFalled = true;
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("falled", value: true);
			HeroineStats.creampied = true;
			HeroineStats.hugeAmount = true;
			PlayerManager.instance.player.GetComponent<Animator>().SetBool("isCumFilled", value: true);
			Scientist.SetActive(value: false);
			blockingKisten.SetActive(value: false);
			blockingWall.SetActive(value: true);
			ScCameAlot.SetActive(value: true);
			scientsits.SetActive(value: false);
			verteileKisten.SetActive(value: true);
			HeroineStats.pregnant = false;
			Debug.Log("Loaded FROM SCSEXAFTERMATH");
		}
		else
		{
			blockingWall.SetActive(value: false);
			ScCameAlot.SetActive(value: false);
			blockingKisten.SetActive(value: true);
			verteileKisten.SetActive(value: false);
		}
	}
}
