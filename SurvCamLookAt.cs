using UnityEngine;

public class SurvCamLookAt : MonoBehaviour
{
	public Transform target;

	public float damping;

	private void LateUpdate()
	{
		Quaternion b = Quaternion.LookRotation(target.position - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * damping);
	}
}
