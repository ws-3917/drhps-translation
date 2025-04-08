using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool ismoving;

    public PlayerManager PlayerManager;

    public float velocityX;

    public float velocityY;

    public Collider2D[] CollidersToDisableOnWalk;

    public bool AnimateAutomatically;

    public bool FinishedMoveTo;

    private Vector2 pos;

    public Animator NPCAnimator;

    public Coroutine CurrentWalkRoutine;

    [Header("Range : -1 to 1")]
    public float DefaultStandDirectionX;

    public float DefaultStandDirectionY;

    private void Start()
    {
        NPCAnimator.SetFloat("MOVEMENTX", DefaultStandDirectionX);
        NPCAnimator.SetFloat("MOVEMENTY", DefaultStandDirectionY);
    }

    private void Update()
    {
        Animate();
    }

    public void WalkToPosition(Vector3 TargetPosition, float Speed)
    {
        if (CurrentWalkRoutine != null)
        {
            StopCoroutine(CurrentWalkRoutine);
        }
        CurrentWalkRoutine = StartCoroutine(WalkTo(TargetPosition, Speed));
    }

    public void RotateNPC(Vector2 Dir)
    {
        NPCAnimator.SetFloat("MOVEMENTX", Dir.x);
        NPCAnimator.SetFloat("MOVEMENTY", Dir.y);
    }

    private IEnumerator WalkTo(Vector3 TargetPosition, float Speed)
    {
        ismoving = true;
        while ((TargetPosition - base.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, TargetPosition, Speed * Time.deltaTime);
            yield return null;
            CalculateVelocity();
        }
        FinishedMoveTo = true;
        ismoving = false;
        CurrentWalkRoutine = null;
    }

    private void CalculateVelocity()
    {
        velocityX = (base.transform.position.x - pos.x) / Time.deltaTime;
        velocityY = (base.transform.position.y - pos.y) / Time.deltaTime;
        pos = base.transform.position;
    }

    private void Animate()
    {
        if (AnimateAutomatically)
        {
            NPCAnimator.SetBool("MOVING", ismoving);
        }
        if (ismoving && AnimateAutomatically)
        {
            NPCAnimator.SetFloat("MOVEMENTX", velocityX);
            NPCAnimator.SetFloat("MOVEMENTY", velocityY);
        }
        Collider2D[] collidersToDisableOnWalk = CollidersToDisableOnWalk;
        for (int i = 0; i < collidersToDisableOnWalk.Length; i++)
        {
            collidersToDisableOnWalk[i].enabled = !ismoving;
        }
    }

    public void PlayAnimation(string AnimationName)
    {
        NPCAnimator.Play(AnimationName);
    }
}
