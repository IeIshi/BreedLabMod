// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Mods
using System.Collections;
using UnityEngine;

public class Mods : MonoBehaviour
{
    public class Sizes
    {
        public enum ButtonType
        {
            Full,
            Bool,
            Plus,
            Minus,
            Descr,
            BackGround
        }

        public static int ScreenWidth;

        public static int ScreenHeight;

        public static float ButtonWidht;

        public static float ButtonHeight;

        public static float ButtonYdist;

        public static float ButtonXdist;

        public static int offsetX;

        public static int offsetY;

        public static void SetSizes()
        {
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;
            ButtonWidht = ScreenWidth / 10;
            ButtonHeight = ScreenHeight / 40;
            ButtonXdist = ButtonWidht / 30f;
            ButtonYdist = ButtonHeight / 5f;
            offsetX = 10;
            offsetY = 10;
        }
    }

    public class Textures
    {
        public static Texture2D btnpresstexture;

        public static Texture2D btntexture;

        public static Texture2D onpresstexture;

        public static Texture2D ontexture;

        public static Texture2D offpresstexture;

        public static Texture2D offtexture;

        public static Texture2D backtexture;

        public static GUIStyle styleButton;

        public static GUIStyle styleTitle;

        public static Texture2D titletexture;

        public static Texture2D NewTexture2D => new Texture2D(1, 1);

        public static Texture2D BtnTexture
        {
            get
            {
                if (btntexture == null)
                {
                    btntexture = NewTexture2D;
                    btntexture.SetPixel(0, 0, Color.black);
                    btntexture.Apply();
                }
                return btntexture;
            }
        }

        public static Texture2D BtnPressTexture
        {
            get
            {
                if (btnpresstexture == null)
                {
                    btnpresstexture = NewTexture2D;
                    btnpresstexture.SetPixel(0, 0, new Color32(130, 130, 130, byte.MaxValue));
                    btnpresstexture.Apply();
                }
                return btnpresstexture;
            }
        }

        public static Texture2D OnPressTexture
        {
            get
            {
                if (onpresstexture == null)
                {
                    onpresstexture = NewTexture2D;
                    onpresstexture.SetPixel(0, 0, new Color32(0, 150, 0, byte.MaxValue));
                    onpresstexture.Apply();
                }
                return onpresstexture;
            }
        }

        public static Texture2D OnTexture
        {
            get
            {
                if (ontexture == null)
                {
                    ontexture = NewTexture2D;
                    ontexture.SetPixel(0, 0, new Color32(0, 100, 0, byte.MaxValue));
                    ontexture.Apply();
                }
                return ontexture;
            }
        }

        public static Texture2D OffPressTexture
        {
            get
            {
                if (offpresstexture == null)
                {
                    offpresstexture = NewTexture2D;
                    offpresstexture.SetPixel(0, 0, Color.green);
                    offpresstexture.Apply();
                }
                return offpresstexture;
            }
        }

        public static Texture2D OffTexture
        {
            get
            {
                if (offtexture == null)
                {
                    offtexture = NewTexture2D;
                    offtexture.SetPixel(0, 0, new Color32(0, 0, 0, 200));
                    offtexture.Apply();
                }
                return offtexture;
            }
        }

        public static Texture2D BackTexture
        {
            get
            {
                if (backtexture == null)
                {
                    backtexture = NewTexture2D;
                    backtexture.SetPixel(0, 0, new Color32(42, 42, 42, 200));
                    backtexture.Apply();
                }
                return backtexture;
            }
        }

        public static Texture2D TitleTexture
        {
            get
            {
                if (titletexture == null)
                {
                    titletexture = NewTexture2D;
                    titletexture.SetPixel(0, 0, new Color32(50, 0, 0, 150));
                    titletexture.Apply();
                }
                return titletexture;
            }
        }

        public static void InitStyles()
        {
            if (styleButton == null)
            {
                styleButton = new GUIStyle();
                styleButton.normal.background = OffTexture;
                styleButton.hover.background = OnTexture;
                styleButton.active.background = OffPressTexture;
                styleButton.onActive.background = null;
                styleButton.normal.textColor = Color.white;
                styleButton.onNormal.textColor = Color.white;
                styleButton.active.textColor = Color.white;
                styleButton.onActive.textColor = Color.white;
                styleButton.fontSize = 16;
                styleButton.fontStyle = FontStyle.Normal;
                styleButton.alignment = TextAnchor.MiddleCenter;
            }
            if (styleTitle == null)
            {
                styleTitle = new GUIStyle();
                styleTitle.normal.background = TitleTexture;
                styleTitle.hover.background = TitleTexture;
                styleTitle.active.background = TitleTexture;
                styleTitle.onActive.background = null;
                styleTitle.normal.textColor = Color.white;
                styleTitle.onNormal.textColor = Color.white;
                styleTitle.active.textColor = Color.white;
                styleTitle.onActive.textColor = Color.white;
                styleTitle.fontSize = 18;
                styleTitle.fontStyle = FontStyle.Normal;
                styleTitle.alignment = TextAnchor.MiddleCenter;
            }
        }
    }

    public static bool ModShow;

    public static Mods instance;

    private int UILine;

    private int UIColumn;

    private int UILines;

    private int UIColumns;

    private string UIDebugText;

    private Queue UIDebugQueue;

    private int UIDebugMaxLines;

    private bool UIDebugVisible;

    private string objectName;

    private bool GameInfStamina;

    private bool GameImmunity;

    private bool GameInfAmmo;

    private bool GameNoDetection;

    private Gun GameCurrentWeapon;

    private HeroineStats GameHeroineStats;

    private CharacterStats GameCharacterStats;

    private PlayerController GamePlayerController;

    private Inventory GameHeroineInventory;

    private EquipmentManager GameEquipmentManager;

    private bool UICreateItemVisible;

    private bool setTimeScaleBack = false;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 10f, 10f), ""))
        {
            ModShow = !ModShow;
        }
        if (ModShow)
        {
            Time.timeScale = 0f;
            setTimeScaleBack = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIMain();
        }
        else if (setTimeScaleBack)
        {
            Time.timeScale = 1f;
            setTimeScaleBack = false;
        }
    }

    public void UIUtils()
    {
        if (Physics.Raycast(Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out var hitInfo))
        {
            Transform transform = hitInfo.transform;
            objectName = transform.gameObject.name;
        }
        GUI.Label(new Rect(Screen.width - 150, 10f, 150f, 25f), objectName);
    }

    private void UIMain()
    {
        if (ModShow)
        {
            ResetUIPosition();
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "NoxTek cheat menu V3", Textures.styleTitle);
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Walk Speed:" + GamePlayerController.walkSpeed, Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Plus), "+", Textures.styleButton))
            {
                GamePlayerController.walkSpeed += 1f;
            }
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Minus), "-", Textures.styleButton))
            {
                GamePlayerController.walkSpeed -= 1f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Run Speed:" + GamePlayerController.runSpeed, Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Plus), "+", Textures.styleButton))
            {
                GamePlayerController.runSpeed += 1f;
            }
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Minus), "-", Textures.styleButton))
            {
                GamePlayerController.runSpeed -= 1f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Infinite Ammo", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), GameInfAmmo.ToString(), Textures.styleButton))
            {
                GameInfAmmo = !GameInfAmmo;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Infinite Stamina", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), GameInfStamina.ToString(), Textures.styleButton))
            {
                GameInfStamina = !GameInfStamina;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "No detection", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), GameNoDetection.ToString(), Textures.styleButton))
            {
                GameNoDetection = !GameNoDetection;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Level Up", Textures.styleButton))
            {
                GameHeroineStats.GainExp(100000f);
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Fertile Cum", Textures.styleButton))
            {
                HeroineStats.fertileCum = true;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Birting", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "Birth", Textures.styleButton))
            {
                HeroineStats.pregnant = true;
                HeroineStats.currentPreg = 99f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Pregnancy", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "Abort", Textures.styleButton))
            {
                HeroineStats.pregnant = false;
                HeroineStats.currentPreg = 0f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Flashlight", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), EquipmentManager.flashNeckOn.ToString(), Textures.styleButton))
            {
                EquipmentManager.flashNeckOn = !EquipmentManager.flashNeckOn;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Potions", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "+1", Textures.styleButton))
            {
                Inventory.energyDrinkCount++;
                Inventory.lovePotionCount++;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Lust", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "+5", Textures.styleButton))
            {
                HeroineStats.currentLust += 5f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Lust", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "Clear", Textures.styleButton))
            {
                HeroineStats.currentLust = 0f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Orgasm", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "+5", Textures.styleButton))
            {
                HeroineStats.currentOrg += 5f;
            }
            UILine++;
            GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Descr), "Orgasm", Textures.styleButton);
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Bool), "Clear", Textures.styleButton))
            {
                HeroineStats.currentOrg = 0f;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Addictive Cum", Textures.styleButton))
            {
                HeroineStats.addictiveCum = true;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Aphrosidiac Cum", Textures.styleButton))
            {
                HeroineStats.lustyCum = true;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Clear Debuffs", Textures.styleButton))
            {
                HeroineStats.lustyCum = false;
                HeroineStats.addictiveCum = false;
                HeroineStats.fertileCum = false;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Wolf Buff", Textures.styleButton))
            {
                HeroineStats.HumanoidBuff = true;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Mantis Buff", Textures.styleButton))
            {
                HeroineStats.MantisBuff = true;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Clean Inventory", Textures.styleButton))
            {
                GameHeroineInventory.items.Clear();
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Create Item >>>", Textures.styleButton))
            {
                UICreateItemVisible = !UICreateItemVisible;
            }
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Debug Menu >>>", Textures.styleButton))
            {
                UIDebugVisible = !UIDebugVisible;
            }
            UILine++;
            UIMenuCreateItem(UICreateItemVisible);
            UIMenuDebug(UIDebugVisible);
        }
    }

    private void InitVariables()
    {
        if (GameHeroineStats == null)
        {
            GameHeroineStats = Object.FindObjectOfType<HeroineStats>();
            UIDebugLog("Heroine Stats Found:" + (GameHeroineStats != null));
        }
        if (GamePlayerController == null)
        {
            GamePlayerController = Object.FindObjectOfType<PlayerController>();
            UIDebugLog("Player Controller:" + (GamePlayerController != null));
        }
        if (GameHeroineInventory == null)
        {
            GameHeroineInventory = Inventory.instance;
            UIDebugLog("Inventory Found:" + (GameHeroineInventory != null));
        }
        if (GameCurrentWeapon == null)
        {
            GameCurrentWeapon = Object.FindObjectOfType<Gun>();
            UIDebugLog("Weapon Found:" + (GameCurrentWeapon != null));
        }
        if (GameEquipmentManager == null)
        {
            GameEquipmentManager = Object.FindObjectOfType<EquipmentManager>();
            UIDebugLog("EquipmentManager Found:" + (GameEquipmentManager != null));
        }
    }

    private void HandleKeys()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ModShow = !ModShow;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerController.iFalled = true;
            PlayerController.gotHitBack = true;
            PlayerController.gotHitFront = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerController.iFalled = true;
            PlayerController.gotHitBack = false;
            PlayerController.gotHitFront = true;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameEquipmentManager.Unequip(7);
        }
    }

    private void Effects()
    {
        if (GameInfStamina)
        {
            HeroineStats.debuffedStam = 0f;
            HeroineStats.currentStamina = HeroineStats.maxStamina;
        }
        if (GameImmunity)
        {
            HeroineStats.immune = GameImmunity;
        }
        if (GameInfAmmo)
        {
            GameCurrentWeapon.weaponRechargeRate = 0.1f;
            GameCurrentWeapon.fireRate = 10000f;
        }
        else
        {
            GameCurrentWeapon.weaponRechargeRate = 10f;
            GameCurrentWeapon.fireRate = 15f;
        }
        if (GameNoDetection)
        {
            EnemyFieldOfView[] array = Object.FindObjectsOfType<EnemyFieldOfView>();
            for (int i = 0; i < array.Length; i++)
            {
                array[i].heroineIsVisible = false;
                array[i].viewRadius = 0f;
            }
        }
    }

    private void Update()
    {
        InitVariables();
        HandleKeys();
        Effects();
    }

    public static void CreateInstance()
    {
        Mods mods = Object.FindObjectOfType<Mods>();
        if (mods == null)
        {
            mods = new GameObject("Mods").AddComponent<Mods>();
        }
        Object.DontDestroyOnLoad(mods);
        instance = mods;
    }

    private void Awake()
    {
        Sizes.SetSizes();
        Textures.InitStyles();
        UIDebugMaxLines = 13;
        UIDebugQueue = new Queue();
    }

    private void ResetUIPosition()
    {
        UILine = 0;
        UIColumn = 0;
    }

    private void Start()
    {
    }

    private Rect CalcPosition(int UILine, int UIColumn, Sizes.ButtonType buttonType)
    {
        return buttonType switch
        {
            Sizes.ButtonType.Full => new Rect((float)Sizes.offsetX + (float)this.UIColumn * Sizes.ButtonWidht + (float)this.UIColumn * Sizes.ButtonXdist, (float)Sizes.offsetY + (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist, Sizes.ButtonWidht, Sizes.ButtonHeight),
            Sizes.ButtonType.Bool => new Rect((float)Sizes.offsetX + (float)this.UIColumn * Sizes.ButtonWidht + (float)this.UIColumn * Sizes.ButtonXdist + Sizes.ButtonWidht / 3f * 2f, (float)Sizes.offsetY + (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist, Sizes.ButtonWidht / 3f, Sizes.ButtonHeight),
            Sizes.ButtonType.Plus => new Rect((float)Sizes.offsetX + (float)this.UIColumn * Sizes.ButtonWidht + (float)this.UIColumn * Sizes.ButtonXdist + Sizes.ButtonWidht / 6f * 5f, (float)Sizes.offsetY + (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist, Sizes.ButtonWidht / 6f, Sizes.ButtonHeight),
            Sizes.ButtonType.Minus => new Rect((float)Sizes.offsetX + (float)this.UIColumn * Sizes.ButtonWidht + (float)this.UIColumn * Sizes.ButtonXdist + Sizes.ButtonWidht / 6f * 4f, (float)Sizes.offsetY + (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist, Sizes.ButtonWidht / 6f, Sizes.ButtonHeight),
            Sizes.ButtonType.Descr => new Rect((float)Sizes.offsetX + (float)this.UIColumn * Sizes.ButtonWidht + (float)this.UIColumn * Sizes.ButtonXdist, (float)Sizes.offsetY + (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist, Sizes.ButtonWidht / 3f * 2f, Sizes.ButtonHeight),
            Sizes.ButtonType.BackGround => new Rect(0f, 0f, Sizes.ButtonWidht * (float)this.UIColumn + (float)this.UIColumn * Sizes.ButtonXdist, (float)this.UILine * Sizes.ButtonHeight + (float)this.UILine * Sizes.ButtonYdist),
            _ => new Rect(0f, 0f, 0f, 0f),
        };
    }

    private void UIMenuDebug(bool visible)
    {
        if (visible)
        {
            GUI.Label(new Rect(Screen.width - 410, 10f, 400f, 200f), UIDebugText, GUI.skin.textArea);
            UIUtils();
        }
    }

    public void UIDebugLog(string message)
    {
        if (UIDebugQueue.Count >= UIDebugMaxLines)
        {
            UIDebugQueue.Dequeue();
        }
        UIDebugQueue.Enqueue(message);
        UIDebugText = "";
        foreach (string item in UIDebugQueue)
        {
            UIDebugText = UIDebugText + item + "\n";
        }
    }

    private void UIMenuCreateItem(bool visible)
    {
        if (!visible)
        {
            return;
        }
        UILine = 0;
        UIColumn++;
        GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Items", Textures.styleButton);
        for (int i = 0; i < 20; i++)
        {
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), GameHeroineInventory.itemList[i].name, Textures.styleButton))
            {
                GameHeroineInventory.Add(GameHeroineInventory.itemList[i]);
            }
        }
        UILine = 0;
        UIColumn++;
        GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), "Items", Textures.styleButton);
        for (int j = 20; j < GameHeroineInventory.itemList.Count; j++)
        {
            UILine++;
            if (GUI.Button(CalcPosition(UILine, UIColumn, Sizes.ButtonType.Full), GameHeroineInventory.itemList[j].name, Textures.styleButton))
            {
                GameHeroineInventory.Add(GameHeroineInventory.itemList[j]);
            }
        }
    }
}
