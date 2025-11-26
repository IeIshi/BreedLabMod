using UnityEngine;

public class RunToSoldier : MonoBehaviour
{
	public GameObject Soldier;

	public GameObject Control;

	public AudioSource awakeSound;

	public bool gangBangScenario;

	public GameObject MatingPActivate;

	public bool isMale;

	private void Start()
	{
		awakeSound.Play();
	}

	private void FixedUpdate()
	{
		FaceTarget();
		base.transform.position = Vector3.MoveTowards(base.transform.position, Soldier.transform.position, 5f * Time.deltaTime);
		if (Vector3.Distance(base.transform.position, Soldier.transform.position) <= 0.1f)
		{
			if (!gangBangScenario)
			{
				Control.GetComponent<SoldierInteractTrigger>().StartMating();
				Object.Destroy(base.gameObject);
			}
			else
			{
				MatingPActivate.SetActive(value: true);
				Control.GetComponent<GangBangActivate>().StartMating(Soldier, isMale, MatingPActivate);
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void FaceTarget()
	{
		Vector3 normalized = (Soldier.transform.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
	}

	public void StepEvent()
	{
	}

	public void Step2Event()
	{
	}
}
