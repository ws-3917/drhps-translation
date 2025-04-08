using System;
using System.Collections;
using UnityEngine;

public class FD_Intro_SiplettArc : MonoBehaviour
{
    public Transform arcTarget;

    public Transform linearTarget;

    public float arcDuration = 2f;

    public float linearDuration = 2f;

    public float arcSpinSpeed = 360f;

    public float linearSpinSpeed = 90f;

    public float arcHeight = 2f;

    public float jumpHeight = 2f;

    [Space(10f)]
    public bool FinishedThrowAnimation;

    public bool FinishedJumpAnimation;

    [Space(10f)]
    [SerializeField]
    private SpriteRenderer SiplettRenderer;

    [SerializeField]
    private Animator SiplettAnimator;

    [SerializeField]
    private AudioClip snd_move;

    [SerializeField]
    private AudioClip snd_jump;

    private void Awake()
    {
        StartCoroutine(MoveInArcAndLine());
        SiplettRenderer.transform.localScale = Vector3.one * 1.35f;
        SiplettRenderer.flipX = true;
        SiplettAnimator.Play("Siplett_Cutscene_CupForm");
    }

    private IEnumerator MoveInArcAndLine()
    {
        Vector3 startPosition = base.transform.position;
        Vector3 targetPosition = arcTarget.position;
        float elapsedTime = 0f;
        while (elapsedTime < arcDuration)
        {
            elapsedTime += Time.deltaTime;
            float num = elapsedTime / arcDuration;
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, num);
            position.y += Mathf.Sin(num * MathF.PI) * arcHeight;
            base.transform.position = position;
            base.transform.Rotate(Vector3.forward, arcSpinSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
        base.transform.position = targetPosition;
        startPosition = base.transform.position;
        targetPosition = linearTarget.position;
        elapsedTime = 0f;
        SiplettRenderer.flipX = false;
        SiplettAnimator.Play("Siplett_Cutscene_CupFormLanded");
        while (elapsedTime < linearDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / linearDuration;
            base.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            base.transform.Rotate(Vector3.forward, linearSpinSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
        base.transform.position = targetPosition;
        FinishedThrowAnimation = true;
    }

    public IEnumerator JumpUpAndDown()
    {
        _ = base.transform.position + Vector3.up * jumpHeight;
        _ = 0.75f / 2f;
        CutsceneUtils.ShakeTransform(base.transform, 0.25f, 1.5f);
        CutsceneUtils.PlaySound(snd_move);
        yield return new WaitForSeconds(2f);
        base.transform.rotation = Quaternion.Euler(Vector3.zero);
        SiplettRenderer.transform.localScale = Vector3.one;
        SiplettAnimator.Play("Siplett_Cutscene_CenteredHurt");
        CutsceneUtils.PlaySound(snd_jump);
        CutsceneUtils.MoveTransformOnArc(base.transform, base.transform.position, 1.5f, 0.6f);
        yield return new WaitForSeconds(0.6f);
        CutsceneUtils.ShakeTransform(base.transform, 0.1f, 1f);
        SiplettAnimator.Play("Siplett_Cutscene_CenteredIdle");
        FinishedJumpAnimation = true;
    }
}
