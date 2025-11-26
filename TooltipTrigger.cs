using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public string content;

	public string header;

	public void OnPointerEnter(PointerEventData eventData)
	{
		TooltipSystem.Show(content, header);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		TooltipSystem.Hide();
	}
}
