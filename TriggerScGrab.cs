using System.Collections;
using UnityEngine;

public class TriggerScGrab : MonoBehaviour
{
	private Inventory inventory;

	public GameObject Scientist;

	public GameObject ScientistClothed;

	private ScXHeroine2Interact scXh;

	private bool grabbed;

	public AudioSource sound;

	public AudioSource sound2;

	private bool sceneTriggered;

	private void Start()
	{
		inventory = Inventory.instance;
		scXh = Scientist.GetComponent<ScXHeroine2Interact>();
		Scientist.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (sceneTriggered || !(other.tag == "Player") || grabbed)
		{
			return;
		}
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].name == "GreenOrbContainer")
			{
				Scientist.SetActive(value: true);
				ScientistClothed.SetActive(value: false);
				StartCoroutine(Grab());
				grabbed = true;
				sceneTriggered = true;
				break;
			}
		}
	}

	private IEnumerator Grab()
	{
		scXh.BlackScreen.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		if (!sound.isPlaying)
		{
			sound.Play();
		}
		yield return new WaitForSeconds(1f);
		if (!sound2.isPlaying)
		{
			sound2.Play();
		}
		yield return new WaitForSeconds(0.5f);
		if (!EquipmentManager.heroineIsNaked)
		{
			EquipmentManager.instance.RipPantsu();
		}
		HeroineStats.currentOrg = 0f;
		HeroineStats.masturbating = true;
		yield return new WaitForSeconds(3f);
		scXh.dialogueTriggered = true;
		scXh.triggerTease = true;
		scXh.heroineAnimator.SetBool("isScared", value: true);
		StartCoroutine(scXh.FadeOutAndDisable());
	}
}
