using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;

    public float runSpeed = 3.5f;

    public float gravity = -12f;

    public float jumpHeight = 1f;

    [Range(0f, 1f)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;

    private float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;

    private float speedSmoothVelocity;

    private float currentSpeed;

    private float velocityY;

    public static bool iFalled;

    public static bool iGetInserted;

    public static bool iGetFucked;

    public static bool heIsFuckingHard;

    public static bool claimed;

    public static bool iFalledBack;

    public static bool iFalledFront;

    public static bool gotHitFront;

    public static bool gotHitBack;

    public static bool gotGrabbedBack;

    public static bool gotGrabbedFront;

    public static Animator animator;

    private Transform cameraT;

    private Camera cam;

    public Dialogue dialogue;

    public Interactable focus;

    public Transform lookingAt;

    public static CharacterController controller;

    public Transform showHeroCam;

    private AudioSource footStep;

    private AudioSource footStepShoes;

    private AudioSource footStepSquischy;

    private AudioSource fall;

    public static bool soundplayed;

    private bool timer;

    private bool decleared;

    private float time;

    private float randomTime;

    public static bool walking;

    public static bool standing;

    public static bool isAheago;

    private Scene currentScene;

    private string sceneName;

    public static bool isSilent;

    public GameObject dm;

    public static bool enemyToClose;

    public LayerMask enemyMask;

    private float test;

    private GameObject gameManager;

    private Vector2 input;

    private Vector2 inputDir;

    private bool resetCamera;

    private void Start()
    {
        if (PlayerManager.loadedrly)
        {
            Vector3 position = new Vector3(Inventory.instance.heroineLocation[0], Inventory.instance.heroineLocation[1], Inventory.instance.heroineLocation[2]);
            Vector3 eulerAngles = new Vector3(0f, Inventory.instance.heroineLocation[3], 0f);
            base.transform.position = position;
            base.transform.eulerAngles = eulerAngles;
        }
        gameManager = GameObject.Find("Game Manager");
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        iFalledBack = false;
        iFalledFront = false;
        gotHitFront = false;
        gotHitBack = false;
        claimed = false;
        iGetInserted = false;
        iGetFucked = false;
        heIsFuckingHard = false;
        isAheago = false;
        footStep = GameObject.Find("FootstepSFX").GetComponent<AudioSource>();
        footStepShoes = GameObject.Find("FootstepShoes").GetComponent<AudioSource>();
        fall = GameObject.Find("FallSFX").GetComponent<AudioSource>();
        footStepSquischy = GameObject.Find("Heroine/FootstepSquish").GetComponent<AudioSource>();
        if (sceneName == "Outdoor")
        {
            iFalled = true;
            PlayerManager.IsVirgin = true;
            if (!PlayerManager.SAB)
            {
                animator.SetBool("isSitting", value: true);
            }
            else
            {
                animator.SetBool("originMasturbate", value: true);
            }
        }
        controller.enabled = true;
    }

    private void Update()
    {
        lookingAt = FieldOfView.target;
        Interact();
        Controlls();
        if (walking || standing)
        {
            isSilent = true;
        }
        else
        {
            isSilent = false;
        }
        if (InventoryUI.inventoryIsOpen || DialogManager.inDialogue)
        {
            animator.SetFloat("speedPercent", 0f, speedSmoothTime, Time.deltaTime);
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        MyState();
        if (!isAheago)
        {
            Blink();
        }
        if (InventoryUI.heroineIsChased)
        {
            if (Physics.CheckSphere(base.transform.position, 2f, enemyMask))
            {
                enemyToClose = true;
            }
            else
            {
                enemyToClose = false;
            }
        }
        else
        {
            enemyToClose = false;
        }
    }

    private void Controlls()
    {
        if (iFalled)
        {
            currentSpeed = 0f;
            if (InventoryUI.inventoryIsOpen)
            {
                cam.transform.position = base.transform.position;
                CameraFollow.target = base.gameObject.transform;
            }
            return;
        }
        if (InventoryUI.inventoryIsOpen)
        {
            resetCamera = true;
            cam.transform.rotation = showHeroCam.rotation;
            cam.transform.position = Vector3.Lerp(cam.transform.position, showHeroCam.position, Time.deltaTime * 5f);
            return;
        }
        if (resetCamera)
        {
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            resetCamera = false;
        }
        if (DialogManager.inDialogue)
        {
            currentSpeed = 0f;
            velocityY = 0f;
            return;
        }
        Vector2 normalized = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (InventoryUI.heroineIsChased || InventoryUI.inventoryIsOpen || DialogManager.inDialogue || iGetFucked || iFalled || HeroineStats.GameOver || !MasturbationArea.mastArea || LarveBirth.larveImpregnated)
            {
                return;
            }
            animator.SetBool("mastStanding", value: true);
            HeroineStats.masturbating = true;
            iFalled = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walking = true;
        }
        else if (!HeroineStats.hugeAmount)
        {
            walking = false;
        }
        float num = (walking ? (currentSpeed / walkSpeed * 0.5f) : (currentSpeed / runSpeed));
        animator.SetFloat("speedPercent", num, speedSmoothTime, Time.deltaTime);
        if (!CameraFollow.shootingMode && !Gun.isReloading)
        {
            Move(normalized, walking);
            if (num == 0f)
            {
                if (PlayerManager.SAB || HeroineStats.corrupted)
                {
                    return;
                }
                if (!iFalled && EquipmentManager.heroineIsNaked)
                {
                    if (Flashlight.FlashOn)
                    {
                        animator.SetBool("isNaked", value: false);
                        return;
                    }
                    if (HeroineStats.currentLust > 40f)
                    {
                        animator.SetBool("isNaked", value: false);
                        return;
                    }
                    animator.SetBool("isNaked", value: true);
                }
            }
            else
            {
                animator.SetBool("isNaked", value: false);
            }
        }
        if (num == 0f)
        {
            standing = true;
        }
        else
        {
            standing = false;
        }
    }

    private void Interact()
    {
        if (iFalled || InventoryUI.heroineIsChased)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogManager.inDialogue)
            {
                dm.GetComponent<DialogManager>().DisplayNextSentence();
            }
            else if (FieldOfView.target != null)
            {
                Interactable component = FieldOfView.target.GetComponent<Interactable>();
                if (component != null)
                {
                    SetFocus(component);
                }
            }
        }
        if (FieldOfView.target == null)
        {
            RemoveFocus();
        }
    }

    public void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
        }
        newFocus.OnFocused(base.transform);
    }

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    private void Move(Vector2 inputDir, bool walking)
    {
        if (inputDir != Vector2.zero)
        {
            float target = Mathf.Atan2(inputDir.x, inputDir.y) * 57.29578f + cameraT.eulerAngles.y;
            base.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(base.transform.eulerAngles.y, target, ref turnSmoothVelocity, turnSmoothTime);
        }
        float num = runSpeed + runSpeed * (PassiveStats.instance.speed.GetValue() / 100f);
        float target2 = (walking ? walkSpeed : num) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, target2, ref speedSmoothVelocity, speedSmoothTime);
        velocityY += Time.deltaTime * gravity;
        Vector3 vector = base.transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(vector * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        velocityY = 0f;
    }

    private void MyState()
    {
        if (HeroineStats.orgasm || heIsFuckingHard)
        {
            animator.SetBool("isAhegao", value: true);
            isAheago = true;
        }
        else
        {
            animator.SetBool("isAhegao", value: false);
            isAheago = false;
        }
        if (!iFalled)
        {
            return;
        }
        if (gotHitFront)
        {
            animator.SetBool("falled", value: true);
            iFalledFront = true;
            iFalledBack = false;
            if (!soundplayed)
            {
                fall.Play();
                soundplayed = true;
            }
            gotHitFront = false;
        }
        if (gotHitBack)
        {
            animator.SetBool("isFalledBack", value: true);
            iFalledFront = false;
            iFalledBack = true;
            if (!soundplayed)
            {
                fall.Play();
                soundplayed = true;
            }
            gotHitBack = false;
        }
        if (iGetFucked && HeroineStats.aroused)
        {
            animator.SetBool("isScared", value: false);
        }
        if (!iGetFucked)
        {
            animator.SetBool("isFucking", value: false);
            animator.SetBool("isFuckingBehind", value: false);
            if (iFalledBack)
            {
                animator.SetBool("isFalledBack", value: true);
            }
        }
    }

    private void Blink()
    {
        if (!timer)
        {
            if (!decleared)
            {
                randomTime = Random.Range(0f, 5f);
                decleared = true;
            }
            time += Time.deltaTime;
            if (time >= randomTime)
            {
                timer = true;
                animator.SetBool("isBlinking", value: true);
                randomTime = 0f;
                time = 0f;
                decleared = false;
            }
        }
        if (timer)
        {
            time += Time.deltaTime;
            if (time >= 1f)
            {
                animator.SetBool("isBlinking", value: false);
                timer = false;
                time = 0f;
            }
        }
    }

    public void TriggerDialogue()
    {
        Object.FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    private void Step()
    {
        if (Physics.Raycast(base.transform.position, -Vector3.up, out var hitInfo, 10f) && hitInfo.transform.tag == "Respawn")
        {
            footStepSquischy.Play();
            if (EquipmentManager.shoesOn && !PlayerManager.infCloth)
            {
                EquipmentManager.shoeDurability -= 0.0012f;
                HeroineStats.shoesDurSlider.fillAmount = EquipmentManager.shoeDurability;
            }
        }
        else
        {
            if (!(currentSpeed > 2.5f))
            {
                return;
            }
            if (EquipmentManager.shoesOn)
            {
                footStepShoes.Play();
                if (!PlayerManager.infCloth)
                {
                    EquipmentManager.shoeDurability -= 0.0015f;
                    HeroineStats.shoesDurSlider.fillAmount = EquipmentManager.shoeDurability;
                }
                if (EquipmentManager.shoeDurability <= 0f)
                {
                    gameManager.GetComponent<EquipmentManager>().RipOff(5);
                }
            }
            else
            {
                footStep.Play();
            }
        }
    }
}
