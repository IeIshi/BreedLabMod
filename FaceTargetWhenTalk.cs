using UnityEngine;

public class FaceTargetWhenTalk : MonoBehaviour
{
	public GameObject HeroineNormal;

	public GameObject HeroineDemon;

	public Transform defaultTarget;

	private void Start()
	{
		if (EndingHandler.triggerScientistEnding)
		{
			Object.Destroy(HeroineDemon);
		}
		if (EndingHandler.triggerTrueEnding)
		{
			Object.Destroy(HeroineNormal);
		}
	}

	private void FixedUpdate()
	{
		if (DialogManager.inDialogue)
		{
			FaceTarget();
		}
		else
		{
			FaceDefaultTarget();
		}
	}

	private void FaceTarget()
	{
		if (HeroineDemon != null || HeroineNormal != null)
		{
			if (HeroineDemon != null)
			{
				Vector3 normalized = (HeroineDemon.transform.position - base.transform.position).normalized;
				Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
			}
			if (HeroineNormal != null)
			{
				Vector3 normalized2 = (HeroineNormal.transform.position - base.transform.position).normalized;
				Quaternion b2 = Quaternion.LookRotation(new Vector3(normalized2.x, 0f, normalized2.z));
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, Time.deltaTime * 5f);
			}
		}
		else
		{
			Vector3 normalized3 = (PlayerManager.instance.player.transform.position - base.transform.position).normalized;
			Quaternion b3 = Quaternion.LookRotation(new Vector3(normalized3.x, 0f, normalized3.z));
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b3, Time.deltaTime * 5f);
		}
	}

	private void FaceDefaultTarget()
	{
		Vector3 normalized = (defaultTarget.position - base.transform.position).normalized;
		Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
	}
}
