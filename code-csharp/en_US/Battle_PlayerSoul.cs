using System.Collections;
using UnityEngine;

public class Battle_PlayerSoul : MonoBehaviour
{
    public enum SoulType
    {
        Red = 0,
        Blue = 1,
        Yellow = 2
    }

    public enum SoulState
    {
        Disabled = 0,
        Active = 1,
        Cutscene = 2
    }

    [Header("- References -")]
    public Rigidbody2D SoulRigidbody;

    public Animator SoulAnimator;

    [SerializeField]
    private AudioSource _soulSource;

    [Space(5f)]
    [Header("- Settings -")]
    public SoulType CurrentSoulType;

    public SoulState CurrentSoulState;

    [SerializeField]
    private float DefaultMoveSpeed;

    [SerializeField]
    private float DefaultSlowMoveSpeed;

    private float CurrentSoulMoveSpeed;

    public bool DamageCooldown;

    private static Battle_PlayerSoul instance;

    private Vector2 _StoredMoveDIR;

    private bool HoldingSlowKey;

    public static Battle_PlayerSoul Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        base.gameObject.SetActive(value: false);
    }

    private void OnEnable()
    {
        DamageCooldown = false;
        instance.SoulAnimator.Play("Idle", 0, 0f);
    }

    private void Update()
    {
        ProcessInputs();
        if (CurrentSoulState == SoulState.Active)
        {
            MoveSoul();
        }
    }

    private void ProcessInputs()
    {
        float horizontalInput = PlayerInput.GetHorizontalInput();
        float verticalInput = PlayerInput.GetVerticalInput();
        _StoredMoveDIR = new Vector2(horizontalInput, verticalInput);
        HoldingSlowKey = Input.GetKey(PlayerInput.Instance.Key_Sprint);
        if (HoldingSlowKey)
        {
            CurrentSoulMoveSpeed = DefaultSlowMoveSpeed;
        }
        else
        {
            CurrentSoulMoveSpeed = DefaultMoveSpeed;
        }
    }

    private void MoveSoul()
    {
        if (CurrentSoulState == SoulState.Active)
        {
            SoulRigidbody.velocity = new Vector2(_StoredMoveDIR.normalized.x * CurrentSoulMoveSpeed * Time.fixedDeltaTime, _StoredMoveDIR.normalized.y * CurrentSoulMoveSpeed * Time.fixedDeltaTime);
        }
    }

    public static void OnSoulHit(Bullet_Generic bullet)
    {
    }

    public static void TakeDamage(Bullet_Generic bullet = null, float amount = 0f)
    {
        if (Instance != null && !instance.DamageCooldown)
        {
            OnSoulHit(bullet);
            instance.StartCoroutine("DamageCooldownFrames");
            instance.SoulAnimator.Play("Hurt", 0, 0f);
            instance._soulSource.PlayOneShot(BattleSystem.Instance.SFX_soul_damage);
        }
    }

    private IEnumerator DamageCooldownFrames()
    {
        DamageCooldown = true;
        yield return new WaitForSeconds(1.5f);
        DamageCooldown = false;
    }
}
