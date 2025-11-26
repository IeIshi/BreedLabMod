using UnityEngine;

public class DamageTranslator : MonoBehaviour
{
	public bool Hugger;

	public bool Wolf;

	public bool Impreg;

	public bool Licker;

	public bool Scientist;

	public bool Lurker;

	public bool Phantom;

	public bool Mantis;

	public bool MayuPhan;

	public bool TentacleTrap;

	public bool Octus;

	public bool TantacleWallEnem;

	public bool PlantWalker;

	public bool Hound;

	public bool Wasp;

	public bool FlyTrap;

	public bool MindFleyer;

	public bool HumSc;

	public void RevieveDamage(float damage)
	{
		if (Hugger)
		{
			GetComponent<HuggerControl>().DrainStaminaInstant(damage);
			return;
		}
		if (Wolf)
		{
			GetComponent<HumanoidController>().drainStamina(damage);
			return;
		}
		if (Impreg)
		{
			GetComponent<ImpregInsectControl>().drainStamina(damage);
			return;
		}
		if (Licker)
		{
			GetComponent<NewLickerControl>().DrainStaminaInstant(damage);
			return;
		}
		if (Scientist)
		{
			GetComponent<ScientistNewControl>().DrainStaminaInstant(damage);
			return;
		}
		if (Lurker)
		{
			GetComponent<NewEnemControl>().DrainStaminaInstant(damage);
			return;
		}
		if (Phantom)
		{
			GetComponent<LewdPhantom>().DrainStaminaInstant(damage);
		}
		if (MayuPhan)
		{
			GetComponent<FutaMounter>().DrainStaminaInstant(damage);
		}
		if (TentacleTrap)
		{
			if (EquipmentManager.antiTentOn)
			{
				GetComponent<TentacleThrustEvent>().DrainStaminaInstant(damage * 2f);
				return;
			}
			GetComponent<TentacleThrustEvent>().DrainStaminaInstant(damage);
		}
		if (Mantis)
		{
			if (HeroineStats.myLevel < 3)
			{
				return;
			}
			GetComponent<MantisAi>().DrainStaminaInstant(damage);
		}
		if (Octus)
		{
			GetComponent<OctoControl>().DrainStaminaInstant(damage);
		}
		else if (TantacleWallEnem)
		{
			if (EquipmentManager.antiTentOn)
			{
				GetComponent<TentButtom>().DrainStaminaInstant(damage * 2f);
			}
			else
			{
				GetComponent<TentButtom>().DrainStaminaInstant(damage);
			}
		}
		else if (Hound)
		{
			GetComponent<HoundControl>().DrainStaminaInstant(damage);
		}
		else if (Wasp && GetComponent<WaspControl>().state != WaspControl.MyState.CUM)
		{
			GetComponent<WaspControl>().DrainStaminaInstant(damage);
		}
		else if (FlyTrap && GetComponent<FlyTrapControl>().grabbed)
		{
			GetComponent<FlyTrapControl>().DrainStaminaInstant(damage);
		}
		else if (MindFleyer)
		{
			GetComponent<MindFleyerControl>().DrainStaminaInstant(damage);
		}
		else if (HumSc)
		{
			GetComponent<ScXHeroine2Interact>().drainStamina(damage);
		}
	}
}
