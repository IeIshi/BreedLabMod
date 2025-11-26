using UnityEngine;
using UnityEngine.UI;

public class MasturbationArea : MonoBehaviour
{
	public static bool mastArea;

	private Image img;

	private void Start()
	{
		img = GameObject.Find("Intercourse").GetComponent<Image>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			img.enabled = true;
			mastArea = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			img.enabled = false;
			mastArea = false;
		}
	}
}
