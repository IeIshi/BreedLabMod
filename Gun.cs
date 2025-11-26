using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour, ISaveable
{
	[Serializable]
	private struct SaveData
	{
		public int currentAmmo;

		public int additionalAmmo;
	}

	private CamShaker shake;

	public float damage = 10f;

	public float range = 100f;

	public float impactForce = 1000f;

	public float fireRate = 15f;

	public float weaponRechargeRate = 10f;

	public int maxAmmo = 6;

	private int basicAmmo;

	public static int additionalAmmo;

	private int currentAmmo = 2;

	public float releadTime = 2f;

	public static bool isReloading;

	public Camera fpsCam;

	public ParticleSystem muzzleFlash;

	public ParticleSystem muzzleFlashLoop;

	public GameObject impactEffect;

	private float nextTimeToFire = 1f;

	public GameObject currentAmmoAnzeige;

	public Text minAmount;

	public Text maxAmount;

	private Slider AmmoCharge;

	private Inventory inventory;

	private bool noAmmo;

	public AudioSource shootSound;

	public AudioSource noAmmoSound;

	public static bool enemyToclose;

	public bool litaPhanSpawned;

	private int layerMask = -8;

	private void Start()
	{
		basicAmmo = 2;
		inventory = Inventory.instance;
		shake = UnityEngine.Object.FindObjectOfType<CamShaker>();
		currentAmmoAnzeige.SetActive(value: false);
		maxAmmo = basicAmmo + additionalAmmo;
		currentAmmo = maxAmmo;
		AmmoCharge = GameObject.Find("WeaponChargeBar").GetComponent<Slider>();
		AmmoCharge.value = 0f;
		AmmoCharge.gameObject.SetActive(value: false);
	}

	public object CaptureState()
	{
		SaveData saveData = default(SaveData);
		saveData.currentAmmo = currentAmmo;
		saveData.additionalAmmo = additionalAmmo;
		return saveData;
	}

	public void RestoreState(object state)
	{
		SaveData saveData = (SaveData)state;
		currentAmmo = saveData.currentAmmo;
		additionalAmmo = saveData.additionalAmmo;
	}

	private void FixedUpdate()
	{
		if (currentAmmo < maxAmmo)
		{
			ChargeAmmo();
		}
	}

	private void Update()
	{
		if (CameraFollow.shootingMode)
		{
			currentAmmoAnzeige.SetActive(value: true);
			if (isReloading)
			{
				return;
			}
			if (currentAmmo <= 0)
			{
				noAmmo = true;
				StartCoroutine(Reload());
				noAmmo = false;
				return;
			}
			if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
			{
				nextTimeToFire = Time.time + 1f / fireRate;
				if (!noAmmo)
				{
					Shoot();
				}
				else
				{
					noAmmoSound.Play();
				}
			}
			maxAmount.text = maxAmmo.ToString();
			minAmount.text = currentAmmo.ToString();
		}
		else if (currentAmmo == maxAmmo)
		{
			currentAmmoAnzeige.SetActive(value: false);
		}
		else
		{
			maxAmount.text = maxAmmo.ToString();
			minAmount.text = currentAmmo.ToString();
		}
	}

	private IEnumerator Reload()
	{
		isReloading = true;
		Debug.Log("Reloading...");
		PlayerController.animator.SetBool("isReloading", value: true);
		muzzleFlashLoop.Play();
		yield return new WaitForSeconds(releadTime);
		PlayerController.animator.SetBool("isReloading", value: false);
		currentAmmo++;
		muzzleFlashLoop.Stop();
		isReloading = false;
	}

	private void ChargeAmmo()
	{
		AmmoCharge.gameObject.SetActive(value: true);
		currentAmmoAnzeige.SetActive(value: true);
		AmmoCharge.value += Time.deltaTime / weaponRechargeRate;
		if (AmmoCharge.value == 1f)
		{
			currentAmmo++;
			AmmoCharge.value = 0f;
			if (currentAmmo == maxAmmo)
			{
				AmmoCharge.gameObject.SetActive(value: false);
			}
		}
	}

	private void Shoot()
	{
		muzzleFlash.Play();
		shootSound.Play();
		currentAmmo--;
		PlayerController.animator.Play("rig|Shoot");
		shake.StartShake(new CamShaker.Properties(0.001f, 0.01f, 10f, 1f, 0.1f, 0.025f, 0.025f, 0.025f));
		if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hitInfo, range, layerMask))
		{
			return;
		}
		Debug.Log(hitInfo.transform.name);
		Debug.Log(hitInfo.transform.tag);
		if (hitInfo.transform.tag == "HumanoidEnemy")
		{
			hitInfo.transform.GetComponent<HumanoidController>().TakeDamage(damage);
		}
		if (hitInfo.transform.tag == "LickerEnemy")
		{
			hitInfo.transform.GetComponent<NewLickerControl>().TakeDamage(damage);
		}
		if (hitInfo.transform.tag == "ImpregEnemy")
		{
			hitInfo.transform.GetComponent<ImpregInsectControl>().TakeDamage(damage);
			Debug.Log("WEAPON HIT");
		}
		if (hitInfo.transform.tag == "Enemy")
		{
			hitInfo.transform.GetComponent<EnemyFieldOfView>().TakeDamage(damage);
			Debug.Log("WEAPON HIT");
		}
		if (hitInfo.transform.tag == "Mantis")
		{
			hitInfo.transform.GetComponent<GotHit>().TakeDamage(damage);
			Debug.Log("WEAPIN HIT");
		}
		if (hitInfo.transform.tag == "Wasp")
		{
			hitInfo.transform.GetComponent<WaspTakeDmg>().TakeDamage(damage);
			Debug.Log("WASP WEAPIN HIT");
		}
		if (hitInfo.transform.tag == "Gaser")
		{
			ListGaserHitDet component = hitInfo.transform.GetComponent<ListGaserHitDet>();
			component.explosionParticle.Play();
			component.GetComponent<SkinnedMeshRenderer>().enabled = false;
			component.GetComponent<SphereCollider>().enabled = false;
			component.dead = true;
			component.triggerPoint.SetActive(value: false);
			Debug.Log("WEAPIN HIT");
		}
		if (hitInfo.transform.tag == "PlantWalker")
		{
			PlantWalkerControl component2 = hitInfo.transform.GetComponent<PlantWalkerControl>();
			if (!litaPhanSpawned)
			{
				component2.SpawnLita();
				litaPhanSpawned = true;
			}
			Debug.Log("WEAPON HIT");
		}
		if (hitInfo.transform.tag == "MindFleyer")
		{
			hitInfo.transform.GetComponent<MFTakeDmg>().TakeDamage(damage);
			Debug.Log("WEAPON IT");
		}
		if (hitInfo.rigidbody != null)
		{
			hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
		}
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)), 2f);
	}
}
