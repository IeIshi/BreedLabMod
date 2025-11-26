using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
	public Text standUp;

	public Text explainWillpower;

	public Text powerBarisActive;

	public Text explainAusruf;

	public Image ausrufezeichen;

	public Text newItem;

	public Text newCloth;

	private float waitTime = 5f;

	private bool willpowerExplained;

	private bool ausrufExplained;

	private bool newItemExplained;

	private bool newClothExplaned;

	private void Start()
	{
		explainWillpower.enabled = false;
		explainAusruf.enabled = false;
		newItem.enabled = false;
		newCloth.enabled = false;
	}

	private void Update()
	{
		if (!PlayerController.iFalled)
		{
			standUp.enabled = false;
			powerBarisActive.enabled = false;
			if (!willpowerExplained)
			{
				explainWillpower.enabled = true;
			}
			if (explainWillpower.enabled)
			{
				StartCoroutine(RemoveWillpowerExplain(waitTime));
			}
		}
		if (Inventory.instance.items.Count > 0)
		{
			if (!newItemExplained)
			{
				newItem.enabled = true;
				newItemExplained = true;
			}
			StartCoroutine(RemoveNewItemExplain(waitTime));
		}
		if (Input.GetKeyDown(KeyCode.I) && Inventory.instance.items.Count > 0)
		{
			newItem.enabled = false;
			newCloth.enabled = true;
			newClothExplaned = true;
		}
		if (newClothExplaned)
		{
			StartCoroutine(RemoveNewClotheqExplain(waitTime));
		}
	}

	private IEnumerator RemoveWillpowerExplain(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		willpowerExplained = true;
		explainWillpower.enabled = false;
	}

	private IEnumerator RemoveNewItemExplain(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		newItem.enabled = false;
		newItemExplained = true;
	}

	private IEnumerator RemoveNewClotheqExplain(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		newCloth.enabled = false;
		newClothExplaned = true;
		GetComponent<Tutorials>().enabled = false;
	}
}
