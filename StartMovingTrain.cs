using System.Collections;
using UnityEngine;

public class StartMovingTrain : MonoBehaviour
{
	private Animator trainAnim;

	public GameObject Tram;

	public GameObject Heroine;

	public GameObject LitaPhan;

	public GameObject Hair1;

	public GameObject Hair2;

	public GameObject Hair3;

	public GameObject Hair4;

	public GameObject Hair5;

	public GameObject Hair6;

	public GameObject gunControl;

	private bool startMoving;

	private float x;

	public AudioSource movingSound;

	public Dialogue dialogue;

	private void Start()
	{
		trainAnim = Tram.GetComponent<Animator>();
		LitaPhan.SetActive(value: false);
	}

	private void FixedUpdate()
	{
		if (startMoving)
		{
			InventoryUI.heroineIsChased = true;
			x -= Time.deltaTime * 2f;
			Tram.transform.position = new Vector3(x, 0f, 0f);
			if (!movingSound.isPlaying)
			{
				movingSound.Play();
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (HeroineStats.pregnant)
			{
				TriggerDialoge();
				return;
			}
			Debug.Log("Close Doors and start moving");
			Heroine.transform.parent = Tram.transform;
			gunControl.GetComponent<CameraFollow>().gunNotUsable = true;
			StartCoroutine(closeDoors());
			Tram.GetComponent<BoxCollider>().isTrigger = false;
		}
	}

	private void DisableHair()
	{
		Hair1.GetComponent<DynamicBone>().enabled = false;
		Hair2.GetComponent<DynamicBone>().enabled = false;
		Hair3.GetComponent<DynamicBone>().enabled = false;
		Hair4.GetComponent<DynamicBone>().enabled = false;
		Hair5.GetComponent<DynamicBone>().enabled = false;
		Hair6.GetComponent<DynamicBone>().enabled = false;
	}

	private IEnumerator closeDoors()
	{
		yield return new WaitForSeconds(2f);
		trainAnim.SetBool("isOpen", value: false);
		yield return new WaitForSeconds(2f);
		DisableHair();
		startMoving = true;
		LitaPhan.SetActive(value: true);
	}

	public void TriggerDialoge()
	{
		Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
	}
}
