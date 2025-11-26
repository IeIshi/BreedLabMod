using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltiip : MonoBehaviour
{
	public TextMeshProUGUI headerField;

	public TextMeshProUGUI contentField;

	public LayoutElement layoutElement;

	public int characterWrapLimit;

	public RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetText(string content, string header = "")
	{
		if (string.IsNullOrEmpty(header))
		{
			headerField.gameObject.SetActive(value: false);
		}
		else
		{
			headerField.gameObject.SetActive(value: true);
			headerField.text = header;
		}
		contentField.text = content;
		int length = headerField.text.Length;
		int length2 = contentField.text.Length;
		layoutElement.enabled = ((length > characterWrapLimit || length2 > characterWrapLimit) ? true : false);
	}
}
