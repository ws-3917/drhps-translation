using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AprilFools_PlayerController : MonoBehaviour
{
    [Header("-= Movement =-")]
    public float walkingSpeed = 7.5f;

    public float runningSpeed = 11.5f;

    public float jumpSpeed = 8f;

    public float gravity = 20f;

    public Camera playerCamera;

    public float lookSpeed = 2f;

    public float lookXLimit = 45f;

    public int Health = 100;

    [Header("-= Shotgun =-")]
    [SerializeField]
    private Animator ShotgunBobber;

    public Animator ShotgunAnimator;

    [SerializeField]
    private bool CanFire;

    [SerializeField]
    private AudioClip SFX_Fire;

    [SerializeField]
    private AudioClip SFX_Reload;

    [SerializeField]
    private AudioClip SFX_Heal;

    [Header("-= UI =-")]
    [SerializeField]
    private Image DamageScreen;

    [SerializeField]
    private Image UIHP_Hundreds;

    [SerializeField]
    private Image UIHP_Tens;

    [SerializeField]
    private Image UIHP_Digits;

    [SerializeField]
    private Sprite[] UIHP_NumberSprites;

    [SerializeField]
    private Sprite NoSprite;

    private CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    private float rotationX;

    [HideInInspector]
    public bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 vector = base.transform.TransformDirection(Vector3.forward);
        Vector3 vector2 = base.transform.TransformDirection(Vector3.right);
        Vector2 vector3 = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        vector3.Normalize();
        float y = moveDirection.y;
        moveDirection = vector * vector3.x * walkingSpeed + vector2 * vector3.y * walkingSpeed;
        if (canMove)
        {
            moveDirection.y = y;
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }
        ShotgunBobber.speed = characterController.velocity.magnitude / 15f;
        if (canMove && !GonerMenu.Instance.GonerMenuOpen)
        {
            characterController.Move(moveDirection * Time.deltaTime);
            rotationX += (0f - Input.GetAxis("Mouse Y")) * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, 0f - lookXLimit, lookXLimit);
            base.transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        }
        if (canMove && CanFire && Input.GetMouseButton(0) && base.gameObject.activeSelf)
        {
            CanFire = false;
            if (base.gameObject.activeSelf)
            {
                StartCoroutine(FireShotgun());
            }
        }
    }

    private IEnumerator FireShotgun()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hitInfo, float.PositiveInfinity) && (bool)hitInfo.transform.GetComponent<AprilFools_Longshot>())
        {
            hitInfo.transform.GetComponent<AprilFools_Longshot>().DamageLongshot();
        }
        CanFire = false;
        CutsceneUtils.PlaySound(SFX_Fire, CutsceneUtils.DRH_MixerChannels.Effect, 0.5f, ShotgunAnimator.speed);
        ShotgunAnimator.Play("AF25_ShotgunFire", -1, 0f);
        yield return new WaitForSeconds(1.433f / ShotgunAnimator.speed);
        CanFire = true;
    }

    public void DamagePlayer(int Damage)
    {
        if (canMove)
        {
            if (Health - Damage <= 0)
            {
                Health = 9999;
                AprilFools_Manager.instance.PlayerGameOver();
                return;
            }
            DamageScreen.color = Color.red;
            CutsceneUtils.FadeOutUIImage(DamageScreen, 2f);
            Health -= Damage;
            Health = Mathf.Clamp(Health, 0, 100);
            UI_UpdateHPGraphics();
        }
    }

    public void UI_UpdateHPGraphics()
    {
        int num = Health / 100;
        int num2 = Health / 10 % 10;
        int num3 = Health % 10;
        UIHP_Hundreds.sprite = ((num > 0) ? UIHP_NumberSprites[num] : NoSprite);
        UIHP_Tens.sprite = ((num > 0 || num2 > 0) ? UIHP_NumberSprites[num2] : NoSprite);
        UIHP_Digits.sprite = UIHP_NumberSprites[num3];
        SetUISpriteSize(UIHP_Hundreds);
        SetUISpriteSize(UIHP_Tens);
        SetUISpriteSize(UIHP_Digits);
    }

    public void UI_DisplayHeal()
    {
        DamageScreen.color = Color.green;
        CutsceneUtils.PlaySound(SFX_Heal, CutsceneUtils.DRH_MixerChannels.Effect, 0.4f);
        CutsceneUtils.FadeOutUIImage(DamageScreen, 2f);
    }

    private void SetUISpriteSize(Image uiImage)
    {
        if (uiImage.sprite != null && uiImage.sprite != NoSprite)
        {
            RectTransform rectTransform = uiImage.rectTransform;
            rectTransform.sizeDelta = new Vector2(uiImage.sprite.rect.width * 4f, rectTransform.sizeDelta.y);
        }
    }
}
