using UnityEngine;
using UnityEngine.UI;

public class DisableRayScreenOnStart : MonoBehaviour
{
	public GameObject ImageHolder;

	public GameObject MindFleyer;

	public GameObject MindFleyerBody;

	private MindFleyerControl mindFleyerControl;

	private Image imageComponent;

	private Color targetColor;

	private bool x;

	private void Start()
	{
		imageComponent = ImageHolder.GetComponent<Image>();
		targetColor = imageComponent.color;
		ImageHolder.SetActive(value: false);
		mindFleyerControl = MindFleyer.GetComponent<MindFleyerControl>();
	}

	private void FixedUpdate()
	{
		if (mindFleyerControl.dead)
		{
			if (!x)
			{
				targetColor.a = 1f;
				x = true;
			}
			targetColor.a -= Time.deltaTime * 0.1f;
			imageComponent.color = targetColor;
			MindFleyerBody.SetActive(value: false);
			if (targetColor.a <= 0f)
			{
				ImageHolder.SetActive(value: false);
				MindFleyer.SetActive(value: false);
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
