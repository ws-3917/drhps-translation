using System.Collections;
using UnityEngine;

public class P_MovementBase : MonoBehaviour
{
    public float _moveSpeed = 240f;

    public float _runPercent = 1.5f;

    public Rigidbody2D _rb;

    public Vector2 _MoveDIR;

    public Animator _anim;

    public bool ismoving;

    public bool sprinting;

    public bool FinishedAutomatedWalking;

    public bool FreezeAnimation;

    public bool AnimationOverriden;

    public Vector2 CurrentPlayerRotation;

    [Header("Player Variables")]
    public bool InDarkworld;

    public bool AllowSprint = true;

    private bool previousDarkworldState;

    public RuntimeAnimatorController LightworldAnimationController;

    public RuntimeAnimatorController DarkworldAnimationController;

    private bool speedtimerstarted;

    public PlayerManager Manager;

    private Coroutine runTimerCoroutine;

    private void Awake()
    {
        _anim.keepAnimatorStateOnDisable = true;
    }

    private void Start()
    {
        ChangeKrisWorldState(InDarkworld);
    }

    private void Update()
    {
        ProcessInputs();
        if (Manager._PlayerState == PlayerManager.PlayerState.Game && !FreezeAnimation)
        {
            Animate();
        }
        if ((Manager._PlayerState == PlayerManager.PlayerState.Cutscene || Manager._PlayerState == PlayerManager.PlayerState.NoPlayerMovement) && !AnimationOverriden && !FreezeAnimation)
        {
            ResetAnimation();
        }
        if (previousDarkworldState != InDarkworld)
        {
            previousDarkworldState = InDarkworld;
            ChangeKrisWorldState(InDarkworld);
        }
        CurrentPlayerRotation = new Vector2(_anim.GetFloat("MOVEMENTX"), _anim.GetFloat("MOVEMENTY"));
    }

    public void WalkToPosition(Vector3 TargetPosition, float Speed)
    {
        StartCoroutine(WalkTo(TargetPosition, Speed));
    }

    private IEnumerator WalkTo(Vector3 TargetPosition, float Speed)
    {
        while ((TargetPosition - base.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, TargetPosition, Speed * Time.deltaTime);
            ismoving = true;
            yield return null;
        }
        FinishedAutomatedWalking = true;
        ismoving = false;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void ProcessInputs()
    {
        if (Manager._PlayerState == PlayerManager.PlayerState.Game)
        {
            _MoveDIR = PlayerInput.MovementInput;
        }
        if (SettingsManager.Instance.GetBoolSettingValue("AutoRun"))
        {
            sprinting = !Input.GetKey(PlayerInput.Instance.Key_Sprint);
        }
        else
        {
            sprinting = Input.GetKey(PlayerInput.Instance.Key_Sprint);
        }
        if (!AllowSprint)
        {
            sprinting = false;
        }
        if (sprinting)
        {
            if (!speedtimerstarted && ismoving)
            {
                speedtimerstarted = true;
                if (runTimerCoroutine != null)
                {
                    StopCoroutine(runTimerCoroutine);
                }
                runTimerCoroutine = StartCoroutine(RunTimer());
            }
        }
        else
        {
            speedtimerstarted = false;
            _runPercent = 1f;
            if (runTimerCoroutine != null)
            {
                StopCoroutine(runTimerCoroutine);
                runTimerCoroutine = null;
            }
        }
        if (!ismoving)
        {
            speedtimerstarted = false;
            _runPercent = 1f;
            if (runTimerCoroutine != null)
            {
                StopCoroutine(runTimerCoroutine);
                runTimerCoroutine = null;
            }
        }
        if (_rb.velocity.magnitude < 0.5f)
        {
            speedtimerstarted = false;
            _runPercent = 1f;
            if (runTimerCoroutine != null)
            {
                StopCoroutine(runTimerCoroutine);
                runTimerCoroutine = null;
            }
        }
    }

    private void MovePlayer()
    {
        if (Manager._PlayerState == PlayerManager.PlayerState.Game)
        {
            _rb.velocity = new Vector2(_MoveDIR.x * _moveSpeed * _runPercent * Time.fixedDeltaTime, _MoveDIR.y * _moveSpeed * _runPercent * Time.fixedDeltaTime);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void Animate()
    {
        if (_MoveDIR.magnitude > 0.1f || _MoveDIR.magnitude < -0.1f)
        {
            ismoving = true;
        }
        else
        {
            ismoving = false;
        }
        if (ismoving)
        {
            float horizontalInput = PlayerInput.GetHorizontalInput();
            float verticalInput = PlayerInput.GetVerticalInput();
            Vector2 normalized = new Vector2(horizontalInput, verticalInput).normalized;
            if (verticalInput != 0f && (horizontalInput == 0f || Mathf.Sign(horizontalInput) != Mathf.Sign(_anim.GetFloat("MOVEMENTX"))))
            {
                _anim.SetFloat("MOVEMENTX", 0f);
                _anim.SetFloat("MOVEMENTY", normalized.y);
            }
            if (horizontalInput != 0f && (verticalInput == 0f || Mathf.Sign(verticalInput) != Mathf.Sign(_anim.GetFloat("MOVEMENTY"))))
            {
                _anim.SetFloat("MOVEMENTX", normalized.x);
                _anim.SetFloat("MOVEMENTY", 0f);
            }
        }
        _anim.SetBool("MOVING", ismoving);
        _anim.SetBool("RUNNING", sprinting);
    }

    public void RotatePlayerAnim(Vector2 Rotation)
    {
        _anim.SetFloat("MOVEMENTX", Rotation.x);
        _anim.SetFloat("MOVEMENTY", Rotation.y);
    }

    public void RotatePlayerAnimTowardsPosition(Vector2 targetPosition)
    {
        Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
        Vector2 vector2 = targetPosition - vector;
        vector2.Normalize();
        _anim.SetFloat("MOVEMENTX", vector2.x);
        _anim.SetFloat("MOVEMENTY", vector2.y);
    }

    public void ChangeKrisWorldState(bool Darkworld)
    {
        float @float = _anim.GetFloat("MOVEMENTX");
        float float2 = _anim.GetFloat("MOVEMENTY");
        InDarkworld = Darkworld;
        if (InDarkworld)
        {
            _anim.runtimeAnimatorController = DarkworldAnimationController;
        }
        else
        {
            _anim.runtimeAnimatorController = LightworldAnimationController;
        }
        RotatePlayerAnim(new Vector2(@float, float2));
    }

    private void ResetAnimation()
    {
        _anim.SetBool("MOVING", value: false);
        _anim.SetBool("RUNNING", value: false);
    }

    private IEnumerator RunTimer()
    {
        _runPercent = 1.2f;
        yield return new WaitForSeconds(0.33f);
        _runPercent = 1.4f;
        yield return new WaitForSeconds(2f);
        _runPercent = 1.6f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        speedtimerstarted = false;
        _runPercent = 1f;
        StopCoroutine(RunTimer());
    }
}
