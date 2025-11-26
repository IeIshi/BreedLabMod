using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
	public GameObject Futa;

	public GameObject myAmmo;

	private Animator futaAnim;

	private GameObject ammo;

	private float timer;

	public float respawnTime = 3f;

	private int spawnCount;

	private void Start()
	{
		futaAnim = Futa.GetComponent<Animator>();
		spawnCount = 0;
	}

	private void FixedUpdate()
	{
		if (!MantisAi.isDed)
		{
			Futa.layer = 0;
			if (ammo == null)
			{
				if (spawnCount < 3)
				{
					timer += Time.deltaTime;
					if (timer > respawnTime)
					{
						ammo = Object.Instantiate(myAmmo, new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z), Quaternion.identity);
						spawnCount++;
						timer = 0f;
					}
				}
				futaAnim.SetBool("give", value: false);
			}
			else
			{
				futaAnim.SetBool("give", value: true);
			}
		}
		else
		{
			if (ammo != null)
			{
				Object.Destroy(ammo);
			}
			futaAnim.SetBool("give", value: false);
		}
	}
}
