using UnityEngine;

public class Interactable : MonoBehaviour
{
	public float radius = 2f;

	private bool isFocus;

	private bool hasInteracted;

	private Transform player;

	public Transform interactionTransform;

	public virtual void Interact()
	{
	}

	private void Update()
	{
		if (!isFocus || hasInteracted)
		{
			return;
		}
		if (Vector3.Distance(player.position, interactionTransform.position) <= radius)
		{
			if (!CameraFollow.shootingMode)
			{
				Interact();
				hasInteracted = true;
			}
		}
		else
		{
			OnDefocused();
		}
	}

	public void OnFocused(Transform playerTransform)
	{
		isFocus = true;
		player = playerTransform;
		hasInteracted = false;
	}

	public void OnDefocused()
	{
		isFocus = false;
		player = null;
		hasInteracted = false;
	}

	private void OnDrawGizmosSelected()
	{
		if (interactionTransform == null)
		{
			interactionTransform = base.transform;
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}
}
