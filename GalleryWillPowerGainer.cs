using UnityEngine;

public class GalleryWillPowerGainer : MonoBehaviour
{
	private void FixedUpdate()
	{
		if (HeroineStats.debuffedStam > 50f)
		{
			HeroineStats.debuffedStam = 0f;
			HeroineStats.currentStamina = HeroineStats.maxStamina;
		}
	}
}
