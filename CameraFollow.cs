using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public float cameraMoveSpeed = 120f;

    public Transform shootingPos;

    private Transform camLaying;

    public GameObject cameraFollowObj;

    public GameObject cameraFollowOnFalled;

    public GameObject cameraFollowOnBackFalled;

    public GameObject cameraFollowHumBehind;

    public Image crossHair;

    private Vector3 followPos;

    public float clampAngle = 80f;

    public float clampShootAngle = 25f;

    public float mouseSensitivity = 5f;

    private float yaw;

    private float pitch;

    private float yawShoot;

    private float pitchShoot;

    private Vector3 currentRotation;

    private Vector3 adjCurrentRotation;

    private Vector3 currentShootRotation;

    private Vector3 rotationSmoothVelocity;

    public float rotationSmoothTime = 0.12f;

    public static Transform target;

    public GameObject heroinePos;

    public static bool shootingMode;

    public bool gunNotUsable;

    private float turnSmoothVelocity;

    private float currentHeroineRotation;

    private bool adjustRot;

    private bool adjustCam;

    private float step;

    private Vector3 rotData;

    private Animator animator;

    private bool adjustPos;

    public Transform savePosition;

    private bool posSaved;

    public Transform currentTarget;

    private void Start()
    {
        base.gameObject.transform.position = heroinePos.transform.position;
        animator = heroinePos.GetComponent<Animator>();
        yaw = 0f;
        pitch = 0f;
        target = cameraFollowObj.transform;
        gunNotUsable = false;
        if (PlayerPrefs.GetInt("MouseSens") == 0)
        {
            mouseSensitivity = 500f;
        }
        else
        {
            mouseSensitivity = PlayerPrefs.GetInt("MouseSens") * 100;
        }
        if (PlayerPrefs.GetInt("MouseSens") == 600)
        {
            Debug.Log("CHECK CHECK IF WORKS");
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void FixedUpdate()
    {
        currentTarget = target;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (PlayerController.iFalled)
            {
                shootingMode = false;
                crossHair.enabled = false;
                animator.SetBool("isImAnschlag", value: false);
            }
            else if (!(EquipmentManager.instance.currentEquipment[7] == null) && !PlayerController.enemyToClose && !InventoryUI.inventoryIsOpen && !DialogManager.inDialogue && !gunNotUsable)
            {
                if (!posSaved)
                {
                    savePosition.rotation = heroinePos.transform.rotation;
                    savePosition.position = heroinePos.transform.position;
                    posSaved = true;
                }
                shootingMode = true;
                adjustPos = true;
                animator.SetBool("isImAnschlag", value: true);
                crossHair.enabled = true;
            }
            return;
        }
        shootingMode = false;
        crossHair.enabled = false;
        animator.SetBool("isImAnschlag", value: false);
        if (!InventoryUI.inventoryIsOpen)
        {
            Rotation();
            if (adjustPos)
            {
                adjustPos = false;
            }
            if (posSaved)
            {
                heroinePos.transform.rotation = savePosition.rotation;
                heroinePos.transform.position = savePosition.position;
                posSaved = false;
            }
        }
    }

    private void Update()
    {
        if (shootingMode)
        {
            adjustRot = true;
            ShootingMode();
            return;
        }
        AdjustHeroineRot();
        CameraUpdater();
        adjustCam = false;
        adjustPos = false;
    }

    private void Rotation()
    {
        currentHeroineRotation = heroinePos.transform.eulerAngles.y;
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        if (!Input.GetMouseButton(2)) pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, 0f - clampAngle, clampAngle);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        base.transform.eulerAngles = currentRotation;
    }

    private void AdjustHeroineRot()
    {
        if (adjustRot)
        {
            heroinePos.transform.eulerAngles = new Vector3(1f, currentRotation.y, currentRotation.z);
            base.transform.eulerAngles = currentRotation;
            adjustRot = false;
        }
    }

    private float newTargetHeight = -999f;
    private void CameraUpdater()
    {
        step = cameraMoveSpeed * Time.deltaTime;

        if (Input.GetMouseButton(2))
        {
            newTargetHeight = target.position.y + Input.GetAxis("Mouse Y") * (mouseSensitivity / 50f) * Time.deltaTime;
        }

        if (target.position.y != newTargetHeight && newTargetHeight != -999f)
        {
            Vector3 newCamFollowObjectPos = new(target.position.x, newTargetHeight, target.position.z);
            target.position = Vector3.MoveTowards(target.position, newCamFollowObjectPos, step);
            newTargetHeight = -999f;
        }

        if (!PlayerController.iFalled)
        {
            target = cameraFollowObj.transform;
        }

        base.transform.position = Vector3.MoveTowards(base.transform.position, target.position, step);
    }

    private void ShootingMode()
    {
        target = shootingPos.transform;
        base.transform.position = target.position;
        TransCam();
    }

    private void TransCam()
    {
        if (!adjustCam)
        {
            adjCurrentRotation = new Vector3(0f + pitch, yaw + base.transform.eulerAngles.y);
            base.transform.eulerAngles = adjCurrentRotation;
            heroinePos.transform.eulerAngles = base.transform.eulerAngles;
            adjustCam = true;
        }
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, 0f - clampShootAngle, clampShootAngle);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        base.transform.eulerAngles = currentRotation;
        heroinePos.transform.eulerAngles = base.transform.eulerAngles;
    }
}
