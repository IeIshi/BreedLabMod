using UnityEngine;

public class ResetEverything : MonoBehaviour
{
	public static ResetEverything instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		resetEverything();
	}

	public void resetEverything()
	{
		InventoryUI.inventoryIsOpen = false;
		InventoryUI.heroineIsChased = false;
		Inventory.energyDrinkCount = 0;
		Inventory.lovePotionCount = 0;
		MainMenu.levelSkipped = false;
		EquipmentManager.pantsuOn = false;
		EquipmentManager.shoesOn = false;
		EquipmentManager.skirtOn = false;
		EquipmentManager.stockingsOn = false;
		EquipmentManager.chestOn = false;
		EquipmentManager.nekomimiOn = false;
		EquipmentManager.flashNeckOn = false;
		EquipmentManager.pantiesOn = false;
		EquipmentManager.antiTentOn = false;
		EquipmentManager.shoeDurability = 1f;
		EquipmentManager.skirtDurability = 1f;
		EquipmentManager.stockingsDurability = 1f;
		EquipmentManager.pantiesDurability = 1f;
		for (int i = 0; i < Inventory.itemSaveArray.Length; i++)
		{
			Inventory.itemSaveArray[i] = 0;
		}
		for (int j = 0; j < EquipmentManager.eqSaveArray.Length; j++)
		{
			EquipmentManager.eqSaveArray[j] = 0;
		}
		PlayerManager.KeyDoorChillArea = false;
		PlayerManager.enteredChallangeRoom = false;
		PlayerManager.ScSexAfterMath = false;
		PlayerManager.finishedDarkTunnelTwo = false;
		PlayerManager.MantisDown = false;
		PlayerManager.BroodmotherMet = false;
		PlayerManager.MasturbationFinished = false;
		PlayerManager.firstTimeEnterBotanicMain = false;
		PlayerManager.litaGaveKey = false;
		PlayerManager.spawnedFutaMounter = false;
		PlayerManager.spawnedFutaChaser = false;
		PlayerManager.loaded = false;
		PlayerManager.loadedrly = false;
		PlayerManager.id1 = false;
		PlayerManager.id11 = false;
		PlayerManager.id2 = false;
		PlayerManager.smallKey = false;
		PlayerManager.KeyB = false;
		PlayerManager.Office = false;
		PlayerManager.Key = false;
		PlayerManager.SAB = false;
		HeroineStats.creampied = false;
		HeroineStats.oralCreampie = false;
		HeroineStats.fartiged = false;
		HeroineStats.myLevel = 0;
		HeroineStats.currentPreg = 0f;
		HeroineStats.currentPower = 0f;
		HeroineStats.currentOrg = 0f;
		HeroineStats.currentLust = 0f;
		HeroineStats.currentStamina = 100f;
		HeroineStats.maxStamina = 100f;
		HeroineStats.buttonHeldDown = false;
		HeroineStats.calculated = false;
		HeroineStats.pregnant = false;
		HeroineStats.staminaLow = false;
		HeroineStats.lewd = 0;
		HeroineStats.lovePotion = false;
		HeroineStats.pregByHum = false;
		HeroineStats.HumanoidBuff = false;
		HeroineStats.MantisBuff = false;
		HeroineStats.orgasm = false;
		HeroineStats.immune = false;
		HeroineStats.debuffedStam = 0f;
		HeroineStats.stunned = false;
		HeroineStats.horny = false;
		HeroineStats.aroused = false;
		HeroineStats.wet = false;
		HeroineStats.edging = false;
		HeroineStats.masturbating = false;
		HeroineStats.GameOver = false;
		HeroineStats.birth = false;
		HeroineStats.fertileCum = false;
		HeroineStats.lustyCum = false;
		HeroineStats.addictiveCum = false;
		HeroineStats.hugeAmount = false;
		PlayerController.iFalled = false;
		PlayerController.iGetInserted = false;
		PlayerController.iGetFucked = false;
		PlayerController.heIsFuckingHard = false;
		PlayerController.claimed = false;
		PlayerController.iFalledBack = false;
		PlayerController.iFalledFront = false;
		PlayerController.gotHitFront = false;
		PlayerController.gotHitBack = false;
		PlayerController.gotGrabbedBack = false;
		PlayerController.gotGrabbedFront = false;
		PlayerController.walking = false;
		PlayerController.isAheago = false;
		PlayerController.isSilent = false;
		PlayerController.enemyToClose = false;
		ScientistGallery.scGallery = false;
		EnergyEngineBotanic.engineIsOn = false;
		EnergyEngine.engineIsOn = false;
		EnergyEngineOne.engineIsOn = false;
		MovementManipulator.mouthClaimed = false;
		MovementManipulator.assClaimed = false;
		MovementManipulator.vaginaClaimed = false;
		MovementManipulator.mouthInject = false;
		MovementManipulator.assInject = false;
		MovementManipulator.getsTeased = false;
		MovementManipulator.occupied = false;
		MovementManipulator.chasingWaspCount = 0;
		Gun.additionalAmmo = 0;
		MasturbationArea.mastArea = false;
		EnterBotanicAfterMF.AfterMindFleyer = false;
		EnterBotanicMain.enteringFromOutside = false;
		HeroineStats.corrupted = false;
		MainMenu.skippedToAct4 = false;
		MainMenu.skippedToAct4Two = false;
		MainMenu.NewGamePlus = false;
		MainMenu.levelSkipped = false;
	}
}
