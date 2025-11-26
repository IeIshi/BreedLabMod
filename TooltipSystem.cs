using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
	private static TooltipSystem current;

	public Tooltiip tooltiip;

	public void Awake()
	{
		current = this;
	}

	public static void Show(string content, string header = "")
	{
		current.tooltiip.SetText(content, header);
		current.tooltiip.gameObject.SetActive(value: true);
	}

	public static void Hide()
	{
		current.tooltiip.gameObject.SetActive(value: false);
	}
}
