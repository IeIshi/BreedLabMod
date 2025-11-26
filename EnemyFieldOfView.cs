using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFieldOfView : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public bool isDed;
	}

	public float viewRadius;

	[Range(0f, 360f)]
	public float viewAngle;

	public LayerMask targetMask;

	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public static Transform enemTarget;

	public bool heroineIsVisible;

	private GameObject heroine;

	private float distance;

	public bool claimedHer;

	private Transform cam;

	private Image healthSlider;

	public Transform targetHealthUI;

	public GameObject healthUIPrefab;

	private Transform healthUI;

	public float maxHealth;

	public float currentHealth;

	public bool gotHit;

	private float time;

	public bool isDed;

	private float healthShowTimer;

	private bool timerRunning;

	private bool activeOnceWhenDead;

	public float closeRangeDetection = 4f;

	private void Start()
	{
		claimedHer = false;
		currentHealth = maxHealth;
		heroine = GameObject.Find("Heroine");
		Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in array)
		{
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				if (healthUIPrefab != null)
				{
					healthUI = UnityEngine.Object.Instantiate(healthUIPrefab, canvas.transform).transform;
					healthSlider = healthUI.GetChild(0).GetComponent<Image>();
				}
				break;
			}
		}
		gotHit = false;
		cam = Camera.main.transform;
		if (healthUIPrefab != null)
		{
			healthUI.gameObject.SetActive(value: false);
		}
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.isDed = isDed;
		return saveData;
	}

	public void RestoreState(object state)
	{
		isDed = ((SaveData)state).isDed;
	}

	private void LateUpdate()
	{
		FindVisibleTargets();
		ShowHealthUI();
		if (visibleTargets.Count != 0)
		{
			heroineIsVisible = true;
		}
		else
		{
			StartCoroutine(LooseSight());
		}
		ReactToShots();
		HealthShowTimer();
	}

	public void TakeDamage(float amount)
	{
		gotHit = true;
		drainHealth(amount);
	}

	private void HealthShowTimer()
	{
		if (gotHit)
		{
			timerRunning = true;
		}
		if (timerRunning)
		{
			healthUI.gameObject.SetActive(value: true);
			healthShowTimer += Time.deltaTime * 1f;
			if (healthShowTimer >= 5f)
			{
				healthUI.gameObject.SetActive(value: false);
				healthShowTimer = 0f;
				timerRunning = false;
			}
		}
	}

	private void ReactToShots()
	{
		if (gotHit)
		{
			GetComponent<NavMeshAgent>().isStopped = true;
			time += Time.deltaTime;
			if (time >= 0.3f)
			{
				time = 0f;
				GetComponent<NavMeshAgent>().isStopped = false;
				gotHit = false;
			}
		}
	}

	public void drainHealth(float drainValue)
	{
		gotHit = true;
		currentHealth -= drainValue;
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
	}

	private void ShowHealthUI()
	{
		if (healthUI != null)
		{
			healthUI.position = targetHealthUI.position;
			healthUI.forward = -cam.forward;
			healthSlider.fillAmount = currentHealth / maxHealth;
			if (currentHealth <= 0f && !activeOnceWhenDead)
			{
				GetComponent<NavMeshAgent>().isStopped = true;
				GetComponent<Animator>().SetBool("isDed", value: true);
				healthUI.gameObject.SetActive(value: false);
				isDed = true;
				InventoryUI.heroineIsChased = false;
				activeOnceWhenDead = true;
			}
		}
	}

	private void FindVisibleTargets()
	{
		distance = Vector3.Distance(heroine.transform.position, base.transform.position);
		Collider[] array = Physics.OverlapSphere(base.transform.position, viewRadius, targetMask);
		for (int i = 0; i < array.Length; i++)
		{
			Transform transform = array[i].transform;
			Vector3 normalized = (transform.position - base.transform.position).normalized;
			float num = Vector3.Distance(base.transform.position, transform.position);
			if (!Physics.Raycast(base.transform.position, normalized, num, obstacleMask))
			{
				if (Vector3.Angle(base.transform.forward, normalized) < viewAngle / 2f)
				{
					visibleTargets.Add(transform);
				}
				else if (num < closeRangeDetection)
				{
					visibleTargets.Add(transform);
				}
				else
				{
					visibleTargets.Clear();
				}
			}
			if (distance > viewRadius)
			{
				visibleTargets.Clear();
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += base.transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * ((float)Math.PI / 180f)), 0f, Mathf.Cos(angleInDegrees * ((float)Math.PI / 180f)));
	}

	private IEnumerator LooseSight()
	{
		yield return new WaitForSeconds(1f);
		heroineIsVisible = false;
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, closeRangeDetection);
	}
}
