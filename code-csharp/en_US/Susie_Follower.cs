using System.Collections.Generic;
using UnityEngine;

public class Susie_Follower : MonoBehaviour
{
    public enum MemberFollowerStates
    {
        Disabled = 0,
        SettingUpPosition = 1,
        CopyingInputs = 2
    }

    public bool FollowingEnabled;

    public Animator SusieAnimator;

    public bool AnimationOverriden;

    public MemberFollowerStates currentState;

    [Header("1 = Susie | 1.125 = Ralsei")]
    public float delay = 0.5f;

    public Queue<Vector3> positions;

    private int frameDelay;

    public Queue<Vector2> rotations;

    private Vector3 lastPlayerPosition;

    private Vector3 lastPosition;

    private Vector3 velocity;

    private void Start()
    {
        positions = new Queue<Vector3>();
        rotations = new Queue<Vector2>();
        delay += 0.125f;
        lastPlayerPosition = PlayerManager.Instance.transform.position;
        lastPosition = base.transform.position;
        if (FollowingEnabled)
        {
            currentState = MemberFollowerStates.CopyingInputs;
        }
        else
        {
            currentState = MemberFollowerStates.SettingUpPosition;
        }
    }

    private void Update()
    {
        frameDelay = Mathf.CeilToInt(delay * 30f);
        if (!AnimationOverriden)
        {
            AnimatePartyMember();
        }
        if (FollowingEnabled && currentState == MemberFollowerStates.SettingUpPosition)
        {
            if (Vector2.Distance(base.transform.position, PlayerManager.Instance.transform.position) > 0.3f)
            {
                base.transform.position = Vector3.MoveTowards(base.transform.position, PlayerManager.Instance.transform.position, 18f * Time.deltaTime);
            }
            else
            {
                currentState = MemberFollowerStates.CopyingInputs;
            }
        }
        if (!FollowingEnabled)
        {
            currentState = MemberFollowerStates.SettingUpPosition;
        }
    }

    public void AdjustForCutscene(bool InCutscene)
    {
        if (InCutscene)
        {
            SusieAnimator.SetBool("InCutscene", value: true);
            AnimationOverriden = true;
            currentState = MemberFollowerStates.Disabled;
            positions.Clear();
            rotations.Clear();
        }
        else
        {
            SusieAnimator.SetBool("InCutscene", value: false);
            AnimationOverriden = false;
            currentState = MemberFollowerStates.SettingUpPosition;
        }
    }

    private void FixedUpdate()
    {
        if (FollowingEnabled && currentState == MemberFollowerStates.CopyingInputs && FollowingEnabled)
        {
            if (PlayerManager.Instance.transform.position != lastPlayerPosition)
            {
                positions.Enqueue(PlayerManager.Instance.transform.position);
                lastPlayerPosition = PlayerManager.Instance.transform.position;
                rotations.Enqueue(PlayerManager.Instance._PMove.CurrentPlayerRotation);
            }
            if (positions.Count > frameDelay)
            {
                base.transform.position = positions.Dequeue();
            }
            if (rotations.Count > frameDelay && !AnimationOverriden)
            {
                RotateSusieToDirection(rotations.Dequeue());
            }
        }
        velocity = (base.transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = base.transform.position;
        SusieAnimator.SetFloat("VelocityMagnitude", velocity.magnitude);
    }

    public void RotateSusieToDirection(Vector2 Dir)
    {
        SusieAnimator.SetFloat("VelocityX", Dir.x);
        SusieAnimator.SetFloat("VelocityY", Dir.y);
    }

    public void RotateSusieTowardsPosition(Vector2 targetPosition)
    {
        Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
        Vector2 vector2 = targetPosition - vector;
        vector2.Normalize();
        SusieAnimator.SetFloat("VelocityX", vector2.x);
        SusieAnimator.SetFloat("VelocityY", vector2.y);
    }

    public void ClearAllMovementHistory()
    {
        positions.Clear();
        rotations.Clear();
    }

    private void AnimatePartyMember()
    {
        if (AnimationOverriden)
        {
            SusieAnimator.SetFloat("VelocityMagnitude", velocity.magnitude);
        }
    }
}
